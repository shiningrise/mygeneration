using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Zeus.UserInterface;
using Zeus.Data;

namespace Zeus.UserInterface.WinForms
{

	public class FormBuilder 
	{
		private Form form;
		private TabControl tabs = null;
		private ToolTip tooltip = new ToolTip();
		private ArrayList orderedGuiControls = new ArrayList();
		private Hashtable guiControls = new Hashtable();
		private Hashtable win32Controls = new Hashtable();
		private FileDialog fileDialog = new OpenFileDialog();
		private FolderBrowserDialog folderDialog = new FolderBrowserDialog();

		public FormBuilder(Form form) 
		{
			this.form = form;
			form.Closing += new CancelEventHandler(OnFormClosing);
		}

		public FormBuilder(Form form, TabControl tabs) 
		{
			this.form = form;
			this.tabs = tabs;
			form.Closing += new CancelEventHandler(OnFormClosing);
		}

		private void addControl(GuiControl gctrl, Control c) 
		{
			if (tabs == null) 
			{
				form.Controls.Add(c);
			}
			else 
			{
				tabs.TabPages[gctrl.TabStripIndex].Controls.Add(c);
			}
			win32Controls[gctrl.ID] = c;
		}

		public void AddToForm(GuiControl control) 
		{
			guiControls.Add(control.ID, control);
			orderedGuiControls.Add(control);

            if (control is GuiLabel) 
			{
				GuiLabel guiLabel = control as GuiLabel;

				Label l = new Label();

				l.Left = guiLabel.Left;
				l.Top = guiLabel.Top;
				l.Width = guiLabel.Width;
				l.Height = guiLabel.Height;
				l.Visible = guiLabel.Visible;
				l.Enabled = guiLabel.Enabled;
				l.Enabled = guiLabel.Enabled;

				l.ForeColor = Color.FromName(guiLabel.ForeColor);
				if (guiLabel.BackColor != string.Empty)
				{
					l.BackColor = Color.FromName(guiLabel.BackColor);
				}

				l.TextAlign = ContentAlignment.BottomLeft;

				l.Name = guiLabel.ID;
				l.Text = guiLabel.Text;
				tooltip.SetToolTip(l, guiLabel.ToolTip);

				Font font = l.Font;
				FontStyle style = FontStyle.Regular;
				if (guiLabel.Bold) 
				{
					style = style | FontStyle.Bold;
				}
				if (guiLabel.Underline) 
				{
					style = style | FontStyle.Underline;
				}
				if (guiLabel.Strikeout) 
				{
					style = style | FontStyle.Strikeout;
				}
				if (guiLabel.Italic) 
				{
					style = style | FontStyle.Italic;
				}
				l.Font = new Font(font, style);

                l.LostFocus += new EventHandler(ControlLostFocus);
                l.Enter += new EventHandler(ControlEnter);

				addControl(control, l);
			}
			else if (control is GuiButton) 
			{
				GuiButton guiButton = control as GuiButton;

				Button b = new Button();

                b.Click += new EventHandler(OnButtonClick);
                b.LostFocus += new EventHandler(ControlLostFocus);
                b.Enter += new EventHandler(ControlEnter);
				
				if (guiButton.ClosesForm) 
				{
					b.Click += new EventHandler(OnButtonOkClick);
				}
				else if (guiButton.CancelGeneration) 
				{
					b.Click += new EventHandler(OnButtonCancelClick);
				}

				b.Text = guiButton.Text;
				b.Left = guiButton.Left;
				b.Top = guiButton.Top;
				b.Width = guiButton.Width;
				b.Height = guiButton.Height;
				b.Name = guiButton.ID;
				b.Visible = guiButton.Visible;
				b.Enabled = guiButton.Enabled;

				b.ForeColor = Color.FromName(guiButton.ForeColor);
				if (guiButton.BackColor != string.Empty)
				{
					b.BackColor = Color.FromName(guiButton.BackColor);
				}

				tooltip.SetToolTip(b, guiButton.ToolTip);

				addControl(control, b);
			}
			else if (control is GuiCheckBox) 
			{
				GuiCheckBox guiCheckBox = control as GuiCheckBox;

				CheckBox cb = new CheckBox();
				
				cb.Checked = guiCheckBox.Checked;

                cb.CheckedChanged += new EventHandler(OnCheckBoxClick);
                cb.LostFocus += new EventHandler(ControlLostFocus);
                cb.Enter += new EventHandler(ControlEnter);

				cb.Text = guiCheckBox.Text;
				cb.Left = guiCheckBox.Left;
				cb.Top = guiCheckBox.Top;
				cb.Width = guiCheckBox.Width;
				cb.Height = guiCheckBox.Height;
				cb.Name = guiCheckBox.ID;
				cb.Visible = guiCheckBox.Visible;
				cb.Enabled = guiCheckBox.Enabled;

				cb.ForeColor = Color.FromName(guiCheckBox.ForeColor);
				if (guiCheckBox.BackColor != string.Empty)
				{
					cb.BackColor = Color.FromName(guiCheckBox.BackColor);
				}

				tooltip.SetToolTip(cb, guiCheckBox.ToolTip);

				addControl(control, cb);
			}
			else if (control is GuiFilePicker) 
			{
				GuiFilePicker guiPicker = control as GuiFilePicker;

				Button b = new Button();
				
				if (guiPicker.PicksFolder)
				{
					b.Click += new EventHandler(OnFolderSelectorClick);
				}
				else
				{
					b.Click += new EventHandler(OnFileSelectorClick);
				}

				b.Text = guiPicker.Text;
				b.Left = guiPicker.Left;
				b.Top = guiPicker.Top;
				b.Width = guiPicker.Width;
				b.Height = guiPicker.Height;
				b.Name = guiPicker.ID;
				b.Visible = guiPicker.Visible;
				b.Enabled = guiPicker.Enabled;

				b.ForeColor = Color.FromName(guiPicker.ForeColor);
				if (guiPicker.BackColor != string.Empty)
				{
					b.BackColor = Color.FromName(guiPicker.BackColor);
				}

				tooltip.SetToolTip(b, guiPicker.ToolTip);

                b.LostFocus += new EventHandler(ControlLostFocus);
                b.Enter += new EventHandler(ControlEnter);

				addControl(control, b);
			}
			else if (control is GuiTextBox) 
			{
				GuiTextBox guiTextBox = control as GuiTextBox;

				TextBox tb = new TextBox();

				tb.Left = guiTextBox.Left;
				tb.Top = guiTextBox.Top;
				tb.Width = guiTextBox.Width;
				tb.Height = guiTextBox.Height;
				tb.Visible = guiTextBox.Visible;
				tb.Enabled = guiTextBox.Enabled;
				tb.Multiline = guiTextBox.Multiline;
				tb.WordWrap = guiTextBox.WordWrap;

				if (guiTextBox.VerticalScroll && guiTextBox.HorizontalScroll)	tb.ScrollBars = ScrollBars.Both;
				else if (guiTextBox.VerticalScroll)								tb.ScrollBars = ScrollBars.Vertical;
				else if (guiTextBox.HorizontalScroll)							tb.ScrollBars = ScrollBars.Horizontal;
				else															tb.ScrollBars = ScrollBars.None;

				tb.ForeColor = Color.FromName(guiTextBox.ForeColor);
				if (guiTextBox.BackColor != string.Empty)
				{
					tb.BackColor = Color.FromName(guiTextBox.BackColor);
				}

				tb.Name = guiTextBox.ID;
				tb.Text = guiTextBox.Text;
				tooltip.SetToolTip(tb, guiTextBox.ToolTip);

                tb.KeyPress += new KeyPressEventHandler(OnTextBoxKeyPress);
                tb.LostFocus += new EventHandler(ControlLostFocus);
                tb.Enter += new EventHandler(ControlEnter);

				addControl(control, tb);
			}
			else if (control is GuiComboBox) 
			{
				GuiComboBox guiComboBox = control as GuiComboBox;

				ComboBox cb = new ComboBox();
				cb.DropDownStyle = ComboBoxStyle.DropDownList;
				cb.Sorted = guiComboBox.Sorted;

				foreach (string val in guiComboBox.Items) 
				{
					ListControlItem item = new ListControlItem(val, guiComboBox[val]);
					cb.Items.Add(item);

					if (val == guiComboBox.SelectedValue) 
					{
						cb.SelectedItem = item;
					}
				}

				cb.SelectedValueChanged += new EventHandler(OnComboBoxChange);
                cb.LostFocus += new EventHandler(ControlLostFocus);
                cb.Enter += new EventHandler(ControlEnter);

				cb.Left = guiComboBox.Left;
				cb.Top = guiComboBox.Top;
				cb.Width = guiComboBox.Width;
				cb.Height = guiComboBox.Height;
				cb.Visible = guiComboBox.Visible;
				cb.Enabled = guiComboBox.Enabled;

				cb.ForeColor = Color.FromName(guiComboBox.ForeColor);
				if (guiComboBox.BackColor != string.Empty)
				{
					cb.BackColor = Color.FromName(guiComboBox.BackColor);
				}

				cb.Name = guiComboBox.ID;

				tooltip.SetToolTip(cb, guiComboBox.ToolTip);

				addControl(control, cb);
			}
			else if (control is GuiListBox) 
			{
				GuiListBox guiListBox = control as GuiListBox;

				ListBox lb = new ListBox();
				if (guiListBox.IsMultiSelect) 
				{
					lb.SelectionMode = SelectionMode.MultiExtended;
				}
				else 
				{
					lb.SelectionMode = SelectionMode.One;
				}
				lb.Sorted = guiListBox.Sorted;

				lb.Left = guiListBox.Left;
				lb.Top = guiListBox.Top;
				lb.Width = guiListBox.Width;
				lb.Height = guiListBox.Height;
				lb.Visible = guiListBox.Visible;
				lb.Enabled = guiListBox.Enabled;

				lb.ForeColor = Color.FromName(guiListBox.ForeColor);
				if (guiListBox.BackColor != string.Empty)
				{
					lb.BackColor = Color.FromName(guiListBox.BackColor);
				}

				lb.Name = guiListBox.ID;

				tooltip.SetToolTip(lb, guiListBox.ToolTip);

				foreach (string val in guiListBox.Items) 
				{
					ListControlItem item = new ListControlItem(val, guiListBox[val]);
					int index = lb.Items.Add(item);

					if (guiListBox.SelectedItems.Contains(val)) 
					{
						lb.SetSelected(index, true);
					}
				}

				// For some reason this fixes all of my timing issues!
				object s;
				foreach (object o in lb.SelectedIndices) s = o;

				lb.KeyUp += new KeyEventHandler(OnListBoxKeyUp);
                lb.SelectedValueChanged += new EventHandler(OnListBoxChange);
                lb.LostFocus += new EventHandler(ControlLostFocus);
                lb.Enter += new EventHandler(ControlEnter);

				addControl(control, lb);
			}
			else if (control is GuiGrid) 
			{
				GuiGrid guiGrid = control as GuiGrid;

				DataGrid dg = new DataGrid();

				dg.Left = guiGrid.Left;
				dg.Top = guiGrid.Top;
				dg.Width = guiGrid.Width;
				dg.Height = guiGrid.Height;
				dg.Visible = guiGrid.Visible;
				dg.Enabled = guiGrid.Enabled;

				if (guiGrid.ForeColor != string.Empty)
				{
					dg.ForeColor = Color.FromName(guiGrid.ForeColor);
				}
				else if (guiGrid.BackColor != string.Empty)
				{
					dg.BackColor = Color.FromName(guiGrid.BackColor);
				}

				dg.Name = guiGrid.ID;
				dg.DataSource = SimpleTableTools.ConvertToDataTable(guiGrid.DataSource);

				tooltip.SetToolTip(dg, guiGrid.ToolTip);

                dg.LostFocus += new EventHandler(ControlLostFocus);
                dg.Enter += new EventHandler(ControlEnter);

				addControl(control, dg);
			}
			else if (control is GuiCheckBoxList) 
			{
				GuiCheckBoxList guiCheckBoxList = control as GuiCheckBoxList;

				CheckedListBox lb = new CheckedListBox();
				lb.Sorted = guiCheckBoxList.Sorted;
				lb.CheckOnClick = true;
				lb.Left = guiCheckBoxList.Left;
				lb.Top = guiCheckBoxList.Top;
				lb.Width = guiCheckBoxList.Width;
				lb.Height = guiCheckBoxList.Height;
				lb.Visible = guiCheckBoxList.Visible;
				lb.Enabled = guiCheckBoxList.Enabled;

				lb.ForeColor = Color.FromName(guiCheckBoxList.ForeColor);
				if (guiCheckBoxList.BackColor != string.Empty)
				{
					lb.BackColor = Color.FromName(guiCheckBoxList.BackColor);
				}

				lb.Name = guiCheckBoxList.ID;
				tooltip.SetToolTip(lb, guiCheckBoxList.ToolTip);

				foreach (string val in guiCheckBoxList.Items) 
				{
					ListControlItem item = new ListControlItem(val, guiCheckBoxList[val]);
					int index = lb.Items.Add(item);

					if (guiCheckBoxList.SelectedItems.Contains(val)) 
					{
						lb.SetItemChecked(index, true);
					}
				}

				// For some reason this fixes all of my timing issues!
				object s;
				foreach (object o in lb.CheckedItems) s = o;

				lb.KeyUp += new KeyEventHandler(OnCheckedListBoxKeyUp);
				lb.SelectedValueChanged += new EventHandler(OnCheckedListBoxChange);
                lb.LostFocus += new EventHandler(ControlLostFocus);
                lb.Enter += new EventHandler(ControlEnter);

				addControl(control, lb);
			}
		}

