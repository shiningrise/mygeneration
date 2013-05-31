using System;namespace Zeus.UserInterface{	public interface IGuiLabel : IGuiControl	{		string Text { get; set; }
		bool Bold { get; set; }
		bool Underline { get; set; }
		bool Italic { get; set; }
		bool Strikeout { get; set; }	}}