using sempacklib;

namespace sempack.Tests.TestUtilities
{
    public static class OptionsAdder
    {
        public const string TestFileName = @"TestFile.txt";
        public const string OutputDirectory = @"c:\";
        public const string Configuration = "configuration";
        public const string Runtime = "runtime";
        public const string Suffix = "suffix";
        public static Options IncludeSource(this Options option)
        {
            option.IncludeSource = true;
            return option;
        }

        public static Options IncludeSymbol(this Options option)
        {
            option.IncludeSymbols = true;
            return option;
        }

        public static Options AddSourceFile(this Options option)
        {
            option.SourceFile = TestFileName;
            return option;
        }

        public static Options NoRestore(this Options option)
        {
            option.NoRestore = true;
            return option;
        }

        public static Options AddOutputDirectory(this Options option)
        {
            option.OutputDirectory = OutputDirectory;
            return option;
        }

        public static Options AddConfiguration(this Options option)
        {
            option.Configuration = Configuration;
            return option;
        }

        public static Options SetRuntime(this Options option)
        {
            option.Runtime = Runtime;
            return option;
        }

        public static Options SetServiceable(this Options option)
        {
            option.Serviceable = true;
            return option;
        }

        public static Options SetSuffix(this Options option)
        {
            option.VersionSuffix = Suffix;
            return option;
        }
    }
}