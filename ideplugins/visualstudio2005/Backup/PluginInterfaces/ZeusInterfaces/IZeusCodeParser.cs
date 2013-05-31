using System;
using System.Collections;

namespace Zeus
{
	public interface IZeusCodeParser
	{
		string EscapeLiteral(string language, string text);
		string BuildOutputCommand(string language, string text, bool isLiteral, bool addNewLine);
		string ParseCustomTag(IZeusCodeSegment segment, string text);
		string GetCustomHeaderCode(IZeusCodeSegment segment, IZeusIntrinsicObject[] iobjs);
		string GetCustomFooterCode(IZeusCodeSegment segment, IZeusIntrinsicObject[] iobjs);
	}
}
