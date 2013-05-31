using System;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace Zeus.DotNetScript
{
	/// <summary>
	/// Summary description for MicrosoftScriptError.
	/// </summary>
	public class DotNetScriptError : IZeusExecutionError
	{
		private string _fileName;
		private string _number;
		private string _source;
		private int _line;
		private int _column;
		private string _message;
		private string _description;
        private string _stackTrace;
        private bool _isWarning = false;
        private bool _isRuntime = false;

		public DotNetScriptError(CompilerError error, IZeusContext context)
		{
			if (context != null)
				this._fileName = context.ExecutingTemplate.FilePath + context.ExecutingTemplate.FileName;
			this._source = error.FileName;
			this._message = error.ErrorText;
			this._description = string.Empty;
			this._number = error.ErrorNumber;
			this._line = error.Line;
			this._column = error.Column;
			this._stackTrace = string.Empty;
			this._isWarning = error.IsWarning;
		}

		public string FileName { get { return _fileName; } }
		public string Number { get { return _number; } }
		public string Source { get { return _source; } }
		public int Line { get { return _line; } }
		public int Column { get { return _column; } }
		public string Message { get { return _message; } }
		public string Description { get { return _description; } }
        public string StackTrace { get { return _stackTrace; } }
        public bool IsWarning { get { return _isWarning; } }
        public bool IsRuntime { get { return _isRuntime; } }
        public Exception Exception { get { return null; } }
	}
}
