using System;

namespace Zeus.ErrorHandling
{
	/// <summary>
	/// Summary description for Zeusception.
	/// </summary>
	public class ZeusException : Exception
	{
        private IZeusTemplate _template;

        public ZeusException(IZeusTemplate template, string message)
            : base(message)
		{
            this._template = template;
		}

        public ZeusException(IZeusTemplate template, string message, Exception exception)
            : base(message, exception)
        {
            this._template = template;
        }

        public IZeusTemplate Template
        {
            get
            {
                return this._template;
            }
        }
	}
}