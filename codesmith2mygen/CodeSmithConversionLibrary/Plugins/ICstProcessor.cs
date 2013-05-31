using System;
using MyGeneration.CodeSmithConversion.Template;
using MyGeneration.CodeSmithConversion.Parser;

namespace MyGeneration.CodeSmithConversion.Plugins
{
	/// <summary>
	/// Summary description for ICstProcessor.
	/// </summary>
	public interface ICstProcessor
	{
		string Name { get; }
		string Author { get; }
		void Process(CstTemplate template, ILog log);
	}
}
