using System;

namespace Zeus
{
	/// <summary>
	/// Summary description for ZeusConstants.
	/// </summary>
	public class ZeusConstants
	{
		public const string START_DIRECTIVE = "##|";
		public const string DEFAULT_START_TAG = "<%";
		public const string DEFAULT_END_TAG = "%>";
		public const char DEFAULT_SHORTCUT_CHAR = '=';
		public const char DEFAULT_SPECIAL_CHAR = '#';
	
		public class Engines
		{
			public const string DOT_NET_SCRIPT = ".Net Script";
			public const string MICROSOFT_SCRIPT = "Microsoft Script";
		}

		public class Types
		{
			public const string GROUP = "Group";
			public const string TEMPLATE = "Template";
		}

		public class Modes
		{
			public const string MARKUP = "Markup";
			public const string PURE = "Pure";
		}

		public class CodeSegmentTypes
		{
			public const string GUI_SEGMENT = "Gui";
			public const string BODY_SEGMENT = "Body";
		}

		public class SourceTypes
		{
			public const string SOURCE = "Source";
			public const string ENCRYPTED = "Encrypted";
			public const string COMPILED = "Compiled";
		}

		public class Directives
		{
			public const string TYPE = "TYPE";
			public const string UNIQUEID = "UNIQUEID";
			public const string NAMESPACE = "NAMESPACE";
			public const string TITLE = "TITLE";
			public const string SOURCE_TYPE = "SOURCE_TYPE";
			public const string INPUT_VARS = "INPUT_VARIABLES";
			public const string OUTPUT_LANGUAGE = "OUTPUT_LANGUAGE";
			public const string COMMENTS_BEGIN = "COMMENTS_BEGIN";
			public const string COMMENTS_END = "COMMENTS_END";
			public const string GUI_ENGINE = "GUI_ENGINE";
			public const string GUI_LANGUAGE = "GUI_LANGUAGE";
			public const string GUI_BEGIN = "GUI_BEGIN";
			public const string GUI_END = "GUI_END";
			public const string BODY_ENGINE = "BODY_ENGINE";
			public const string BODY_LANGUAGE = "BODY_LANGUAGE";
			public const string BODY_MODE = "BODY_MODE";
			public const string BODY_TAG_START = "BODY_TAG_START";
			public const string BODY_TAG_END = "BODY_TAG_END";
			public const string BODY_BEGIN = "BODY_BEGIN";
			public const string BODY_END = "BODY_END";
			public const string INCLUDE_TEMPLATE = "TEMPLATE_INCLUDE";
		}

		public class Languages
		{
			public const string NONE = "None";
			public const string VBSCRIPT = "VBScript";
			public const string JSCRIPT = "JScript";
			public const string CSHARP = "C#";
			public const string VBNET = "VB.Net";
			public const string JSCRIPTNET = "JScript.Net";
			public const string JSHARP = "J#";
			public const string JAVA = "Java";
			public const string VB = "Visual Basic";
			public const string XML = "XML";
			public const string HTML = "HTML";
			public const string SQL = "SQL";
			public const string TSQL = "Transact-SQL";
			public const string PLSQL = "PL/SQL";
			public const string JETSQL = "Jet SQL";
			public const string PERL = "Perl";
			public const string PHP = "PHP";	
		}
	}
}
