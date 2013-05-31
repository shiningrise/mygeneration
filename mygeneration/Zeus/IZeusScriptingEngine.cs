using System;

namespace Zeus
{
	/// <summary>
	/// Summary description for IZeusScriptingEngine.
	/// </summary>
	public interface IZeusScriptingEngine
	{
		string EngineName { get; }
		string[] SupportedLanguages { get; }
		IZeusCodeParser CodeParser { get; }
		IZeusExecutioner Executioner { get; }

		bool IsSupportedLanguage(string language);
		string GetNewTemplateText(string language);
		string GetNewGuiText(string language);
	}
}
