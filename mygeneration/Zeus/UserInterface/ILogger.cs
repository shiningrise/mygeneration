using System;

namespace Zeus.UserInterface
{
	/// <summary>
	/// Summary description for ILogger.
	/// </summary>
	public interface ILogger
	{
		void Log(string input);
		void LogLn(string input);
	}
}
