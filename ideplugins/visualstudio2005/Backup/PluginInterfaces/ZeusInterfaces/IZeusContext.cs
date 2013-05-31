using System;
using System.Collections;

namespace Zeus
{
	public interface IZeusContext
	{
		IZeusInput Input { get; }
		IZeusOutput Output { get; }
		IZeusGuiControl Gui { get; }
		ILog Log { get; }
		Hashtable Objects { get; }
		IZeusIntrinsicObject[] IntrinsicObjects { get; }
		int ExecutionDepth { get; }
		IZeusTemplateStub ExecutingTemplate { get; }
		void ExecuteTemplate(string path); // Depricated
        void Execute(string path, bool copyContext);
        IZeusContext Copy();
        string Describe(string varName, object obj);
	}
}