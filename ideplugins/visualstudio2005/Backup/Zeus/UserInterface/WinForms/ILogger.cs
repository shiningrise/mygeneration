using System;

namespace Zeus.UserInterface
{
	/// <summary>
	/// Summary description for ILogger.
	/// </summary>
	public interface ILogger
	{
		void LogException(Exception ex);
		void LogLn(string input);
	}
}
