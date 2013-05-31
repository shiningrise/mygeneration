using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Zeus.UserInterface;

namespace Zeus.UserInterface.WinForms
{
	/// <summary>
	/// Summary description for DynamicForm.
	/// </summary>
	public class DynamicForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		protected GuiController guiController;
		protected IZeusFunctionExecutioner executioner;
		protected FormBuilder builder;
		protected ILogger logger;
		protected TabControl tabs = null;
		protected Button buttonOk;

		public DynamicForm(GuiController gui, IZeusFunctionExecutioner executioner)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.guiController = gui;
			this.executioner = executioner;

			if (gui.TabNames.Count > 0) 
			{
				buttonOk = new Button();
				buttonOk.Dock =  DockStyle.Bottom;
				buttonOk.Text = "OK";
				buttonOk.Click += new EventHandler(ButtonOk_Click);
				this.Controls.Add(buttonOk);

				this.tabs = new TabControl();
				this.tabs.Dock = DockStyle.Fill;
				foreach (string name in gui.TabNames) 
				{
					this.tabs.TabPages.Add( new TabPage(name) );
				}
				this.Controls.Add(tabs);

				builder = new FormBuilder(this, tabs);
			}
			else 
			{
				builder = new FormBuilder(this);
			}

			builder.CheckedListBoxChange += new EventHandler(checkedListBoxChanged);
			builder.ListBoxChange += new EventHandler(listBoxChanged);
			builder.ComboBoxChange += new EventHandler(comboBoxChanged);
			builder.ButtonClick += new EventHandler(buttonClicked);
			builder.CheckBoxClick += new EventHandler(checkBoxClicked);
			builder.FormClosing += new EventHandler(formClosing);
			builder.FileSelectorSelect += new EventHandler(fileSelectorSelect);
            builder.TextBoxKeyPress += new EventHandler(textBoxKeyPress);
            builder.ControlOnBlur += new EventHandler(controlOnBlur);
            builder.ControlOnFocus += new EventHandler(controlOnFocus);
		}

		private void ButtonOk_Click(object sender, EventArgs e) 
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>

		/// Required method for Designer support - do not modify

		/// the contents of this method with the code editor.

		/// </summary>

		private void InitializeComponent()

		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DynamicForm));
			// 
			// DynamicForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(568, 454);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DynamicForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Dynamic Template Form";
			this.Load += new System.EventHandler(this.DynamicForm_Load);
			this.Closed += new System.EventHandler(this.DynamicForm_Closed);

		}

		#endregion

		private void DynamicForm_Load(object sender, System.EventArgs e)
		{
			this.guiController.AddOkButtonIfNonExistant();
			this.Text = guiController.Title;
			this.Height = guiController.Height;
			this.Width = guiController.Width;

			if ((guiController.Top != Int32.MinValue) && 
				(guiController.Left != Int32.MinValue)) 
			{
					guiController.StartPosition = "manual";
			}

			switch (guiController.StartPosition) 
			{
				case "centerscreen":
					this.StartPosition = FormStartPosition.CenterScreen;
					break;
				case "defaultbounds":
					this.StartPosition = FormStartPosition.WindowsDefaultBounds;
					break;
				case "defaultlocation":
					this.StartPosition = FormStartPosition.WindowsDefaultLocation;
					break;
				case "manual":
					this.StartPosition = FormStartPosition.Manual;
					this.Top = guiController.Top;
					this.Left = guiController.Left;
					break;
				case "centerparent":
				default:
					this.StartPosition = FormStartPosition.CenterParent;
					break;
			}
			
			if (guiController.ForeColor != string.Empty) 
				this.ForeColor = Color.FromName(guiController.ForeColor);
			if (guiController.BackColor != string.Empty) 
				this.BackColor = Color.FromName(guiController.BackColor);

			foreach (GuiControl control in guiController) 
			{
				builder.AddToForm(control);
			}

			if (this.guiController.Defaults.Count > 0) 
			{
				builder.InitializeControlData(guiController.Defaults);
			}

            this.BringToFront();
		}

		private void DynamicForm_Closed(object sender, System.EventArgs e)
		{
			builder.UpdateData();
		}

		public ILogger Logger 
		{
			get 
			{
				return this.logger;
			}
			set 
			{
				this.logger = value;
			}
        }

        private void controlOnFocus(object sender, System.EventArgs e)
        {
            try
            {
                if (sender is Control)
                {
                    Control tb = sender as Control;
                    GuiControl gtb = guiController[tb.Name] as GuiControl;

                    foreach (string functionName in gtb.GetEventHandlers("onfocus"))
                    {
                        this.executioner.ExecuteFunction(functionName, gtb);
                    }

                }
            }
            catch (Exception x)
            {
                //ZeusDisplayError formError = new ZeusDisplayError(x);
                //formError.ShowDialog(this);
                if (logger != null)
                    logger.LogException(x);
            }
        }

        private void controlOnBlur(object sender, System.EventArgs e)
        {
            try
            {
                if (sender is Control)
                {
                    Control tb = sender as Control;
                    GuiControl gtb = guiController[tb.Name] as GuiControl;

                    foreach (string functionName in gtb.GetEventHandlers("onblur"))
                    {
                        this.executioner.ExecuteFunction(functionName, gtb);
                    }

                }
            }
            catch (Exception x)
            {
                //ZeusDisplayError formError = new ZeusDisplayError(x);
                //formError.ShowDialog(this);
                if (logger != null)
                    logger.LogException(x);
            }
        }

        private void textBoxKeyPress(object sender, System.EventArgs e)
        {
            try
            {
                if (sender is TextBox)
                {
                    TextBox tb = sender as TextBox;
                    GuiTextBox gtb = guiController[tb.Name] as GuiTextBox;

                    foreach (string functionName in gtb.GetEventHandlers("onkeypress"))
                    {
                        this.executioner.ExecuteFunction(functionName, gtb);
                    }

                }
            }
            catch (Exception x)
            {
                //ZeusDisplayError formError = new ZeusDisplayError(x);
                //formError.ShowDialog(this);
                if (logger != null)
                    logger.LogException(x);
            }
        }

		private void checkedListBoxChanged(object sender, System.EventArgs e) 
		{
			try 
			{
				if (sender is CheckedListBox) 
				{
					CheckedListBox lb = sender as CheckedListBox;
					GuiCheckBoxList glb = guiController[lb.Name] as GuiCheckBoxList;

					foreach (string functionName in glb.GetEventHandlers("onchange")) 
					{
						this.executioner.ExecuteFunction(functionName, glb);
					}
				
				}
			}
			catch (Exception x)
			{
				//ZeusDisplayError formError = new ZeusDisplayError(x);
				//formError.ShowDialog(this);
                if (logger != null)
                    logger.LogException(x);
			}
        }

        private void UpdateAutoBinding(GuiControl gcb)
        {
            foreach (GuiControl abc in gcb.AutoBindingChildControls)
            {
                if (abc is IGuiBindableListControl)
                {
                    guiController.UpdateAutoBinding(abc);
                }
            }
        }

		private void listBoxChanged(object sender, System.EventArgs e) 
		{
			try 
			{
				if (sender is ListBox) 
				{
					ListBox lb = sender as ListBox;
					GuiListBox glb = guiController[lb.Name] as GuiListBox;

                    UpdateAutoBinding(glb);

					foreach (string functionName in glb.GetEventHandlers("onchange")) 
					{
						this.executioner.ExecuteFunction(functionName, glb);
					}
				
				}
			}
			catch (Exception x)
			{
				//ZeusDisplayError formError = new ZeusDisplayError(x);
				//formError.ShowDialog(this);
                if (logger != null)
                    logger.LogException(x);
			}
		}

		private void comboBoxChanged(object sender, System.EventArgs e) 
		{
			try 
			{
				if (sender is ComboBox) 
				{
					ComboBox cb = sender as ComboBox;
					GuiComboBox gcb = guiController[cb.Name] as GuiComboBox;

                    UpdateAutoBinding(gcb);

					foreach (string functionName in gcb.GetEventHandlers("onchange")) 
					{
						this.executioner.ExecuteFunction(functionName, gcb);
					}
				}
			}
			catch (Exception x)
			{
				//ZeusDisplayError formError = new ZeusDisplayError(x);
				//formError.ShowDialog(this);
                if (logger != null)
                    logger.LogException(x);
			}
		}

		private void buttonClicked(object sender, System.EventArgs e) 
		{
			try 
			{
				if (sender is Button) 
				{
					Button b = sender as Button;
					GuiButton gb = guiController[b.Name] as GuiButton;

					foreach (string functionName in gb.GetEventHandlers("onclick")) 
					{
						this.executioner.ExecuteFunction(functionName, gb);
					}
				}
			}
			catch (Exception x)
			{
				//ZeusDisplayError formError = new ZeusDisplayError(x);
				//formError.ShowDialog(this);
                if (logger != null)
                    logger.LogException(x);
			}
		}

		private void fileSelectorSelect(object sender, System.EventArgs e) 
		{
			try 
			{
				if (sender is Button) 
				{
					Button b = sender as Button;
					GuiFilePicker gb = guiController[b.Name] as GuiFilePicker;

					foreach (string functionName in gb.GetEventHandlers("onselect")) 
					{
						this.executioner.ExecuteFunction(functionName, gb);
					}
				}
			}
			catch (Exception x)
			{
				//ZeusDisplayError formError = new ZeusDisplayError(x);
				//formError.ShowDialog(this);
                if (logger != null)
                    logger.LogException(x);
			}
		}
		private void checkBoxClicked(object sender, System.EventArgs e) 
		{
			try 
			{
				if (sender is CheckBox) 
				{
					CheckBox cb = sender as CheckBox;
					GuiCheckBox gcb = guiController[cb.Name] as GuiCheckBox;

					foreach (string functionName in gcb.GetEventHandlers("onclick")) 
					{
						this.executioner.ExecuteFunction(functionName, gcb);
					}
				}
			}
			catch (Exception x)
			{
				//ZeusDisplayError formError = new ZeusDisplayError(x);
				//formError.ShowDialog(this);
                if (logger != null)
                    logger.LogException(x);
			}
		}

		private void formClosing(object sender, System.EventArgs e) 
		{
			try 
			{
				foreach (string functionName in guiController.GetEventHandlers("onclose")) 
				{
					this.executioner.ExecuteFunction(functionName, guiController);
				}
			}
			catch (Exception x)
			{
				//ZeusDisplayError formError = new ZeusDisplayError(x);
				//formError.ShowDialog(this);
                if (logger != null)
                    logger.LogException(x);
			}
		}
	}
}