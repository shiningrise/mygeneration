using System;
using System.Xml;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

using MyMeta;


namespace MyGeneration
{
	/// <summary>
	/// Summary description for GlobalUserMetaData.
	/// </summary>
    public class GlobalUserMetaData : DockContent, IMyGenContent
    {
        private IMyGenerationMDI mdi;
		private System.Windows.Forms.ToolBarButton toolBarButton_Save;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.DataGrid Grid;
		private System.Windows.Forms.DataGridTableStyle MyStyle;
		private System.Windows.Forms.DataGridTextBoxColumn colKey;
		private System.Windows.Forms.DataGridTextBoxColumn colValue;

		private Type stringType = Type.GetType("System.String");

        public GlobalUserMetaData(IMyGenerationMDI mdi)
		{
			InitializeComponent();

            this.mdi = mdi;
			this.ShowHint = DockState.DockRight;

			DefaultSettings settings = DefaultSettings.Instance;
			this.UserMetaDataFileName = settings.UserMetaDataFileName;
        }

        protected override string GetPersistString()
        {
            return this.GetType().FullName;
        }

		public MetaDataBrowser MetaDataBrowser
		{
			set
			{
				_metaDataBrowser = value;
			}
		}

		private void BindGrid(IPropertyCollection properties)
		{
			this.properties = properties;

			DataSet ds = new DataSet();
			DataTable dt = new DataTable("this");
			dt.Columns.Add("this",  Type.GetType("System.Object"));
			ds.Tables.Add(dt);
			dt.Rows.Add(new object[] { this });

			dt = new DataTable("MyData");
			DataColumn k = dt.Columns.Add("Key", stringType);
			k.AllowDBNull = false;
			DataColumn v = dt.Columns.Add("Value", stringType);
			v.AllowDBNull = false;
			ds.Tables.Add(dt);

			UniqueConstraint pk = new UniqueConstraint(k, false);

			dt.Constraints.Add(pk);
			ds.EnforceConstraints = true;

			this.colKey.HeaderText  = "Key";
			this.colKey.TextBox.Enabled = true;

			this.colValue.HeaderText  = "Value";
			this.colValue.MappingName = "Value";

			DataRowCollection rows = dt.Rows;

			foreach(IProperty prop in properties)
			{
				rows.Add(new object[] { prop.Key, prop.Value } );
			}

			this.Grid.DataSource = dt.DefaultView;
			this.InitializeGrid();

			this.colKey.TextBox.BorderStyle = BorderStyle.None;
			this.colValue.TextBox.BorderStyle = BorderStyle.None;

			this.colKey.TextBox.Move   += new System.EventHandler(this.ColorTextBox);
			this.colValue.TextBox.Move += new System.EventHandler(this.ColorTextBox);

			dt.RowChanged  += new DataRowChangeEventHandler(PropertyGridRowChanged);
			dt.RowDeleting += new DataRowChangeEventHandler(PropertyGridRowDeleting);
			dt.RowDeleted  += new DataRowChangeEventHandler(PropertyGridRowDeleted);
		}

		public void Edit(Table obj)
		{
			BindGrid(obj.GlobalProperties);
		}

//		public void Edit(Database obj)
//		{
//			BindGrid(obj.GlobalProperties);
//		}

		public void Edit(Domain obj)
		{
			BindGrid(obj.GlobalProperties);
		}

		public void Edit(Column obj)
		{
			BindGrid(obj.GlobalProperties);
		}

		public void Edit(Index obj)
		{
			BindGrid(obj.GlobalProperties);
		}

		public void Edit(MyMeta.View obj)
		{
			BindGrid(obj.GlobalProperties);
		}

		public void Edit(ForeignKey obj)
		{
			BindGrid(obj.GlobalProperties);
		}

		public void Edit(Procedure obj)
		{
			BindGrid(obj.GlobalProperties);
		}

		public void Edit(ResultColumn obj)
		{
			BindGrid(obj.GlobalProperties);
		}

