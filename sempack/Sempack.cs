using CommandLine;
using Newtonsoft.Json;
using NLog;
using NLog.Targets;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;


namespace sempack
{
    public class Sempack
    {
    	private string[] _args;
    	private static Logger _log;

    	public Sempack(string[] args)
    	{
    		_args = args;
    		ParseArguments();
    	}

    	private void ParseArguments()
    	{
    		var results = Parser.Default.ParseArguments<Options>(_args);
    		if (results.Tag == ParserResultType.Parsed)
    		{
	    		results.WithParsed<Options>((opts) => RunOptionsAndReturnExitCode(opts));        
	    	}
    	}

    	private void BuildLoggingConfiguration(bool verbose)
    	{
        	var config = new LoggingConfiguration();

	        var consoleTarget = new ConsoleTarget("target1")
	        {
	            Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}"
	        };
	        config.AddTarget(consoleTarget);

	        if(verbose)
	        {
	        	config.AddRuleForOneLevel(LogLevel.Trace, consoleTarget); // only errors to file
	        }
	        else 
	        {
	        	config.AddRuleForOneLevel(LogLevel.Error, consoleTarget); // only errors to file
	        }

    	    LogManager.Configuration = config;
    	    _log = LogManager.GetCurrentClassLogger();
    	    _log.Trace("Trace Logging Enabled");
    	}

    	private void RunOptionsAndReturnExitCode(Options options)
    	{
    		BuildLoggingConfiguration(options.Verbose);
    		_log.Trace("Handling Valid Options");
    		var builder = new CommandBuilder(options);
    		var commandString = builder.BuildPassThroughCommandString();
    	}


    }
}