		public void UpdateData() 
		{
			Control w32ctrl;
			foreach (GuiControl control in guiControls.Values) 
			{
				w32ctrl = win32Controls[control.ID] as Control;

				if (control is GuiLabel) 
				{
					GuiLabel guiLabel = control as GuiLabel;
					Label l = w32ctrl as Label;

					guiLabel.Text = l.Text;
				}
				else if (control is GuiButton) 
				{
					GuiButton guiButton = control as GuiButton;
					Button b = w32ctrl as Button;

					guiButton.Text = b.Text;
				}
				else if (control is GuiCheckBox) 
				{
					GuiCheckBox guiCheckBox = control as GuiCheckBox;
					CheckBox cb = w32ctrl as CheckBox;

					guiCheckBox.Text = cb.Text;
					guiCheckBox.Checked = cb.Checked;
				}
				else if (control is GuiFilePicker) 
				{
					GuiFilePicker guiPicker = control as GuiFilePicker;
					Button b = w32ctrl as Button;

					guiPicker.Text = b.Text;
					b.Tag = win32Controls[guiPicker.TargetControl];
				}
				else if (control is GuiTextBox) 
				{
					GuiTextBox guiTextBox = control as GuiTextBox;
					TextBox tb = w32ctrl as TextBox;

					guiTextBox.Text = tb.Text;
				}
				else if (control is GuiComboBox) 
				{
					GuiComboBox guiComboBox = control as GuiComboBox;
					ComboBox cb = w32ctrl as ComboBox;

					if (cb.SelectedItem is ListControlItem)
						guiComboBox.SelectedValue = ((ListControlItem)cb.SelectedItem).Value;
				}
				else if (control is GuiListBox) 
				{
					GuiListBox guiListBox = control as GuiListBox;
					ListBox lb = w32ctrl as ListBox;

					guiListBox.Clear();
					foreach (ListControlItem item in lb.Items)
					{
						guiListBox[item.Value] = item.Text;
					}
					foreach (ListControlItem item in lb.SelectedItems)
					{
						guiListBox.SelectedItems.Add(item.Value);
					}
				}
				else if (control is GuiGrid) 
				{
					GuiGrid guiGrid = control as GuiGrid;
					DataGrid dg = w32ctrl as DataGrid;

					guiGrid.DataSource = SimpleTableTools.ConvertToSimpleTable(dg.DataSource as DataTable);
				}
				else if (control is GuiCheckBoxList) 
				{
					GuiCheckBoxList guiCheckBoxList = control as GuiCheckBoxList;
					CheckedListBox lb = w32ctrl as CheckedListBox;

					guiCheckBoxList.Clear();
					foreach (ListControlItem item in lb.Items)
					{
						guiCheckBoxList[item.Value] = item.Text;
					}
					foreach (ListControlItem item in lb.CheckedItems)
					{
						guiCheckBoxList.SelectedItems.Add(item.Value);
					}
				}
			}
		}

