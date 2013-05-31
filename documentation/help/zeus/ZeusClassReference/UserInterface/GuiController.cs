using System;
using System.Collections;

namespace Zeus.UserInterface
{
	/// <summary>
	/// The ui object is only available in the user interface block of the template. 
	/// This object is very similar to a form object in Windows programming. Controls 
	/// are added to a collection of controls and a simple auto-layout algorithm is 
	/// applied. You can customize the layout of the form by editing each of the 
	/// control's top, left, width, and height properties. 
	/// </summary>
	/// <remarks>
	/// The GuiController is an object that generically describes a graphical user interface. 
	/// It is used along to gather input from the user for use in the template body. For example,
	/// You may want to gather a table's name and a tablespace name for the generation
	/// of a data-access object or stored procedure. Below are some examples showing how the
	/// GuiController can be used in the setup function of the interface code segment.
	/// </remarks> 
	/// <example>
	///	VBScript sample of the GuiController (ui)
	///	<code>
	/// ui.Title = "Template Form Title Here"
	/// ui.Width  = 330
	/// ui.Height = 420
	///
	/// ui.AddLabel "lblPath", "Output file path: ",  "Select the output path."
	/// ui.AddTextBox "txtPath", sOutputPath, "Select the Output Path."
	/// ui.AddFilePicker "btnPath", "Select Path", "Select the Output Path.", "txtPath", true
	/// ui.AddLabel "lblDatabases", "Select a database:", "Select a database in the dropdown below."
	///
	/// Set cmbDatabases = ui.AddComboBox("cmbDatabase", "Select a database.")
	/// cmbDatabases.AttachEvent "onchange", "cmbDatabases_onchange"
	/// 
	/// ui.AddLabel "lblTables", "Select tables:", "Select tables from the listbox below."
	/// Set lstTables = ui.AddListBox ("lstTables", "Select tables:")
	/// lstTables.Height = 150
	/// </code>
	/// </example>
	/// <example>
	///	JScript sample of the GuiController (ui)
	///	<code>
	/// ui.Title = "Template Form Title Here"
	/// ui.Width = 350;
	/// ui.Height = 420;
	///
	/// var lblError = ui.AddLabel("lblError", "", "");
	/// lblError.ForeColor = "Red";
	///
	/// ui.AddLabel("lblPath", "Select the output path:", "Select the output path in the field below.");
	/// ui.AddTextBox("txtPath", sOutputPath, "Select the Output Path.");
	/// ui.AddFilePicker("btnPath", "Select Path", "Select the Output Path.", "txtPath", true);
	/// ui.AddLabel("lblDatabases", "Select a database:", "Select a database in the dropdown below.");
	///
	/// var cmbDatabases = ui.AddComboBox("cmbDatabase", "Select a database.");
	/// cmbDatabases.AttachEvent("onchange", "cmbDatabases_onchange");
	///
	/// ui.AddLabel("lblTables", "Select tables:", "Select tables from the listbox below.");
	/// var lstTables = ui.AddListBox("lstTables", "Select tables.");
	/// lstTables.Height = 150;
	/// </code>
	/// </example>
	/// <example>
	///	C# sample of the GuiController (ui)
	///	<code>
	/// ui.Title = "Template Form Title Here";
	/// ui.Width = 340;
	/// ui.Height = 420;
	///
	/// GuiLabel lblError = ui.AddLabel("lblError", "", "");
	/// lblError.ForeColor = "Red";
	///
	/// ui.AddLabel("lblPath", "Select the output path:", "Select the output path in the field below.");
	/// ui.AddTextBox("txtPath", sOutputPath, "Select the Output Path.");
	/// ui.AddFilePicker("btnPath", "Select Path", "Select the Output Path.", "txtPath", true);
	///
	/// ui.AddLabel("lblDatabases", "Select a database:", "Select a database in the dropdown below.");
	/// GuiComboBox cmbDatabases = ui.AddComboBox("cmbDatabase", "Select a database.");
	/// cmbDatabases.AttachEvent("onchange", "cmbDatabases_onchange");
	///
	/// ui.AddLabel("lblTables", "Select tables:", "Select tables from the listbox below.");
	/// GuiListBox lstTables = ui.AddListBox("lstTables", "Select tables.");
	/// lstTables.Height = 80;
	///
	/// ui.AddLabel("lblViews", "Select view:", "Select views from the listbox below.");
	/// GuiListBox lstViews = ui.AddListBox("lstViews", "Select views.");
	/// lstViews.Height = 80;
	/// </code>
	/// </example>
	[Serializable()]
	public class GuiController : GuiControl, IGuiController, IEnumerable
	{
		private const int TAB_WIDTH_OFFSET = 15;
		private const int FORM_WIDTH_OFFSET = 30;

