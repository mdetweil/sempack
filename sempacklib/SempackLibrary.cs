using CommandLine;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace sempacklib
{
    public class SempackLibrary : ISempackLibrary
    {
    	private readonly ILogger<SempackLibrary> _log;
			private CommandLine.Parser _parser;
    	private const string _command = "dotnet pack";
      public event EventHandler<CommandCompletedArgs> CommandCompleted; 
    	public SempackLibrary(ILogger<SempackLibrary> logger, CommandLine.Parser parser)
    	{
    		 _log = logger;
				 _parser = parser;
    	}

    	public bool TryParseArguments(IEnumerable<string> args, out ParserResult<Options> results)
    	{
				_log.LogTrace("Trying to parse Options");
				results = _parser.ParseArguments<Options>(args);

				return results.Tag == ParserResultType.Parsed;
    	}

			public void ReportParseErrors(IEnumerable<CommandLine.Error> errors)
			{
				//Will display the Help screen
				return;
			}
    
    	public void RunOptionsAndReturnExitCode(Options options)
    	{
    		_log.LogTrace("Handling Valid Options");
    		var builder = new CommandBuilder(options);

            string result = string.Empty;

    		if(!builder.TryBuildCommandString(out result))
    		{
    			_log.LogError($"Invalid Arguments: {result}");
    			return;
    		}

    		var projModifier = new CsProjModifier(builder.GetPath(), options);
    		if(!projModifier.TryModifyProjectFile())
    		{
    			_log.LogError($"Failed to modify {options.SourceFile} exiting application");
    			return;
    		}

    		var runner = new CommandRunner(_command, result);

    		if(!runner.TryRunCommand())
    		{
    		  _log.LogError($"COMMAND FAILED");
          CommandCompleted(this, new CommandCompletedArgs(false));
        }
    		else 
    		{
    			_log.LogTrace($"COMMAND SUCCESSFUL");
          CommandCompleted(this, new CommandCompletedArgs(true));
    		}
    	}
    }
}
