using System;
using System.Collections;

using Zeus.UserInterface;

namespace Zeus.DotNetScript
{
	/// <summary>
	/// Summary description for DotNetScriptUI.
	/// </summary>
	public abstract class _DotNetScriptGui
	{
		protected ZeusGuiContext context;
		protected ZeusInput input;
		protected Hashtable objects;
		protected GuiController ui;

		public _DotNetScriptGui(ZeusGuiContext context)
		{
			this.input = context.Input;
			this.objects = context.Objects;
			this.ui = context.Gui;
		}

		public abstract void Setup();
	}
}
