using sempacklib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sempack
{
	public class Application
	{
		private bool _applicationComplete;
		private string[] _args;

		public Application(IEnumerable<string> args)
		{
			_args = args.ToArray();
			_applicationComplete = false;
		}

		public void Configure()
		{

		}

		public void Register()
		{

		}

		public void OnCommandCompleted(object sender, CommandCompletedArgs e)
		{
			_applicationComplete = true;
		}

		public void Run()
		{
			var sempack = new SempackLibrary(_args);
			sempack.CommandCompleted += OnCommandCompleted;
           	sempack.ParseArguments();
			while (!_applicationComplete)
			{

			}
		}
	}
}