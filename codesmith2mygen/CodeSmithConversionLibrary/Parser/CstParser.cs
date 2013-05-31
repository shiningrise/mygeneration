using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

using MyGeneration.CodeSmithConversion.Template;

namespace MyGeneration.CodeSmithConversion.Parser
{
	/// <summary>
	/// Summary description for CstParser.
	/// </summary>
	public class CstParser
	{
		private const string TAG_NAME = "TagName";
		private const string ATTRIBUTE_NAME = "AttributeName";
		private const string ATTRIBUTE_VALUE = "AttributeValue";
		private const string DATA = "Data";

		private const string REGEXP_TAG_INNERDS = @"(((\s|\n)+(?<" + ATTRIBUTE_NAME + @">\w+)((\s|\n)*=(\s|\n)*(?:”(?<" + ATTRIBUTE_VALUE + @">.*?)”|""(?<" + ATTRIBUTE_VALUE + @">.*?)""|'(?<" + ATTRIBUTE_VALUE + @">.*?)'|(?<" + ATTRIBUTE_VALUE + @">[^'""”>\s]+)))?)+(\s|\n)*|(\s|\n)*)";
		private const string REGEXP_INCLUDE = @"(<!--\s*#include" + REGEXP_TAG_INNERDS + "-->)";
		private const string REGEXP_DIRECTIVE = @"(<%@\s*(?<" + TAG_NAME + @">\w+)" + REGEXP_TAG_INNERDS + @"%>)";
		private const string REGEXP_COMMENT = @"(<%--(?<" + DATA + @">(?:\n|.)*?)%>)";
		private const string REGEXP_TEMPLATE_SCRIPT = @"(<script\s+runat\s*=\s*(?:""template""|”template”|template)\s*>(?<" + DATA + @">(?:\n|.)*?)</script>)";
		private const string REGEXP_ASP_BLOCK = @"(<%(?<" + DATA + @">(?:\n|.)*?)%>)";
		private const string REGEXP_TOKENIZE = TTO + @"(?<" + DATA + @">\w+?)" + TTC;

		private const string TTO = "§¥";
		private const string TTC = "¥§";
		private const string TMP_COMMENT = TTO + "COMMENT" + TTC;
		private const string TMP_SCRIPT = TTO + "SCRIPT" + TTC;
		private const string TMP_ASP = TTO + "ASP" + TTC;
		private const string TMP_START = TTO + "START" + TTC;
		private const string TMP_END = TTO + "END" + TTC;

		private CstTemplate template;
		private string cstText;
		private FileInfo cstFileInfo;
		private ILog log;
		private ArrayList comments = new ArrayList();
		private ArrayList scriptBlocks = new ArrayList();
		private ArrayList aspBlocks = new ArrayList();

		public CstParser(ILog log)
		{
			this.log = log;
		}

		public CstTemplate Parse(string filename) 
		{
			cstFileInfo = LoadFileText(filename, ref cstText);
			
			template = new CstTemplate();
			template.RawCode = cstText;
			template.Filename = cstFileInfo.Name.Substring(0, cstFileInfo.Name.LastIndexOf("."));

			IncludeFiles();
			ReplaceComments();
			ReplaceScriptBlocks();
			GrabDirectives();
			ReplaceEscapedASPTags();
			ReplaceAspBlocks();
			BuildTokens();

			return template;
		}

		private void IncludeFiles() 
		{
			log.AddEntry("Searching for included files...");

			Regex ex = new Regex(REGEXP_INCLUDE, (RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture));
			
			int currentStartIndex = 0;
			string filename = string.Empty, 
				filetext = string.Empty;
			while(currentStartIndex >= 0) 
			{
				Match match = ex.Match(cstText, currentStartIndex);

				if (match.Success) 
				{
					log.AddEntry("--> Found match: {0}", match.Value.ToString());
					
					filename = PullCapturedAttribute(match, "file");
					log.AddEntry("--> Including file: {0}", filename );

					filename = Path.Combine(cstFileInfo.DirectoryName, filename);

					LoadFileText(filename, ref filetext);
					
					cstText = cstText.Remove(match.Index, match.Length);
					cstText = cstText.Insert(match.Index, filetext);

					currentStartIndex = match.Index + filetext.Length;
				}
				else
				{
					currentStartIndex = -1;
				}
			}
		}

		private void ReplaceComments() 
		{
			log.AddEntry("Searching for comments...");

			Regex ex = new Regex(REGEXP_COMMENT, (RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture));
			
			int currentStartIndex = 0;
			int commentIndex = 0;
			string data = string.Empty;
			while(currentStartIndex >= 0) 
			{
				Match match = ex.Match(cstText, currentStartIndex);

				if (match.Success) 
				{
					log.AddEntry("--> Found match");//: {0}", match.Value.ToString());
					
					data = PullCapturedGroup(match, DATA);

					cstText = cstText.Remove(match.Index, match.Length);
					data = data.Trim().TrimEnd('-');

					if (data.Length > 0) 
					{
						commentIndex = comments.Add(data);
						cstText = cstText.Insert(match.Index, TMP_COMMENT);
						currentStartIndex = match.Index + (TMP_COMMENT.Length);

						log.AddEntry("--> Replacing Comment");//: {0}", data);
					}
					else 
					{
						log.AddEntry("--> Removed Empty Comment.");
					}
				}
				else
				{
					currentStartIndex = -1;
				}
			}
		}

