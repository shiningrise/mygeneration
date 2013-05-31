using System;

namespace Zeus
{
	/// <summary>
	/// Summary description for IScriptExecutioner.
	/// </summary>
	public interface IZeusFunctionExecutioner
	{
		void ExecuteFunction(string functionName, params object[] parameters); 
	}
}
