using System;
using System.IO;
using System.Text;
using System.Collections;

namespace Zeus
{
	/// <summary>
	/// Summary description for ZeusBodyParser.
	/// </summary>
	public class ZeusCodeParser
	{
		public static void ParseCode(ZeusCodeSegment segment) 
		{
			StringReader reader;
			StringBuilder builder;
			IZeusScriptingEngine engine;

			segment.ExtraData.Clear();

			// If this is a compiled template, don't try to parse it!
			if ((segment.Template.SourceType == ZeusConstants.SourceTypes.COMPILED) && 
				(segment.CachedAssembly != null))
			{
				return;
			}

			if (!segment.IsEmpty) 
			{
				if (segment.Template.SourceType == ZeusConstants.SourceTypes.ENCRYPTED)
				{
					reader = new StringReader( ZeusEncryption.Decrypt(segment.CodeUnparsed) );
				}
				else 
				{
					reader = new StringReader(segment.CodeUnparsed);
				}

				builder = new StringBuilder();
				engine = ZeusFactory.GetEngine(segment.Engine);

				if (segment.Mode == ZeusConstants.Modes.MARKUP) 
				{
					ParseMarkup(engine, segment, reader, builder);
				}
				else
				{
					ParsePure(engine, segment, reader, builder);
				}
				segment.Code = builder.ToString();
			}

		}

		#region Parse Markup Code
		protected static void ParseMarkup(IZeusScriptingEngine engine, ZeusCodeSegment segment, StringReader reader, StringBuilder builder) 
		{
			string line, nextline;
			string tagStart =  segment.Template.TagStart, 
				tagSpecial =  segment.Template.TagStartSpecial, 
				tagShortcut =  segment.Template.TagStartShortcut, 
				tagEnd = segment.Template.TagEnd,
				language = segment.Language;
			ArrayList extraData = segment.ExtraData;
			int index;
			bool inBlock = false;
			bool isShortcut = false;
			bool isCustom = false;
			string nextTagToFind = string.Empty;
			IZeusCodeParser codeParser = engine.CodeParser;
            var sb = new StringBuilder();

			int headerInsertIndex = builder.Length;

			line = reader.ReadLine();
			int i = reader.Peek();
			while (line != null)
			{
				nextline = reader.ReadLine();

				index = line.IndexOf(inBlock ? tagEnd : tagStart);
				while (index >= 0) 
				{
					if (inBlock) 
					{
						inBlock = false;

						if (isShortcut) 
						{
                            sb.Append(line.Substring(0, index));

							//TODO: ***If the line in the shortcut has more than one command (a semicolon) throw an exception.
							builder.Append( codeParser.BuildOutputCommand(language, sb.ToString() , false, false) );
                            sb.Clear();

							isShortcut = false;
						}
						else if (isCustom) 
						{
							//TODO: ***If the line in the include has more than one command (a semicolon) throw an exception.
							builder.Append( codeParser.ParseCustomTag(segment, line.Substring(0, index)) );
							
							isCustom = false;
						}
						else
						{
							builder.Append(line.Substring(0, index).Trim() + "\r\n");
						}
						line = line.Substring(index + tagEnd.Length);
					}
					else
					{
						inBlock = true;
						if (index > 0)
						{
                            // Append the code before the open tag
							builder.Append( codeParser.BuildOutputCommand(language, line.Substring(0, index), true, false) );
						}

						if (index == line.IndexOf(tagShortcut)) 
						{
							isCustom = false;
							isShortcut = true;
                            // Set the line to the actual code
							line = line.Substring(index + tagShortcut.Length);
						}
						else if (index == line.IndexOf(tagSpecial)) 
						{
							isCustom = true;
							isShortcut = false;
							line = line.Substring(index + tagSpecial.Length);
						}
						else 
						{
							isCustom = false;
							isShortcut = false;
							line = line.Substring(index + tagStart.Length);
						}
					}

					index = line.IndexOf(inBlock ? tagEnd : tagStart);
				}
				
				// Custom tags have to start and end on the same line!
				isCustom = false;

                if (isShortcut) {
                    sb.Append(line.Trim() + "\r\n");
                }
				else if (inBlock) 
				{
					builder.Append(line.Trim() + "\r\n");
				}
				else 
				{
					if ( !((nextline == null) && (line == string.Empty)) ) 
					{
						builder.Append( codeParser.BuildOutputCommand(language, line, true, true) );
					}
				}

				line = nextline;
			}

			builder.Insert(headerInsertIndex, codeParser.GetCustomHeaderCode(segment, ZeusFactory.IntrinsicObjectsArray));
			builder.Append(codeParser.GetCustomFooterCode(segment, ZeusFactory.IntrinsicObjectsArray));
		}
		#endregion

		#region Parse Pure Code
		protected static void ParsePure(IZeusScriptingEngine engine, ZeusCodeSegment segment, StringReader reader, StringBuilder builder) 
		{
			string line, nextline;
			string tagSpecial = segment.Template.TagStartSpecial, 
				tagEnd = segment.Template.TagEnd,
				language = segment.Language;
			ArrayList extraData = segment.ExtraData;
			bool isGui = (segment.SegmentType == ZeusConstants.CodeSegmentTypes.GUI_SEGMENT);
			int index;
			bool inBlock = false;
			IZeusCodeParser codeParser = engine.CodeParser;

			line = reader.ReadLine();
			int i = reader.Peek();
			while (line != null)
			{
				nextline = reader.ReadLine();

				index = line.IndexOf(inBlock ? tagEnd : tagSpecial);
				while (index >= 0) 
				{
					if (inBlock) 
					{
						inBlock = false;
						builder.Append( codeParser.ParseCustomTag(segment, line.Substring(0, index)) );

						line = line.Substring(index + tagEnd.Length);
					}
					else
					{
						inBlock = true;

						if (index > 0)
						{
							builder.Append(line.Substring(0, index));
						}
						
						line = line.Substring(index + tagSpecial.Length);
					}

					index = line.IndexOf(inBlock ? tagEnd : tagSpecial);
				}
				
				// with pure script mode, tags can NOT span more than one line!
				inBlock = false;
				
				builder.Append(line.Trim() + "\r\n");
				
				line = nextline;
			}

			builder.Insert(0, codeParser.GetCustomHeaderCode(segment, ZeusFactory.IntrinsicObjectsArray));
			builder.Append(codeParser.GetCustomFooterCode(segment, ZeusFactory.IntrinsicObjectsArray));
		}
		#endregion
	}
}
