using System;
using System.Collections;
using System.Collections.Specialized;

namespace Zeus.UserInterface
{
	/// <summary>
	/// A CheckBox input control used to gather boolean input. 
	/// </summary>
	/// <remarks>
	/// A CheckBox input control used to gather boolean input. 
	/// </remarks>
	/// <example>
	/// Attaching the onclick event (jscript)
	/// <code>
	/// var chkBox = ui.AddCheckBox("chkBox", "Write Proc's to the Access Database", false, "Checking this will cause the stored procedures to be created in your database");
	/// chkBox.AttachEvent("onclick", "chkBox_onclick");
	/// </code>
	/// </example>
	public class GuiCheckBox : GuiControl, IGuiCheckBox
	{
		private bool _value = false;
		private string _text = string.Empty;
		private ArrayList _onclickEvents = new ArrayList();

		/// <summary>
		/// Creates a new instance of a GuiCheckBox control.
		/// </summary>
		public GuiCheckBox() {}

		/// <summary>
		/// Returns the the value of the checkbox (true/false).
		/// </summary>
		public override object Value 
		{
			get
			{
				return this.Checked;
			}
		}
		
		/// <summary>
		/// Returns the text label attached to the checkbox.
		/// </summary>
		public string Text 
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
			}
		}

		/// <summary>
		/// Returns the the value of the checkbox (true/false)
		/// </summary>
		public bool Checked
		{
			get
			{
				return this._value;
			}
			set 
			{
				this._value = value;
			}
		}

		/// <summary>
		/// Attaches the functionName to the eventType event.  
		/// The GuiCheckBox supports the onclick event.
		/// </summary>
		/// <param name="eventType">The type of event to attach.</param>
		/// <param name="functionName">Name of the function to be called when the event fires.
		/// The functionName should have the signature in the Gui code: 
		/// "eventHandlerFunctionName(GuiCheckBox control)"</param>
		public override void AttachEvent(string eventType, string functionName)
		{
			if (eventType.ToLower() == "onclick")
			{
				this._onclickEvents.Add(functionName);
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
			if (eventType == "onclick") 
			{
				return (this._onclickEvents.Count > 0);
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
			if (eventType == "onclick") 
			{
				return this._onclickEvents.ToArray(typeof(String)) as String[];
			}
			else
			{
				return base.GetEventHandlers(eventType);
			}
		}
	}
}
