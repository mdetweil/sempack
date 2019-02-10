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

		public CsProjModifier(string projPath, bool incrementMajor, bool incrementMinor)
		{
			_projPath = projPath;
			_log = LogManager.GetCurrentClassLogger();
			_incrementMajor = incrementMajor;
			_incrementMinor = incrementMinor;
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
			var revision = GetBuildRevision();
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
								if(_incrementMinor)
								{
									_log.Trace("Incrementing Minor");
									minorVersion = minor + 1;
								}
							}
							break;
						default:
							break;
					}
				}
			}
			
			var newVersion = $"{majorVersion}.{minorVersion}.{buildVersion}.{revision}";
			_log.Trace($"New Build number is: {newVersion}");
			return newVersion;	
			
		}


		private int GetBuildVersion()
		{
			var now = DateTime.Now;
			var then = new DateTime(2000, 1, 1);

			return (int)(now - then).TotalDays;
		}

		private int GetBuildRevision()
		{
			var sinceMidnight = DateTime.Now - DateTime.Today;
		    return (int)sinceMidnight.TotalSeconds / 2;
		}
	}
}