		private Hashtable controls = new Hashtable();
		private ArrayList orderedControls = new ArrayList();
		private int _width = 800;
		private int _height = 600;
		private bool _showGui = false;
		private string _title = "Zeus Dynamic Form";
		private string _startPostion = "centerparent";
		private ArrayList _oncloseEvents = new ArrayList();
		private bool _canceled = false;
		private bool _forceDisplay = false;
		private Hashtable _defaults = null;
		private ArrayList _tabs = new ArrayList();

		/// <summary>
		/// Creates a new GuiController object.
		/// </summary>
		public GuiController() 
		{
			this.Top = Int32.MinValue;
			this.Left = Int32.MinValue;
		}

		/// <summary>
		/// Returns the GuiControl that correlates to the string indexer.
		/// </summary>
		public GuiControl this[string id] 
		{
			get 
			{
				return controls[id] as GuiControl;
			}
		}

		/// <summary>
		/// This stores a set of default values for the controls to use
		/// </summary>
		public Hashtable Defaults
		{
			get 
			{
				if (_defaults == null) 
				{
					_defaults = new Hashtable();
				}
				return _defaults;
			}
		}

		/// <summary>
		/// If this flag is set, we force the GUI to display.
		/// </summary>
		public bool ForceDisplay
		{
			get 
			{
				return _forceDisplay;
			}
			set 
			{
				_forceDisplay = value;
			}
		}

		private GuiControl getLastControlForTab(int tabIndex) 
		{
			GuiControl cntrl = null;
			if (orderedControls.Count > 0) 
			{
				cntrl = orderedControls[(orderedControls.Count-1)] as GuiControl;
				if (cntrl.TabStripIndex != tabIndex) 
				{
					cntrl = null;
				}
			}
			return cntrl;
		}

		/// <summary>
		/// Adds a GuiControl to the GuiController collection.
		/// </summary>
		/// <param name="control">A control to add to the form.</param>
		protected void AddGuiControl(IGuiControl control)
		{
			if (!controls.ContainsKey(control.ID)) 
			{
				int tabIndex = 0;
				control.Left = 10;
				
				control.Width = this._width;
				if (FORM_WIDTH_OFFSET < control.Width)
				{
					control.Width = control.Width - FORM_WIDTH_OFFSET;
				}
				
				// If there are tabs, add this to the current tab
				if (this._tabs.Count > 0) 
				{
					tabIndex = (this._tabs.Count-1);
					if (TAB_WIDTH_OFFSET < control.Width)
					{
						control.Width = control.Width - TAB_WIDTH_OFFSET;
					}
				}
				((GuiControl)control).TabStripIndex = tabIndex;

				GuiControl lastControl = getLastControlForTab(tabIndex);
				if (lastControl != null) 
				{
					control.Top = lastControl.Top + lastControl.Height + 10;
				}
				else
				{
					control.Top = 10;
				}

				if (control is GuiLabel || control is GuiButton) 
				{
					if (this.ForeColor != string.Empty) 
					{
						control.ForeColor = this.ForeColor;
					}
					if (this.BackColor != string.Empty) 
					{
						control.BackColor = this.BackColor;
					}
				}

				controls[control.ID] = control;
				orderedControls.Add(control);
			}
			else 
			{
				throw new Exception("The control ID " + control.ID + " has already been taken by another control on the form.");
			}
		}

		/// <summary>
		/// Adds a GuiLabel to the GuiController collection.
		/// </summary>
		/// <param name="id">The id of this new control</param>
		/// <param name="text">The visible text of the label</param>
		/// <param name="tooltip">The mouseover tooltip of the label</param>
		/// <returns>The newly created label.</returns>
		public GuiLabel AddLabel(string id, string text, string tooltip) 
		{
			GuiLabel label = new GuiLabel();
			label.ID = id;
			label.Text = text;
			label.ToolTip = tooltip;

			AddGuiControl(label);

			return label;
		}

		/// <summary>
		/// Adds a GuiCheckBox to the form control collection.
		/// </summary>
		/// <param name="id">The id of this control</param>
		/// <param name="text">The visible text of the checkbox</param>
		/// <param name="isChecked">Set to true if the checkbix is to be checked by default.</param>
		/// <param name="tooltip">The mouseover text of this control</param>
		/// <returns>The newly created control.</returns>
		public GuiCheckBox AddCheckBox(string id, string text, bool isChecked, string tooltip) 
		{
			GuiCheckBox checkbox = new GuiCheckBox();
			checkbox.ID = id;
			checkbox.Text = text;
			checkbox.Checked = isChecked;
			checkbox.ToolTip = tooltip;

			AddGuiControl(checkbox);

			return checkbox;
		}

