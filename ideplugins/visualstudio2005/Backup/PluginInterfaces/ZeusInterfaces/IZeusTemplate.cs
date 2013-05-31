using System;
using System.Collections;
using System.Collections.Specialized;

namespace Zeus
{
	public interface IZeusTemplate
	{
		IZeusCodeSegment BodySegment { get; }
		IZeusCodeSegment GuiSegment { get; }
		bool HasCommentBlock { get; }
		NameValueCollection Directives { get; }
		ArrayList RequiredInputVariables { get; }
		string RequiredInputVariablesString { get; set; }
		ArrayList IncludedTemplatePaths { get; }
		ArrayList IncludedTemplates { get; }
		string[] NamespacePath { get; set; }
		string NamespacePathString { get; set; }
		string FileName { get; set; }
		string FilePath { get; set; }
		string Type { get; set; }
		string SourceType { get; set; }
		string UniqueID { get; set; }
		string Title { get; set; }
		string Comments { get; set; }
		string TagStart { get; set; }
		string TagStartShortcut { get; }
		string TagStartSpecial { get; }
		string TagEnd { get; set; }
		string OutputLanguage { get; set; }
		bool IsValidMode(string mode);
		bool IsValidTemplateType(string type);
		void AddDirective(string name, string val);
		void AddIncludedTemplatePath(string templatePath);
		IZeusTemplate LoadTemplateFromFile(string path);
		void Save();
		void Save(string filename);
		void Compile();
		void Encrypt();
	}
}
