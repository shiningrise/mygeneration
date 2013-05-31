using System;

namespace Zeus
{
	public delegate void ShowGUIEventHandler(IZeusGuiControl controller, IZeusFunctionExecutioner executioner);

	public interface IZeusExecutionHelper : IZeusFunctionExecutioner
	{
		void EngineExecuteGuiCode(IZeusCodeSegment segment, IZeusContext context);
		void EngineExecuteCode(IZeusCodeSegment segment, IZeusContext context);
		void SetShowGuiHandler(ShowGUIEventHandler guiEventHandler);
		int Timeout { get; set; }
		void Cleanup();
		IZeusExecutionError[] Errors { get; }
		void ClearErrors();
		bool HasErrors { get; }
	}
}
