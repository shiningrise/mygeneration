using System;
using System.IO;
using System.Collections;

namespace Zeus.DotNetScript
{
	/// <summary>
	/// Summary description for DotNetScriptParser.
	/// </summary>
	public class DotNetScriptCodeParser : IZeusCodeParser
	{
		protected const string INCLUDE_FILE = "FILE ";
		protected const string INCLUDE_REFERENCE = "REFERENCE ";
		protected const string INCLUDE_REFERENCE_ALT = "REF ";
		protected const string INCLUDE_NAMESPACE = "NAMESPACE ";
        protected const string INCLUDE_NAMESPACE_ALT = "NS ";
        protected const string INCLUDE_DEBUG = "DEBUG";
        protected const string COMPILER_VERSION = "COMPILERVERSION";
        protected const string COMPILER_VERSION_ALT = "VERSION";
		protected const string WRITE = "output.write";
		protected const string WRITELN = "output.writeln";

		protected string[,] _csReplaceChars = new string[5, 2] { {"\\", "\\\\"}, {"\"", "\\\""}, {"\r", "\\r"}, {"\n", "\\n"}, {"\t", "\\t"} };
		protected string[,] _vbReplaceChars = new string[4, 2] { {"\"", "\"\""}, {"\r\n", "\" & vbcrlf & \""}, {"\n", "\" & vbcrlf & \""}, {"\r", "\" & vbcrlf & \""} };
		protected DotNetScriptEngine _engine;

		public DotNetScriptCodeParser(DotNetScriptEngine engine) 
		{
			_engine = engine;
		}
		
		public string ParseCustomTag(IZeusCodeSegment segment, string text)
		{
			ArrayList extraData = segment.ExtraData;
			string data = null, path = null;
			string returnValue = string.Empty;

			if (text.StartsWith(INCLUDE_FILE))
			{
				data = text.Substring(INCLUDE_FILE.Length).Trim();
				path = DotNetScriptEngine.MakeAbsolute(data, segment.ITemplate.FilePath);
				returnValue = this.IncludeFile(path);
            }
            else if (text.StartsWith(COMPILER_VERSION))
            {
                data = text.Substring(COMPILER_VERSION.Length).Trim();
                this.SetCompilerVersion(data, extraData);
            }
            else if (text.StartsWith(COMPILER_VERSION_ALT))
            {
                data = text.Substring(COMPILER_VERSION_ALT.Length).Trim();
                this.SetCompilerVersion(data, extraData);
            }
			else if (text.StartsWith(INCLUDE_REFERENCE))
			{
				data = text.Substring(INCLUDE_REFERENCE.Length).Trim();
				this.IncludeReference(data, extraData);
			}
			else if (text.StartsWith(INCLUDE_REFERENCE_ALT))
			{
				data = text.Substring(INCLUDE_REFERENCE_ALT.Length).Trim();
				this.IncludeReference(data, extraData);
			}
			else if (text.StartsWith(INCLUDE_NAMESPACE))
			{
				data = text.Substring(INCLUDE_NAMESPACE.Length).Trim();
				this.IncludeNamespace(data, extraData);
			}
			else if (text.StartsWith(INCLUDE_NAMESPACE_ALT))
			{
				data = text.Substring(INCLUDE_NAMESPACE_ALT.Length).Trim();
				this.IncludeNamespace(data, extraData);
			}
			else if (text.StartsWith(INCLUDE_DEBUG))
			{
				extraData.Add(new string[2] { DotNetScriptEngine.DEBUG, Boolean.TrueString });
			}
			
			return returnValue;
		}

		public string EscapeLiteral(string language, string text)
		{
			string[,] replaceChars;
			string escapedString = text;

			if (language == ZeusConstants.Languages.VBNET)
			{
				replaceChars = this._vbReplaceChars;
			}
			else
			{
				replaceChars = this._csReplaceChars;
			}
			
			for (int i = 0; i < (replaceChars.Length / 2); i++) 
			{
				escapedString = escapedString.Replace(replaceChars[i, 0], replaceChars[i, 1]);
			}

			return "\"" + escapedString + "\"";
		}

