using System;
using System.Collections.Generic;

namespace Zeus.UserInterface
{
	/// <summary>
	/// A TextBox input control for attaining textual input from the user. 
	/// </summary>
	/// <remarks>
	/// A TextBox input control for attaining textual input from the user.
	/// </remarks>
	/// <example>
	/// Adding the GuiTextBox to the GuiController (jscript)
	/// <code>
	/// var txtPath = ui.AddTextBox("txtPath", sOutputPath, "Select the Output Path.");
	/// </code>
	/// </example>
	public class GuiTextBox : GuiControl, IGuiTextBox
	{
		private string _text = string.Empty;
		private bool _wordWrap = false;
		private bool _multiline = false;
		private bool _verticalScroll = false;
        private bool _horizontalScroll = false;
        private List<string> _onkeypressEvents = new List<string>();

		/// <summary>
		/// Creates a new instance of a GuiTextBox control.
		/// </summary>
		public GuiTextBox() {}

		/// <summary>
		/// Returns the text in the textbox.
		/// </summary>
		public override object Value 
		{
			get { return this.Text; }
		}

		/// <summary>
		/// Returns the text in the textbox.
		/// </summary>
		public string Text 
		{
			get { return this._text; }
			set { this._text = value; }
		}

		/// <summary>
		/// Enables or disables wordwrap
		/// </summary>
		public bool WordWrap 
		{
			get { return this._wordWrap; }
			set { this._wordWrap = value; }
		}

		/// <summary>
		/// Enables or disables multi-line functionality
		/// </summary>
		public bool Multiline 
		{
			get { return this._multiline; }
			set { this._multiline = value; }
		}

		/// <summary>
		/// If it's a Multiline textbox, enable Vertical Scrolling
		/// </summary>
		public bool VerticalScroll 
		{
			get { return this._verticalScroll; }
			set { this._verticalScroll = value; }
		}

		/// <summary>
		/// If it's a Multiline textbox, enable Horizontal Scrolling
		/// </summary>
		public bool HorizontalScroll 
		{
			get { return this._horizontalScroll; }
			set { this._horizontalScroll = value; }
        }

        /// <summary>
        /// Attaches the functionName to the eventType event.  
        /// The GuiFilePicker supports the onselect event.
        /// </summary>
        /// <param name="eventType">The type of event to attach.</param>
        /// <param name="functionName">Name of the function to be called when the event fires.
        /// The functionName should have the signature in the Gui code: 
        /// "eventHandlerFunctionName(GuiFilePicker control)"</param>
        public override void AttachEvent(string eventType, string functionName)
        {
            if (eventType.ToLower() == "onkeypress")
            {
                this._onkeypressEvents.Add(functionName);
            }
            else
            {
                base.AttachEvent(eventType, functionName);
            }
        }

        /// <summary>
        /// This returns true if there are any event handlers for the passed in eventType.
        /// </summary>
        /// <param name="eventType">The type of event to check for.</param>
        /// <returns>Returns true if there are any event handlers of the given type.</returns>
        public override bool HasEventHandlers(string eventType)
        {
            if (eventType.ToLower() == "onkeypress")
            {
                return (this._onkeypressEvents.Count > 0);
            }
            else
            {
                return base.HasEventHandlers(eventType);
            }
        }

        /// <summary>
        ///  This returns all of the event handler functions that have been assigned 
        ///  to this object for the passed in eventType. 
        /// </summary>
        /// <param name="eventType">The type of event to return function names for.</param>
        /// <returns>All of the event handler functions that have been assigned 
        ///  to this object for the given eventType</returns>
        public override string[] GetEventHandlers(string eventType)
        {
            if (eventType.ToLower() == "onkeypress")
            {
                return this._onkeypressEvents.ToArray();
            }
            else
            {
                return base.GetEventHandlers(eventType);
            }
        }
	}
}

