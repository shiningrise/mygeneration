using System;
using System.IO;
using System.Text;
using System.Collections;
using Zeus;

namespace Zeus
{
	/// <summary>
	/// Summary description for Log.
	/// </summary>
	public class Log : ILog
	{
		private string _logFile;
		private bool _isOpen = false;
		private bool _isConsoleEnabled = true;
		private bool _isLogEnabled = false;
        private bool _isInternalUseMode = false;
		private StreamWriter _writer;

		public Log() {}

		public Log(string fileName)
		{
			_logFile = fileName;
		}

		public void Open()
		{
			if(_isOpen) 
			{
				this.Close();
			}

			if (_logFile != null) 
			{
				try 
				{
					_writer = File.AppendText(_logFile);
				}
				catch 
				{
					_writer = null;
				}
			}

			_isOpen = (_writer != null);
		}

		public void Close()
		{
			if (_writer != null) 
			{
				_writer.Flush();
				_writer.Close();
				_writer = null;
			}

			_isOpen = false;
		}

		public void Write(string text)
		{

			if(_isLogEnabled) 
			{
				if(!_isOpen) this.Open();

                if (_isOpen)
                {
                    if (!_isInternalUseMode)
                    {
                        _writer.Write(System.DateTime.Now);
                        _writer.Write(" ");
                    }
                    _writer.Write(text);
                    _writer.WriteLine();
                    _writer.Flush();
                }
			}
			
			if(_isConsoleEnabled)
			{
                if (!_isInternalUseMode)
                {
                    Console.Write(System.DateTime.Now);
                    Console.Write(" ");
                }
				Console.Write(text);
				Console.WriteLine();
				Console.Out.Flush();
			}			
		}
	
		public void Write(string format, params object[] args)
		{
			Write(string.Format(format, args));
		}

		public void Write(Exception ex)
		{
			Write("ERROR: [{0}] - {1}", ex.GetType().FullName, ex.Message);
#if DEBUG
            ArrayList list = new ArrayList();
            bool allGone = false;
            while (!allGone)
            {
                if (!list.Contains(ex)) list.Add(ex);

                if ((ex.InnerException != null) && !list.Contains(ex.InnerException))
                {
                    ex = ex.InnerException;
                }
                else
                {
                    allGone = true;
                }
            }

            foreach (Exception nex in list)
            {
                Write(nex.StackTrace);
            }
#endif
        }

        public bool IsInternalUseMode
        {
            get { return _isInternalUseMode; }
            set { _isInternalUseMode = value; }
        }

		public bool IsConsoleEnabled
		{
			get { return _isConsoleEnabled; }
			set { _isConsoleEnabled = value; }
		}

		public bool IsLogEnabled
		{
			get { return _isLogEnabled; }
			set { _isLogEnabled = value; }
		}

		public bool IsOpen
		{
			get { return _isOpen; }
		}

		public string FileName
		{
			get { return this._logFile; }
			set 
			{ 
				if (this._isOpen) 
				{
					this.Close();
				}
				_logFile = value; 	

				if (_logFile != null) 
				{
					this.Open();
				}
			}
		}

	}
}
