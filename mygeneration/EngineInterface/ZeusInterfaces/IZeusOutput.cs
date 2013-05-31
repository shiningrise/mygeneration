using System;
using System.Collections;

namespace Zeus
{
	public interface IZeusOutput
	{
        void rollback(int num);
		void write(string text);
		void writeln(string text);
		void autoTab(string text);
		void autoTabLn(string text);
		void incTab();
		void decTab();
		int tabLevel { get; set; }
        string text { get; set; }
        ICollection SavedFiles { get; }
		void clear();
		void append(string path);
		void save(string path, object action);
		void saveEnc(string path, object action, object encoding);
		void setPreserveSource(string path, string prefix, string suffix);
		void preserve(string key);
		string getPreservedData(string key);
		string getPreserveBlock(string key);
	}
}
