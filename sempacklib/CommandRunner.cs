using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Text;


namespace sempacklib
{
	public class CommandRunner
	{
		private ILogger<CommandRunner> _log;
		private string _commandArg;

		private ProcessStartInfo _processStartInfo;
		private StringBuilder _processOutput;
		private Process _process;
		private const string COMMAND = "dotnet pack";

		public CommandRunner(ILogger<CommandRunner> log, ProcessStartInfo startInfo, Process process)
		{
			
			_log = log;
			_processOutput = new StringBuilder();
			_processStartInfo = startInfo;
			_process = process;
		}

		public bool TryRunCommand(string arg)
		{
			_commandArg = $"{COMMAND} {arg}";
			_log.LogTrace($"Running Command {_commandArg}");
			
			BuildProcessStartInfo();
			BuildProcess();
			
			_process.Start();
			_process.BeginOutputReadLine();
			_process.BeginErrorReadLine();
			_process.WaitForExit();
			_process.CancelOutputRead();
			_process.CancelErrorRead();

			if (_process.ExitCode == 0)
			{
				return true;
			}
			return false;
		}

		private void BuildProcessStartInfo()
		{
			_log.LogTrace("Building Process Start Info");
			_processStartInfo.CreateNoWindow = true;
			_processStartInfo.RedirectStandardOutput = true;
			_processStartInfo.RedirectStandardInput = true;
			_processStartInfo.RedirectStandardError = true;
			_processStartInfo.UseShellExecute = false;
			

			if(OperatingSystem.IsWindows())
			{
				_log.LogTrace("Running process from cmd.exe");
				_processStartInfo.Arguments = $"/c {_commandArg}";
				_processStartInfo.FileName = "cmd.exe";
			}
			else 
			{
				_log.LogTrace("Running process from bash");
				_processStartInfo.Arguments = $"-c \"{_commandArg}\"";
				_processStartInfo.FileName = "/bin/bash";
			}
		}

		private void BuildProcess()
		{
			_log.LogTrace("Building Process");
			_process.StartInfo = _processStartInfo;
			_process.EnableRaisingEvents = true;
			_process.OutputDataReceived += new DataReceivedEventHandler
			(
				delegate(object sender, DataReceivedEventArgs args)
				{
					_log.LogTrace(args.Data);
				}
			);

			_process.ErrorDataReceived += new DataReceivedEventHandler
			(
				delegate(object sender, DataReceivedEventArgs args)
				{
					_log.LogTrace(args.Data);
				}
			);
		}

	}
}
