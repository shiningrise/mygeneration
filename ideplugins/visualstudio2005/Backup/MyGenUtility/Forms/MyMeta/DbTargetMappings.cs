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
	/// Summary description for DbTargetMappings.
	/// </summary>
    public class DbTargetMappings : DockContent, IMyGenContent
	{
        private IMyGenerationMDI mdi;
        GridLayoutHelper gridLayoutHelper;
		private System.ComponentModel.IContainer components;

		private System.Windows.Forms.ComboBox cboxDbTarget;
		private System.Windows.Forms.DataGrid XmlEditor;

		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.ToolBarButton toolBarButton_Save;
		private System.Windows.Forms.ToolBarButton toolBarButton_New;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolBarButton toolBarButton_Delete;
		private System.Windows.Forms.DataGridTextBoxColumn col_From;
		private System.Windows.Forms.DataGridTextBoxColumn col_To;

		private System.Windows.Forms.DataGridTableStyle MyXmlStyle;

        public DbTargetMappings(IMyGenerationMDI mdi)
		{
			InitializeComponent();
            this.mdi = mdi;
			this.ShowHint = DockState.DockRight;
        }

        protected override string GetPersistString()
        {
            return this.GetType().FullName;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbTargetMappings));
            this.XmlEditor = new System.Windows.Forms.DataGrid();
            this.MyXmlStyle = new System.Windows.Forms.DataGridTableStyle();
            this.col_From = new System.Windows.Forms.DataGridTextBoxColumn();
            this.col_To = new System.Windows.Forms.DataGridTextBoxColumn();
            this.cboxDbTarget = new System.Windows.Forms.ComboBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.toolBarButton_Save = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton_New = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton_Delete = new System.Windows.Forms.ToolBarButton();
            ((System.ComponentModel.ISupportInitialize)(this.XmlEditor)).BeginInit();
            this.SuspendLayout();
            // 
            // XmlEditor
            // 
            this.XmlEditor.AlternatingBackColor = System.Drawing.Color.Moccasin;
            this.XmlEditor.BackColor = System.Drawing.Color.LightGray;
            this.XmlEditor.BackgroundColor = System.Drawing.Color.LightGray;
            this.XmlEditor.CaptionVisible = false;
            this.XmlEditor.DataMember = "";
            this.XmlEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.XmlEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.XmlEditor.GridLineColor = System.Drawing.Color.BurlyWood;
            this.XmlEditor.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.XmlEditor.Location = new System.Drawing.Point(2, 49);
            this.XmlEditor.Name = "XmlEditor";
            this.XmlEditor.Size = new System.Drawing.Size(476, 951);
            this.XmlEditor.TabIndex = 7;
            this.XmlEditor.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.MyXmlStyle});
            // 
            // MyXmlStyle
            // 
            this.MyXmlStyle.AlternatingBackColor = System.Drawing.Color.LightGray;
            this.MyXmlStyle.BackColor = System.Drawing.Color.LightSteelBlue;
            this.MyXmlStyle.DataGrid = this.XmlEditor;
            this.MyXmlStyle.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.col_From,
            this.col_To});
            this.MyXmlStyle.GridLineColor = System.Drawing.Color.DarkGray;
            this.MyXmlStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.MyXmlStyle.MappingName = "DbTarget";
            // 
            // col_From
            // 
            this.col_From.Format = "";
            this.col_From.FormatInfo = null;
            this.col_From.HeaderText = "From";
            this.col_From.MappingName = "From";
            this.col_From.NullText = "";
            this.col_From.Width = 75;
            // 
            // col_To
            // 
            this.col_To.Format = "";
            this.col_To.FormatInfo = null;
            this.col_To.HeaderText = "To";
            this.col_To.MappingName = "To";
            this.col_To.NullText = "";
            this.col_To.Width = 75;
            // 
            // cboxDbTarget
            // 
            this.cboxDbTarget.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboxDbTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxDbTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxDbTarget.Location = new System.Drawing.Point(2, 2);
            this.cboxDbTarget.Name = "cboxDbTarget";
            this.cboxDbTarget.Size = new System.Drawing.Size(476, 21);
            this.cboxDbTarget.TabIndex = 12;
            this.cboxDbTarget.SelectionChangeCommitted += new System.EventHandler(this.cboxLanguage_SelectionChangeCommitted);
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
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton_Save,
            this.toolBarButton_New,
            this.toolBarButton1,
            this.toolBarButton_Delete});
            this.toolBar1.Divider = false;
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.imageList1;
            this.toolBar1.Location = new System.Drawing.Point(2, 23);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(476, 26);
            this.toolBar1.TabIndex = 14;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // toolBarButton_Save
            // 
            this.toolBarButton_Save.ImageIndex = 0;
            this.toolBarButton_Save.Name = "toolBarButton_Save";
            this.toolBarButton_Save.Tag = "save";
            this.toolBarButton_Save.ToolTipText = "Save DbTarget Mappings";
            // 
            // toolBarButton_New
            // 
            this.toolBarButton_New.ImageIndex = 2;
            this.toolBarButton_New.Name = "toolBarButton_New";
            this.toolBarButton_New.Tag = "new";
            this.toolBarButton_New.ToolTipText = "Create New DbTarget Mapping";
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton_Delete
            // 
            this.toolBarButton_Delete.ImageIndex = 1;
            this.toolBarButton_Delete.Name = "toolBarButton_Delete";
            this.toolBarButton_Delete.Tag = "delete";
            this.toolBarButton_Delete.ToolTipText = "Delete DbTarget Mappings";
            // 
            // DbTargetMappings
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(480, 1002);
            this.Controls.Add(this.XmlEditor);
            this.Controls.Add(this.toolBar1);
            this.Controls.Add(this.cboxDbTarget);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DbTargetMappings";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.TabText = "DbTarget Mappings";
            this.Text = "DbTarget Mappings";
            this.Load += new System.EventHandler(this.DbTargetMappings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.XmlEditor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public void DefaultSettingsChanged(DefaultSettings settings)
		{
		}

		public bool CanClose(bool allowPrevent)
		{
			return PromptForSave(allowPrevent);
		}

		private bool PromptForSave(bool allowPrevent)
		{
			bool canClose = true;

			if(this.isDirty)
			{
				DialogResult result;

				if(allowPrevent)
				{
					result = MessageBox.Show("The DbTarget Mappings have been Modified, Do you wish to save them?", 
						"DbTarget Mappings", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				}
				else
				{
					result = MessageBox.Show("The DbTarget Mappings have been Modified, Do you wish to save them?", 
						"DbTarget Mappings", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}

				switch(result)
				{
					case DialogResult.Yes:
					{
						DefaultSettings settings = DefaultSettings.Instance;
						xml.Save(settings.DbTargetMappingFile);
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

		private void MarkAsDirty(bool isDirty)
		{
			this.isDirty = isDirty;
			this.toolBarButton_Save.Visible = isDirty;
        }

        public new void Show(DockPanel dockManager)
        {
            DefaultSettings settings = DefaultSettings.Instance;
            if (!System.IO.File.Exists(settings.DbTargetMappingFile))
            {
                MessageBox.Show(this, "Database Target Mapping File does not exist at: " + settings.DbTargetMappingFile + "\r\nPlease fix this in DefaultSettings.");
            }
            else
            {
                base.Show(dockManager);
            }
        }

        private void DbTargetMappings_Load(object sender, System.EventArgs e)
        {
            this.col_From.TextBox.BorderStyle = BorderStyle.None;
            this.col_To.TextBox.BorderStyle = BorderStyle.None;

            this.col_From.TextBox.Move += new System.EventHandler(this.ColorTextBox);
            this.col_To.TextBox.Move += new System.EventHandler(this.ColorTextBox);

            DefaultSettings settings = DefaultSettings.Instance;

            this.dbDriver = settings.DbDriver;


            this.xml.Load(settings.DbTargetMappingFile);

            PopulateComboBox(settings);
            PopulateGrid(this.dbDriver);

            MarkAsDirty(false);

            gridLayoutHelper = new GridLayoutHelper(XmlEditor, MyXmlStyle, new decimal[] { 0.50M, 0.50M }, new int[] { 100, 100 });
        }

		private void cboxLanguage_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			DefaultSettings settings = DefaultSettings.Instance;
			PopulateGrid(this.dbDriver);
		}

		private void PopulateComboBox(DefaultSettings settings)
		{
			this.cboxDbTarget.Items.Clear();

			// Populate the ComboBox
			dbRoot myMeta = new dbRoot();
			myMeta.DbTargetMappingFileName	= settings.DbTargetMappingFile;
			myMeta.DbTarget					= settings.DbTarget;

			string[] targets = myMeta.GetDbTargetMappings(settings.DbDriver);

			if(null != targets)
			{
				for(int i = 0; i < targets.Length; i++)
				{
					this.cboxDbTarget.Items.Add(targets[i]);
				}

				this.cboxDbTarget.SelectedItem = myMeta.DbTarget;

				if(this.cboxDbTarget.SelectedIndex == -1)
				{
					// The default doesn't exist, set it to the first in the list
					this.cboxDbTarget.SelectedIndex = 0;
				}
			}
		}

		private void PopulateGrid(string dbDriver)
		{
			string target;

			if(this.cboxDbTarget.SelectedItem != null)
			{
				XmlEditor.Enabled = true;
				target = this.cboxDbTarget.SelectedItem as String;
			}
			else
			{
				XmlEditor.Enabled = false;
				target = "";
			}

			this.Text = "DbTarget Mappings for " + dbDriver;

			langNode = this.xml.SelectSingleNode(@"//DbTargets/DbTarget[@From='" + dbDriver + 
				"' and @To='" + target + "']");

			DataSet ds = new DataSet();
			DataTable dt = new DataTable("this");
			dt.Columns.Add("this",  Type.GetType("System.Object"));
			ds.Tables.Add(dt);
			dt.Rows.Add(new object[] { this });

			dt = new DataTable("DbTarget");
			DataColumn from = dt.Columns.Add("From", Type.GetType("System.String"));
			from.AllowDBNull = false;
			DataColumn to  = dt.Columns.Add("To", Type.GetType("System.String"));
			to.AllowDBNull = false;
			ds.Tables.Add(dt);

			UniqueConstraint pk = new UniqueConstraint(from, false);
			dt.Constraints.Add(pk);
			ds.EnforceConstraints = true;

			if(null != langNode)
			{
				foreach(XmlNode mappingpNode in langNode.ChildNodes)
				{
					XmlAttributeCollection attrs = mappingpNode.Attributes;

					string sFrom = attrs["From"].Value;
					string sTo   = attrs["To"].Value;

					dt.Rows.Add( new object[] { sFrom, sTo } );
				}

				dt.AcceptChanges();
			}

			XmlEditor.DataSource = dt;

			dt.RowChanged  += new DataRowChangeEventHandler(GridRowChanged);
			dt.RowDeleting += new DataRowChangeEventHandler(GridRowDeleting);
			dt.RowDeleted  += new DataRowChangeEventHandler(GridRowDeleted);
		}

		private static void GridRowChanged(object sender, DataRowChangeEventArgs e)
		{
			DbTargetMappings This = e.Row.Table.DataSet.Tables["this"].Rows[0]["this"] as DbTargetMappings;
			This.MarkAsDirty(true);
			This.GridRowChangedEvent(sender, e);
		}

		private static void GridRowDeleting(object sender, DataRowChangeEventArgs e)
		{
			DbTargetMappings This = e.Row.Table.DataSet.Tables["this"].Rows[0]["this"] as DbTargetMappings;
			This.MarkAsDirty(true);
			This.GridRowDeletingEvent(sender, e);
		}

		private static void GridRowDeleted(object sender, DataRowChangeEventArgs e)
		{
			DbTargetMappings This = e.Row.Table.DataSet.Tables["this"].Rows[0]["this"] as DbTargetMappings;
			This.MarkAsDirty(true);
			This.GridRowDeletedEvent(sender, e);
		}

		private void GridRowChangedEvent(object sender, DataRowChangeEventArgs e)
		{
			try
			{
				string sFrom = e.Row["From"] as string;
				string sTo   = e.Row["To"]   as string;

				string  xPath;
				XmlNode typeNode;

				switch(e.Row.RowState)
				{
					case DataRowState.Added:
					{
						typeNode = langNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Type", null);
						langNode.AppendChild(typeNode);

						XmlAttribute attr;

						attr = langNode.OwnerDocument.CreateAttribute("From");
						attr.Value = sFrom;
						typeNode.Attributes.Append(attr);

						attr = langNode.OwnerDocument.CreateAttribute("To");
						attr.Value = sTo;
						typeNode.Attributes.Append(attr);
					}
						break;

					case DataRowState.Modified:
					{
						xPath = @"./Type[@From='" + sFrom + "']";
						typeNode = langNode.SelectSingleNode(xPath);

						typeNode.Attributes["To"].Value = sTo;
					}
						break;
				}
			}
			catch{}
		}

		private void GridRowDeletingEvent(object sender, DataRowChangeEventArgs e)
		{
			string xPath = @"./Type[@From='" + e.Row["From"] + "' and @To='" + e.Row["To"] + "']";
			XmlNode node = langNode.SelectSingleNode(xPath);

			if(node != null)
			{
				node.ParentNode.RemoveChild(node);
			}
		}

		private void GridRowDeletedEvent(object sender, DataRowChangeEventArgs e)
		{
			DataTable dt = e.Row.Table;

			dt.RowChanged  -= new DataRowChangeEventHandler(GridRowChanged);
			dt.RowDeleting -= new DataRowChangeEventHandler(GridRowDeleting);
			dt.RowDeleted  -= new DataRowChangeEventHandler(GridRowDeleted);

			dt.AcceptChanges();

			dt.RowChanged  += new DataRowChangeEventHandler(GridRowChanged);
			dt.RowDeleting += new DataRowChangeEventHandler(GridRowDeleting);
			dt.RowDeleted  += new DataRowChangeEventHandler(GridRowDeleted);
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

			int row = this.XmlEditor.CurrentCell.RowNumber;
			int col = this.XmlEditor.CurrentCell.ColumnNumber;

			if(isEven(row))
				txtBox.BackColor = Color.LightSteelBlue;
			else
				txtBox.BackColor = Color.LightGray;

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

		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			DefaultSettings settings = DefaultSettings.Instance;

			switch(e.Button.Tag as string)
			{		
				case "save":

					xml.Save(settings.DbTargetMappingFile);
					MarkAsDirty(false);
					break;

				case "new":
				{
					int count = this.cboxDbTarget.Items.Count;
					string[] dbTargets = new string[count];

					for(int i = 0; i < this.cboxDbTarget.Items.Count; i++)
					{
						dbTargets[i] = this.cboxDbTarget.Items[i] as string;
					}
					
					AddDbTargetMappingDialog dialog = new AddDbTargetMappingDialog(dbTargets, this.dbDriver);
					if(dialog.ShowDialog() == DialogResult.OK)
					{
						if(dialog.BasedUpon != string.Empty)
						{
							string xPath = @"//DbTargets/DbTarget[@From='" + this.dbDriver + "' and @To='" + 
								dialog.BasedUpon + "']";

							XmlNode node = xml.SelectSingleNode(xPath);

							XmlNode newNode = node.CloneNode(true);
							newNode.Attributes["To"].Value = dialog.NewDbTarget;

							node.ParentNode.AppendChild(newNode);
						}
						else
						{
							XmlNode parentNode = xml.SelectSingleNode(@"//DbTargets");

							XmlAttribute attr;

							// Language Node
							langNode = xml.CreateNode(XmlNodeType.Element, "DbTarget", null);
							parentNode.AppendChild(langNode);

							attr = xml.CreateAttribute("From");
							attr.Value = settings.DbDriver;
							langNode.Attributes.Append(attr);

							attr = xml.CreateAttribute("To");
							attr.Value = dialog.NewDbTarget;
							langNode.Attributes.Append(attr);
						}

						this.cboxDbTarget.Items.Add(dialog.NewDbTarget);
						this.cboxDbTarget.SelectedItem = dialog.NewDbTarget;

						PopulateGrid(this.dbDriver);
						MarkAsDirty(true);
					}
				}
				break;

				case "delete":

					if(this.cboxDbTarget.SelectedItem != null)
					{
						string target = this.cboxDbTarget.SelectedItem as String;

						DialogResult result = MessageBox.Show("Delete '" + target + "' Mappings. Are you sure?", 
							"Delete DbTarget Mappings", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

						if(result == DialogResult.Yes)
						{
							string xPath = @"//DbTargets/DbTarget[@From='" + this.dbDriver + "' and @To='" + target + "']";
							XmlNode node = xml.SelectSingleNode(xPath);
							node.ParentNode.RemoveChild(node);

							this.cboxDbTarget.Items.Remove(target);
							if(this.cboxDbTarget.Items.Count > 0)
							{
								this.cboxDbTarget.SelectedItem = this.cboxDbTarget.SelectedIndex = 0;
							}
							PopulateGrid(this.dbDriver);
							MarkAsDirty(true);
						}
					}
					break;
			}
		}

		private XmlDocument xml = new XmlDocument();
		private XmlNode langNode = null;
		private string dbDriver = "";
		private bool isDirty = false;

        #region IMyGenContent Members

        public ToolStrip ToolStrip
        {
            get { return null; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            if (command.Equals("UpdateDefaultSettings", StringComparison.CurrentCultureIgnoreCase))
            {
                PromptForSave(false);

                DefaultSettings settings = DefaultSettings.Instance;
                this.dbDriver = settings.DbDriver;

                PopulateComboBox(settings);
                PopulateGrid(this.dbDriver);

                MarkAsDirty(false);
            }
        }

        public DockContent DockContent
        {
            get { return this; }
        }

        #endregion
    }
}
