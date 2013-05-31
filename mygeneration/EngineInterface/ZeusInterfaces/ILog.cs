using System;

namespace Zeus
{
	public interface ILog
	{
		void Write(string text);
        void Write(string text, params object[] args);
		void Write(Exception ex);
	}
}
