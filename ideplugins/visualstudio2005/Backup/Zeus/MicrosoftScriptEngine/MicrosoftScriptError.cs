using System;
using MSScriptControl;

namespace Zeus.MicrosoftScript
{
	/// <summary>
	/// Summary description for MicrosoftScriptError.
	/// </summary>
	public class MicrosoftScriptError : IZeusExecutionError
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
        private bool _isRuntime = true;

		public MicrosoftScriptError(MSScriptControl.Error error, IZeusContext context)
		{
			if (context != null)
				this._fileName = context.ExecutingTemplate.FilePath + context.ExecutingTemplate.FileName;
			this._source = error.Source;
			this._message = error.Description;
			this._description = string.Empty;
			this._stackTrace = error.Text;
			this._number = error.Number.ToString();
			this._line = error.Line;
			this._column = error.Column;
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
        public bool IsRuntime { get { return _isRuntime; } set { _isRuntime = value; } }
        public Exception Exception { get { return null; } }
	}
}
