using System;

namespace sempacklib
{
	public class CommandCompletedArgs : EventArgs
	{

		public bool CommandSuccessful {get; private set;}

		public CommandCompletedArgs(bool commandSuccessful)
		{
			CommandSuccessful = commandSuccessful;
		}
	}
}