using System;
using System.Collections;
using System.Collections.Specialized;

namespace Zeus.UserInterface
{
	/// <summary>
	/// A Button input control used to trigger click events on the form. 
	/// </summary>
	/// <remarks>
	/// A Button input control used to trigger click events on the form. 
	/// </remarks>
	/// <example>
	/// Attaching the onclick event (jscript)
	/// <code>
	/// var btnTest = ui.AddButton("btnTest", "Execute Test", "Execute Test Click?");
	/// btnTest.AttachEvent("onclick", "btnTest_onclick");
	/// </code>
	/// </example>
	public class GuiButton : GuiControl, IGuiButton
	{
		private string _text = string.Empty;
		private int _hieght = 24;
		private bool _closesForm = false;
		private bool _cancelGeneration = false;
		private ArrayList _onclickEvents = new ArrayList();

		/// <summary>
		/// Creates a new instance of a GuiButton control.
		/// </summary>
		public GuiButton() 
		{
			this.IsDataControl = false;
		}

		/// <summary>
		/// Returns the text of the control.
		/// </summary>
		public override object Value 
		{
			get { return this.Text; }
		}
	
		/// <summary>
		/// Returns the text of the control.
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
		/// Sets or gets the height of the control.
		/// </summary>
		public override int Height 
		{
			get
			{
				return this._hieght;
			}
			set 
			{
				this._hieght = value;
			}
		}

		/// <summary>
		/// If set to true, this button will close the input form and the template will be rendered.
		/// </summary>
		public bool ClosesForm 
		{
			get
			{
				return this._closesForm;
			}
			set 
			{
				this._closesForm = value;
			}
		}

		public bool CancelGeneration 
		{
			get
			{
				return this._cancelGeneration;
			}
			set 
			{
				this._cancelGeneration = value;
			}
		}

		/// <summary>
		/// Attaches the functionName to the eventType event.  
		/// The GuiButton supports the onclick event.
		/// </summary>
		/// <param name="eventType">The type of event to attach.</param>
		/// <param name="functionName">Name of the function to be called when the event fires.
		/// The functionName should have the signature in the Gui code: 
		/// "eventHandlerFunctionName(GuiButton control)"</param>
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

