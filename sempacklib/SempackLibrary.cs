using CommandLine;
using NLog;
using NLog.Targets;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace sempacklib
{
    public class SempackLibrary
    {
    	private string[] _args;
    	private static Logger _log;
    	private const string _command = "dotnet pack";

        public event EventHandler<CommandCompletedArgs> CommandCompleted; 

    	public SempackLibrary(IEnumerable<string> args)
    	{
    		_args = args.ToArray();
    	}

    	public void ParseArguments()
    	{
            var parser = new Parser(cfg => cfg.CaseInsensitiveEnumValues = true);
            var results = parser.ParseArguments<Options>(_args);

            //var results = Parser.Default.ParseArguments<Options>(_args);
    		
            if (results.Tag == ParserResultType.Parsed)
    		{
	    		results.WithParsed<Options>((opts) => RunOptionsAndReturnExitCode(opts));        
	    	}
    	}

    	private void BuildLoggingConfiguration(bool verbose)
    	{
        	var config = new LoggingConfiguration();
        	var layoutString = string.Empty;
        	if(verbose)
        	{
        		layoutString = @"${date:format=HH\:mm\:ss} VERBOSE ${message} ${exception}";
        	}
        	else 
        	{
        		layoutString = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}";
        	}

	        var consoleTarget = new ConsoleTarget("target1")
	        {
	            Layout = layoutString
	        };
	        config.AddTarget(consoleTarget);

	        if(verbose)
	        {
	        	config.AddRuleForAllLevels(consoleTarget); // only errors to file
	        }
	        else 
	        {
	        	config.AddRuleForOneLevel(LogLevel.Error, consoleTarget); // only errors to file
	        }

    	    LogManager.Configuration = config;
    	    _log = LogManager.GetCurrentClassLogger();
    	    _log.Trace("VERBOSE Logging Enabled");
    	}



    	private void RunOptionsAndReturnExitCode(Options options)
    	{
    		BuildLoggingConfiguration(options.VerbosityLevel > 0);
    		_log.Trace("Handling Valid Options");
    		var builder = new CommandBuilder(options);

            string result = string.Empty;

    		if(!builder.TryBuildCommandString(out result))
    		{
    			_log.Error($"Invalid Arguments: {result}");
    			return;
    		}

    		var projModifier = new CsProjModifier(builder.GetPath(), options);
    		if(!projModifier.TryModifyProjectFile())
    		{
    			_log.Error($"Failed to modify {options.SourceFile} exiting application");
    			return;
    		}

    		var runner = new CommandRunner(_command, result);
    		
            var handler = CommandCompleted;

    		if(!runner.TryRunCommand())
    		{
    			_log.Error($"COMMAND FAILED");
                handler(this, new CommandCompletedArgs(false));
    		}
    		else 
    		{
    			_log.Trace($"COMMAND SUCCESSFUL");
                handler(this, new CommandCompletedArgs(true));
    		}
    	}
    }
}
