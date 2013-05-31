using System;
using System.Collections;

using Zeus.UserInterface;

namespace Zeus.DotNetScript
{
	public abstract class _DotNetScriptTemplate
	{
		protected ZeusTemplateContext context;
		protected ZeusInput input;
		protected ZeusOutput output;
		protected Hashtable objects;

		public _DotNetScriptTemplate(ZeusTemplateContext context)
		{
			this.input = context.Input;
			this.objects = context.Objects;
			this.output = context.Output;
		}

		public abstract void Render();
	}

}

