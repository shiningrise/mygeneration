using System;
using System.Collections;

namespace Zeus
{
	/// <summary>
	/// Summary description for IZeusecutioner.
	/// </summary>
	public interface IZeusExecutioner
	{
		void ExecuteFunction(string functionName, params object[] parameters); 
		bool ExecuteGuiCode(ZeusCodeSegment segment, ZeusGuiContext context);
		void ExecuteCode(ZeusCodeSegment segment, IZeusContext context);
		void ExecuteCode(ZeusTemplate template, ZeusTemplateContext context, ArrayList templateGroupIds);
		int ScriptTimeout { get; set; }
		void SetShowGuiHandler(ShowGUIEventHandler guiEventHandler);
		void Cleanup();
	}
}
