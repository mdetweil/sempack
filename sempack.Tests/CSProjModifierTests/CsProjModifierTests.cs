using Microsoft.Extensions.Logging;
using Moq;
using sempacklib;
using System;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using Xunit;

namespace sempack.Tests.CSProjModifierTests
{
    public class CsProjModifierTests: IDisposable
    {
        private readonly CsProjModifier _csProjModifier;
        private readonly string _path;
        public CsProjModifierTests()
        {
            var logger = new Mock<ILogger<CsProjModifier>>();
            _csProjModifier = new CsProjModifier(logger.Object);
            var directory = Directory.GetCurrentDirectory();
            _path = Path.Combine(directory, "Test.csproj");
        }

        [Theory]
        [MemberData("GetModifyProjectFileTestCaseTitles", MemberType = typeof(CsProjModifierTestCases))]
        public void CsProjModifier_TryModifyProjectFile_Returns_Correct_Results(string name)
        {
            //Arrange
            var input = CsProjModifierTestCases.GetModifyProjectFilesTestCase(name);
            var options = new Options()
            {
                Major = input.IncrementMajor,
                Minor = input.IncrementMinor,
                Build = input.IncrementBuild,
                Revision = input.IncrementRevision
            };
            WriteFileContents(input.PresetPrefixVersion, input.DeleteVersion, input.DeleteVersionPrefix);

            //Act
            var result = _csProjModifier.TryModifyProjectFile(options, _path);

            var doc = XElement.Load(_path);
            var propertyGroup = doc.Element("PropertyGroup");
            var versionPrefix = propertyGroup.Element("VersionPrefix");
            var splitVersion = versionPrefix.Value.Split('.');
            
            //Assert
            Assert.Equal(input.Result, result);
            Assert.Null(propertyGroup.Element("Version"));
            Assert.NotNull(versionPrefix);
            Assert.NotNull(splitVersion[0]);
            Assert.NotNull(splitVersion[1]);
            Assert.NotNull(splitVersion[2]);
            Assert.NotNull(splitVersion[3]);
            Assert.Equal(input.ExpectedMajorVersion, splitVersion[0]);
            Assert.Equal(input.ExpectedMinorVersion, splitVersion[1]);

        }

        public void Dispose()
        {
            if (!File.Exists(_path))
            {
                File.Delete(_path);
            }
        }

        private void WriteFileContents(string presetVersion, bool removeVersion, bool removeVersionPrefix)
        {
            if (!File.Exists(_path))
            {
                File.Create(_path);
            }

            string fileContents =
                "<Project Sdk=\"Microsoft.NET.Sdk\">  <PropertyGroup>	<Version></Version>	<VersionPrefix></VersionPrefix>  </PropertyGroup></Project>";
            var doc = XElement.Parse(fileContents);

            if (removeVersion)
            {
                doc.Element("PropertyGroup").Element("Version").Remove();
            }

            if (removeVersionPrefix)
            {
                doc.Element("PropertyGroup").Element("VersionPrefix").Remove();
            }
            else
            {
                doc.Element("PropertyGroup").Element("VersionPrefix").SetValue(presetVersion);
            }

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;

            Thread.Sleep(1000);

            using (var writer = XmlWriter.Create(_path, settings))
            {
                doc.Save(writer);
            }
        }
    }
}