		protected void UpdateForm(Control eventSource) 
		{
			Control w32ctrl;
			foreach (GuiControl control in guiControls.Values) 
			{
				w32ctrl = win32Controls[control.ID] as Control;
                if (eventSource != w32ctrl)
                {
                    if (control is GuiLabel)
                    {
                        GuiLabel guiLabel = control as GuiLabel;

                        Label l = w32ctrl as Label;
                        l.Text = guiLabel.Text;

                        Font font = l.Font;
                        FontStyle style = FontStyle.Regular;
                        if (guiLabel.Bold)
                        {
                            style = style | FontStyle.Bold;
                        }
                        if (guiLabel.Underline)
                        {
                            style = style | FontStyle.Underline;
                        }
                        if (guiLabel.Strikeout)
                        {
                            style = style | FontStyle.Strikeout;
                        }
                        if (guiLabel.Italic)
                        {
                            style = style | FontStyle.Italic;
                        }
                        l.Font = new Font(font, style);
                    }
                    else if (control is GuiButton)
                    {
                        GuiButton guiButton = control as GuiButton;

                        Button b = w32ctrl as Button;
                        b.Text = guiButton.Text;
                    }
                    else if (control is GuiCheckBox)
                    {
                        GuiCheckBox guiCheckBox = control as GuiCheckBox;

                        CheckBox b = w32ctrl as CheckBox;

                        b.CheckedChanged -= new EventHandler(OnCheckBoxClick);
                        b.Checked = guiCheckBox.Checked;
                        b.CheckedChanged += new EventHandler(OnCheckBoxClick);

                        b.Text = guiCheckBox.Text;
                    }
                    else if (control is GuiFilePicker)
                    {
                        GuiFilePicker guiPicker = control as GuiFilePicker;

                        Button b = w32ctrl as Button;
                        b.Text = guiPicker.Text;
                    }
                    else if (control is GuiTextBox)
                    {
                        GuiTextBox guiTextBox = control as GuiTextBox;

                        TextBox tb = w32ctrl as TextBox;
                        tb.Text = guiTextBox.Text;
                        tb.Multiline = guiTextBox.Multiline;
                        tb.WordWrap = guiTextBox.WordWrap;

                        if (guiTextBox.VerticalScroll && guiTextBox.HorizontalScroll) tb.ScrollBars = ScrollBars.Both;
                        else if (guiTextBox.VerticalScroll) tb.ScrollBars = ScrollBars.Vertical;
                        else if (guiTextBox.HorizontalScroll) tb.ScrollBars = ScrollBars.Horizontal;
                        else tb.ScrollBars = ScrollBars.None;

                    }
                    else if (control is GuiComboBox)
                    {
                        GuiComboBox guiComboBox = control as GuiComboBox;

                        ComboBox cb = w32ctrl as ComboBox;
                        cb.SelectedValueChanged -= new EventHandler(OnComboBoxChange);

                        cb.Items.Clear();
                        foreach (string val in guiComboBox.Items)
                        {
                            ListControlItem item = new ListControlItem(val, guiComboBox[val]);
                            cb.Items.Add(item);

                            if (item.Value == guiComboBox.SelectedValue)
                            {
                                cb.SelectedItem = item;
                            }
                        }
                        cb.SelectedValueChanged += new EventHandler(OnComboBoxChange);
                    }
                    else if (control is GuiListBox)
                    {
                        GuiListBox guiListBox = control as GuiListBox;

                        ListBox lb = w32ctrl as ListBox;

                        lb.SelectedValueChanged -= new EventHandler(OnListBoxChange);

                        lb.Items.Clear();
                        foreach (string val in guiListBox.Items)
                        {
                            ListControlItem item = new ListControlItem(val, guiListBox[val]);
                            lb.Items.Add(item);

                            if (guiListBox.SelectedItems.Contains(val))
                            {
                                lb.SetSelected(lb.Items.IndexOf(item), true);
                            }
                        }

                        lb.SelectedValueChanged += new EventHandler(OnListBoxChange);

                        lb.SelectionMode = guiListBox.IsMultiSelect ? SelectionMode.MultiExtended : SelectionMode.One;
                        lb.Sorted = guiListBox.Sorted;
                    }
                    else if (control is GuiGrid)
                    {
                        GuiGrid guiGrid = control as GuiGrid;

                        DataGrid dg = w32ctrl as DataGrid;
                        dg.DataSource = SimpleTableTools.ConvertToDataTable(guiGrid.DataSource);
                    }
                    else if (control is GuiCheckBoxList)
                    {
                        GuiCheckBoxList guiCheckBoxList = control as GuiCheckBoxList;

                        CheckedListBox lb = w32ctrl as CheckedListBox;

                        lb.SelectedValueChanged -= new EventHandler(OnCheckedListBoxChange);

                        lb.Items.Clear();
                        foreach (string val in guiCheckBoxList.Items)
                        {
                            ListControlItem item = new ListControlItem(val, guiCheckBoxList[val]);
                            lb.Items.Add(item);

                            if (guiCheckBoxList.SelectedItems.Contains(val))
                            {
                                lb.SetItemChecked(lb.Items.IndexOf(item), true);
                            }
                        }

                        lb.SelectedValueChanged += new EventHandler(OnCheckedListBoxChange);

                        lb.Sorted = guiCheckBoxList.Sorted;
                    }
                }
				
				w32ctrl.Left = control.Left;
				w32ctrl.Top = control.Top;
				w32ctrl.Width = control.Width;
				w32ctrl.Height = control.Height;
				w32ctrl.Visible = control.Visible;
                w32ctrl.Enabled = control.Enabled;
                if (control.ForeColor != string.Empty) w32ctrl.ForeColor = Color.FromName(control.ForeColor);
                if (control.BackColor != string.Empty) w32ctrl.BackColor = Color.FromName(control.BackColor);

				tooltip.SetToolTip(w32ctrl, control.ToolTip);
			}
		}
	
