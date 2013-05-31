using System;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace Zeus.ErrorHandling
{
	/// <summary>
    /// Summary description for ZeusRuntimeException.
	/// </summary>
	public class ZeusRuntimeException : ZeusException
    {
        private bool _isTemplateScript = true;

        public ZeusRuntimeException(IZeusTemplate template, Exception exception, bool isTemplateScript)
            : base(template, "Template Runtime Exception", exception) 
        {
            this._isTemplateScript = isTemplateScript;
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
