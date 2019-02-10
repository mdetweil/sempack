using NLog;
using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace sempacklib
{

	public class CsProjModifier
	{
		private readonly string _projPath;
		private readonly Logger _log;
		private bool _incrementMajor;
		private bool _incrementMinor;
		private bool _incrementBuild;
		private bool _incrementRevision;

		public CsProjModifier(string projPath, Options options)
		{
			_projPath = projPath;
			_log = LogManager.GetCurrentClassLogger();
			_incrementMajor = options.Major;
			_incrementMinor = options.Minor;
			_incrementBuild = options.Build;
			_incrementRevision = options.Revision;
		}

		public bool TryModifyProjectFile()
		{
			var doc = XElement.Load(_projPath);
			var propertyGroup = doc.Element("PropertyGroup");
			var versionElement = propertyGroup.Element("Version");
			var version = versionElement?.Value;
			var newVersionNumber = CreateNewBuildNumber(version);

			if (versionElement is null)
			{
				_log.Trace($"Adding New Version Attribute");
				propertyGroup.Add(new XElement("Version", newVersionNumber));
			}
			else 
			{
				versionElement.SetValue(newVersionNumber);			
			}


			var sb = new StringBuilder();
			var settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = true;
			settings.Indent = true;

			using (var writer = XmlWriter.Create(_projPath, settings))
			{
				doc.Save(writer);
			}
			return true;
		}

		private string CreateNewBuildNumber(string currentVersion = null)
		{
			var majorVersion = 1;
			var minorVersion = 0;
			var buildVersion = GetBuildVersion();
			var revisionVersion = GetBuildRevision();
			if (!string.IsNullOrEmpty(currentVersion))
			{
				_log.Trace($"Current Version Value is: {currentVersion}");
				var splitVersion = currentVersion.Split('.');
				
				for(int i = 0; i < splitVersion.Length; i++)
				{
					switch(i)
					{
						case 0:
							if (int.TryParse(splitVersion[i], out int major))
							{
								majorVersion = major;
								if(_incrementMajor)
								{
									_log.Trace("Incrementing Major");
									majorVersion = major + 1;
								}
							}

							break;
						case 1:
							if (int.TryParse(splitVersion[i], out int minor))
							{
								minorVersion = minor;
								if(_incrementMinor)
								{
									_log.Trace("Incrementing Minor");
									minorVersion = minor + 1;
								}
							}
							break;
						case 2:
							if (int.TryParse(splitVersion[i], out int build))
							{
								buildVersion = build;
								if(_incrementBuild)
								{
									_log.Trace("Incrementing Build");
									buildVersion = build + 1;
								}
							}
							break;
						case 3:
							if (int.TryParse(splitVersion[i], out int revision))
							{
								revisionVersion = revision;
								if(_incrementRevision)
								{
									_log.Trace("Incrementing Revision");
									revisionVersion = revision + 1;
								}
							}
							break;
					}
				}
			}
			
			var newVersion = $"{majorVersion}.{minorVersion}.{buildVersion}.{revisionVersion}";
			_log.Trace($"New Build number is: {newVersion}");
			return newVersion;	
		}


		private int GetBuildVersion()
		{
			var then = new DateTime(2000, 1, 1);
			return (int)((DateTime.Today - then).TotalDays);
		}

		private int GetBuildRevision()
		{
			var sinceMidnight = DateTime.Now - DateTime.Today;
		    return (int)sinceMidnight.TotalSeconds / 2;
		}
	}
}
