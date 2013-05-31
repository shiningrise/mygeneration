using System;

namespace Zeus
{
	public interface IZeusScriptingEngine
	{
		string EngineName { get; }
		string[] SupportedLanguages { get; }
		IZeusCodeParser CodeParser { get; }
		IZeusExecutionHelper ExecutionHelper { get; }

		bool IsSupportedLanguage(string language);
		string GetNewTemplateText(string language);
		string GetNewGuiText(string language);
	}
}
