using System;
using System.Collections;

namespace Zeus
{
	/// <summary>
	/// Summary description for IZeusCodeParser.
	/// </summary>
	public interface IZeusCodeParser
	{
		string EscapeLiteral(string language, string text);
		string BuildOutputCommand(string language, string text, bool isLiteral, bool addNewLine);
		string ParseCustomTag(ZeusCodeSegment segment, string text);
		string GetCustomHeaderCode(ZeusCodeSegment segment);
		string GetCustomFooterCode(ZeusCodeSegment segment);
	}
}