		/// <summary>
		/// Adds a GuiButton to the form control collection.
		/// </summary>
		/// <param name="id">The id of this control</param>
		/// <param name="text">The visible text of the control</param>
		/// <param name="tooltip">The mouseover text of this control</param>
		/// <returns>The newly created control.</returns>
		public GuiButton AddButton( string id, string text, string tooltip) 
		{
			GuiButton button = new GuiButton();
			button.ID = id;
			button.Text = text;
			button.ToolTip = tooltip;

			AddGuiControl(button);

			return button;
		}

		/// <summary>
		/// Adds a GuiFilePicker to the form control collection.
		/// </summary>
		/// <param name="id">The id of this control</param>
		/// <param name="text">The visible text of this control</param>
		/// <param name="tooltip">The mouseover text of this control</param>
		/// <param name="targetcontrol">The target control (textbox) that will hold the file path.</param>
		/// <param name="picksFolder">If this is true, this FilePicker will actually pick a folder. 
		/// Otherwise, the FilePicker will pick a file.</param>
		/// <returns>The newly created control.</returns>
		public GuiFilePicker AddFilePicker(string id, string text, string tooltip, string targetcontrol, bool picksFolder) 
		{
			GuiFilePicker picker = new GuiFilePicker();
			picker.ID = id;
			picker.Text = text;
			picker.ToolTip = tooltip;
			picker.PicksFolder = picksFolder;
			picker.TargetControl = targetcontrol;

			AddGuiControl(picker);

			return picker;
		}

		/// <summary>
		/// Returns true if the OK button exists already
		/// </summary>
		public bool DoesOkButtonExist
		{
			get 
			{
				bool found = false;
				foreach (GuiControl gc in this.controls.Values) 
				{
					if (gc is GuiButton) 
					{
						GuiButton gb = gc as GuiButton;
						if (gb.ClosesForm) 
						{
							found = true;
							break;
						}
					}
				}

				return found;
			}
		}

		/// <summary>
		/// Adds a GuiButton to the form control collection that will close the form if 
		/// it's not already there.
		/// </summary>
		public void AddOkButtonIfNonExistant() 
		{
			if (!DoesOkButtonExist) 
			{
				if (this._tabs.Count == 0) 
				{
					this.AddOkButton("ButtonOk__internal", "OK", "Click here when you're finished with the form.");
				}
			}
		}

		/// <summary>
		/// Adds a GuiButton to the form control collection that will close the form.
		/// </summary>
		/// <param name="id">The id of this control</param>
		/// <param name="text">The visible text of this control</param>
		/// <param name="tooltip">The mouseover text of this control</param>
		/// <returns>The newly created control.</returns>
		public GuiButton AddOkButton( string id, string text, string tooltip) 
		{
			GuiButton button = new GuiButton();
			button.ID = id;
			button.Text = text;
			button.ToolTip = tooltip;
			button.ClosesForm = true;

			AddGuiControl(button);

			return button;
		}

		/// <summary>
		/// Adds a GuiTextBox to the form control collection.
		/// </summary>
		/// <param name="id">The id of this control</param>
		/// <param name="text">The visible text of this control</param>
		/// <param name="tooltip">The mouseover text of this control</param>
		/// <returns>The newly created control.</returns>
		public GuiTextBox AddTextBox(string id, string text, string tooltip) 
		{
			GuiTextBox textbox = new GuiTextBox();
			textbox.ID = id;
			textbox.Text = text;
			textbox.ToolTip = tooltip;

			AddGuiControl(textbox);

			return textbox;
		}

		/// <summary>
		/// Adds a GuiComboBox to the form control collection.
		/// </summary>
		/// <param name="id">The id of this control</param>
		/// <param name="tooltip">The mouseover text of this control</param>
		/// <returns>The newly created control.</returns>
		public GuiComboBox AddComboBox(string id, string tooltip) 
		{
			GuiComboBox combobox = new GuiComboBox();
			combobox.ID = id;
			combobox.ToolTip = tooltip;

			AddGuiControl(combobox);

			return combobox;
		}

		/// <summary>
		/// Adds a GuiListBox to the form control collection.
		/// </summary>
		/// <param name="id">The id of this control</param>
		/// <param name="tooltip">The mouseover text of this control</param>
		/// <returns>The newly created control.</returns>
		public GuiListBox AddListBox(string id, string tooltip) 
		{
			GuiListBox listbox = new GuiListBox();
			listbox.ID = id;
			listbox.ToolTip = tooltip;

			AddGuiControl(listbox);

			return listbox;
		}