		public void Edit(Parameter obj)
		{
			BindGrid(obj.GlobalProperties);
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobalUserMetaData));
            this.toolBarButton_Save = new System.Windows.Forms.ToolBarButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.Grid = new System.Windows.Forms.DataGrid();
            this.MyStyle = new System.Windows.Forms.DataGridTableStyle();
            this.colKey = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // toolBarButton_Save
            // 
            this.toolBarButton_Save.ImageIndex = 0;
            this.toolBarButton_Save.Name = "toolBarButton_Save";
            this.toolBarButton_Save.Tag = "save";
            this.toolBarButton_Save.ToolTipText = "Save Language Mappings";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            // 
            // toolBar1
            // 
            this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar1.AutoSize = false;
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton_Save});
            this.toolBar1.Divider = false;
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.imageList1;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(776, 26);
            this.toolBar1.TabIndex = 10;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // Grid
            // 
            this.Grid.BackColor = System.Drawing.Color.LightGreen;
            this.Grid.BackgroundColor = System.Drawing.Color.LightGreen;
            this.Grid.CaptionVisible = false;
            this.Grid.DataMember = "";
            this.Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.Grid.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Grid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.Grid.Location = new System.Drawing.Point(0, 26);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(776, 976);
            this.Grid.TabIndex = 20;
            this.Grid.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.MyStyle});
            // 
            // MyStyle
            // 
            this.MyStyle.AlternatingBackColor = System.Drawing.Color.LightGreen;
            this.MyStyle.BackColor = System.Drawing.Color.PaleGreen;
            this.MyStyle.DataGrid = this.Grid;
            this.MyStyle.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.colKey,
            this.colValue});
            this.MyStyle.GridLineColor = System.Drawing.Color.OliveDrab;
            this.MyStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.MyStyle.MappingName = "MyData";
            // 
            // colKey
            // 
            this.colKey.Format = "";
            this.colKey.FormatInfo = null;
            this.colKey.HeaderText = "Key";
            this.colKey.MappingName = "Key";
            this.colKey.NullText = "";
            this.colKey.Width = 75;
            // 
            // colValue
            // 
            this.colValue.Format = "";
            this.colValue.FormatInfo = null;
            this.colValue.HeaderText = "Value";
            this.colValue.MappingName = "Value";
            this.colValue.NullText = "";
            this.colValue.Width = 75;
            // 
            // GlobalUserMetaData
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(776, 1002);
            this.Controls.Add(this.Grid);
            this.Controls.Add(this.toolBar1);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GlobalUserMetaData";
            this.TabText = "Global User Meta Data";
            this.Text = "Global User Meta Data";
            this.Load += new System.EventHandler(this.GlobalUserMetaData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		public void DefaultSettingsChanged(DefaultSettings settings)
		{
			//PromptForSave(false);
			//this.Clear();
		}

		public void Clear()
		{
			this.Grid.DataSource = null;

			if(gridInitialized)
			{
				this.InitializeGrid();
			}
		}

		 public bool CanClose(bool allowPrevent)
		{
			return PromptForSave(allowPrevent);
		}

		private bool PromptForSave(bool allowPrevent)
		{
			bool canClose = true;

			if(this._metaDataBrowser.IsUserDataDirty)
			{
				DialogResult result;

				if(allowPrevent)
				{
					result = MessageBox.Show("The Global User Meta Data has been Modified, Do you wish to save the modifications?", 
						"Global User Meta Data", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				}
				else
				{
					result = MessageBox.Show("The Global User Meta Data has been Modified, Do you wish to save the modifications?", 
						"Global User Meta Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}

				switch(result)
				{
					case DialogResult.Yes:
					{
						_metaDataBrowser.SaveUserMetaData();
						MarkAsDirty(false);
					}
						break;

					case DialogResult.Cancel:

						canClose = false;
						break;
				}
			}

			return canClose;
		}

		private void GlobalUserMetaData_Load(object sender, System.EventArgs e)
		{
			this.toolBarButton_Save.Visible = false;
		}

		private void MarkAsDirty(bool dirty)
		{
			this._metaDataBrowser.IsUserDataDirty = dirty;
		}

		public void MetaDataStateChanged(bool dirty)
		{
			this.toolBarButton_Save.Visible = dirty;
		}

		public void MetaBrowserRefresh()
		{
			this.Clear();
		}

		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			DefaultSettings settings = DefaultSettings.Instance;

			switch(e.Button.Tag as string)
			{		
				case "save":

					_metaDataBrowser.SaveUserMetaData();
					MarkAsDirty(false);
					break;
			}
		}

		private void InitializeGrid()
		{
			if(!gridInitialized)
			{
				if(MyStyle.GridColumnStyles.Count > 0)
				{
					gridInitialized = true;
					gridHelper = new GridLayoutHelper(this.Grid, this.MyStyle,
						new decimal[] { 0.50M, 0.50M },	new int[] { 30, 30 });
				}
			}
		}

		private void ColorTextBox(object sender, System.EventArgs e)
		{
			TextBox txtBox = (TextBox)sender;

			// Bail if we're in la la land
			Size size = txtBox.Size;
			if(size.Width == 0 && size.Height == 0)
			{
				return;
			}

			int row = this.Grid.CurrentCell.RowNumber;
			int col = this.Grid.CurrentCell.ColumnNumber;

			if(isEven(row))
				txtBox.BackColor = Color.PaleGreen;
			else
				txtBox.BackColor = Color.LightGreen;

			if(col == 0)
			{
				if(txtBox.Text == string.Empty)
					txtBox.ReadOnly = false;
				else 
					txtBox.ReadOnly = true;
			}
		}

		private bool isEven(int x)
		{
			return (x & 1) == 0;
		}

		public string UserMetaDataFileName
		{
			get	{ return _userMetaDataFileName; }
			set	{_userMetaDataFileName = value; }
		}

		private static void PropertyGridRowChanged(object sender, DataRowChangeEventArgs e)
		{
			GlobalUserMetaData This = e.Row.Table.DataSet.Tables["this"].Rows[0]["this"] as GlobalUserMetaData;
			This.MarkAsDirty(true);
			This.PropertyGridRowChangedEvent(sender, e);
		}

		private static void PropertyGridRowDeleting(object sender, DataRowChangeEventArgs e)
		{
			GlobalUserMetaData This = e.Row.Table.DataSet.Tables["this"].Rows[0]["this"] as GlobalUserMetaData;
			This.MarkAsDirty(true);
			This.PropertyGridRowDeletingEvent(sender, e);
		}

		private static void PropertyGridRowDeleted(object sender, DataRowChangeEventArgs e)
		{
			GlobalUserMetaData This = e.Row.Table.DataSet.Tables["this"].Rows[0]["this"] as GlobalUserMetaData;
			This.MarkAsDirty(true);
			This.PropertyGridRowDeletedEvent(sender, e);
		}

		private void PropertyGridRowChangedEvent(object sender, DataRowChangeEventArgs e)
		{
			try
			{
				string sKey   = (string)e.Row["Key"];
				string sValue = (string)e.Row["Value"];

				IProperty p = this.properties[sKey];

				if(p != null)
				{
					p.Value = sValue;
				}
				else
				{
					this.properties.AddKeyValue(sKey, sValue);
				}
			}
			catch {}
		}

		private void PropertyGridRowDeletingEvent(object sender, DataRowChangeEventArgs e)
		{
			this.properties.RemoveKey(e.Row["Key"] as string);
		}

		private void PropertyGridRowDeletedEvent(object sender, DataRowChangeEventArgs e)
		{
			DataTable dt = e.Row.Table;

			dt.RowChanged  -= new DataRowChangeEventHandler(PropertyGridRowChanged);
			dt.RowDeleting -= new DataRowChangeEventHandler(PropertyGridRowDeleting);
			dt.RowDeleted  -= new DataRowChangeEventHandler(PropertyGridRowDeleted);

			dt.AcceptChanges();

			dt.RowChanged  += new DataRowChangeEventHandler(PropertyGridRowChanged);
			dt.RowDeleting += new DataRowChangeEventHandler(PropertyGridRowDeleting);
			dt.RowDeleted  += new DataRowChangeEventHandler(PropertyGridRowDeleted);
		}

		private string _userMetaDataFileName = string.Empty;		

		private IPropertyCollection properties = null;
		private GridLayoutHelper gridHelper;
		private bool gridInitialized = false;
		private MetaDataBrowser _metaDataBrowser = null;

        #region IMyGenContent Members

        public ToolStrip ToolStrip
        {
            get { return null; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            if (command == "UpdateDefaultSettings")
            {
                PromptForSave(false);
                this.Clear();
            }
        }

        public DockContent DockContent
        {
            get { return this; }
        }

        #endregion
    }
}
