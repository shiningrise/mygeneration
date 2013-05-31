using System;
using System.Text;
using System.IO;

using Zeus;

namespace Zeus.MicrosoftScript
{
	/// <summary>
	/// Summary description for DotNetScriptEngine.
	/// </summary>
	public class MicrosoftScriptEngine : IZeusScriptingEngine
	{
		protected MicrosoftScriptCodeParser _microsoftScriptCodeParser = null;
		protected MicrosoftScriptExecutioner _microsoftScriptExecutioner = null;

		public const string MICROSOFT_SCRIPT = "Microsoft Script";
		public const string JSCRIPT = "JScript";
		public const string VBSCRIPT = "VBScript";

		protected string[] _supportedLanguages = new string[] 
			   { 
				   JSCRIPT, 
				   VBSCRIPT
			   };

		public MicrosoftScriptEngine() {}

		public string EngineName
		{
			get { return MICROSOFT_SCRIPT; }
		}

		public IZeusCodeParser CodeParser
		{
			get 
			{
				if (_microsoftScriptCodeParser == null) 
				{
					_microsoftScriptCodeParser = new MicrosoftScriptCodeParser(this);
				}
				return _microsoftScriptCodeParser;
			}
		}

		public IZeusExecutionHelper ExecutionHelper
		{
			get 
			{
				if (_microsoftScriptExecutioner == null) 
				{
					_microsoftScriptExecutioner = new MicrosoftScriptExecutioner(this);
				}
				return _microsoftScriptExecutioner;
			}
		}

		public string[] SupportedLanguages
		{
			get { return _supportedLanguages; }
		}

		public bool IsSupportedLanguage(string language) 
		{
			foreach (string lang in _supportedLanguages) 
			{
				if (lang == language) return true;
			}
			return false;
		}

		public bool Execute(IZeusInput input, IZeusOutput output, string code)
		{
			return false;
		}

		#region Default text for a new template
		public string GetNewTemplateText(string language)
		{
			return string.Empty;
		}

		public string GetNewGuiText(string language)
		{
			return string.Empty;
		}
		#endregion

		#region Static File Methods
		public static string MakeAbsolute(string pathToChange, string basePath) 
		{
			string newPath = pathToChange;

			DirectoryInfo dinfo = new DirectoryInfo(basePath);
			string p1 = dinfo.FullName;
			if (!p1.EndsWith("\\")) p1 += "\\";

			if (dinfo.Exists) 
			{
				if (pathToChange.StartsWith("\\"))
				{
					newPath = dinfo.Root + pathToChange;
				}
				else if ((pathToChange.StartsWith(".")) ||
					(pathToChange.IndexOf(":") == -1))
				{
					newPath = p1 + pathToChange;
				}

				FileInfo finfo = new FileInfo(newPath);
				if (finfo.Exists) 
				{
					newPath = finfo.FullName;
				}
				else
				{
					finfo = new FileInfo(pathToChange);
					if (finfo.Exists) 
					{
						newPath = finfo.FullName;
					}
				}

			}

			return newPath;
		}
		#endregion

	}
}
