using Microsoft.Extensions.Logging;
using Moq;
using sempack.Tests.TestUtilities;
using sempacklib;
using System;
using System.IO;
using Xunit;

namespace sempack.Tests
{
    public class CommandBuilderTests : IDisposable
    {
        private readonly CommandBuilder _commandBuilder;
        private readonly string _path;
        public CommandBuilderTests()
        {
            var logger = new Mock<ILogger<CommandBuilder>>();
            _commandBuilder = new CommandBuilder(logger.Object);
            var directory = Directory.GetCurrentDirectory();
            _path = Path.Combine(directory, OptionsAdder.TestFileName);
            if (!File.Exists(_path))
            {
                File.Create(_path);
            }
        }

        [Theory]
        [MemberData("GetBuildCommandTestCaseTitles", MemberType = typeof(CommandBuilderTestCases))]
        public void TryBuildCommandString_ReturnsProperResults(string name)
        {
            var input = CommandBuilderTestCases.GetBuildCommandTestCase(name);
            var result = _commandBuilder.TryBuildCommandString(input.Options, out var actual);
            Assert.Equal(input.Result, result);
            Assert.Equal(input.ResponseString, actual);
        }

        public void Dispose()
        {
            if (!File.Exists(_path))
            {
                File.Delete(_path);
            }
        }
    }
}