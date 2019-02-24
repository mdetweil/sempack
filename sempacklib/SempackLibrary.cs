using CommandLine;
using Microsoft.Extensions.Logging;
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
			private CommandBuilder _builder;
			private CsProjModifier _csProjModifier;
    	private CommandRunner _commandRunner;
      public event EventHandler<CommandCompletedArgs> CommandCompleted; 
    	public SempackLibrary(
				ILogger<SempackLibrary> logger, 
				CommandLine.Parser parser, 
				CommandBuilder builder,
				CsProjModifier csProjModifier,
				CommandRunner runner)
    	{
    		 _log = logger;
				 _parser = parser;
				 _builder = builder;
				 _csProjModifier = csProjModifier;
				 _commandRunner = runner;
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

        string result = string.Empty;

    		if(!_builder.TryBuildCommandString(options, out result))
    		{
    			_log.LogError($"Invalid Arguments: {result}");
    			return;
    		}

    		if(!_csProjModifier.TryModifyProjectFile(options, _builder.GetPath()))
    		{
    			_log.LogError($"Failed to modify {options.SourceFile} exiting application");
    			return;
    		}

    		if(!_commandRunner.TryRunCommand(result))
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
