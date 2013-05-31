using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Zeus;
using Zeus.Configuration;
using Zeus.Projects;
using Zeus.ErrorHandling;
using Zeus.UserInterface;
using Zeus.UserInterface.WinForms;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for ViewSavedInput.
	/// </summary>
	public class FormViewSavedInput : System.Windows.Forms.Form
	{
		private System.Windows.Forms.DataGrid dataGridSavedInput;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private SavedTemplateInput _savedObject = null;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyleDefault;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn3;
		private DataTable _table = null;

		public FormViewSavedInput()
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
			this.dataGridSavedInput = new System.Windows.Forms.DataGrid();
			this.dataGridTableStyleDefault = new System.Windows.Forms.DataGridTableStyle();
			this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.dataGridSavedInput)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridSavedInput
			// 
			this.dataGridSavedInput.DataMember = "";
			this.dataGridSavedInput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridSavedInput.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridSavedInput.Location = new System.Drawing.Point(0, 0);
			this.dataGridSavedInput.Name = "dataGridSavedInput";
			this.dataGridSavedInput.ReadOnly = true;
			this.dataGridSavedInput.RowHeadersVisible = false;
			this.dataGridSavedInput.Size = new System.Drawing.Size(808, 438);
			this.dataGridSavedInput.TabIndex = 0;
			this.dataGridSavedInput.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																										   this.dataGridTableStyleDefault});
			this.dataGridSavedInput.Resize += new System.EventHandler(this.dataGridSavedInput_Resize);
			// 
			// dataGridTableStyleDefault
			// 
			this.dataGridTableStyleDefault.AlternatingBackColor = System.Drawing.SystemColors.InactiveCaptionText;
			this.dataGridTableStyleDefault.BackColor = System.Drawing.SystemColors.Window;
			this.dataGridTableStyleDefault.DataGrid = this.dataGridSavedInput;
			this.dataGridTableStyleDefault.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
																														this.dataGridTextBoxColumn1,
																														this.dataGridTextBoxColumn2,
																														this.dataGridTextBoxColumn3});
			this.dataGridTableStyleDefault.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyleDefault.MappingName = "";
			// 
			// dataGridTextBoxColumn1
			// 
			this.dataGridTextBoxColumn1.Format = "";
			this.dataGridTextBoxColumn1.FormatInfo = null;
			this.dataGridTextBoxColumn1.HeaderText = "Variable";
			this.dataGridTextBoxColumn1.MappingName = "Variable";
			this.dataGridTextBoxColumn1.NullText = "";
			this.dataGridTextBoxColumn1.ReadOnly = true;
			this.dataGridTextBoxColumn1.Width = 75;
			// 
			// dataGridTextBoxColumn2
			// 
			this.dataGridTextBoxColumn2.Format = "";
			this.dataGridTextBoxColumn2.FormatInfo = null;
			this.dataGridTextBoxColumn2.HeaderText = "Data Type";
			this.dataGridTextBoxColumn2.MappingName = "Data Type";
			this.dataGridTextBoxColumn2.NullText = "";
			this.dataGridTextBoxColumn2.ReadOnly = true;
			this.dataGridTextBoxColumn2.Width = 75;
			// 
			// dataGridTextBoxColumn3
			// 
			this.dataGridTextBoxColumn3.Format = "";
			this.dataGridTextBoxColumn3.FormatInfo = null;
			this.dataGridTextBoxColumn3.HeaderText = "Data";
			this.dataGridTextBoxColumn3.MappingName = "Value";
			this.dataGridTextBoxColumn3.NullText = "";
			this.dataGridTextBoxColumn3.ReadOnly = true;
			this.dataGridTextBoxColumn3.Width = 75;
			// 
			// FormViewSavedInput
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(808, 438);
			this.Controls.Add(this.dataGridSavedInput);
			this.Name = "FormViewSavedInput";
			this.Text = "Saved Input";
			((System.ComponentModel.ISupportInitialize)(this.dataGridSavedInput)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		public SavedTemplateInput SavedObject 
		{
			get 
			{
				return _savedObject;
			}
			set 
			{
				_savedObject = value;

				this.Text = "Object Data: " + _savedObject.SavedObjectName;
				BuildTable();
				this.dataGridSavedInput.DataSource = this._table;
				dataGridSavedInput_Resize(this.dataGridSavedInput, new EventArgs());
			}
		}

		private void BuildTable() 
		{
			_table = new DataTable();

			_table.Columns.Add("Variable");
			_table.Columns.Add("Data Type");
			_table.Columns.Add("Value");

			_table.DefaultView.Sort = "Variable ASC";

			DataRow row;
			foreach (InputItem item in this._savedObject.InputItems) 
			{
				row = _table.NewRow(); 

				row[0] = item.VariableName;
				row[1] = item.DataTypeName;
				row[2] = item.Data;

				_table.Rows.Add(row);
			}
		}

		private void dataGridSavedInput_Resize(object sender, System.EventArgs e)
		{
			int width = this.dataGridSavedInput.Width;
			DataGridTableStyle style = this.dataGridSavedInput.TableStyles[0];
			if (style != null) 
			{
				style.GridColumnStyles[0].Width = (int)(width * 0.2m);
				style.GridColumnStyles[1].Width = (int)(width * 0.1m);
				style.GridColumnStyles[2].Width = (int)(width * 0.6m);
			}
		}
	}
}