		private void ReplaceScriptBlocks() 
		{
			log.AddEntry("Searching for template script blocks...");

			Regex ex = new Regex(REGEXP_TEMPLATE_SCRIPT, (RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture));
			
			int currentStartIndex = 0;
			int scriptIndex = 0;
			string data = string.Empty;
			while(currentStartIndex >= 0) 
			{
				Match match = ex.Match(cstText, currentStartIndex);

				if (match.Success) 
				{
					log.AddEntry("--> Found match");//: {0}", match.Value.ToString());
					
					data = PullCapturedGroup(match, DATA);

					cstText = cstText.Remove(match.Index, match.Length);

					if (data.Length > 0) 
					{
						scriptIndex = scriptBlocks.Add(data);
						cstText = cstText.Insert(match.Index, TMP_SCRIPT);
						currentStartIndex = match.Index + (TMP_SCRIPT.Length);

						log.AddEntry("--> Replacing Script");//: {0}", data);
					}
					else 
					{
						log.AddEntry("--> Removed Empty Script.");
					}
				}
				else
				{
					currentStartIndex = -1;
				}
			}
		}

		private void ReplaceAspBlocks() 
		{
			log.AddEntry("Searching for asp style blocks...");

			Regex ex = new Regex(REGEXP_ASP_BLOCK, (RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture));
			
			int currentStartIndex = 0;
			int scriptIndex = 0;
			string data = string.Empty;
			while(currentStartIndex >= 0) 
			{
				Match match = ex.Match(cstText, currentStartIndex);

				if (match.Success) 
				{
					log.AddEntry("--> Found match");//: {0}", match.Value.ToString());
					
					data = PullCapturedGroup(match, DATA);

					cstText = cstText.Remove(match.Index, match.Length);

					if (data.Length > 0) 
					{
						scriptIndex = aspBlocks.Add(data);
						cstText = cstText.Insert(match.Index, TMP_ASP);
						currentStartIndex = match.Index + (TMP_ASP.Length);

						log.AddEntry("--> Replacing ASP Block");//: {0}", data);
					}
					else 
					{
						log.AddEntry("--> Removed Empty ASP Block.");
					}
				}
				else
				{
					currentStartIndex = -1;
				}
			}
		}

		private void ReplaceEscapedASPTags() 
		{
			log.AddEntry("Replacing special characters...");
			cstText = cstText.Replace("<%%", CstParser.TMP_START).Replace("%%>", CstParser.TMP_END);
		}

		private void GrabDirectives() 
		{
			log.AddEntry("Searching for directives...");

			Regex ex = new Regex(REGEXP_DIRECTIVE, (RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture));
			
			bool usesSchemaExplorer = false;
			bool hasDatabaseProperty = false;
			int currentStartIndex = 0;
			string tagname, tmp;
			CstProperty p;
			while(currentStartIndex >= 0) 
			{
				Match match = ex.Match(cstText, currentStartIndex);

				if (match.Success) 
				{
					log.AddEntry("--> Found match: {0}", match.Value.ToString());
					
					tagname = this.PullCapturedGroup(match, CstParser.TAG_NAME).ToLower();
					switch (tagname) 
					{
						case "codetemplate": 
							//Language, TargetLanguage, Description, Inherits, Src, Debug
							template.Language = this.PullCapturedAttribute(match, "Language");
							template.TargetLanguage = this.PullCapturedAttribute(match, "TargetLanguage");
							template.Description = this.PullCapturedAttribute(match, "Description");
							template.Inherits = this.PullCapturedAttribute(match, "Inherits");
							template.Src = this.PullCapturedAttribute(match, "Src");
							tmp = this.PullCapturedAttribute(match, "Debug");
							if (tmp != string.Empty)
								template.Debug = Convert.ToBoolean(tmp);
							break;
						case "property": 
							//Name, Type, Default, Category, Description, Optional
							p = new CstProperty();
							p.Name = this.PullCapturedAttribute(match, "Name");
							p.Type = this.PullCapturedAttribute(match, "Type");
							p.DefaultValue = this.PullCapturedAttribute(match, "Default");
							p.Category = this.PullCapturedAttribute(match, "Category");
							p.Description = this.PullCapturedAttribute(match, "Description");
							tmp = this.PullCapturedAttribute(match, "Optional");
							if (tmp != string.Empty)
								p.Optional = Convert.ToBoolean(tmp);
							
							if (p.Type.StartsWith("SchemaExplorer"))
								usesSchemaExplorer = true;

							if (p.Type == "SchemaExplorer.DatabaseSchema")
								hasDatabaseProperty = true;

							template.Properties.Add(p);
							break;
						case "assembly": 
							//Name
							tmp = this.PullCapturedAttribute(match, "Name");
							template.Assemblies.Add(tmp);
							break;
						case "import": 
							//NameSpace
							tmp = this.PullCapturedAttribute(match, "NameSpace");
							template.NameSpaces.Add(tmp);
							break;
					}

					cstText = cstText.Remove(match.Index, match.Length);
				}
				else
				{
					currentStartIndex = -1;
				}
			}

			if (usesSchemaExplorer && !hasDatabaseProperty) 
			{
				p = new CstProperty();
				p.Name = "Database";
				p.Type = "SchemaExplorer.DatabaseSchema";
				p.Category = "Context";
				p.Description = "Select the Database to use for this template.";
				p.Optional = false;

				template.Properties.Insert(0, p);
			}
		}

