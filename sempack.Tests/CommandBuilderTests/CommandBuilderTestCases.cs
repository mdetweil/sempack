using System.Collections.Generic;
using System.Linq;
using sempacklib;

namespace sempack.Tests
{
    public class CommandBuilderTestCases
    {
        private static Dictionary<string, BuildCommandTestCase> _cases;

        public static BuildCommandTestCase GetBuildCommandTestCase(string key)
        {
            if (_cases == null)
            {
                BuildTestCases();
            }
            return _cases[key];
        }

        public static IEnumerable<object[]> GetBuildCommandTestCaseTitles
        {
            get
            {
                if (_cases == null)
                {
                    BuildTestCases();
                }
                return _cases.Select(x => new object[]{x.Key});
            }
        }

        private static void BuildTestCases()
        {
            _cases = new Dictionary<string, BuildCommandTestCase>();
            _cases.Add("No Source File Failed Result", BuildFailedSourceFileTest());
            _cases.Add("Source File Successful Result", BuildSuccessfulSourceFileTest());
            _cases.Add("Include Source Successful Result", BuildIncludeSourceTest());
            _cases.Add("Include Symbol Successful Result", BuildIncludeSymbolsTests());
            _cases.Add("Add Configuration Successful Result", BuildAddConfigurationTest());
            _cases.Add("Add Output Directory Successful Result", BuildAddOutputDirectoryTest());
            _cases.Add("No Restore Successful Result", BuildNoRestoreTest());
            _cases.Add("Set Runtime Successful Result", BuildRuntimeTest());
            _cases.Add("Set Serviceable Successful Result", BuildSetServiceableTest());
            _cases.Add("Add Suffix Successful Result", BuildSetSuffixTest());

        }

        private static BuildCommandTestCase BuildFailedSourceFileTest()
        {
            return new BuildCommandTestCase()
            {
                Options = new Options(),
                Result = false,
                ResponseString = "No source file given."
            };
        }

        private static BuildCommandTestCase BuildSuccessfulSourceFileTest()
        {
            return new BuildCommandTestCase()
            {
                Options = new Options().AddSourceFile(),
                Result = true,
                ResponseString = $"{OptionsAdder.TestFileName}"
            };
        }

        private static BuildCommandTestCase BuildIncludeSourceTest()
        {
            return new BuildCommandTestCase()
            {
                Options = new Options().AddSourceFile().IncludeSource(),
                Result = true,
                ResponseString = $"{OptionsAdder.TestFileName} --include-source"
            };
        }
        private static BuildCommandTestCase BuildIncludeSymbolsTests()
        {
            return new BuildCommandTestCase()
            {
                Options = new Options().AddSourceFile().IncludeSymbol(),
                Result = true,
                ResponseString = $"{OptionsAdder.TestFileName} --include-symbols"
            };
        }

        private static BuildCommandTestCase BuildAddConfigurationTest()
        {
            return new BuildCommandTestCase()
            {
                Options = new Options().AddSourceFile().AddConfiguration(),
                Result = true,
                ResponseString = $"{OptionsAdder.TestFileName} -c {OptionsAdder.Configuration}"
            };
        }

        private static BuildCommandTestCase BuildAddOutputDirectoryTest()
        {
            return new BuildCommandTestCase()
            {
                Options = new Options().AddSourceFile().AddOutputDirectory(),
                Result = true,
                ResponseString = $"{OptionsAdder.TestFileName} -o {OptionsAdder.OutputDirectory}"
            };
        }

        private static BuildCommandTestCase BuildNoRestoreTest()
        {
            return new BuildCommandTestCase()
            {
                Options = new Options().AddSourceFile().NoRestore(),
                Result = true,
                ResponseString = $"{OptionsAdder.TestFileName} --no-restore"
            };
        }

        private static BuildCommandTestCase BuildRuntimeTest()
        {
            return new BuildCommandTestCase()
            {
                Options = new Options().AddSourceFile().SetRuntime(),
                Result = true,
                ResponseString = $"{OptionsAdder.TestFileName} --runtime {OptionsAdder.Runtime}"
            };
        }

        private static BuildCommandTestCase BuildSetServiceableTest()
        {
            return new BuildCommandTestCase()
            {
                Options = new Options().AddSourceFile().SetServiceable(),
                Result = true,
                ResponseString = $"{OptionsAdder.TestFileName} -s"
            };
        }

        private static BuildCommandTestCase BuildSetSuffixTest()
        {
            return new BuildCommandTestCase()
            {
                Options = new Options().AddSourceFile().SetSuffix(),
                Result = true,
                ResponseString = $"{OptionsAdder.TestFileName} --version-suffix {OptionsAdder.Suffix}"
            };
        }
    }
}