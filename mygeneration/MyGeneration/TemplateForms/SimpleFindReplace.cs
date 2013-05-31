using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for SimpleFindReplace.
	/// </summary>
	public class SimpleFindReplace : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBoxFind;
		private System.Windows.Forms.TextBox textBoxReplace;
		private System.Windows.Forms.Button buttonFind;
		private System.Windows.Forms.Button buttonReplaceNext;
		private System.Windows.Forms.CheckBox checkBoxAllowReplace;
		private System.Windows.Forms.GroupBox groupBoxReplace;
		private System.Windows.Forms.Button buttonReplaceAll;
		private System.Windows.Forms.Label labelFind;
		private System.Windows.Forms.Label labelReplace;

		private MDIParent _parent = null;
		private System.Windows.Forms.CheckBox checkBoxCaseSensitive;
		private System.Windows.Forms.CheckBox checkBoxUseRegEx;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SimpleFindReplace()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		}

		public void Show(MDIParent parent, bool enableReplace, string defaultFindValue) 
		{
			bool defaultValueUsed = false;
			if (_parent == null)
			{
				_parent = parent;

				this.Left = parent.Left + (Math.Abs(parent.Width - this.Width) / 2);
				this.Top = parent.Top + (Math.Abs(parent.Height - this.Height) / 2);
			}

			if ((defaultFindValue != null) && (defaultFindValue != string.Empty) && (defaultFindValue != "\0")) 
			{
				this.textBoxFind.Text = defaultFindValue;
				defaultValueUsed = true;
			}
			else 
			{
				this.textBoxFind.Text = parent.CurrentEditControl.LastSearchText;
			}

			this.checkBoxAllowReplace.Checked = enableReplace;
			this.checkBoxUseRegEx.Checked = parent.CurrentEditControl.LastSearchIsRegex;
			this.checkBoxCaseSensitive.Checked = parent.CurrentEditControl.LastSearchIsCaseSensitive;

			if (!this.Visible) 
			{
				this.Show();
		

				Application.DoEvents();
				if (enableReplace && defaultValueUsed) 
				{
					this.textBoxReplace.Focus();
				}
				else
				{
					this.textBoxFind.Focus();
				}
			}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SimpleFindReplace));
			this.textBoxFind = new System.Windows.Forms.TextBox();
			this.textBoxReplace = new System.Windows.Forms.TextBox();
			this.buttonFind = new System.Windows.Forms.Button();
			this.buttonReplaceNext = new System.Windows.Forms.Button();
			this.checkBoxAllowReplace = new System.Windows.Forms.CheckBox();
			this.groupBoxReplace = new System.Windows.Forms.GroupBox();
			this.buttonReplaceAll = new System.Windows.Forms.Button();
			this.labelReplace = new System.Windows.Forms.Label();
			this.labelFind = new System.Windows.Forms.Label();
			this.checkBoxCaseSensitive = new System.Windows.Forms.CheckBox();
			this.checkBoxUseRegEx = new System.Windows.Forms.CheckBox();
			this.groupBoxReplace.SuspendLayout();
			this.SuspendLayout();
			// 
			// textBoxFind
			// 
			this.textBoxFind.Location = new System.Drawing.Point(8, 24);
			this.textBoxFind.Name = "textBoxFind";
			this.textBoxFind.Size = new System.Drawing.Size(208, 20);
			this.textBoxFind.TabIndex = 1;
			this.textBoxFind.Text = "";
			// 
			// textBoxReplace
			// 
			this.textBoxReplace.Location = new System.Drawing.Point(8, 40);
			this.textBoxReplace.Name = "textBoxReplace";
			this.textBoxReplace.Size = new System.Drawing.Size(200, 20);
			this.textBoxReplace.TabIndex = 6;
			this.textBoxReplace.Text = "";
			// 
			// buttonFind
			// 
			this.buttonFind.Location = new System.Drawing.Point(232, 24);
			this.buttonFind.Name = "buttonFind";
			this.buttonFind.Size = new System.Drawing.Size(80, 23);
			this.buttonFind.TabIndex = 2;
			this.buttonFind.Text = "Find Next";
			this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
			// 
			// buttonReplaceNext
			// 
			this.buttonReplaceNext.Location = new System.Drawing.Point(224, 16);
			this.buttonReplaceNext.Name = "buttonReplaceNext";
			this.buttonReplaceNext.Size = new System.Drawing.Size(80, 23);
			this.buttonReplaceNext.TabIndex = 7;
			this.buttonReplaceNext.Text = "Replace Next";
			this.buttonReplaceNext.Click += new System.EventHandler(this.buttonReplaceNext_Click);
			// 
			// checkBoxAllowReplace
			// 
			this.checkBoxAllowReplace.Location = new System.Drawing.Point(8, 72);
			this.checkBoxAllowReplace.Name = "checkBoxAllowReplace";
			this.checkBoxAllowReplace.Size = new System.Drawing.Size(120, 24);
			this.checkBoxAllowReplace.TabIndex = 4;
			this.checkBoxAllowReplace.Text = "Enable Replace";
			this.checkBoxAllowReplace.CheckedChanged += new System.EventHandler(this.checkBoxAllowReplace_CheckedChanged);
			// 
			// groupBoxReplace
			// 
			this.groupBoxReplace.Controls.Add(this.buttonReplaceAll);
			this.groupBoxReplace.Controls.Add(this.buttonReplaceNext);
			this.groupBoxReplace.Controls.Add(this.textBoxReplace);
			this.groupBoxReplace.Controls.Add(this.labelReplace);
			this.groupBoxReplace.Enabled = false;
			this.groupBoxReplace.Location = new System.Drawing.Point(8, 104);
			this.groupBoxReplace.Name = "groupBoxReplace";
			this.groupBoxReplace.Size = new System.Drawing.Size(312, 80);
			this.groupBoxReplace.TabIndex = 5;
			this.groupBoxReplace.TabStop = false;
			this.groupBoxReplace.Text = "Find / Replace Options";
			// 
			// buttonReplaceAll
			// 
			this.buttonReplaceAll.Location = new System.Drawing.Point(224, 48);
			this.buttonReplaceAll.Name = "buttonReplaceAll";
			this.buttonReplaceAll.Size = new System.Drawing.Size(80, 23);
			this.buttonReplaceAll.TabIndex = 8;
			this.buttonReplaceAll.Text = "Replace All";
			this.buttonReplaceAll.Click += new System.EventHandler(this.buttonReplaceAll_Click);
			// 
			// labelReplace
			// 
			this.labelReplace.Location = new System.Drawing.Point(8, 24);
			this.labelReplace.Name = "labelReplace";
			this.labelReplace.Size = new System.Drawing.Size(200, 16);
			this.labelReplace.TabIndex = 5;
			this.labelReplace.Text = "Replace Text";
			// 
			// labelFind
			// 
			this.labelFind.Location = new System.Drawing.Point(8, 8);
			this.labelFind.Name = "labelFind";
			this.labelFind.Size = new System.Drawing.Size(216, 16);
			this.labelFind.TabIndex = 0;
			this.labelFind.Text = "Find Text";
			// 
			// checkBoxCaseSensitive
			// 
			this.checkBoxCaseSensitive.Checked = true;
			this.checkBoxCaseSensitive.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxCaseSensitive.Location = new System.Drawing.Point(8, 48);
			this.checkBoxCaseSensitive.Name = "checkBoxCaseSensitive";
			this.checkBoxCaseSensitive.Size = new System.Drawing.Size(120, 24);
			this.checkBoxCaseSensitive.TabIndex = 3;
			this.checkBoxCaseSensitive.Text = "Case Sensitive";
			// 
			// checkBoxUseRegEx
			// 
			this.checkBoxUseRegEx.Location = new System.Drawing.Point(144, 48);
			this.checkBoxUseRegEx.Name = "checkBoxUseRegEx";
			this.checkBoxUseRegEx.Size = new System.Drawing.Size(168, 24);
			this.checkBoxUseRegEx.TabIndex = 6;
			this.checkBoxUseRegEx.Text = "Use Regular Expression";
			// 
			// SimpleFindReplace
			// 
			this.AcceptButton = this.buttonFind;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(328, 192);
			this.Controls.Add(this.checkBoxUseRegEx);
			this.Controls.Add(this.checkBoxCaseSensitive);
			this.Controls.Add(this.textBoxFind);
			this.Controls.Add(this.labelFind);
			this.Controls.Add(this.groupBoxReplace);
			this.Controls.Add(this.checkBoxAllowReplace);
			this.Controls.Add(this.buttonFind);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SimpleFindReplace";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Find/Replace Text";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.SimpleFindReplace_Closing);
			this.groupBoxReplace.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonFind_Click(object sender, System.EventArgs e)
		{
			if (_parent.CurrentEditControl != null)
			{
				bool success = _parent.CurrentEditControl.FindNextAndHighlight(this.textBoxFind.Text, this.checkBoxCaseSensitive.Checked, this.checkBoxUseRegEx.Checked);
				if (!success) MessageBox.Show(this, "No matches found.");
			}
		}

		private void buttonReplaceNext_Click(object sender, System.EventArgs e)
		{
			if (_parent.CurrentEditControl != null)
			{
				int count = _parent.CurrentEditControl.ReplaceNext(this.textBoxFind.Text, this.textBoxReplace.Text, this.checkBoxCaseSensitive.Checked, this.checkBoxUseRegEx.Checked);
				if (count == 0) MessageBox.Show(this, "No matches found.");
			}
		}

		private void buttonReplaceAll_Click(object sender, System.EventArgs e)
		{
			if (_parent.CurrentEditControl != null)
			{
				int count = _parent.CurrentEditControl.ReplaceAll(this.textBoxFind.Text, this.textBoxReplace.Text, this.checkBoxCaseSensitive.Checked, this.checkBoxUseRegEx.Checked);
				MessageBox.Show(this, count.ToString() + " Item(s) Replaced");
			}
		}

		private void checkBoxAllowReplace_CheckedChanged(object sender, System.EventArgs e)
		{
			this.groupBoxReplace.Enabled = this.checkBoxAllowReplace.Checked;
		}

		private void SimpleFindReplace_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
			this._parent.CurrentEditControl.GrabFocus();
		}
	}
}
