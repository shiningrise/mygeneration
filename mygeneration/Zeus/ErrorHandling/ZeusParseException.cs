using System;

namespace Zeus.ErrorHandling
{
	public enum ZeusParseError
	{
		InvalidScriptingLanguage = 0,
		InvalidOutputLanguage,
		OutdatedTemplateStructure,
		InvalidTemplateMode
	}

	public class ZeusParseException : ZeusException
	{
		protected ZeusParseError _parseError;

        public ZeusParseException(IZeusTemplate template, ZeusParseError parseError, string message)
            : base(template, message)
		{
			this._parseError = parseError;
		}

		public override string Message
		{
			get
			{
				return base.Message;
			}
		}

		public ZeusParseError ParseError
		{
			get
			{
				return _parseError;
			}
		}
	}
}