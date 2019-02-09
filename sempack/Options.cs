using CommandLine;
using NLog;
using System;
using System.Text;

namespace sempack
{
	public class Options
	{
        [Option('v', "verbose", 
        	Required = false, 
        	HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option('c', "configuration", 
        	Required = false, 
        	Default = "Debug", 
        	HelpText = "Defines the build Configuration.")]
		public string Configuration {get; set;}

		[Option("include-source", 
			Required = false, 
			HelpText = "Includes the source files in the NuGet package. The sources files are included in the `src` folder within the `nupkg`.")]
		public bool IncludeSource {get; set;}

		[Option("include-symbols", 
			Required = false, 
			HelpText = "Generates the symbols `nupkg`.")]
		public bool IncludeSymbols {get; set;}

		[Option("no-restore",
			Required = false,
			HelpText = "Doesn't execute an implicit restore when running the command.")]
		public bool NoRestore {get; set;}

		[Option('o', "output",
			Required = false,
			HelpText = "Places the built packages in the output directory specified.")]
		public string OutputDirectory {get; set;}

		[Option("runtime",
			Required = false,
			HelpText = "Specifies the target runtime to restore packages for. See RID Catalog.")]
		public string Runtime {get; set;}
		
		[Option('s', "serviceable",
			Required = false,
			HelpText = "Sets the serviceable flag in the package.")]
		public bool Serviceable {get; set;}

		[Option("version-suffix",
			Required = false,
			HelpText = "Defines the value for the $(VersionSuffix) MSBuild property in the project.")]
		public string VersionSuffix {get; set;}

		private static readonly Logger _log = LogManager.GetCurrentClassLogger();

		public Options()
		{

		}


	}

}