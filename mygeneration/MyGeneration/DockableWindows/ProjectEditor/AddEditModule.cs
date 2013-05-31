using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Zeus;
using Zeus.Projects;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for AddEditModule.
	/// </summary>
	public class FormAddEditModule : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label labelName;
		private System.Windows.Forms.Label labelDescription;
		private System.Windows.Forms.TextBox textBoxName;
		private System.Windows.Forms.TextBox textBoxDescription;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.ErrorProvider errorProviderRequiredFields;

		private ZeusModule _module;
		private bool _isActivated = false;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormAddEditModule()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.labelName = new System.Windows.Forms.Label();
			this.labelDescription = new System.Windows.Forms.Label();
			this.textBoxName = new System.Windows.Forms.TextBox();
			this.textBoxDescription = new System.Windows.Forms.TextBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.errorProviderRequiredFields = new System.Windows.Forms.ErrorProvider();
			this.SuspendLayout();
			// 
			// labelName
			// 
			this.labelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelName.Location = new System.Drawing.Point(16, 8);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(256, 23);
			this.labelName.TabIndex = 0;
			this.labelName.Text = "Name:";
			this.labelName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelDescription
			// 
			this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.labelDescription.Location = new System.Drawing.Point(16, 56);
			this.labelDescription.Name = "labelDescription";
			this.labelDescription.Size = new System.Drawing.Size(256, 23);
			this.labelDescription.TabIndex = 1;
			this.labelDescription.Text = "Description:";
			this.labelDescription.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textBoxName
			// 
			this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxName.Location = new System.Drawing.Point(16, 32);
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.Size = new System.Drawing.Size(256, 20);
			this.textBoxName.TabIndex = 2;
			this.textBoxName.Text = "";
			this.textBoxName.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxName_Validating);
			// 
			// textBoxDescription
			// 
			this.textBoxDescription.AcceptsReturn = true;
			this.textBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxDescription.Location = new System.Drawing.Point(16, 80);
			this.textBoxDescription.Multiline = true;
			this.textBoxDescription.Name = "textBoxDescription";
			this.textBoxDescription.Size = new System.Drawing.Size(256, 80);
			this.textBoxDescription.TabIndex = 3;
			this.textBoxDescription.Text = "";
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.CausesValidation = false;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(208, 168);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.Location = new System.Drawing.Point(128, 168);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.TabIndex = 5;
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// errorProviderRequiredFields
			// 
			this.errorProviderRequiredFields.ContainerControl = this;
			// 
			// FormAddEditModule
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 198);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.textBoxDescription);
			this.Controls.Add(this.textBoxName);
			this.Controls.Add(this.labelDescription);
			this.Controls.Add(this.labelName);
			this.Name = "FormAddEditModule";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add/Edit Module";
			this.Load += new System.EventHandler(this.FormAddEditModule_Load);
			this.Activated += new System.EventHandler(this.FormAddEditModule_Activated);
			this.ResumeLayout(false);

		}
		#endregion

		public ZeusModule Module 
		{
			get 
			{
				return _module;
			}
			set 
			{
				_module = value;

				if(_module.Name != null) 
				{
					this.textBoxName.Text = _module.Name;
					this.Text = "Module: " + _module.Name;
				}
				else 
				{
					this.textBoxName.Text = string.Empty;
					this.Text = "Module: [New] ";
				}

				if(_module.Description != null)
					this.textBoxDescription.Text = _module.Description;
				else 
					this.textBoxDescription.Text = string.Empty;

				this._isActivated = false;
			}
		}

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			CancelEventArgs args = new CancelEventArgs();
			this.textBoxName_Validating(this, args);

			if (!args.Cancel)  
			{
				this.Module.Name = this.textBoxName.Text;
				if (this.textBoxDescription.Text.Length > 0)
				{
					this.Module.Description = this.textBoxDescription.Text;
				}
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void textBoxName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (this.textBoxName.Text.Trim() == string.Empty) 
			{
				e.Cancel = true;
				this.errorProviderRequiredFields.SetError(this.textBoxName, "Name is Required!");
			}
			else 
			{
				this.errorProviderRequiredFields.SetError(this.textBoxName, string.Empty);
			}
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void FormAddEditModule_Load(object sender, System.EventArgs e)
		{
			this.errorProviderRequiredFields.SetError(this.textBoxName, string.Empty);
			this.errorProviderRequiredFields.SetIconAlignment(this.textBoxName, ErrorIconAlignment.TopRight);
		}

		private void FormAddEditModule_Activated(object sender, System.EventArgs e)
		{
			if (!this._isActivated) 
			{
				this.textBoxName.Focus();
				this._isActivated = true;
			}
		}
	}
}
