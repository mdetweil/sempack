using CommandLine;
using System;
using System.Collections.Generic;

namespace sempacklib
{
	public interface ISempackLibrary
	{
		event EventHandler<CommandCompletedArgs> CommandCompleted;
		bool TryParseArguments(IEnumerable<string> args, out ParserResult<Options> results);
		void ReportParseErrors(IEnumerable<CommandLine.Error> errors);
		void RunOptionsAndReturnExitCode(Options options);
	}
}