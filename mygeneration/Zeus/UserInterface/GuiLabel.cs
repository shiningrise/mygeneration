using System;

namespace Zeus.UserInterface
{
	/// <summary>
	/// A label control for painting text onto the form. 
	/// </summary>
	/// <remarks>
	/// A label control for painting text onto the form.
	/// </remarks>
	/// <example>
	/// Adding the GuiLabel to the GuiController (jscript)
	/// <code>
	/// var lblPath = ui.AddLabel("lblPath", "Select the output path:", "Select the output path in the field below.");
	/// </code>
	/// </example>
	public class GuiLabel : GuiControl, IGuiLabel
	{
		private string _text = string.Empty;
		private bool _bold = false;
		private bool _underline = false;
		private bool _strikeout = false;
		private bool _italic = false;

		/// <summary>
		/// Creates a new instance of a GuiLabel control.
		/// </summary>
		public GuiLabel() 
		{
			this.IsDataControl = false;
		}

		/// <summary>
		/// Returns the text of the label.
		/// </summary>
		public override object Value 
		{
			get { return this.Text; }
		}

		/// <summary>
		/// Gets or sets the text of the label.
		/// </summary>
		public string Text 
		{
			get { return this._text; }
			set { this._text = value; }
		}

		/// <summary>
		/// Make the label text bold.
		/// </summary>
		public bool Bold 
		{
			get { return this._bold; }
			set { this._bold = value; }
		}

		/// <summary>
		/// Underline the label text.
		/// </summary>
		public bool Underline 
		{
			get { return this._underline; }
			set { this._underline = value; }
		}

		/// <summary>
		/// Make the label text italic.
		/// </summary>
		public bool Italic 
		{
			get { return this._italic; }
			set { this._italic = value; }
		}

		/// <summary>
		/// Strikeout the label text.
		/// </summary>
		public bool Strikeout 
		{
			get { return this._strikeout; }
			set { this._strikeout = value; }
		}
	}
}

