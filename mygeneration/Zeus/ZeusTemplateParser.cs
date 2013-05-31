using System;
using System.IO;
using System.Text;
using System.Collections;

using Zeus.ErrorHandling;
using Zeus.Legacy;

namespace Zeus
{
	/// <summary>
	/// Summary description for ZeusTemplateParser.
	/// </summary>
	public class ZeusTemplateParser
	{
		public ZeusTemplateParser() {}

		public static ZeusTemplateHeader LoadTemplateHeader(string filePath) 
		{
			ZeusTemplateParser parser = new ZeusTemplateParser();
			return parser.LoadTemplateHeaderStruct(filePath);
		}

		public ZeusTemplateHeader LoadTemplateHeaderStruct(string filePath)
		{
			ZeusTemplateHeader header = new ZeusTemplateHeader();
			try 
			{
				StreamReader reader = new StreamReader(filePath, true);
				LoadIntoTemplateHeader(reader, filePath, ref header);
				reader.Close();
			}
			catch (ZeusParseException ex)
			{
				header = new ZeusTemplateHeader();
				throw ex;
			}

			return header;
		}

		public ZeusTemplate LoadTemplate(string filePath)
		{
			ZeusTemplate template = new ZeusTemplate();
			LoadIntoTemplate(filePath, template);

			return template;
		}

		public void LoadIntoTemplate(string filePath, ZeusTemplate template)
		{
			try 
			{
				StreamReader reader = new StreamReader(filePath, true);
				LoadIntoTemplate(reader, filePath, template);
				reader.Close();
			}
			catch (ZeusParseException parseEx)
			{
				if (parseEx.ParseError == ZeusParseError.OutdatedTemplateStructure) 
				{
					LegacyTemplateParser parser = new LegacyTemplateParser();
					parser.LoadIntoTemplate(filePath, template);
				}
				else
				{
					throw parseEx;
				}
			}
		}

		public ZeusTemplate LoadIntoTemplate(Stream stream, string filepath, ZeusTemplate template)
		{
			StreamReader reader = new StreamReader(stream, true);
			LoadIntoTemplate(reader, filepath, template);
			reader.Close();
			return template;
		}


		public void LoadIntoTemplateHeader(StreamReader reader, string filepath, ref ZeusTemplateHeader header)
		{
			char[] whitespace = new char[4] {'\n', '\r', '\t', ' '};
			string key, val, line;
			int tagLength = ZeusConstants.START_DIRECTIVE.Length;
			int x, y;
			bool inGroupMode = false;
			string startGroupTag = string.Empty;
			StringBuilder builder = null;

			if (filepath != null) 
			{
				int lastIndex = filepath.LastIndexOf('\\');
				header.FileName = filepath.Substring(lastIndex + 1);
				header.FilePath = filepath.Substring(0, lastIndex + 1);
			}

			line = reader.ReadLine();
			while (line != null)
			{
				if (line.StartsWith(ZeusConstants.START_DIRECTIVE)) 
				{
					x = line.IndexOfAny(whitespace);
					if (x < 0) x = line.Length;
					y = x - tagLength;
				
					key = line.Substring(tagLength, y);

					if (!inGroupMode) 
					{
						if (IsGroupStartTag(key)) 
						{
							inGroupMode = true;
							startGroupTag = key;
							builder = new StringBuilder();
						}
						else 
						{
							val = line.Substring(x).Trim();
							AssignProperty(ref header, key, val);
						}
					}
					else 
					{
						if (IsGroupEndTag(key)) 
						{
							AssignGroup(ref header, key, builder);

							inGroupMode = false;
							startGroupTag = string.Empty;
						}
						else
						{
							//TODO: *** Could put a warning here. Possibly have a warnings collection. Maybe a CompileInfo class?
							builder.Append(line + "\r\n");
						}
					}
				}
				else if (inGroupMode)
				{
					builder.Append(line + "\r\n");
				}

				line = reader.ReadLine();
			}
		}

