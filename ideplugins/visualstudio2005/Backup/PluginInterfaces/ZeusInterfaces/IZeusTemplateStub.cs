using System;
using System.Collections;
using System.Collections.Specialized;

namespace Zeus
{
	public interface IZeusTemplateStub
	{
		IZeusCodeSegmentStub BodySegmentStub { get; }
		IZeusCodeSegmentStub GuiSegmentStub { get; }
		bool HasCommentBlock { get; }
		string[] NamespacePath { get; }
		string NamespacePathString { get; }
		string FileName { get; }
		string FilePath { get; }
		string Type { get; }
		string SourceType { get; }
		string UniqueID { get; }
		string Title { get; }
		string Comments { get; }
		string TagStart { get; }
		string TagStartShortcut { get; }
		string TagStartSpecial { get; }
		string TagEnd { get; }
		string OutputLanguage { get; }
	}
}