		private void BuildTokens() 
		{
			cstText = cstText.TrimStart();

			log.AddEntry("Building Token List...");

			Regex ex = new Regex(REGEXP_TOKENIZE, (RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture));
			
			int lastIndex = 0, currentIndex = 0;
			string data, rawtag;
			int commentListIndex = 0, scriptListIndex = 0, aspListIndex = 0;
			CstToken token = null;
			while(lastIndex >= 0) 
			{
				Match match = ex.Match(cstText, lastIndex);

				if (match.Success) 
				{
					log.AddEntry("--> Found token");
					
					currentIndex = match.Index;

					if (currentIndex > lastIndex) 
					{
						token = new CstToken(CstTokenType.Literal, cstText.Substring(lastIndex, currentIndex - lastIndex));
						template.Tokens.Add(token);
					}

					rawtag = match.Value;
					switch (rawtag) 
					{
						case CstParser.TMP_START:
							token = new CstToken(CstTokenType.EscapedStartTag, "<%");
							break;
						case CstParser.TMP_END:
							token = new CstToken(CstTokenType.EscapedEndTag, "%>");
							break;
						case CstParser.TMP_COMMENT:
							data = comments[commentListIndex].ToString();
							token = new CstToken(CstTokenType.Comment, data);
							commentListIndex++;
							break;
						case CstParser.TMP_SCRIPT:
							data = scriptBlocks[scriptListIndex].ToString();
							token = new CstToken(CstTokenType.RunAtServerCode, data);
							scriptListIndex++;
							break;
						case CstParser.TMP_ASP:
							data = aspBlocks[aspListIndex].ToString();
							if (data.StartsWith("=")) 
								token = new CstToken(CstTokenType.ResponseWriteShortcutCode, data.Remove(0, 1));
							else 
								token = new CstToken(CstTokenType.Code, data);
							aspListIndex++;
							break;
					}
					template.Tokens.Add(token);

					lastIndex = currentIndex + rawtag.Length;
				}
				else
				{
					data = cstText.Substring(lastIndex);
					token = new CstToken(CstTokenType.Literal, data);
					template.Tokens.Add(token);

					lastIndex = -1;
				}
			}
		}

		#region RegEx Helper Functions
		private string PullCapturedGroup(Match match, string attributeName) 
		{
			Group attrName = match.Groups[attributeName];
			string returnVal = string.Empty;
			
			if (attrName.Success) 
			{
				returnVal = attrName.Value;
			}

			return returnVal;
		}

		private string PullCapturedAttribute(Match match, string attributeName) 
		{
			int captureIndex = 0;
			string returnVal = string.Empty;
	
			try 
			{
				Group attrName = match.Groups[ATTRIBUTE_NAME];
				Group attrValue = match.Groups[ATTRIBUTE_VALUE];
				if (attrName.Success && attrValue.Success) 
				{
					foreach (Capture nameCapture in attrName.Captures)
					{
						if (nameCapture.Value.ToLower() == attributeName.ToLower()) 
						{
							break;
						}
						captureIndex++;
					}

					if (attrValue.Length > captureIndex) 
					{
						Capture valueCapture = attrValue.Captures[captureIndex];
						returnVal = valueCapture.Value;
					}
				}
			}
			catch {}
			
			return returnVal;
		}

		/// <summary>
		/// For Debugging and testing
		/// </summary>
		/// <param name="groups"></param>
		private void BuildGroupTree(GroupCollection groups) 
		{
			int i = 0;
			foreach (Group group in groups)
			{
				log.AddEntry("{1}:Group: {0}", group.Value, i);
				
				int j = 0;
				foreach (Capture capture in group.Captures)
				{
					log.AddEntry("{1}:Capture: {0}", capture.Value, j);
					j++;
				}
				i++;
			}
		}
		#endregion

		private static FileInfo LoadFileText(string filename, ref string text) 
		{
			FileInfo info = new FileInfo(filename);
			if (info.Exists) 
			{
				StreamReader reader = null;
				reader = info.OpenText();
				text = reader.ReadToEnd();
				reader.Close();
			}
			else 
			{
				throw new FileNotFoundException( "CodeSmith Template/Include file not found.", filename );
			}

			return info;
		}
	}
}
