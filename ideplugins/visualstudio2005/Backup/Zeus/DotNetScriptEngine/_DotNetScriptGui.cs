using System;
using System.Collections;

namespace Zeus.DotNetScript
{
	/// <summary>
	/// Summary description for DotNetScriptUI.
	/// </summary>
	public abstract class _DotNetScriptGui
	{
		protected IZeusContext context;
		protected IZeusInput input;
		protected Hashtable objects;

		public _DotNetScriptGui(IZeusContext context)
		{
			this.context = context;
			this.input = context.Input;
			this.objects = context.Objects;
		}

		public abstract void Setup();
	}
}
