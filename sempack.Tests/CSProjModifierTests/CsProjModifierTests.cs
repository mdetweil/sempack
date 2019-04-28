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
    public class CsProjModifierTests
    {
        private readonly CsProjModifier _csProjModifier;
        public CsProjModifierTests()
        {
            var logger = new Mock<ILogger<CsProjModifier>>();
            _csProjModifier = new CsProjModifier(logger.Object);
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
            var directory = Directory.GetCurrentDirectory();
            var guid = Guid.NewGuid().ToString();
            var path = Path.Combine(directory, $"{guid}.csproj");

            WriteFileContents(path, input.PresetPrefixVersion, input.DeleteVersion, input.DeleteVersionPrefix);

            //Act
            var result = _csProjModifier.TryModifyProjectFile(options, path);

            var doc = XElement.Load(path);
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
            if (input.IncrementBuild)
            {
                var version = input.PresetPrefixVersion.Split('.')[2];
                var parsed = int.Parse(version);
                Assert.Equal((parsed + 1).ToString(), splitVersion[2]);
            }
            else
            {
                var then = new DateTime(2000, 1, 1);
                Assert.True(int.Parse(splitVersion[2]) > (int)((DateTime.Today - then).TotalDays) - 1);
            }

            if (input.IncrementRevision)
            {
                var version = input.PresetPrefixVersion.Split('.')[3];
                var parsed = int.Parse(version);
                Assert.Equal((parsed + 1).ToString(), splitVersion[3]);
            }
            else
            {
                var sinceMidnight = DateTime.Now - DateTime.Today;
                Assert.True(int.Parse(splitVersion[3]) > ((int)sinceMidnight.TotalSeconds / 2) - 2);
            }

            Dispose(path);
        }

        public void Dispose(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private void WriteFileContents(string path, string presetVersion, bool removeVersion, bool removeVersionPrefix)
        {
            using (var stream = File.Create(path))
            {
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

                using (var writer = XmlWriter.Create(stream, settings))
                {
                    doc.WriteTo(writer);
                    writer.Dispose();
                }
            }
        }
    }
}