		public void InitializeControlData(Hashtable input) 
		{
			Control w32ctrl;
			object objData;

			foreach (GuiControl control in this.orderedGuiControls) 
			{
				try 
				{
					w32ctrl = win32Controls[control.ID] as Control;

					if (input.Contains(control.ID)) 
					{
						objData = input[control.ID];

						if (control is GuiCheckBox) 
						{
							GuiCheckBox guiCheckBox = control as GuiCheckBox;

							CheckBox b = w32ctrl as CheckBox;
							b.Checked = Convert.ToBoolean(objData);
						}
						else if (control is GuiLabel) 
						{
							GuiLabel guiLabel = control as GuiLabel;

							Label l = w32ctrl as Label;
							l.Text = Convert.ToString(objData);
						}
						else if (control is GuiTextBox) 
						{
							GuiTextBox guiTextBox = control as GuiTextBox;

							TextBox tb = w32ctrl as TextBox;
							tb.Text = Convert.ToString(objData);
						}
						else if (control is GuiComboBox) 
						{
							GuiComboBox guiComboBox = control as GuiComboBox;

							ComboBox cb = w32ctrl as ComboBox;
							foreach (ListControlItem item in cb.Items)
							{
								if ( item.Value == Convert.ToString(objData) ) 
								{
									cb.SelectedItem = item;
									break;
								}
							}
						}
						else if (control is GuiListBox) 
						{
							GuiListBox guiListBox = control as GuiListBox;

							ListBox lb = w32ctrl as ListBox;

							ArrayList list = objData as ArrayList;
							if (list != null) 
							{
								for (int i = 0; i < lb.Items.Count; i++)
								{
									ListControlItem item = lb.Items[i] as ListControlItem;
									lb.SetSelected( i, list.Contains(item.Value) );
								}
							}
						}
						else if (control is GuiGrid) 
						{
							GuiGrid guiGrid = control as GuiGrid;

							DataGrid dg = w32ctrl as DataGrid;

							SimpleTable table = objData as SimpleTable;
							if (table != null) 
							{
								guiGrid.DataSource = table;
								dg.DataSource = SimpleTableTools.ConvertToDataTable(table);
							}
						}
						else if (control is GuiCheckBoxList) 
						{
							GuiCheckBoxList guiListBox = control as GuiCheckBoxList;

							CheckedListBox lb = w32ctrl as CheckedListBox;

							ArrayList list = objData as ArrayList;
							if (list != null) 
							{
								for (int i = 0; i < lb.Items.Count; i++)
								{
									ListControlItem item = lb.Items[i] as ListControlItem;
									lb.SetItemChecked( i, list.Contains(item.Value) );
								}
							}
						}
					}
				}
				catch 
				{
					// Do nothing in the catch for now. We want it to fill in as many items as possible.
				}
			}
		}
        
