using System;
using System.Text;
using System.Collections;

using MyGeneration.CodeSmithConversion.Template;
using MyGeneration.CodeSmithConversion.Plugins;

namespace MyGeneration.CodeSmithConversion.Conversion
{
	/// <summary>
	/// Summary description for LanguageHelper.
	/// MAP
	/// ---------------------------------------
	/// Response.WriteLine ==> output.writeln
	/// Response.Write ==> output.write
	/// .cst ==> .zeus
	/// </summary>
	public abstract class LanguageHelper
	{
		public static LanguageHelper CreateInstance(string language) 
		{
			if (language.ToLower().StartsWith("c")) 
			{
				return new CSharpHelper();
			}
			else 
			{
				return new VBNetHelper();
			}
		}

		protected abstract string PropertyText { get; }
		protected abstract string BodySegmentText { get; }
		protected abstract string GuiSegmentText { get; }
		protected abstract string StringAppendChar { get; }
		protected abstract string BuildGuiCodeForProperties(CstTemplate template); 
		protected abstract string BuildBindingFunctions(CstTemplate template);
		protected abstract void ApplyMaps(CstToken token);

		public string BuildTemplate(CstTemplate template, ILog log)
		{
			string language = template.Language.ToLower().StartsWith("c") ? "C#" : "VB.Net"; 

			foreach (ICstProcessor cp in PluginController.Plugins) 
			{
				cp.Process(template, log);
			}

			string strout = AssembleTemplate(
				template.Filename, 
				language, 
				template.TargetLanguage, 
				template.Description, 
				this.BuildGuiCodeSegment(template), 
				this.BuildBodyCodeSegment(template));

			return strout;
		}

		protected string BuildInputProperty(CstProperty prop) 
		{
			return PropertyText.Replace("§NAME§", prop.Name).Replace("§TYPE§", prop.Type);
		}

		protected string BuildBodyCodeSegment(CstTemplate template) 
		{
			StringBuilder sbProps = new StringBuilder(),
				sbRunAtCode = new StringBuilder(),
				sbCode = new StringBuilder(),
				sbHeaders = new StringBuilder();

			if (template.Debug) 
			{
				sbHeaders.Append( DEBUG );
			}
			
			if (template.Assemblies.Count > 0) 
			{
				string assemblies = string.Join(", ", template.Assemblies.ToArray( typeof(string) ) as string[]);
				sbHeaders.Append( String.Format(REFERENCE, assemblies) );
			}

			if (template.NameSpaces.Count > 0) 
			{
				string namespaces = string.Join(", ", template.NameSpaces.ToArray( typeof(string) ) as string[]);
				sbHeaders.Append( String.Format(NAMESPACE, namespaces) );
			}

			foreach (CstProperty property in template.Properties) 
			{
				sbProps.Append( this.BuildInputProperty(property) ) ;
			}

			foreach (CstToken token in template.Tokens) 
			{
				if (token.TokenType == CstTokenType.Comment)
				{
					sbCode.Append( String.Format(COMMENT, token.Text) );
				}
				else if (token.TokenType == CstTokenType.EscapedStartTag)
				{
					sbCode.Append( String.Format(STARTTAG, token.Text) );
				}
				else if (token.TokenType == CstTokenType.EscapedEndTag)
				{
					sbCode.Append( String.Format(ENDTAG, token.Text) );
				}
				else if (token.TokenType == CstTokenType.Literal)
				{
					sbCode.Append( token.Text );
				}
				else if (token.TokenType == CstTokenType.RunAtServerCode)
				{
					this.ApplyMaps(token);
					sbRunAtCode.Append( token.Text );
				}
				else if (token.TokenType == CstTokenType.Code)
				{
					this.ApplyMaps(token);
					sbCode.Append( String.Format(CODE, token.Text) );
				}
				else if (token.TokenType == CstTokenType.ResponseWriteShortcutCode)
				{
					this.ApplyMaps(token);
					sbCode.Append( String.Format(WRITE, token.Text) );
				}
			}

			string code = this.BodySegmentText;
			code = code.Replace("§INPUT_PROPERTIES§", sbProps.ToString());
			code = code.Replace("§RUNAT_TEMPLATE_CODE§", sbRunAtCode.ToString());
			code = code.Replace("§CODE§", sbCode.ToString());
			code = code.Replace("§DIRECTIVES§", sbHeaders.ToString());

			return code;
		}

		protected string BuildGuiCodeSegment(CstTemplate template) 
		{
			StringBuilder sbMemberCode = new StringBuilder(),
				sbSetupCode = new StringBuilder();

			sbSetupCode.Append( BuildGuiCodeForProperties(template) ) ;
			sbMemberCode.Append( BuildBindingFunctions(template) );

			string code = this.GuiSegmentText;
			code = code.Replace("§TITLE§", template.Description.Replace("\"", "\"\""));
			code = code.Replace("§SETUPCODE§", sbSetupCode.ToString());
			code = code.Replace("§MEMBERCODE§", sbMemberCode.ToString());

			return code;
		}

		private string AssembleTemplate(string title, string language, string outLangauge, string comments, string guiCode, string bodyCode) 
		{
			return string.Format(TEMPLATE_BASE, Guid.NewGuid(), title, outLangauge, outLangauge, comments, language, guiCode, bodyCode);
		}

		private string COMMENT = "<%--{0}%>";
		private string WRITE = "<%={0}%>";
		private string CODE = "<%{0}%>";
		private string STARTTAG = "<%=\"<\"{0}\"%\"%>";
		private string ENDTAG = "<%=\"%\"{0}\">\"%>";
		private string NAMESPACE = "<%#NAMESPACE {0} %>";
		private string REFERENCE = "<%#REFERENCE {0} %>";
		private string DEBUG = "<%#DEBUG%>";

		private string TEMPLATE_BASE = @"##|TYPE Template
##|UNIQUEID {0}
##|TITLE {1}
##|NAMESPACE MyGeneration.Converted.{2}
##|OUTPUT_LANGUAGE {3}
##|COMMENTS_BEGIN
{4}
##|COMMENTS_END
##|GUI_ENGINE .Net Script
##|GUI_LANGUAGE {5}
##|GUI_BEGIN
{6}
##|GUI_END
##|BODY_MODE Markup
##|BODY_ENGINE .Net Script
##|BODY_LANGUAGE {5}
##|BODY_TAG_START <%
##|BODY_TAG_END %>
##|BODY_BEGIN
{7}
##|BODY_END";
	}
}