		public void LoadIntoTemplate(StreamReader reader, string filepath, ZeusTemplate template)
		{
			char[] whitespace = new char[4] {'\n', '\r', '\t', ' '};
			string key, val, line;
			int tagLength = ZeusConstants.START_DIRECTIVE.Length;
			int x, y;
			bool inGroupMode = false;
			string startGroupTag = string.Empty;
			StringBuilder builder = null;

			if (filepath != null) 
			{
				int lastIndex = filepath.LastIndexOf('\\');
				template.FileName = filepath.Substring(lastIndex + 1);
				template.FilePath = filepath.Substring(0, lastIndex + 1);
			}

			line = reader.ReadLine();
			if (!line.StartsWith(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.TYPE)) 
			{
                throw new ZeusParseException(template, ZeusParseError.OutdatedTemplateStructure, "OutdatedTemplateStructure");
			}
			while (line != null)
			{
				if (line.StartsWith(ZeusConstants.START_DIRECTIVE)) 
				{
					x = line.IndexOfAny(whitespace);
					if (x < 0) x = line.Length;
					y = x - tagLength;
				
					key = line.Substring(tagLength, y);

					if (!inGroupMode) 
					{
						if (IsGroupStartTag(key)) 
						{
							inGroupMode = true;
							startGroupTag = key;
							builder = new StringBuilder();
						}
						else 
						{
							val = line.Substring(x).Trim();
							AssignProperty(template, key, val);
						}
					}
					else 
					{
						if (IsGroupEndTag(key)) 
						{
							AssignGroup(template, key, builder);

							inGroupMode = false;
							startGroupTag = string.Empty;
						}
						else
						{
							//TODO: *** Could put a warning here. Possibly have a warnings collection. Maybe a CompileInfo class?
							builder.Append(line + "\r\n");
						}
					}
				}
				else if (inGroupMode)
				{
					builder.Append(line + "\r\n");
				}

				line = reader.ReadLine();
			}
		}

		protected void AssignGroup(ZeusTemplate template, string key, StringBuilder builder) 
		{
			switch (key) 
			{
				case ZeusConstants.Directives.BODY_END:
					template.BodySegment.CodeUnparsed = builder.ToString().TrimEnd();
					break;
				case ZeusConstants.Directives.GUI_END:
					template.GuiSegment.CodeUnparsed = builder.ToString().TrimEnd();
					break;
				case ZeusConstants.Directives.COMMENTS_END:
					template.Comments = builder.ToString().TrimEnd();
					break;
			}
		}


		protected void AssignProperty(ZeusTemplate template, string key, string val) 
		{
			template.AddDirective(key, val);

			switch (key) 
			{
				case ZeusConstants.Directives.TYPE:
					template.Type = val;
					break;
				case ZeusConstants.Directives.SOURCE_TYPE:
					template.SourceType = val;
					break;
				case ZeusConstants.Directives.BODY_ENGINE:
					template.BodySegment.Engine = val;
					break;
				case ZeusConstants.Directives.BODY_LANGUAGE:
					template.BodySegment.Language = val;
					break;
				case ZeusConstants.Directives.BODY_MODE:
					template.BodySegment.Mode = val;
					break;
				case ZeusConstants.Directives.GUI_ENGINE:
					template.GuiSegment.Engine = val;
					break;
				case ZeusConstants.Directives.GUI_LANGUAGE:
					template.GuiSegment.Language = val;
					break;
				case ZeusConstants.Directives.OUTPUT_LANGUAGE:
					template.OutputLanguage = val;
					break;
				case ZeusConstants.Directives.UNIQUEID:
					template.UniqueID = val;
					break;
				case ZeusConstants.Directives.TITLE:
					template.Title = val;
					break;
				case ZeusConstants.Directives.NAMESPACE:
					template.NamespacePathString = val;
					break;
				case ZeusConstants.Directives.BODY_TAG_START:
					template.TagStart = val;
					break;
				case ZeusConstants.Directives.BODY_TAG_END:
					template.TagEnd = val;
					break;
				case ZeusConstants.Directives.INCLUDE_TEMPLATE:
					template.AddIncludedTemplatePath(val);
					break;
			}
		}
		
		protected void AssignGroup(ref ZeusTemplateHeader header, string key, StringBuilder builder) 
		{
			switch (key) 
			{
				case ZeusConstants.Directives.COMMENTS_END:
					header.Comments = builder.ToString().TrimEnd();
					break;
			}
		}

		protected void AssignProperty(ref ZeusTemplateHeader header, string key, string val) 
		{
			switch (key) 
			{
				case ZeusConstants.Directives.UNIQUEID:
					header.UniqueID = val;
					break;
				case ZeusConstants.Directives.TITLE:
					header.Title = val;
					break;
				case ZeusConstants.Directives.NAMESPACE:
					header.Namespace = val;
					break;
				case ZeusConstants.Directives.SOURCE_TYPE:
					header.SourceType = val;
					break;
			}
		}

		protected bool IsGroupStartTag(string key) 
		{
			bool isValid = false;
			switch (key) 
			{
				case ZeusConstants.Directives.BODY_BEGIN:
				case ZeusConstants.Directives.COMMENTS_BEGIN:
				case ZeusConstants.Directives.GUI_BEGIN:
					isValid = true;
					break;
			}
			return isValid;
		}

		protected bool IsGroupEndTag(string key) 
		{
			bool isValid = false;
			switch (key) 
			{
				case ZeusConstants.Directives.BODY_END:
				case ZeusConstants.Directives.COMMENTS_END:
				case ZeusConstants.Directives.GUI_END:
					isValid = true;
					break;
			}
			return isValid;
		}
	}
}
