using System;

namespace Zeus
{
	public interface IZeusFunctionExecutioner
	{
		void ExecuteFunction(string functionName, params object[] parameters); 
	}
}
