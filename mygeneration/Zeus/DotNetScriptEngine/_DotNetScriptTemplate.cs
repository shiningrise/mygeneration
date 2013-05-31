using System;
using System.Collections;

//using Zeus.UserInterface;

namespace Zeus.DotNetScript
{
	public abstract class _DotNetScriptTemplate
	{
		protected IZeusContext context;
		protected IZeusInput input;
		protected IZeusOutput output;
		protected Hashtable objects;

		public _DotNetScriptTemplate(IZeusContext context)
		{
			this.context = context;
			this.input = context.Input;
			this.objects = context.Objects;
			this.output = context.Output;
		}

		public abstract void Render();
	}
}

