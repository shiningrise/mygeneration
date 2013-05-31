using System;

namespace Zeus
{
	public interface IZeusExecutionError
	{
		string FileName { get; }
		string Number { get; }
		string Source { get; }
		int Line { get; }
		int Column { get; }
		string Message { get; }
		string Description { get; }
        string StackTrace { get; }
        bool IsWarning { get; }
        bool IsRuntime { get; }
	}
}
