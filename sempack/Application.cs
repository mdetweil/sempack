using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Targets;
using NLog.Config;
using NLog.Extensions.Logging;
using sempacklib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sempack
{
	public class Application
	{
		private bool _applicationComplete;
		private IServiceProvider _services;
		private string[] _args;
		private ISempackLibrary _sempackLibrary;

		public Application(IServiceCollection serviceCollection, IEnumerable<string> args)
		{
			_args = args.ToArray();
			_applicationComplete = false;
			_services = ConfigureServices(serviceCollection);
			_sempackLibrary = _services.GetService<ISempackLibrary>();
		}

		public void OnCommandCompleted(object sender, CommandCompletedArgs e)
		{
			_applicationComplete = true;
		}

		public void Run()
		{
			ParserResult<Options> results;
           	if (_sempackLibrary.TryParseArguments(_args, out results))
			{
				_sempackLibrary.CommandCompleted += OnCommandCompleted;
				results.WithParsed<Options>((opts) => 
				{
					//Reset the Log Manager
					NLog.LogManager.Configuration = GetLoggingConfiguration(opts);
					_sempackLibrary.RunOptionsAndReturnExitCode(opts);
				});
				while (!_applicationComplete)
				{

				}
			}
			else
			{
				results.WithNotParsed<Options>((errs) => _sempackLibrary.ReportParseErrors(errs));
			}
		}

		private IServiceProvider ConfigureServices(IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
			serviceCollection.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
			serviceCollection.AddSempackLib();
			var serviceProvider = serviceCollection.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            
            loggerFactory.AddNLog(new NLogProviderOptions
            {
                CaptureMessageTemplates = true,
                CaptureMessageProperties = true
            });
            
            NLog.LogManager.Configuration = GetLoggingConfiguration();
            return serviceProvider;
		}

		private LoggingConfiguration GetLoggingConfiguration(Options opts = null)
		{
        	var config = new LoggingConfiguration();
        	var layoutString = @"${date:format=HH\:mm\:ss} VERBOSE ${message} ${exception}";

	        var consoleTarget = new ConsoleTarget("target1")
	        {
	            Layout = layoutString
	        };
	        config.AddTarget(consoleTarget);

			if (opts == null)
			{
	        	config.AddRuleForAllLevels(consoleTarget);
			}
			else
			{
				var level = VerbosityToNlogConverter.ConvertLevels(opts.VerbosityLevel);
				config.AddRule(level, NLog.LogLevel.Fatal, consoleTarget);
			}
	       
			return config;
		}
	}
}