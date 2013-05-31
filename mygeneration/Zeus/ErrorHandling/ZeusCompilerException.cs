using System;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace Zeus.ErrorHandling
{
	/// <summary>
	/// Summary description for ZeusCompilerException.
	/// </summary>
	public class ZeusCompilerException : ZeusException
	{
		protected CompilerErrorCollection _errors;
		private bool _isTemplateScript = true;

		public ZeusCompilerException(CompilerErrorCollection errors, string message) : base(message)
		{
			this._errors = errors;
		}

		public CompilerErrorCollection Errors 
		{
			get 
			{
				return _errors;
			}
		}

		public bool IsTemplateScript
		{
			get 
			{
				return _isTemplateScript;
			}
			set 
			{
				_isTemplateScript = value;
			}
		}

	}
} 