		public string BuildOutputCommand(string language, string text, bool isLiteral, bool addNewLine)
		{
			string cmd = (addNewLine ? WRITELN : WRITE);
			if (language == ZeusConstants.Languages.VBNET)
			{
				if (isLiteral) 
				{
					cmd += "(" + EscapeLiteral(language, text) + ")\r\n";
				}
				else 
				{
					cmd += "(" + text + ")\r\n";
				}
			}
			else 
			{
				if (isLiteral) 
				{
					cmd += "(" + EscapeLiteral(language, text) + ");\r\n";
				}
				else 
				{
					cmd += "(" + text + ");\r\n";
				}
			}
			return cmd;
		}

		public string GetCustomHeaderCode(IZeusCodeSegment segment, IZeusIntrinsicObject[] iobjs) 
		{
			bool isGui = (segment.SegmentType == ZeusConstants.CodeSegmentTypes.GUI_SEGMENT);
			ArrayList imports = new ArrayList();

			imports.Add("System");
			imports.Add("System.Collections");
			imports.Add("Zeus");
			imports.Add("Zeus.Data");
			imports.Add("Zeus.DotNetScript");
			if (isGui) 
			{
					imports.Add("Zeus.UserInterface");
            }

            foreach (string tns in Zeus.Configuration.ZeusConfig.Current.TemplateNamespaces)
            {
                if (!imports.Contains(tns)) imports.Add(tns);
            }

			foreach (IZeusIntrinsicObject obj in iobjs) 
			{
                if (!obj.Disabled)
                {
                    if (obj.Namespace != null)
                    {
                        if (!imports.Contains(obj.Namespace))
                            imports.Add(obj.Namespace);
                    }
                }
			}

			ArrayList tmpExtraData = segment.ExtraData;
			string[] array;
			foreach (object obj in tmpExtraData) 
			{
				if (obj is String[]) 
				{
					array = (string[])obj;
					if ((array.Length == 2) && (array[0] == DotNetScriptEngine.USE_NAMESPACE))
					{
						if (!imports.Contains(array[1]))
							imports.Add(array[1]);
					}
				}
			}

			return this._engine.BuildImportStatments(segment.Language, imports);
		}

		public string GetCustomFooterCode(IZeusCodeSegment segment, IZeusIntrinsicObject[] iobjs) 
		{
			if (segment.SegmentType == ZeusConstants.CodeSegmentTypes.GUI_SEGMENT) 
			{
				return this._engine.BuildGuiClass(segment.Language, iobjs);
			}
			else
			{
				return this._engine.BuildBodyClass(segment.Language, iobjs);
			}
		}

		private string IncludeFile(string filename) 
		{
			string returnval = string.Empty;
			if (File.Exists(filename)) 
			{
				StreamReader reader = File.OpenText(filename);
				returnval = reader.ReadToEnd();
				reader.Close();
			}
			else 
			{
				//TODO: Could throw an error here.
			}

			return returnval;
        }

        private void SetCompilerVersion(string ver, ArrayList extraData)
        {
            ver = ver.Trim();
            if (ver.Trim() != string.Empty)
            {
                extraData.Add(new string[2] { DotNetScriptEngine.VERSION, ver });
            }
        }

        private void IncludeReference(string dllrefs, ArrayList extraData)
        {
            string[] refs = dllrefs.Split(',');

            foreach (string r in refs)
            {
                string dllref = r.Trim();
                if (dllref != string.Empty)
                {
                    extraData.Add(new string[2] { DotNetScriptEngine.DLLREF, dllref });
                }
            }
        }

		private void IncludeNamespace(string namespaceRefs, ArrayList extraData) 
		{
			string[] array = namespaceRefs.Split(',');

			foreach (string ns in array) 
			{
				string nsTrimmed = ns.Trim();
				if (nsTrimmed != string.Empty) 
				{
					extraData.Add(new string[2] { DotNetScriptEngine.USE_NAMESPACE, nsTrimmed } );
				}
			}
		}
	}
}