		/// <summary>
		/// Adds a GuiGrid to the form control collection.
		/// </summary>
		/// <param name="id">The id of this control</param>
		/// <param name="tooltip">The mouseover text of this control</param>
		/// <returns>The newly created control.</returns>
		public GuiGrid AddGrid(string id, string tooltip) 
		{
			GuiGrid grid = new GuiGrid();
			grid.ID = id;
			grid.ToolTip = tooltip;

			AddGuiControl(grid);

			return grid;
		}

		/// <summary>
		/// Adds a GuiCheckBoxList to the form control collection.
		/// </summary>
		/// <param name="id">The id of this control</param>
		/// <param name="tooltip">The mouseover text of this control</param>
		/// <returns>The newly created control.</returns>
		public GuiCheckBoxList AddCheckBoxList(string id, string tooltip) 
		{
			GuiCheckBoxList checkboxlist = new GuiCheckBoxList();
			checkboxlist.ID = id;
			checkboxlist.ToolTip = tooltip;

			AddGuiControl(checkboxlist);

			return checkboxlist;
		}

		/// <summary>
		/// Sets or gets the width of the form.
		/// </summary>
		public override int Width 
		{
			get { return _width; }
			set { _width = value; }
		}

		/// <summary>
		/// Sets or gets the height of the form.
		/// </summary>
		public override int Height
		{
			get { return _height; }
			set { _height = value; }
		}

		/// <summary>
		/// Sets or gets the start position of the form. Options include: Manual (default), CenterParent, CenterScreen, DefaultLocation, DefaultBounds
		/// </summary>
		public string StartPosition 
		{
			get { return _startPostion; }
			set { if (value != null) _startPostion = value.ToLower(); }
		}

		/// <summary>
		/// Sets or gets the title of the form.
		/// </summary>
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		/// <summary>
		/// Returns the title of the form.
		/// </summary>
		public override object Value
		{
			get 
			{
				return _title;
			}
		}

		/// <summary>
		/// Returns the names of all the tabs
		/// </summary>
		public ArrayList TabNames
		{
			get 
			{
				return _tabs;
			}
		}


		/// <summary>
		/// When set to true, the Gui will be displayed before redering the template. 
		/// If false, it will not display the form and jump directly to the template code.
		/// </summary>
		public bool ShowGui
		{
			get { return _showGui; }
			set { _showGui = value; }
		}

		/// <summary>
		/// If the Gui was cancelled, (Upper right-hand red X was checked) return true.
		/// </summary>
		public bool IsCanceled
		{
			get { return this._canceled; }
			set { _canceled = value; }
		}

		/// <summary>
		/// Returns an enumerator for all of the controls that have been added to this object.
		/// </summary>
		/// <returns>Returns the enumerator for this object.</returns>
		public IEnumerator GetEnumerator()
		{
			return orderedControls.GetEnumerator();
		}

		/// <summary>
		/// Attaches an event to the GuiController. The GuiController supports the onclose event.
		/// </summary>
		/// <param name="eventType">The name of the event (onclose)</param>
		/// <param name="functionName">The event handler function that handles this 
		/// event. The functionName should have the signature in the Gui code: 
		/// "eventHandlerFunctionName(GuiController control)"</param>
		public override void AttachEvent(string eventType, string functionName)
		{
			if (eventType.ToLower() == "onclose")
			{
				this._oncloseEvents.Add(functionName);
            }
            else
            {
                base.AttachEvent(eventType, functionName);
            }
		}

		/// <summary>
		/// Returns true if there are any event handlers attached for this event.
		/// </summary>
		/// <param name="eventType">The event type to check out.</param>
		/// <returns>Returns true if there are any of this type of event handler.</returns>
		public override bool HasEventHandlers(string eventType)
		{
			if (eventType == "onclose") 
			{
				return (this._oncloseEvents.Count > 0);
			}
			else
			{
				return base.HasEventHandlers(eventType);
			}
		}

		/// <summary>
		/// Returns the event handler function names for a specific event type in a string array.
		/// </summary>
		/// <param name="eventType">The event type</param>
		/// <returns>Returns the event handler function names</returns>
		public override string[] GetEventHandlers(string eventType)
		{
			if (eventType == "onclose") 
			{
				return this._oncloseEvents.ToArray(typeof(String)) as String[];
			}
			else
			{
				return base.GetEventHandlers(eventType);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tabText"></param>
		public void AddTab(string tabText) 
		{
			this._tabs.Add(tabText);

			if (this.controls.Count > 0) 
			{
				if (this._tabs.Count == 1) 
				{
					foreach (GuiControl c in controls.Values) 
					{
						if (TAB_WIDTH_OFFSET < c.Width)
						{
							c.Width = (c.Width - TAB_WIDTH_OFFSET);
						}
					}
				}
			}
		}
	}
}