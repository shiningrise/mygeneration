using System;
using System.Xml;
using System.Data;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

using MyMeta;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for UserMetaData.
	/// </summary>
    public class UserMetaData : DockContent, IMyGenContent
    {
        private IMyGenerationMDI mdi;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolBarButton toolBarButton_Save;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox txtNiceName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DataGrid Grid;
		private System.Windows.Forms.DataGridTableStyle MyStyle;
		private System.Windows.Forms.DataGridTextBoxColumn col_0;
		private System.Windows.Forms.DataGridTextBoxColumn col_1;
		private System.ComponentModel.IContainer components;

		private Type stringType = Type.GetType("System.String");

        protected override string GetPersistString()
        {
            return this.GetType().FullName;
        }

        public UserMetaData(IMyGenerationMDI mdi)
		{
            InitializeComponent();
            this.mdi = mdi;

			this.ShowHint = DockState.DockRight;

            DefaultSettings settings = DefaultSettings.Instance;
			this.UserMetaDataFileName = settings.UserMetaDataFileName;
		}

		public MetaDataBrowser MetaDataBrowser
		{
			set
			{
				_metaDataBrowser = value;
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserMetaData));
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.toolBarButton_Save = new System.Windows.Forms.ToolBarButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.Grid = new System.Windows.Forms.DataGrid();
            this.MyStyle = new System.Windows.Forms.DataGridTableStyle();
            this.col_0 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.col_1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtNiceName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
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
            this.toolBar1.Size = new System.Drawing.Size(832, 26);
            this.toolBar1.TabIndex = 1;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
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
            // Grid
            // 
            this.Grid.BackColor = System.Drawing.Color.Thistle;
            this.Grid.BackgroundColor = System.Drawing.Color.Thistle;
            this.Grid.CaptionVisible = false;
            this.Grid.DataMember = "";
            this.Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.Grid.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Grid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.Grid.Location = new System.Drawing.Point(0, 74);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(832, 939);
            this.Grid.TabIndex = 9;
            this.Grid.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.MyStyle});
            // 
            // MyStyle
            // 
            this.MyStyle.AlternatingBackColor = System.Drawing.Color.Thistle;
            this.MyStyle.BackColor = System.Drawing.Color.Plum;
            this.MyStyle.DataGrid = this.Grid;
            this.MyStyle.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.col_0,
            this.col_1});
            this.MyStyle.GridLineColor = System.Drawing.Color.Orchid;
            this.MyStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.MyStyle.MappingName = "MyData";
            // 
            // col_0
            // 
            this.col_0.Format = "";
            this.col_0.FormatInfo = null;
            this.col_0.MappingName = "Key";
            this.col_0.NullText = "";
            this.col_0.Width = 75;
            // 
            // col_1
            // 
            this.col_1.Format = "";
            this.col_1.FormatInfo = null;
            this.col_1.MappingName = "Value";
            this.col_1.NullText = "";
            this.col_1.Width = 75;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(832, 12);
            this.panel1.TabIndex = 8;
            // 
            // txtNiceName
            // 
            this.txtNiceName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtNiceName.Location = new System.Drawing.Point(0, 42);
            this.txtNiceName.Name = "txtNiceName";
            this.txtNiceName.Size = new System.Drawing.Size(832, 20);
            this.txtNiceName.TabIndex = 7;
            this.txtNiceName.Leave += new System.EventHandler(this.PropertyNiceNameLeave);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(832, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Alias:";
            // 
            // UserMetaData
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(832, 1013);
            this.Controls.Add(this.Grid);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtNiceName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolBar1);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UserMetaData";
            this.TabText = "User Meta Data";
            this.Text = "User Meta Data";
            this.Load += new System.EventHandler(this.UserMetaData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void UserMetaData_Load(object sender, System.EventArgs e)
		{
			this.toolBarButton_Save.Visible = false;
		}

		/*public void DefaultSettingsChanged(DefaultSettings settings)
		{
			PromptForSave(false);
			this.Clear();
		}*/

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
					result = MessageBox.Show("The User Meta Data has been Modified, Do you wish to save the modifications?", 
						"User Meta Data", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				}
				else
				{
					result = MessageBox.Show("The User Meta Data has been Modified, Do you wish to save the modifications?", 
						"User Meta Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}

				switch(result)
				{
					case DialogResult.Yes:
					{
						_metaDataBrowser.SaveUserMetaData();
					}
						break;

					case DialogResult.Cancel:

						canClose = false;
						break;
				}
			}

			return canClose;
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
					break;
			}
		}

		public string UserMetaDataFileName
		{
			get	{ return _userMetaDataFileName; }
			set	{_userMetaDataFileName = value; }
		}

		public void Clear()
		{
			this.Grid.DataSource = null;

			if(gridInitialized)
			{
				this.InitializeGrid();
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

		#region MyMeta.Collection Logic

		public void EditNiceNames(Databases coll)
		{
			DataTable dt = ProLogForNiceNames(coll);
			DataRowCollection rows = dt.Rows;

			foreach(Database o in coll)
			{
				rows.Add(new object[] { o.Name, o.Alias, o } );
			}

			EpilogForNiceNames(dt);
		}

		public void EditNiceNames(Columns coll)
		{
			DataTable dt = ProLogForNiceNames(coll);
			DataRowCollection rows = dt.Rows;

			foreach(Column o in coll)
			{
				rows.Add(new object[] { o.Name, o.Alias, o } );
			}

			EpilogForNiceNames(dt);
		}

		public void EditNiceNames(Tables coll)
		{
			DataTable dt = ProLogForNiceNames(coll);
			DataRowCollection rows = dt.Rows;

			foreach(Table o in coll)
			{
				rows.Add(new object[] { o.Name, o.Alias, o } );
			}

			EpilogForNiceNames(dt);
		}

		public void EditNiceNames(Views coll)
		{
			DataTable dt = ProLogForNiceNames(coll);
			DataRowCollection rows = dt.Rows;

			foreach(MyMeta.View o in coll)
			{
				rows.Add(new object[] { o.Name, o.Alias, o } );
			}

			EpilogForNiceNames(dt);
		}

		public void EditNiceNames(Procedures coll)
		{
			DataTable dt = ProLogForNiceNames(coll);
			DataRowCollection rows = dt.Rows;

			foreach(Procedure o in coll)
			{
				rows.Add(new object[] { o.Name, o.Alias, o } );
			}

			EpilogForNiceNames(dt);
		}

		public void EditNiceNames(Domains coll)
		{
			DataTable dt = ProLogForNiceNames(coll);
			DataRowCollection rows = dt.Rows;

			foreach(Domain o in coll)
			{
				rows.Add(new object[] { o.Name, o.Alias, o } );
			}

			EpilogForNiceNames(dt);
		}

		public void EditNiceNames(Parameters coll)
		{
			DataTable dt = ProLogForNiceNames(coll);
			DataRowCollection rows = dt.Rows;

			foreach(Parameter o in coll)
			{
				rows.Add(new object[] { o.Name, o.Alias, o } );
			}

			EpilogForNiceNames(dt);
		}

		public void EditNiceNames(Indexes coll)
		{
			DataTable dt = ProLogForNiceNames(coll);
			DataRowCollection rows = dt.Rows;

			foreach(Index o in coll)
			{
				rows.Add(new object[] { o.Name, o.Alias, o } );
			}

			EpilogForNiceNames(dt);
		}

		public void EditNiceNames(ForeignKeys coll)
		{
			DataTable dt = ProLogForNiceNames(coll);
			DataRowCollection rows = dt.Rows;

			foreach(ForeignKey o in coll)
			{
				rows.Add(new object[] { o.Name, o.Alias, o } );
			}

			EpilogForNiceNames(dt);
		}

		public void EditNiceNames(ResultColumns coll)
		{
			DataTable dt = ProLogForNiceNames(coll);
			DataRowCollection rows = dt.Rows;

			foreach(ResultColumn o in coll)
			{
				rows.Add(new object[] { o.Name, o.Alias, o } );
			}

			EpilogForNiceNames(dt);
		}

		private DataTable ProLogForNiceNames(MyMeta.Collection coll)
		{
			this.txtNiceName.Text = "";
			this.txtNiceName.Tag = null;

			DataSet ds = new DataSet();
			DataTable dt = new DataTable("this");
			dt.Columns.Add("this",  Type.GetType("System.Object"));
			ds.Tables.Add(dt);
			dt.Rows.Add(new object[] { this });

			dt = new DataTable("MyData");
			DataColumn k = dt.Columns.Add("Key", stringType);
			k.AllowDBNull = false;
			DataColumn a = dt.Columns.Add("Alias", stringType);
			a.AllowDBNull = false;

			dt.Columns.Add("obj", Type.GetType("System.Object"));
			ds.Tables.Add(dt);

			UniqueConstraint pk = new UniqueConstraint(a, false);

			dt.Constraints.Add(pk);
			ds.EnforceConstraints = true;

			this.txtNiceName.Enabled = false;

			this.col_0.HeaderText  = "Name";
			this.col_1.HeaderText  = "Alias";
			this.col_1.MappingName = "Alias";

			return dt;
		}

		private void EpilogForNiceNames(DataTable dt)
		{
			dt.DefaultView.AllowNew    = false;
			dt.DefaultView.AllowDelete = false;

			this.Grid.DataSource = dt.DefaultView;;
			this.InitializeGrid();
			
			this.col_0.TextBox.BorderStyle = BorderStyle.None;
			this.col_0.TextBox.ReadOnly    = true;
			this.col_1.TextBox.BorderStyle = BorderStyle.None;
 			this.col_1.TextBox.ReadOnly    = false;

			this.col_0.TextBox.Move += new System.EventHandler(this.ColorTextBox);
			this.col_1.TextBox.Move += new System.EventHandler(this.ColorTextBox);

			dt.RowChanged  += new DataRowChangeEventHandler(CollectionGridRowChanged);
		}

		private static void CollectionGridRowChanged(object sender, DataRowChangeEventArgs e)
		{
			UserMetaData This = e.Row.Table.DataSet.Tables["this"].Rows[0]["this"] as UserMetaData;
			This.CollectionGridRowChangedEvent(sender, e);
		}

		private void CollectionGridRowChangedEvent(object sender, DataRowChangeEventArgs e)
		{
			MyMeta.Single obj = e.Row["obj"] as MyMeta.Single;
			obj.Alias = e.Row["Alias"] as string;

			e.Row.Table.RowChanged  -= new DataRowChangeEventHandler(CollectionGridRowChanged);
			e.Row["Alias"] = obj.Alias;
			e.Row.Table.RowChanged  += new DataRowChangeEventHandler(CollectionGridRowChanged);
			MarkAsDirty(true);
		}

		#endregion
		
		#region MyMeta.Single Logic

		public void EditSingle(MyMeta.Single obj, string niceName)
		{
			this.single = obj;

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

			this.txtNiceName.Enabled = true;

			this.col_0.HeaderText  = "Key";
			this.col_0.TextBox.Enabled = true;

			this.col_1.HeaderText  = "Value";
			this.col_1.MappingName = "Value";

			this.txtNiceName.Text = niceName;
			this.txtNiceName.Tag = obj;

			IPropertyCollection properties = obj.Properties;
			DataRowCollection rows = dt.Rows;

			foreach(IProperty prop in properties)
			{
				rows.Add(new object[] { prop.Key, prop.Value } );
			}

			dt.AcceptChanges();

			this.Grid.DataSource = dt.DefaultView;
			this.InitializeGrid();

			this.col_0.TextBox.BorderStyle = BorderStyle.None;
			this.col_1.TextBox.BorderStyle = BorderStyle.None;
 
			this.col_0.TextBox.Move += new System.EventHandler(this.ColorTextBox);
			this.col_1.TextBox.Move += new System.EventHandler(this.ColorTextBox);
 
			dt.RowChanged  += new DataRowChangeEventHandler(PropertyGridRowChanged);
			dt.RowDeleting += new DataRowChangeEventHandler(PropertyGridRowDeleting);
			dt.RowDeleted  += new DataRowChangeEventHandler(PropertyGridRowDeleted);
		}

		private static void PropertyGridRowChanged(object sender, DataRowChangeEventArgs e)
		{
			UserMetaData This = e.Row.Table.DataSet.Tables["this"].Rows[0]["this"] as UserMetaData;
			This.MarkAsDirty(true);
			This.PropertyGridRowChangedEvent(sender, e);
		}

		private static void PropertyGridRowDeleting(object sender, DataRowChangeEventArgs e)
		{
			UserMetaData This = e.Row.Table.DataSet.Tables["this"].Rows[0]["this"] as UserMetaData;
			This.MarkAsDirty(true);
			This.PropertyGridRowDeletingEvent(sender, e);
		}

		private static void PropertyGridRowDeleted(object sender, DataRowChangeEventArgs e)
		{
			UserMetaData This = e.Row.Table.DataSet.Tables["this"].Rows[0]["this"] as UserMetaData;
			This.MarkAsDirty(true);
			This.PropertyGridRowDeletedEvent(sender, e);
		}

		private void PropertyGridRowChangedEvent(object sender, DataRowChangeEventArgs e)
		{
			try
			{
				string sKey   = (string)e.Row["Key"];
				string sValue = (string)e.Row["Value"];

				IProperty p = this.single.Properties[sKey];

				if(p != null)
				{
					p.Value = sValue;
				}
				else
				{
					this.single.Properties.AddKeyValue(sKey, sValue);
				}
			}
			catch {}
		}

		private void PropertyGridRowDeletingEvent(object sender, DataRowChangeEventArgs e)
		{
			this.single.Properties.RemoveKey(e.Row["Key"] as string);
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

		private void PropertyNiceNameLeave(object sender, System.EventArgs e)
		{
			object o = Grid.DataSource;
			if(null != o)
			{
				this.single.Alias = this.txtNiceName.Text;

				// Setting it to blank changes it back, let's reset in case
				this.txtNiceName.Text = this.single.Alias;

				MarkAsDirty(true);
			}
		}

		#endregion

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
				txtBox.BackColor = Color.Plum;
			else
				txtBox.BackColor = Color.Thistle;

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

		private string _userMetaDataFileName = string.Empty;

		private GridLayoutHelper gridHelper;
		private bool gridInitialized = false;
		private MetaDataBrowser _metaDataBrowser = null;
		private MyMeta.Single single = null;

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
                Clear();
            }
        }

        public DockContent DockContent
        {
            get { return this; }
        }

        #endregion
	}
}
