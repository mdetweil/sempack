using NLog;
using System;
using System.Text;

namespace sempack
{
	public class CommandBuilder
	{
		private readonly Logger _log;
		private readonly Options _options;
		private StringBuilder _command;

		public CommandBuilder(Options options)
		{
			_options = options;
			_log = LogManager.GetCurrentClassLogger();
			_command = new StringBuilder();
		}

		public bool TryBuildCommandString(out string result)
		{
			string failedResult;
			if(ValidateArgs(out failedResult))
			{
				result = BuildPassThroughCommandString();
				return true;
			}
			else
			{
				result = failedResult;
				return false;
			}
		}

		private bool ValidateArgs(out string result)
		{
			result = string.Empty;
			//return false;
			throw new NotImplementedException();
		}

		private string BuildPassThroughCommandString()
		{
			_log.Trace("Building pass through command string.");
			
			SetConfiguration();
			SetIncludeSource();
			SetIncludeSymbols();
			SetNoRestore();
			SetOutputDirectory();
			SetRuntime();
			SetServiceable();
			SetVersionSuffix();

			return _command.ToString();
		}

		private void SetConfiguration()
		{
			if (!string.IsNullOrEmpty(_options.Configuration))
			{
				_log.Trace($"Adding Configuration Option {_options.Configuration}.");
				_command.Append($" -c {_options.Configuration}");
			}
			else 
			{
				_log.Trace($"Configuration option not set, using default value.");	
			}
		}

		private void SetIncludeSource()
		{
			if(_options.IncludeSource)
			{
				_log.Trace($"Adding Include Source option");
				_command.Append($" --include-source");
			}
			else 
			{
				_log.Trace($"Include Source option not set");
			}
		}

		private void SetIncludeSymbols()
		{
			if(_options.IncludeSymbols)
			{
				_log.Trace($"Adding Include Symbols option");
				_command.Append($" --include-symbols");
			}
			else 
			{
				_log.Trace($"Include Symbols option not set");
			}
		}

		private void SetNoRestore()
		{
			if(_options.NoRestore)
			{
				_log.Trace($"Adding No Restore option");
				_command.Append($" --no-restore");
			}
			else 
			{
				_log.Trace($"No Restore option not set");
			}
		}

		private void SetOutputDirectory()
		{
			if(!string.IsNullOrEmpty(_options.OutputDirectory))
			{
				_log.Trace($"Setting Output Directory to {_options.OutputDirectory}");
				_command.Append($" -o {_options.OutputDirectory}");
			}
			else 
			{
				_log.Trace($"No Output Directory to include in command");
			}
		}

		private void SetRuntime()
		{
			if(!string.IsNullOrEmpty(_options.Runtime))
			{
				_log.Trace($"Setting Runtime to {_options.Runtime}");
				_command.Append($" --runtime {_options.Runtime}");
			}
			else 
			{
				_log.Trace($"No Runtime to include in command");
			}
		}

		private void SetServiceable()
		{
			if(_options.Serviceable)
			{
				_log.Trace($"Adding Serviceable option");
				_command.Append($" -s");
			}
			else 
			{
				_log.Trace($"Serviceable option not set");
			}
		}

		private void SetVersionSuffix()
		{
			if(!string.IsNullOrEmpty(_options.VersionSuffix))
			{
				_log.Trace($"Setting Version Suffix to {_options.VersionSuffix}");
				_command.Append($" --version-suffix {_options.VersionSuffix}");
			}
			else 
			{
				_log.Trace($"No Version Suffix to include in command");	
			}
		}
	}
}