        public void OnListBoxKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if ((e.KeyCode == Keys.A) && e.Control) 
			{
				ListBox list = sender as ListBox;
				if (list.SelectionMode == SelectionMode.MultiSimple || list.SelectionMode == SelectionMode.MultiExtended) 
				{
					for (int i =0; i < list.Items.Count; i++) 
					{
						list.SetSelected(i, true);
					}
				}

			}
		}

		public void OnCheckedListBoxKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if ((e.KeyCode == Keys.A) && e.Control) 
			{
				e.Handled = true;

				CheckedListBox list = sender as CheckedListBox;

				for (int i =0; i < list.Items.Count; i++) 
				{
					list.SetItemCheckState(i, CheckState.Checked);
				}
			}
		}

		public void OnFormClosing(object sender, CancelEventArgs e) 
		{
			if (FormClosing != null)
			{
				FormClosing(sender, e);
			}
        }

        public void ControlEnter(object sender, EventArgs e)
        {
            if (ControlOnFocus != null)
            {
                Control s = sender as Control;
                GuiControl gc = guiControls[s.Name] as GuiControl;
                if (gc.HasEventHandlers("onfocus"))
                {
                    UpdateData();
                    ControlOnFocus(sender, e);
                    UpdateForm(sender as Control);
                }
            }
        }

        public void ControlLostFocus(object sender, EventArgs e)
        {
            if (ControlOnBlur != null)
            {
                Control s = sender as Control;
                GuiControl gc = guiControls[s.Name] as GuiControl;
                if (gc.HasEventHandlers("onblur"))
                {
                    UpdateData();
                    ControlOnBlur(sender, e);
                    UpdateForm(sender as Control);
                }
            }
        }

        public void OnTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (TextBoxKeyPress != null)
            {
                Control s = sender as Control;
                GuiControl gc = guiControls[s.Name] as GuiControl;
                if (gc.HasEventHandlers("onkeypress"))
                {
                    UpdateData();
                    TextBoxKeyPress(sender, e);
                    UpdateForm(sender as Control);
                }
            }
        }

		public void OnComboBoxChange(object sender, EventArgs e) 
		{
			if (ComboBoxChange != null)
			{
				Control s = sender as Control;
				GuiControl gc = guiControls[s.Name] as GuiControl;
                if (gc.HasEventHandlers("onchange") || gc.AutoBindingChildControls.Count > 0) 
				{
					UpdateData();
					ComboBoxChange(sender, e);
					UpdateForm(sender as Control);
				}
			}
		}

		public void OnListBoxChange(object sender, EventArgs e) 
		{
			if (ListBoxChange != null)
			{
				Control s = sender as Control;
				GuiControl gc = guiControls[s.Name] as GuiControl;
                if (gc.HasEventHandlers("onchange") || gc.AutoBindingChildControls.Count > 0)  
				{
					UpdateData();
					ListBoxChange(sender, e);
					UpdateForm(sender as Control);
				}
			}
		}

		public void OnCheckedListBoxChange(object sender, EventArgs e) 
		{
			if (CheckedListBoxChange != null)
			{
				Control s = sender as Control;
				GuiControl gc = guiControls[s.Name] as GuiControl;
				if (gc.HasEventHandlers("onchange")) 
				{
					UpdateData();
					CheckedListBoxChange(sender, e);
					UpdateForm(sender as Control);
				}
			}
		}

		public void OnButtonOkClick(object sender, EventArgs e) 
		{
			this.form.DialogResult = DialogResult.OK;
			this.form.Close();
		}

		public void OnButtonCancelClick(object sender, EventArgs e) 
		{
			this.form.DialogResult = DialogResult.Cancel;
			this.form.Close();
		}

		public void OnButtonClick(object sender, EventArgs e) 
		{
			if (ButtonClick != null)
			{
				Control s = sender as Control;
				GuiControl gc = guiControls[s.Name] as GuiControl;
				if (gc.HasEventHandlers("onclick")) 
				{
					UpdateData();
					ButtonClick(sender, e);
					UpdateForm(sender as Control);
				}
			}
		}

		public void OnCheckBoxClick(object sender, EventArgs e) 
		{
			if (CheckBoxClick != null)
			{
				Control s = sender as Control;
				GuiControl gc = guiControls[s.Name] as GuiControl;
				if (gc.HasEventHandlers("onclick")) 
				{
					UpdateData();
					CheckBoxClick(sender, e);
					UpdateForm(sender as Control);
				}
			}
		}
		
		public void OnFileSelectorClick(object sender, EventArgs e) 
		{
			UpdateData();
			
			Button b = sender as Button;
			if (b != null) 
			{
				if (b.Tag is Control) 
				{
					Control c = b.Tag as Control;
					this.fileDialog.FileName = c.Text;
					this.fileDialog.RestoreDirectory = true;
				}
			}
			DialogResult result = this.fileDialog.ShowDialog();
			if ( (sender is Button) && (result == DialogResult.OK) )
			{
				UpdateFromDialogSelection(sender as Button);

				UpdateData();
				FileSelectorSelect(sender, e);
				UpdateForm(sender as Control);
			}
		}
		
		public void OnFolderSelectorClick(object sender, EventArgs e) 
		{
			UpdateData();

			Button b = sender as Button;
			if (b != null) 
			{
				if (b.Tag is Control) 
				{
					Control c = b.Tag as Control;
					this.folderDialog.SelectedPath = c.Text;
					this.folderDialog.ShowNewFolderButton = true;
				}
			}
			DialogResult result = this.folderDialog.ShowDialog();
			if ( (sender is Button) && (result == DialogResult.OK) )
			{
				UpdateFromDialogSelection(sender as Button);

				UpdateData();
				FileSelectorSelect(sender, e);
				UpdateForm(sender as Control);
			}
		}

		protected void UpdateFromDialogSelection(Button btn) 
		{
			string targetField = null;
			GuiFilePicker guiPicker = null;
			foreach (GuiControl ctrl in this.guiControls.Values)
			{
				if (ctrl.ID == btn.Name) 
				{
					guiPicker = ctrl as GuiFilePicker;

					if (guiPicker.PicksFolder)
						guiPicker.ItemData = this.folderDialog.SelectedPath;
					else
						guiPicker.ItemData = this.fileDialog.FileName;

					targetField = guiPicker.TargetControl;
					break;
				}
			}

			if ((targetField != null) && (guiPicker != null))
			{
				foreach (GuiControl ctrl in this.guiControls.Values)
				{
					if ( (ctrl.ID == targetField) && (ctrl is GuiTextBox) )
					{
						((GuiTextBox)ctrl).Text = guiPicker.ItemData;

						break;
					}
				}
			}

			UpdateForm(btn);
		}
		
		public event EventHandler ComboBoxChange;
		public event EventHandler ListBoxChange;
		public event EventHandler ButtonClick;
		public event EventHandler CheckBoxClick;
		public event EventHandler FormClosing;
		public event EventHandler FileSelectorSelect;
        public event EventHandler CheckedListBoxChange;
        public event EventHandler TextBoxKeyPress;
        public event EventHandler ControlOnBlur;
        public event EventHandler ControlOnFocus;
	}

	public class ListControlItem
	{
		private string text = string.Empty;
		private string val = string.Empty;

		public ListControlItem(string val, string text) 
		{
			this.val = val;
			this.text = text;
		}

		public override string ToString()
		{
			return text;
		}

		public string Text 
		{
			get 
			{
				return text;
			}
		}

		public string Value 
		{
			get 
			{
				return val;
			}
		}
	}
}