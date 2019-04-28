using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;

namespace sempacklib
{
    public class CommandBuilder
    {
        private readonly ILogger<CommandBuilder> _log;
        private StringBuilder _command;
        private string _path;

        public CommandBuilder(ILogger<CommandBuilder> log)
        {
            _log = log;
            _command = new StringBuilder();
        }

        public bool TryBuildCommandString(Options options, out string result)
        {
            string failedResult;
            if (ValidateArgs(options, out failedResult))
            {
                result = BuildPassThroughCommandString(options);
                return true;
            }

            result = failedResult;
            return false;
        }

        public string GetPath()
        {
            return _path;
        }

        private bool ValidateArgs(Options options, out string result)
        {
            if (options.SourceFile != null)
            {
                result = string.Empty;
                var currentDirectory = Directory.GetCurrentDirectory();
                _path = Path.Combine(currentDirectory, options.SourceFile);

                if (File.Exists(_path))
                {
                    result = _path;
                    return true;
                }
                result = $"Unable to locate project file: {options.SourceFile}";
            }

            result = $"No source file given.";
            return false;
        }

        private string BuildPassThroughCommandString(Options options)
        {
            _log.LogTrace("Building pass through command string.");

            SetVerbosity(options.VerbosityLevel);
            SetProjectFile(options.SourceFile);
            SetConfiguration(options.Configuration);
            SetIncludeSource(options.IncludeSource);
            SetIncludeSymbols(options.IncludeSymbols);
            SetNoRestore(options.NoRestore);
            SetOutputDirectory(options.OutputDirectory);
            SetRuntime(options.Runtime);
            SetServiceable(options.Serviceable);
            SetVersionSuffix(options.VersionSuffix);

            return _command.ToString();
        }

        private void SetVerbosity(VerbosityLevel level)
        {
            _log.LogTrace("Adding Verbosity Level");
            if (level > 0)
            {
                if (level.ToString().Length == 1)
                {
                    level = level - 1;
                }
                _command.Append($"--verbosity {level.ToString().ToLower()}");
            }
        }

        private void SetProjectFile(string sourceFile)
        {
            if (_command.Length > 0)
            {
                _command.Append(" ");
            }
            _command.Append($"{sourceFile}");
        }

        private void SetConfiguration(string configuration)
        {
            if (!string.IsNullOrEmpty(configuration))
            {
                _log.LogTrace($"Adding Configuration Option {configuration}.");
                _command.Append($" -c {configuration}");
            }
            else
            {
                _log.LogTrace($"Configuration option not set, using default value.");
            }
        }

        private void SetIncludeSource(bool source)
        {
            if (source)
            {
                _log.LogTrace($"Adding Include Source option");
                _command.Append($" --include-source");
            }
            else
            {
                _log.LogTrace($"Include Source option not set");
            }
        }

        private void SetIncludeSymbols(bool symbols)
        {
            if (symbols)
            {
                _log.LogTrace($"Adding Include Symbols option");
                _command.Append($" --include-symbols");
            }
            else
            {
                _log.LogTrace($"Include Symbols option not set");
            }
        }

        private void SetNoRestore(bool noRestore)
        {
            if (noRestore)
            {
                _log.LogTrace($"Adding No Restore option");
                _command.Append($" --no-restore");
            }
            else
            {
                _log.LogTrace($"No Restore option not set");
            }
        }

        private void SetOutputDirectory(string outputDir)
        {
            if (!string.IsNullOrEmpty(outputDir))
            {
                _log.LogTrace($"Setting Output Directory to {outputDir}");
                _command.Append($" -o {outputDir}");
            }
            else
            {
                _log.LogTrace($"No Output Directory to include in command");
            }
        }

        private void SetRuntime(string runtime)
        {
            if (!string.IsNullOrEmpty(runtime))
            {
                _log.LogTrace($"Setting Runtime to {runtime}");
                _command.Append($" --runtime {runtime}");
            }
            else
            {
                _log.LogTrace($"No Runtime to include in command");
            }
        }

        private void SetServiceable(bool serviceable)
        {
            if (serviceable)
            {
                _log.LogTrace($"Adding Serviceable option");
                _command.Append($" -s");
            }
            else
            {
                _log.LogTrace($"Serviceable option not set");
            }
        }

        private void SetVersionSuffix(string versionSuffix)
        {
            if (!string.IsNullOrEmpty(versionSuffix))
            {
                _log.LogTrace($"Setting Version Suffix to {versionSuffix}");
                _command.Append($" --version-suffix {versionSuffix}");
            }
            else
            {
                _log.LogTrace($"No Version Suffix to include in command");
            }
        }
    }
}
