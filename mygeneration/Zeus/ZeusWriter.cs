using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Zeus
{
	/// <summary>
	/// Summary description for ZeusWriter.
	/// </summary>
#if ENTERPRISE
	using System.Runtime.InteropServices;
	using System.EnterpriseServices;
	[GuidAttribute("C8058E5A-FD98-4f42-865A-01BE7736DE27"), ClassInterface(ClassInterfaceType.AutoDual)]
	public class ZeusWriter : ServicedComponent
#else
	public class ZeusWriter 
#endif 
	{

        public static void UpdateTemplateProperty(string filePath, string key, string newValue)
        {
            bool isUpdated = false;
            string newl = ZeusConstants.START_DIRECTIVE + key + " " + newValue;
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string l = lines[i];
                if (l.StartsWith(ZeusConstants.START_DIRECTIVE + key))
                {
                    lines[i] = newl;
                    isUpdated = true;
                    break;
                }
            }

            if (!isUpdated)
            {
                List<string> newLines = new List<string>(lines);
                for (int i = 0; i < newLines.Count; i++)
                {
                    string l = newLines[i];
                    if (l.StartsWith(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.UNIQUEID))
                    {
                        if (newLines.Count > (i + 1)) newLines.Insert(i + 1, newl);
                        else newLines.Insert(i, newl);
                        isUpdated = true;
                        lines = newLines.ToArray();
                        break;
                    }
                }
            }

            if (isUpdated)
            {
                File.WriteAllLines(filePath, lines);
            }
        }

		public ZeusWriter() {}

		public string Write(ZeusTemplate template) 
		{
			StringWriter writer = new StringWriter();
			BuildTemplateText(writer, template);
			return writer.ToString();
		}

		public void Write(string filepath, ZeusTemplate template) 
		{
			FileStream fs = File.Create(filepath);
			ZeusWriter writer = new ZeusWriter();
			this.Write(fs, template);
			fs.Close();
		}

		protected void Write(Stream stream, ZeusTemplate template) 
		{
			StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
			BuildTemplateText(writer, template);
		}

		protected void BuildTemplateText(TextWriter writer, ZeusTemplate template) 
		{
			writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.TYPE + " " + template.Type);
			writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.UNIQUEID + " " + template.UniqueID);
			writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.TITLE + " " + template.Title);
			writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.NAMESPACE + " " + template.NamespacePathString);
			writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.SOURCE_TYPE + " " + template.SourceType);
			writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.OUTPUT_LANGUAGE + " " + template.OutputLanguage);

			string inputvars = template.RequiredInputVariablesString;
			if (inputvars != string.Empty)
			{
				writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.INPUT_VARS + " " + inputvars);
			}

			if (template.HasCommentBlock) 
			{
				writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.COMMENTS_BEGIN);
				writer.WriteLine(template.Comments);
				writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.COMMENTS_END);
			}
			
			writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.GUI_ENGINE + " " + template.GuiSegment.Engine);
			writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.GUI_LANGUAGE + " " + template.GuiSegment.Language);
			if (!template.GuiSegment.IsEmpty)
			{
				writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.GUI_BEGIN);
				writer.WriteLine(template.GuiSegment.CodeUnparsed);
				writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.GUI_END);
			}

			writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.BODY_MODE + " " + template.BodySegment.Mode);
			writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.BODY_ENGINE + " " + template.BodySegment.Engine);
			writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.BODY_LANGUAGE + " " + template.BodySegment.Language);
			if (template.BodySegment.Mode == ZeusConstants.Modes.MARKUP) 
			{
				writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.BODY_TAG_START + " " + template.TagStart);
				writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.BODY_TAG_END + " " + template.TagEnd);
			}
			if (!template.BodySegment.IsEmpty)
			{

				writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.BODY_BEGIN);
				writer.WriteLine(template.BodySegment.CodeUnparsed);
				writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.BODY_END);
			}

			if (template.Type == ZeusConstants.Types.GROUP) 
			{
				foreach (string path in template.IncludedTemplatePaths) 
				{
					writer.WriteLine(ZeusConstants.START_DIRECTIVE + ZeusConstants.Directives.INCLUDE_TEMPLATE + " " + path);
				}
			}

			writer.Flush();
		}
	}
}
