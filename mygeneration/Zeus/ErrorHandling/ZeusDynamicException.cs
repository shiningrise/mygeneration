using System;

namespace Zeus.ErrorHandling
{
	public enum ZeusDynamicExceptionType 
	{
		ScriptingEnginePluginInvalid = 0,
		IntrinsicObjectPluginInvalid
	}

	/// <summary>
	/// Summary description for ZeusDynamicException.
	/// </summary>
	/// 
	public class ZeusDynamicException : ZeusException
	{
		protected ZeusDynamicExceptionType _exceptionType = ZeusDynamicExceptionType.ScriptingEnginePluginInvalid;

        public ZeusDynamicException(ZeusDynamicExceptionType type, string message)
            : base(null, message)
		{
			this._exceptionType = type;
		}

		public ZeusDynamicExceptionType DynamicExceptionType 
		{
			get 
			{
				return _exceptionType;
			}
		}

		public string DynamicExceptionTypeString 
		{
			get 
			{
				string val = string.Empty;

				switch (_exceptionType) 
				{
					case ZeusDynamicExceptionType.IntrinsicObjectPluginInvalid:
						val = "Intrinsic Object Definition Invalid";
						break;
					case ZeusDynamicExceptionType.ScriptingEnginePluginInvalid:
						val = "Scripting Engine Plugin Invalid";
						break;
				}

				return val;
			}
		}
	}
}
