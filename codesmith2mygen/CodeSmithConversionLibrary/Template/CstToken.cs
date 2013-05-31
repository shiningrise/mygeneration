using System;
using System.Collections;

namespace MyGeneration.CodeSmithConversion.Template
{
	public enum CstTokenType 
	{
		Code = 0,
		ResponseWriteShortcutCode,
		RunAtServerCode,
		Literal, 
		Comment,
		EscapedStartTag,
		EscapedEndTag
	}

	/// <summary>
	/// Summary description for CstToken.
	/// </summary>
	public class CstToken
	{
		private string text;
		private CstTokenType tokenType = CstTokenType.Literal;

		public CstToken(CstTokenType tokenType, string text)
		{
			this.tokenType = tokenType;
			this.text = text;
		}

		public string Text 
		{
			get { return text; }
			set { text = value; }
		}

		public CstTokenType TokenType 
		{
			get { return tokenType; }
			set { tokenType = value; }
		}
 	}
}
