using System;
using System.Collections;
using System.Collections.Specialized;

namespace Zeus.UserInterface
{
	/// <summary>
	/// A button that pops up a file or folder selction dialog and outputs the selected path 
	/// into a target textbox control. 
	/// </summary>
	/// <remarks>
	/// A button that pops up a file or folder selction dialog and outputs the selected path 
	/// into a target textbox control. 
	/// </remarks>
	/// <example>
	/// Using the GuiFilePicker and attaching the onselect event (jscript)
	/// <code>
	/// var txtPath = ui.AddTextBox("txtPath", sOutputPath, "Select the Output Path.");
	///
	/// var btnPath = ui.AddFilePicker("btnPath", "Select Path", "Select the Output Path.", "txtPath", true);
	/// btnPath.AttachEvent("onselect", "btnPath_onselect");
	/// </code>
	/// </example>
	public class GuiFilePicker : GuiControl, IGuiFilePicker
	{
		private string _text = string.Empty;
		private string _itemdata = string.Empty;
		private string _targetcontrol = string.Empty;
		private int _hieght = 24;
		private bool _picksFolder = false;
		private ArrayList _onselectEvents = new ArrayList();

		/// <summary>
		/// Creates a new instance of a GuiFilePicker control.
		/// </summary>
		public GuiFilePicker() {}

		/// <summary>
		/// Returns the text on the FilePicker Button.
		/// </summary>
		public override object Value 
		{
			get { return this.ItemData; }
		}
	
		/// <summary>
		/// Returns the text on the FilePicker Button.
		/// </summary>
		public string Text 
		{
			get { return this._text; }
			set { this._text = value; }
		}

		/// <summary>
		/// Returns the last seleted path/file
		/// </summary>
		public string ItemData
		{
			get { return this._itemdata; }
			set { this._itemdata = value; }
		}

		/// <summary>
		/// The id of the TextBox that the path should be placed in after selection.
		/// </summary>
		public string TargetControl
		{
			get { return this._targetcontrol; }
			set { this._targetcontrol = value; }
		}

		/// <summary>
		/// Sets or gets the height of the control.
		/// </summary>
		public override int Height 
		{
			get { return this._hieght; }
			set { this._hieght = value; }
		}

		/// <summary>
		/// Set to true if you want to select a folder, set to false if you want to select a file.
		/// </summary>
		public bool PicksFolder 
		{
			get { return this._picksFolder; }
			set { this._picksFolder = value; }
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
			if (eventType.ToLower() == "onselect")
			{
				this._onselectEvents.Add(functionName);
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
			if (eventType == "_onselectEvents") 
			{
				return (this._onselectEvents.Count > 0);
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
			if (eventType == "onselect") 
			{
				return this._onselectEvents.ToArray(typeof(String)) as String[];
			}
			else
			{
				return base.GetEventHandlers(eventType);
			}
		}
	}
}

