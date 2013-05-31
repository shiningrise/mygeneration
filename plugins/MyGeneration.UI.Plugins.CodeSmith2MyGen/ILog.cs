using System;

namespace MyGeneration.CodeSmithConversion
{
	/// <summary>
	/// Summary description for ILog.
	/// </summary>
	public interface ILog
	{
		void AddEntry(string message, params object[] args);
		void AddEntry(Exception ex);
	}
}
