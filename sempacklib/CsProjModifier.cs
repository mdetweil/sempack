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
			
			//If Version Element Exists, delete it
			propertyGroup.Element("Version")?.Remove();

			//Update or create Version Prefix Element
			var versionPrefixElement = propertyGroup.Element("VersionPrefix");
			var version = versionPrefixElement?.Value;
			var newVersionNumber = CreateNewBuildNumber(version);

			if (versionPrefixElement is null)
			{
				_log.Trace($"Adding New Version Attribute");
				propertyGroup.Add(new XElement("VersionPrefix", newVersionNumber));
			}
			else 
			{
				versionPrefixElement.SetValue(newVersionNumber);			
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
			var splitVersion = new string[4];

			if (!string.IsNullOrEmpty(currentVersion))
			{
				_log.Trace($"Current Version Value is: {currentVersion}");
				splitVersion = currentVersion.Split('.');		
			}
			var majorVersion = GetMajorVersion(splitVersion);
			var minorVersion = GetMinorVersion(splitVersion);
			var buildVersion = GetBuildVersion(splitVersion);
			var revisionVersion = GetBuildRevision(splitVersion);

			var newVersion = $"{majorVersion}.{minorVersion}.{buildVersion}.{revisionVersion}";
			_log.Trace($"New Build number is: {newVersion}");
			return newVersion;
		}

		private int GetMajorVersion(string[] splitVersion)
		{
			if(!string.IsNullOrEmpty(splitVersion[0]) &&
				int.TryParse(splitVersion[0], out int majorVers))
			{
				if(_incrementMajor)
				{
					return majorVers + 1;
				}
				return majorVers;
			}			
			return 1;
		}

		private int GetMinorVersion(string[] splitVersion)
		{
			if(!string.IsNullOrEmpty(splitVersion[1]) &&
				int.TryParse(splitVersion[1], out int minorVers))
			{
				if(_incrementMinor)
				{
					return minorVers + 1;
				}
				return minorVers;
			}			
			return 0;
			
		}

		private int GetBuildVersion(string[] splitVersion)
		{
			if(_incrementBuild)
			{
				if(!string.IsNullOrEmpty(splitVersion[2]) &&
					int.TryParse(splitVersion[2], out int build))
				{
					return build + 1;
				}
			}
			var then = new DateTime(2000, 1, 1);
			return (int)((DateTime.Today - then).TotalDays);
		}

		private int GetBuildRevision(string[] splitVersion)
		{
			if(_incrementRevision)
			{
				if(!string.IsNullOrEmpty(splitVersion[3]) &&
					int.TryParse(splitVersion[3], out int revision))
				{
					return revision + 1;
				}
			}
			var sinceMidnight = DateTime.Now - DateTime.Today;
		    return (int)sinceMidnight.TotalSeconds / 2;
		}
	}
}