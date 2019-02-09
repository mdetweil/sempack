using NLog;
using System;
using System.Text;

namespace sempack
{
	public class CommandBuilder
	{
		private static readonly Logger _log = LogManager.GetCurrentClassLogger();
		private readonly Options _options;

		public CommandBuilder(Options options)
		{
			_options = options;
		}

		public string BuildPassThroughCommandString()
		{
			_log.Trace("Building pass through command string.");
			var sb = new StringBuilder();
			if (!string.IsNullOrEmpty(_options.Configuration))
			{
				_log.Trace($"Adding Configuration Option {_options.Configuration}.");
				sb.Append($"-c {_options.Configuration}");
			}
			else 
			{
				_log.Trace($"Configuration option not set, using default value.");	
			}

			return sb.ToString();
		}
	}
}