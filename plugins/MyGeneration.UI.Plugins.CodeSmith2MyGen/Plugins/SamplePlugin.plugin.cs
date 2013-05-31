using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Reflection;

using MyGeneration.CodeSmithConversion;
using MyGeneration.CodeSmithConversion.Plugins;
using MyGeneration.CodeSmithConversion.Template;

/*
The DLLs Referenced are:
	System.dll
	System.Xml.dll
	System.Data.dll
	System.Drawing.dll
	System.Windows.Forms.dll
	MyGeneration.CodeSmithConversion.dll
*/
namespace MyGeneration.CodeSmithConversion
{
	/// <summary>
	/// A Sample Plugin for customizing the CodeSmith conversion.
	/// </summary>
	public class SamplePlugin : ICstProcessor
	{
		/// <summary>
		/// You must have a constructor with 0 parameters.
		/// </summary>
		public SamplePlugin() {}

		/// <summary>
		/// This is the method where you have the ability to change the parsed CST 
		/// Template object before the translation completes.
		/// </summary>
		/// <param name="template"></param>
		public void Process(CstTemplate template, ILog log)
		{
			foreach (CstToken token in template.Tokens) 
			{
				// I put this break here just to save on performance because this sample plugin does nothing.
				break;

				// If the token is a template code token, then do the replacements.
				if ((token.TokenType == CstTokenType.Code) ||
					(token.TokenType == CstTokenType.ResponseWriteShortcutCode) ||
					(token.TokenType == CstTokenType.RunAtServerCode)) 
				{
					if (template.Language == CstTemplate.LANGUAGE_CSHARP) 
					{
						token.Text = token.Text.Replace("Response.WriteLine()", "output.writeln(\"\")");
					}

					if (template.Language == CstTemplate.LANGUAGE_VBNET) 
					{
						token.Text = token.Text.Replace("Response.WriteLine", "output.writeln \"\")");
					}
				}

				log.AddEntry("Ran Special Plugin Conversion.");
			}
		}

		/// <summary>
		/// Authors name
		/// </summary>
		public string Author
		{
			get { return "Justin Greenwood"; }
		}

		/// <summary>
		/// Plugin Description/Name
		/// </summary>
		public string Name
		{
			get { return "Sample Plugin"; }
		}

	}
}
