using System;
using System.Collections.Generic;

namespace Zeus.UserInterface
{
	/// <summary>
	/// Each of the supported controls (GuiController, GuiTextbox, GuiComboBox, GuiLabel, GuiButton, etc) 
	/// inherit from GuiControl.
	/// </summary>
	[Serializable()]
	public abstract class GuiControl : IGuiControl
	{
		protected string _id = string.Empty;
		protected string _tooltip = string.Empty;
		protected string _foreColor = "Black";
		protected string _backColor = string.Empty;
		private int _hieght = 16;
		private int _width = 300;
		private int _top = 10;
		private int _left = 10;
		private bool _visible = true;
		private bool _enabled = true;
		private bool _isDataControl = true;
        private int _tabStripIndex = 0;
        private List<string> _onblurEvents = new List<string>();
        private List<string> _onfocusEvents = new List<string>();
        

		/// <summary>
		/// Sets or gets the controls ID.
		/// </summary>
		public virtual string ID 
		{
			get { return _id; } 
			set { _id = value; }
		}

		/// <summary>
		/// If this control stores input data, this will be true.
		/// </summary>
		public virtual bool IsDataControl 
		{
			get { return _isDataControl; }
			set { _isDataControl = value; }
		}

		/// <summary>
		/// Sets or gets the controls tooltip.
		/// </summary>
		public virtual string ToolTip 
		{
			get { return _tooltip; } 
			set { _tooltip = value; }
		}

		/// <summary>
		/// The control is invisible if this is false.
		/// </summary>
		public virtual bool Visible 
		{
			get { return _visible; }
			set { _visible = value; }
		}

		/// <summary>
		/// True if enabled, false if disabled.
		/// </summary>
		public virtual bool Enabled 
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		/// <summary>
		/// Sets or gets the controls ForeColor.
		/// </summary>
		public virtual string ForeColor 
		{
			get { return _foreColor; }
			set { _foreColor = value; }
		}

		/// <summary>
		/// Sets or gets the controls BackColor.
		/// </summary>
		public virtual string BackColor 
		{
			get { return _backColor; }
			set { _backColor = value; }
		}

		/// <summary>
		/// Returns the value of the control.
		/// </summary>
		public abstract object Value 
		{
			get;
		}

		/// <summary>
		/// Sets or gets the width of the control.
		/// </summary>
		public virtual int Width 
		{
			get { return this._width; }
			set { this._width = value; }
		}

		/// <summary>
		/// Sets or gets the height of the control.
		/// </summary>
		public virtual int Height 
		{
			get { return this._hieght; }
			set { this._hieght = value; }
		}

		/// <summary>
		/// Sets or gets the top coordinate of the control.
		/// </summary>
		public virtual int Top 
		{
			get { return this._top; }
			set { this._top = value; }
		}

		/// <summary>
		/// Sets or gets the left coordinate of the control.
		/// </summary>
		public virtual int Left
		{
			get { return this._left; }
			set { this._left = value; }
		}

		/// <summary>
		/// The tabstrip index this tab is assigned to
		/// </summary>
		internal int TabStripIndex
		{
			get { return this._tabStripIndex; }
			set { this._tabStripIndex = value; }
		}



        /// <summary>
        /// Attaches the functionName to the eventType event.  
        /// The GuiFilePicker supports the onselect event.
        /// </summary>
        /// <param name="eventType">The type of event to attach.</param>
        /// <param name="functionName">Name of the function to be called when the event fires.
        /// The functionName should have the signature in the Gui code: 
        /// "eventHandlerFunctionName(GuiFilePicker control)"</param>
        public virtual void AttachEvent(string eventType, string functionName)
        {
            if (eventType.ToLower() == "onblur")
            {
                this._onblurEvents.Add(functionName);
            }
            else if (eventType.ToLower() == "onfocus")
            {
                this._onfocusEvents.Add(functionName);
            }
        }

        /// <summary>
        /// This returns true if there are any event handlers for the passed in eventType.
        /// </summary>
        /// <param name="eventType">The type of event to check for.</param>
        /// <returns>Returns true if there are any event handlers of the given type.</returns>
        public virtual bool HasEventHandlers(string eventType)
        {
            if (eventType.ToLower() == "onblur")
            {
                return (this._onblurEvents.Count > 0);
            }
            else if (eventType.ToLower() == "onfocus")
            {
                return (this._onfocusEvents.Count > 0);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///  This returns all of the event handler functions that have been assigned 
        ///  to this object for the passed in eventType. 
        /// </summary>
        /// <param name="eventType">The type of event to return function names for.</param>
        /// <returns>All of the event handler functions that have been assigned 
        ///  to this object for the given eventType</returns>
        public virtual string[] GetEventHandlers(string eventType)
        {
            if (eventType.ToLower() == "onblur")
            {
                return this._onblurEvents.ToArray();
            }
            else if (eventType.ToLower() == "onfocus")
            {
                return this._onfocusEvents.ToArray();
            }
            else
            {
                return new string[0];
            }
        }
	}
}