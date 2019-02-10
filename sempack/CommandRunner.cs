using NLog;
using System;
using System.Diagnostics;
using System.Text;


namespace sempack
{
	public class CommandRunner
	{
		private Logger _log;
		private string _commandArg;

		private ProcessStartInfo _processStartInfo;
		private StringBuilder _processOutput;
		private Process _process;


		public CommandRunner(string command, string arg)
		{
			_commandArg = $"{command} {arg}";
			_log = LogManager.GetCurrentClassLogger();
			_processOutput = new StringBuilder();

			BuildProcessStartInfo();
			BuildProcess();
		}

		public bool TryRunCommand(out string result)
		{
			_log.Trace($"Running Command {_commandArg}");
			_process.Start();
			_process.BeginOutputReadLine();
			_process.BeginErrorReadLine();
			_process.WaitForExit();
			_process.CancelOutputRead();
			_process.CancelErrorRead();

			result = _processOutput.ToString();
			
			if (_process.ExitCode == 0)
			{
				return true;
			}
			return false;
		}

		private void BuildProcessStartInfo()
		{
			_log.Trace("Building Process Start Info");
			_processStartInfo = new ProcessStartInfo();
			_processStartInfo.CreateNoWindow = true;
			_processStartInfo.RedirectStandardOutput = true;
			_processStartInfo.RedirectStandardInput = true;
			_processStartInfo.RedirectStandardError = true;
			_processStartInfo.UseShellExecute = false;
			_processStartInfo.Arguments = $"-c \"{_commandArg}\"";

			if(OperatingSystem.IsWindows())
			{
				_log.Trace("Running process from cmd.exe");
				_processStartInfo.FileName = "cmd.exe";
			}
			else 
			{
				_log.Trace("Running process from bash");
				_processStartInfo.FileName = "/bin/bash";
			}
		}

		private void BuildProcess()
		{
			_log.Trace("Building Process");
			_process = new Process();
			_process.StartInfo = _processStartInfo;
			_process.EnableRaisingEvents = true;
			_process.OutputDataReceived += new DataReceivedEventHandler
			(
				delegate(object sender, DataReceivedEventArgs args)
				{
					_processOutput.Append(args.Data);
				}
			);

			_process.ErrorDataReceived += new DataReceivedEventHandler
			(
				delegate(object sender, DataReceivedEventArgs args)
				{
					_processOutput.Append(args.Data);
				}
			);
		}

	}
}