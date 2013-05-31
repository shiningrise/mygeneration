using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;

using ADODB;

using WeifenLuo.WinFormsUI.Docking;

using MyMeta;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for MetaDataBrowser.
	/// </summary>
    public class MetaDataBrowser : DockContent, IMyGenContent
    {
        private static TreeNode static_rootNode, loadingNode;
        private static dbRoot static_myMeta;

        public bool _isUserDataDirty = false;
        private IMyGenerationMDI mdi = null;
        public MetaProperties MetaData = null;
        public UserMetaData UserData = null;
        public GlobalUserMetaData GlobalUserData = null;
        private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.ImageList ToolbarImageList;
		private System.ComponentModel.IContainer components;

		private string startupPath;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.TreeView MyTree;
		public System.Windows.Forms.ImageList TreeImageList;
		private System.Windows.Forms.CheckBox chkSystem;
        private ToolBarButton toolBarButtonExecute;
		private System.Windows.Forms.ToolBarButton toolBarButton2;

        private delegate void AddRootNodeCallback();
        private System.Windows.Forms.Timer timerIconAnimate;

        private  static dbRoot StaticMyMeta
        {
            get
            {
                if (static_myMeta == null) static_myMeta = new dbRoot();
                return static_myMeta;
            }
            set
            {
                if (value == null && static_myMeta != null)
                {
                    static_myMeta.Dispose();
                    static_myMeta = null;
                }
            }
        }

        public static object StaticMyMetaObj
        {
            get
            {
                return StaticMyMeta;
            }
        }

        private class AsyncLoadInfo 
        {
            public TreeNode RootNode; 
            //public dbRoot MyMeta; 
            public bool ShowSystemEntities; 
            public string Error;
            public TreeNode BlankNode
            {
                get
                {
                    TreeNode blankNode = new TreeNode(string.Empty);
                    blankNode.Tag = "Blank";
                    return blankNode;
                }
            }
        }

        public MetaDataBrowser(IMyGenerationMDI mdi, MetaProperties p, UserMetaData u, GlobalUserMetaData g)
		{
            InitializeComponent();
            this.mdi = mdi;
            this.MetaData = p;
            this.UserData = u;
            this.GlobalUserData = g;
			this.ShowHint = DockState.DockLeft;

            this.UserData.MetaDataBrowser = this;
            this.GlobalUserData.MetaDataBrowser = this;

            loadingNode = new TreeNode("Tree Loading...");

            MetaDataBrowser_Enter(this, EventArgs.Empty);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MetaDataBrowser));
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonExecute = new System.Windows.Forms.ToolBarButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.ToolbarImageList = new System.Windows.Forms.ImageList(this.components);
            this.MyTree = new System.Windows.Forms.TreeView();
            this.TreeImageList = new System.Windows.Forms.ImageList(this.components);
            this.chkSystem = new System.Windows.Forms.CheckBox();
            this.timerIconAnimate = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // toolBar1
            // 
            this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton2,
            this.toolBarButtonExecute});
            this.toolBar1.Divider = false;
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.imageList1;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(264, 26);
            this.toolBar1.TabIndex = 2;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // toolBarButton2
            // 
            this.toolBarButton2.ImageIndex = 0;
            this.toolBarButton2.Name = "toolBarButton2";
            this.toolBarButton2.Tag = "refresh";
            // 
            // toolBarButtonExecute
            // 
            this.toolBarButtonExecute.Enabled = false;
            this.toolBarButtonExecute.ImageIndex = 1;
            this.toolBarButtonExecute.Name = "toolBarButtonExecute";
            this.toolBarButtonExecute.Tag = "execute";
            this.toolBarButtonExecute.ToolTipText = "Execute SQL in active document";
            this.toolBarButtonExecute.Visible = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "toolbar_execute_16x16.bmp");
            // 
            // ToolbarImageList
            // 
            this.ToolbarImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ToolbarImageList.ImageStream")));
            this.ToolbarImageList.TransparentColor = System.Drawing.Color.Fuchsia;
            this.ToolbarImageList.Images.SetKeyName(0, "");
            this.ToolbarImageList.Images.SetKeyName(1, "");
            this.ToolbarImageList.Images.SetKeyName(2, "");
            this.ToolbarImageList.Images.SetKeyName(3, "");
            this.ToolbarImageList.Images.SetKeyName(4, "");
            this.ToolbarImageList.Images.SetKeyName(5, "");
            this.ToolbarImageList.Images.SetKeyName(6, "");
            this.ToolbarImageList.Images.SetKeyName(7, "");
            this.ToolbarImageList.Images.SetKeyName(8, "");
            // 
            // MyTree
            // 
            this.MyTree.BackColor = System.Drawing.Color.LightYellow;
            this.MyTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MyTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MyTree.FullRowSelect = true;
            this.MyTree.HideSelection = false;
            this.MyTree.HotTracking = true;
            this.MyTree.ImageIndex = 21;
            this.MyTree.ImageList = this.TreeImageList;
            this.MyTree.Location = new System.Drawing.Point(0, 26);
            this.MyTree.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.MyTree.Name = "MyTree";
            this.MyTree.SelectedImageIndex = 21;
            this.MyTree.Size = new System.Drawing.Size(264, 711);
            this.MyTree.TabIndex = 4;
            this.MyTree.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MyTree_MouseClick);
            this.MyTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.MyTree_BeforeExpand);
            this.MyTree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.MyTree_BeforeSelect);
            // 
            // TreeImageList
            // 
            this.TreeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TreeImageList.ImageStream")));
            this.TreeImageList.TransparentColor = System.Drawing.Color.White;
            this.TreeImageList.Images.SetKeyName(0, "");
            this.TreeImageList.Images.SetKeyName(1, "");
            this.TreeImageList.Images.SetKeyName(2, "");
            this.TreeImageList.Images.SetKeyName(3, "");
            this.TreeImageList.Images.SetKeyName(4, "");
            this.TreeImageList.Images.SetKeyName(5, "");
            this.TreeImageList.Images.SetKeyName(6, "");
            this.TreeImageList.Images.SetKeyName(7, "");
            this.TreeImageList.Images.SetKeyName(8, "");
            this.TreeImageList.Images.SetKeyName(9, "");
            this.TreeImageList.Images.SetKeyName(10, "");
            this.TreeImageList.Images.SetKeyName(11, "");
            this.TreeImageList.Images.SetKeyName(12, "");
            this.TreeImageList.Images.SetKeyName(13, "");
            this.TreeImageList.Images.SetKeyName(14, "");
            this.TreeImageList.Images.SetKeyName(15, "");
            this.TreeImageList.Images.SetKeyName(16, "");
            this.TreeImageList.Images.SetKeyName(17, "");
            this.TreeImageList.Images.SetKeyName(18, "");
            this.TreeImageList.Images.SetKeyName(19, "");
            this.TreeImageList.Images.SetKeyName(20, "");
            this.TreeImageList.Images.SetKeyName(21, "");
            this.TreeImageList.Images.SetKeyName(22, "");
            this.TreeImageList.Images.SetKeyName(23, "");
            this.TreeImageList.Images.SetKeyName(24, "");
            this.TreeImageList.Images.SetKeyName(25, "");
            this.TreeImageList.Images.SetKeyName(26, "Refresh16x16_1.bmp");
            this.TreeImageList.Images.SetKeyName(27, "Refresh16x16_2.bmp");
            this.TreeImageList.Images.SetKeyName(28, "Refresh16x16_3.bmp");
            this.TreeImageList.Images.SetKeyName(29, "Refresh16x16_4.bmp");
            // 
            // chkSystem
            // 
            this.chkSystem.Location = new System.Drawing.Point(28, 4);
            this.chkSystem.Name = "chkSystem";
            this.chkSystem.Size = new System.Drawing.Size(148, 16);
            this.chkSystem.TabIndex = 5;
            this.chkSystem.Text = "Show System Data";
            this.chkSystem.CheckedChanged += new System.EventHandler(this.chkSystem_CheckedChanged);
            // 
            // timerIconAnimate
            // 
            this.timerIconAnimate.Interval = 75;
            this.timerIconAnimate.Tick += new System.EventHandler(this.timerIconAnimate_Tick);
            // 
            // MetaDataBrowser
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(264, 737);
            this.ControlBox = false;
            this.Controls.Add(this.chkSystem);
            this.Controls.Add(this.MyTree);
            this.Controls.Add(this.toolBar1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MetaDataBrowser";
            this.TabText = "MyMeta  Browser";
            this.Text = "MyMeta  Browser";
            this.Load += new System.EventHandler(this.MetaDataBrowser_Load);
            this.Enter += new System.EventHandler(this.MetaDataBrowser_Enter);
            this.ResumeLayout(false);
            this.PerformLayout();
		}
		#endregion

        #region Load Tree Asyncronously
        private IAsyncResult asyncres;

        private void timerIconAnimate_Tick(object sender, EventArgs e)
        {
            if (loadingNode != null)
            {
                loadingNode.ImageIndex = (loadingNode.ImageIndex >= 29) ? 26 : (loadingNode.ImageIndex + 1);
                loadingNode.SelectedImageIndex = loadingNode.ImageIndex;
                MyTree.Invalidate();
            }
        }

        private bool IsTreeBusy
        {
            get
            {
                return ((asyncres != null) && !asyncres.IsCompleted);
            }
        }

        public void SetupAsync()
        {
            if (IsTreeBusy) return;

            this.toolBar1.Enabled = false;

            this.Text = "MyMeta Browser (" + DefaultSettings.Instance.DbDriver + ")";

            static_rootNode = new TreeNode();

            this.MyTree.BeginUpdate();
            this.MyTree.Nodes.Clear();
            this.MyTree.Nodes.Add(loadingNode);
            this.MyTree.EndUpdate();
            this.MyTree.Invalidate();

            loadingNode.ImageIndex = 26;
            loadingNode.SelectedImageIndex = loadingNode.ImageIndex;
            this.MyTree.Invalidate();

            timerIconAnimate.Start();

            // RESET MYMETA!
            StaticMyMeta = null;

            AsyncLoadInfo ali = new AsyncLoadInfo();
            ali.RootNode = static_rootNode;
            ali.ShowSystemEntities = this.chkSystem.Checked;
            ali.Error = string.Empty;

            this.Invalidate();
            this.Refresh();

            ParameterizedThreadStart ts = new ParameterizedThreadStart(SetupAndBuildNodeTree);
            asyncres = ts.BeginInvoke(ali, new AsyncCallback(SetupAsyncCompleted), null);
        }

        private void SetupAsyncCompleted(IAsyncResult ar)
        {
            this.AddRootNodeThreadSafe();
        }

        private void AddRootNodeThreadSafe()
        {
            if (this.InvokeRequired)
            {
                AddRootNodeCallback d = new AddRootNodeCallback(AddRootNodeThreadSafe);
                this.Invoke(d, new object[] { });
            }
            else
            {
                timerIconAnimate.Stop();

                this.MyTree.BeginUpdate();
                this.MyTree.Nodes.Clear();
                if (static_rootNode != null)
                {
                    foreach (TreeNode root in static_rootNode.Nodes)
                    {
                        this.MyTree.Nodes.Add(root);
                    }
                }

                this.MyTree.EndUpdate();
                this.MyTree.Scrollable = true;

                this.UserData.MetaBrowserRefresh();
                this.GlobalUserData.MetaBrowserRefresh();
                this.MetaData.MetaBrowserRefresh();

                this.toolBar1.Enabled = true;
                this.Invalidate();
                this.Refresh();
            }
        }

        private static void SetupAndBuildNodeTree(object parentObj)
        {
            AsyncLoadInfo ali = parentObj as AsyncLoadInfo;
            DefaultSettings settings = DefaultSettings.Instance;

            //Setup
            try 
            {
                if (string.IsNullOrEmpty(ali.Error)) 
				{
                    StaticMyMeta.Connect(settings.DbDriver, settings.ConnectionString);
                    StaticMyMeta.ShowDefaultDatabaseOnly = settings.ShowDefaultDatabaseOnly;
                    StaticMyMeta.LanguageMappingFileName = settings.LanguageMappingFile;
                    StaticMyMeta.Language = settings.Language;
                    StaticMyMeta.DbTargetMappingFileName = settings.DbTargetMappingFile;
                    StaticMyMeta.DbTarget = settings.DbTarget;
                    StaticMyMeta.UserMetaDataFileName = settings.UserMetaDataFileName;

                    StaticMyMeta.DomainOverride = settings.DomainOverride;

                    StaticMyMeta.ShowSystemData = ali.ShowSystemEntities;

                    StaticMyMeta.UserDataDatabaseMappings.Clear();
                    foreach (string key in settings.DatabaseUserDataXmlMappings.Keys)
                    {
                        StaticMyMeta.UserDataDatabaseMappings[key] = settings.DatabaseUserDataXmlMappings[key];
                    }
				}
			}
			catch(Exception ex)
			{
                ali.Error = ex.Message;
			}

            //build tree
			InitializeTreeAsync(ali);
        }

        private static void InitializeTreeAsync(AsyncLoadInfo ali)
        {
            string nodeText = string.IsNullOrEmpty(ali.Error) ? StaticMyMeta.DriverString : ali.Error;

            TreeNode serverNode = new TreeNode("MyMeta (" + nodeText + ")");
            serverNode.Tag = new NodeData(NodeType.MYMETA, StaticMyMeta);
            serverNode.SelectedImageIndex = serverNode.ImageIndex = 21;
            ali.RootNode.Nodes.Add(serverNode);

            if (!string.IsNullOrEmpty(ali.Error))
            {
                // There's an error when trying to connect, let's bail with the error text in the node 
                serverNode.Expand();
            }
            else if (StaticMyMeta.Databases != null)
            {
                try
                {
                    TreeNode databasesNode = new TreeNode("Databases");
                    databasesNode.Tag = new NodeData(NodeType.DATABASES, StaticMyMeta.Databases);
                    databasesNode.SelectedImageIndex = databasesNode.ImageIndex = 0;

                    serverNode.Nodes.Add(databasesNode);

                    //foreach (IDatabase database in ali.MyMeta.Databases)
                    foreach (IDatabase database in StaticMyMeta.Databases)
                    {
                        TreeNode dbNode = new TreeNode(database.Name);
                        dbNode.Tag = new NodeData(NodeType.DATABASE, database);
                        dbNode.SelectedImageIndex = dbNode.ImageIndex = 1;
                        dbNode.Nodes.Add(ali.BlankNode);
                        databasesNode.Nodes.Add(dbNode);
                    }

                    serverNode.Expand();
                    databasesNode.Expand();
                }
                catch (Exception ex)
                {
                    if (serverNode != null)
                    {
                        serverNode.Text = "MyMeta (" + ex.Message + " )";
                    }
                }
            }
        }
        #endregion

        public void DefaultSettingsChanged(DefaultSettings settings)
		{
			bool doRefresh = false;

            if (!IsTreeBusy)
            {
                try
                {
                    if ((StaticMyMeta.DriverString != settings.DbDriver) ||
                        (StaticMyMeta.ConnectionString != settings.ConnectionString) ||
                        (StaticMyMeta.ShowDefaultDatabaseOnly != settings.ShowDefaultDatabaseOnly) ||
                        (StaticMyMeta.LanguageMappingFileName != settings.LanguageMappingFile) ||
                        (StaticMyMeta.Language != settings.Language) ||
                        (StaticMyMeta.DbTargetMappingFileName != settings.DbTargetMappingFile) ||
                        (StaticMyMeta.DbTarget != settings.DbTarget) ||
                        (StaticMyMeta.UserMetaDataFileName != settings.UserMetaDataFileName) ||
                        (StaticMyMeta.DomainOverride != settings.DomainOverride) ||
                        (!CompareUserDataDatabaseMappings(StaticMyMeta.UserDataDatabaseMappings, settings.DatabaseUserDataXmlMappings)))
                    {
                        doRefresh = true;
                    }
                }
                catch
                {
                    doRefresh = true;
                }
            }

            if (doRefresh) this.SetupAsync();
		}

        private bool CompareUserDataDatabaseMappings(Dictionary<string, string> d1, Dictionary<string, string>  d2)
        {
            bool same = false;
            if (d1.Count == d2.Count) 
            {
                same = true;
                foreach (string key in d1.Keys)
                {
                    if (!d2.ContainsKey(key) || (d2[key] != d1[key]))
                    {
                        same = false;
                        break;
                    }
                }
            }
            return same;
        }

		private void OpenUserData()
		{
			System.IO.Stream myStream;
			OpenFileDialog openFileDialog = new OpenFileDialog();
       
			openFileDialog.InitialDirectory = this.startupPath + @"\settings";
			openFileDialog.Filter = "User Data File (*.xml)|*.xml";
			openFileDialog.RestoreDirectory = true;
       
			if(openFileDialog.ShowDialog() == DialogResult.OK)
			{
				myStream = openFileDialog.OpenFile();
				if(null != myStream) 
				{
					myStream.Close();
				}
			}
		}

		private void MetaDataBrowser_Load(object sender, System.EventArgs e)
		{
            this.startupPath = Zeus.FileTools.ApplicationPath;

			DefaultSettings settings = DefaultSettings.Instance;
            
            this.Setup();

		}
        
#if !DEBUG
		private static bool isFirstRun = true;
#endif
        private void Setup()
		{
            SetupAsync();
        }

		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            if (IsTreeBusy) return; 

			switch(e.Button.Tag as string)
			{		
				case "refresh":

					if(this.IsUserDataDirty)
					{
						DialogResult result = MessageBox.Show("The User Meta Data has been Modified, Do you wish to save the modifications?", 
							"User Meta Data", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

						if(result == DialogResult.Yes)
						{
							this.SaveUserMetaData();
						}
					}

					//DefaultSettings settings = DefaultSettings.Instance;
					//this.Setup(settings);
                    this.Setup();
					break;
                case "execute":
                    if (this.mdi.DockPanel.ActiveDocument != null)
                    {
                        if ((this.mdi.DockPanel.ActiveDocument is IMyGenDocument) && (StaticMyMeta.IsConnected))
                        {
                            IMyGenDocument doc = this.mdi.DockPanel.ActiveDocument as IMyGenDocument;
                            
                            Recordset rs = StaticMyMeta.DefaultDatabase.ExecuteSql(doc.TextContent);
                            if ((rs != null) && (rs.State != (int)ADODB.ObjectStateEnum.adStateClosed)) rs.Close();
                        }
                    }
                    break;
			}
		}

		private void chkSystem_CheckedChanged(object sender, System.EventArgs e)
		{
            this.Setup();
		}

		public string ConfigurationPath
		{
			get
			{
				return _configurationPath;
			}

			set
			{
				_configurationPath = value;
			}
		}

		private string _configurationPath;

		enum NodeType
		{
			BLANK,
			MYMETA,
			DATABASES,
			DATABASE,
			TABLES,
			TABLE,
			VIEWS,
			VIEW,
			SUBVIEWS,
			SUBTABLES,
			PROCEDURES,
			PROCEDURE,
			COLUMNS,
			COLUMN,
			FOREIGNKEYS,
			INDIRECTFOREIGNKEYS,
			FOREIGNKEY,
			INDEXES,
			INDEX,
			PRIMARYKEYS,
			PRIMARKYKEY,
			PARAMETERS,
			PARAMETER,
			RESULTCOLUMNS,
			RESULTCOLUMN,
			DOMAINS,
			DOMAIN,
			PROVIDERTYPE,
			PROVIDERTYPES
		}

		private class NodeData
		{
			public NodeData(NodeType type, object meta)
			{
				this.Type = type;
				this.Meta = meta;
			}

			public NodeType Type = NodeType.BLANK;
			public object   Meta = null;
		}

		private TreeNode BlankNode
		{
			get
			{
				TreeNode blankNode = new TreeNode("");
				blankNode.Tag = "Blank";
				return blankNode;
			}
		}

		private bool HasBlankNode(TreeNode node)
		{
			if(node.Nodes.Count == 1 && "Blank" == node.Nodes[0].Tag as string)
			{
				node.Nodes.Clear();
				return true;
			}
			else
				return false;
        }

        private void BeforeNodeSelected(TreeNode node)
        {
            if (node != null)
            {
                if (this.MetaData == null || node.Tag == null) return;

                NodeData data = node.Tag as NodeData;
                MetaObject obj = null;

                if (data.Type != NodeType.MYMETA)
                {
                    obj = data.Meta as MetaObject;
                }

                if (data != null)
                {
                    switch (data.Type)
                    {
                        case NodeType.COLUMNS:
                            UserData.EditNiceNames(data.Meta as Columns);
                            break;

                        case NodeType.DATABASES:
                            UserData.EditNiceNames(data.Meta as Databases);
                            break;

                        case NodeType.TABLES:
                        case NodeType.SUBTABLES:
                            UserData.EditNiceNames(data.Meta as Tables);
                            break;

                        case NodeType.VIEWS:
                        case NodeType.SUBVIEWS:
                            UserData.EditNiceNames(data.Meta as Views);
                            break;

                        case NodeType.FOREIGNKEYS:
                        case NodeType.INDIRECTFOREIGNKEYS:
                            UserData.EditNiceNames(data.Meta as ForeignKeys);
                            break;

                        case NodeType.PARAMETERS:
                            UserData.EditNiceNames(data.Meta as MyMeta.Parameters);
                            break;

                        case NodeType.RESULTCOLUMNS:
                            UserData.EditNiceNames(data.Meta as ResultColumns);
                            break;

                        case NodeType.INDEXES:
                            UserData.EditNiceNames(data.Meta as Indexes);
                            break;

                        case NodeType.PROCEDURES:
                            UserData.EditNiceNames(data.Meta as Procedures);
                            break;

                        case NodeType.DOMAINS:
                            UserData.EditNiceNames(data.Meta as Domains);
                            break;

                        default:
                            UserData.Clear();
                            break;
                    }

                    switch (data.Type)
                    {
                        case NodeType.DATABASE:
                            {
                                Database o = obj as Database;
                                MetaData.DisplayDatabaseProperties(o, node);
                                UserData.EditSingle(o, o.Alias);
                                //GlobalUserData.Edit(o);
                            }
                            break;

                        case NodeType.COLUMN:
                            {
                                Column o = obj as Column;
                                MetaData.DisplayColumnProperties(o, node);
                                UserData.EditSingle(o, o.Alias);
                                GlobalUserData.Edit(o);
                            }
                            break;

                        case NodeType.TABLE:
                            {

                                Table o = obj as Table;
                                MetaData.DisplayTableProperties(o, node);
                                UserData.EditSingle(o, o.Alias);
                                GlobalUserData.Edit(o);
                            }
                            break;

                        case NodeType.VIEW:
                            {

                                MyMeta.View o = obj as MyMeta.View;
                                MetaData.DisplayViewProperties(o, node);
                                UserData.EditSingle(o, o.Alias);
                                GlobalUserData.Edit(o);
                            }
                            break;

                        case NodeType.PARAMETER:
                            {

                                MyMeta.Parameter o = obj as MyMeta.Parameter;
                                MetaData.DisplayParameterProperties(o, node);
                                UserData.EditSingle(o, o.Alias);
                                GlobalUserData.Edit(o);
                            }
                            break;

                        case NodeType.RESULTCOLUMN:
                            {

                                ResultColumn o = obj as ResultColumn;
                                MetaData.DisplayResultColumnProperties(o, node);
                                UserData.EditSingle(o, o.Alias);
                                GlobalUserData.Edit(o);
                            }
                            break;

                        case NodeType.FOREIGNKEY:
                            {
                                ForeignKey o = obj as ForeignKey;
                                MetaData.DisplayForeignKeyProperties(o, node);
                                UserData.EditSingle(o, o.Alias);
                                GlobalUserData.Edit(o);
                            }
                            break;

                        case NodeType.INDEX:
                            {
                                Index o = obj as Index;
                                MetaData.DisplayIndexProperties(o, node);
                                UserData.EditSingle(o, o.Alias);
                                GlobalUserData.Edit(o);
                            }
                            break;

                        case NodeType.PROCEDURE:
                            {
                                Procedure o = obj as Procedure;
                                MetaData.DisplayProcedureProperties(o, node);
                                UserData.EditSingle(o, o.Alias);
                                GlobalUserData.Edit(o);
                            }
                            break;

                        case NodeType.DOMAIN:
                            {
                                Domain o = obj as Domain;
                                MetaData.DisplayDomainProperties(o, node);
                                UserData.EditSingle(o, o.Alias);
                                GlobalUserData.Edit(o);
                            }
                            break;

                        default:
                            MetaData.Clear();
                            GlobalUserData.Clear();
                            break;
                    }
                }
            }
        }

        private void MyTree_MouseClick(object sender, MouseEventArgs e)
        {
            TreeNode node = MyTree.GetNodeAt(e.X, e.Y) as TreeNode;
            if ((node != null) && (node == MyTree.SelectedNode))
            {
                try
                {
                    BeforeNodeSelected(node);
                }
#if DEBUG
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
#else
			catch {}
#endif
            }
        }

		private void MyTree_BeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			try
			{
                BeforeNodeSelected(e.Node);
			}
#if DEBUG
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex);
			}
#else
			catch {}
#endif
		}

		private void MyTree_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			try
			{
				NodeData data = (NodeData)e.Node.Tag;

				if(null != data)
				{
					switch(data.Type)
					{
						case NodeType.DATABASE:

							ExpandDatabase(data.Meta as IDatabase, e.Node);
							break;

						case NodeType.TABLES:
						case NodeType.SUBTABLES:

							ExpandTables(data.Meta as ITables, e.Node);
							break;

						case NodeType.TABLE:

							ExpandTable(data.Meta as ITable, e.Node);
							break;

						case NodeType.VIEWS:
						case NodeType.SUBVIEWS:

							ExpandViews(data.Meta as IViews, e.Node);
							break;

						case NodeType.VIEW:

							ExpandView(data.Meta as IView, e.Node);
							break;

						case NodeType.PROCEDURES:

							ExpandProcedures(data.Meta as IProcedures, e.Node);
							break;

						case NodeType.PROCEDURE:

							ExpandProcedure(data.Meta as IProcedure, e.Node);
							break;

						case NodeType.COLUMNS:
						case NodeType.PRIMARYKEYS:

							ExpandColumns(data.Meta as IColumns, e.Node);
							break;

						case NodeType.PARAMETERS:

							ExpandParameters(data.Meta as IParameters, e.Node);
							break;

						case NodeType.RESULTCOLUMNS:

							ExpandResultColumns(data.Meta as IResultColumns, e.Node);
							break;

						case NodeType.INDEXES:

							ExpandIndexes(data.Meta as IIndexes, e.Node);
							break;

						case NodeType.FOREIGNKEYS:

							this.ExpandForeignKeys(data.Meta as IForeignKeys, e.Node);
							break;

						case NodeType.DOMAINS:

							this.ExpandDomains(data.Meta as IDomains, e.Node);
							break;

						case NodeType.PROVIDERTYPES:

//							this.ExpandProviderTypes(data.Meta as IProviderTypes, e.Node);
							break;
					}
				}
			}
#if DEBUG
			catch (Exception ex)
			{
				throw ex;
			}
#else
			catch {}
#endif
		}

		private void ExpandDatabase(IDatabase database, TreeNode dbNode)
		{
            if (IsTreeBusy) return; 

			if(HasBlankNode(dbNode))
			{
				IDatabase db = StaticMyMeta.Databases[database.Name];
			
				TreeNode node;

				if(db.Tables.Count > 0)
				{
					node = new TreeNode("Tables");
					node.Tag = new NodeData(NodeType.TABLES, database.Tables);
					node.SelectedImageIndex = node.ImageIndex = 2;
					dbNode.Nodes.Add(node);
					node.Nodes.Add(this.BlankNode);
				}

				if(db.Views.Count > 0)
				{
					node = new TreeNode("Views");
					node.Tag = new NodeData(NodeType.VIEWS, database.Views);
					node.SelectedImageIndex = node.ImageIndex = 5;
					dbNode.Nodes.Add(node);
					node.Nodes.Add(this.BlankNode);
				}

				if(db.Procedures.Count > 0)
				{
					node = new TreeNode("Procedures");
					node.Tag = new NodeData(NodeType.PROCEDURES,database.Procedures);
					node.SelectedImageIndex = node.ImageIndex = 7;
					dbNode.Nodes.Add(node);
					node.Nodes.Add(this.BlankNode);
				}

				if(db.Domains.Count > 0)
				{
					node = new TreeNode("Domains");
					node.Tag = new NodeData(NodeType.DOMAINS,database.Domains);
					node.SelectedImageIndex = node.ImageIndex = 24;
					dbNode.Nodes.Add(node);
					node.Nodes.Add(this.BlankNode);
				}
			}
		}

		private void ExpandTables(ITables tables, TreeNode node)
		{
			if(HasBlankNode(node))
			{
				foreach(ITable table in tables)
				{
					TreeNode n = new TreeNode(table.Name);
					n.Tag = new NodeData(NodeType.TABLE, table);
					n.SelectedImageIndex = n.ImageIndex = 3;
					node.Nodes.Add(n);
					n.Nodes.Add(this.BlankNode);
				}
			}
		}

		private void ExpandTable(ITable table, TreeNode node)
		{
			if(HasBlankNode(node))
			{
				TreeNode n;

				if(table.Columns.Count > 0)
				{
					n = new TreeNode("Columns");
					n.Tag = new NodeData(NodeType.COLUMNS, table.Columns);
					n.SelectedImageIndex = n.ImageIndex = 9;
					node.Nodes.Add(n);
					n.Nodes.Add(this.BlankNode);
				}

				if(table.ForeignKeys.Count > 0)
				{
					n = new TreeNode("ForeignKeys");
					n.Tag = new NodeData(NodeType.FOREIGNKEYS, table.ForeignKeys);
					n.SelectedImageIndex = n.ImageIndex = 11;
					node.Nodes.Add(n);
					n.Nodes.Add(this.BlankNode);
				}

				if(table.Indexes.Count > 0)
				{
					n = new TreeNode("Indexes");
					n.Tag = new NodeData(NodeType.INDEXES, table.Indexes);
					n.SelectedImageIndex = n.ImageIndex = 14;
					node.Nodes.Add(n);
					n.Nodes.Add(this.BlankNode);
				}

				if(table.PrimaryKeys.Count > 0)
				{
					n = new TreeNode("PrimaryKeys");
					n.Tag = new NodeData(NodeType.PRIMARYKEYS, table.PrimaryKeys);
					n.SelectedImageIndex = n.ImageIndex = 16;
					node.Nodes.Add(n);
					n.Nodes.Add(this.BlankNode);
				}
			}
		}

		private void ExpandViews(IViews views, TreeNode node)
		{
			if(HasBlankNode(node))
			{
				foreach(IView view in views)
				{
					TreeNode n = new TreeNode(view.Name);
					n.Tag = new NodeData(NodeType.VIEW, view);
					n.SelectedImageIndex = n.ImageIndex = 6;
					node.Nodes.Add(n);
					n.Nodes.Add(this.BlankNode);
				}
			}
		}

		private void ExpandView(IView view, TreeNode node)
		{
			if(HasBlankNode(node))
			{
				TreeNode n;

				if(view.Columns.Count > 0)
				{
					n = new TreeNode("Columns");
					n.Tag = new NodeData(NodeType.COLUMNS, view.Columns);
					n.SelectedImageIndex = n.ImageIndex = 9;
					node.Nodes.Add(n);
					n.Nodes.Add(this.BlankNode);
				}

				if(view.SubTables.Count > 0)
				{
					n = new TreeNode("SubTables");
					n.Tag = new NodeData(NodeType.SUBTABLES, view.SubTables);
					n.SelectedImageIndex = n.ImageIndex = 2;
					node.Nodes.Add(n);
					n.Nodes.Add(this.BlankNode);
				}

				if(view.SubViews.Count > 0)
				{
					n = new TreeNode("SubViews");
					n.Tag = new NodeData(NodeType.SUBVIEWS, view.SubViews);
					n.SelectedImageIndex = n.ImageIndex = 5;
					node.Nodes.Add(n);
					n.Nodes.Add(this.BlankNode);
				}
			}
		}

		private void ExpandProcedures(IProcedures procedures, TreeNode node)
		{
			if(HasBlankNode(node))
			{
				foreach(IProcedure procedure in procedures)
				{
					TreeNode n = new TreeNode(procedure.Name);
					n.Tag = new NodeData(NodeType.PROCEDURE, procedure);
					n.SelectedImageIndex = n.ImageIndex = 8;
					node.Nodes.Add(n);
					n.Nodes.Add(this.BlankNode);
				}
			}
		}

		private void ExpandProcedure(IProcedure procedure, TreeNode node)
		{
			if(HasBlankNode(node))
			{
				TreeNode n;

				if(procedure.Parameters.Count > 0)
				{
					n = new TreeNode("Parameters");
					n.Tag = new NodeData(NodeType.PARAMETERS, procedure.Parameters);
					n.SelectedImageIndex = n.SelectedImageIndex = n.ImageIndex = 17;
					node.Nodes.Add(n);
					n.Nodes.Add(this.BlankNode);
				}

				if(procedure.ResultColumns.Count > 0)
				{
					n = new TreeNode("ResultColumns");
					n.Tag = new NodeData(NodeType.RESULTCOLUMNS, procedure.ResultColumns);
					n.SelectedImageIndex = n.ImageIndex = 19;
					node.Nodes.Add(n);
					n.Nodes.Add(this.BlankNode);
				}
			}
		}

		private void ExpandColumns(IColumns columns, TreeNode node)
		{
			if(HasBlankNode(node))
			{
				foreach(IColumn column in columns)
				{
					TreeNode n = new TreeNode(column.Name);
					n.Tag = new NodeData(NodeType.COLUMN, column);

					if(!column.IsInPrimaryKey)
						n.SelectedImageIndex = n.ImageIndex = 10;
					else
						n.SelectedImageIndex = n.ImageIndex = 13;

					node.Nodes.Add(n);

					if(column.ForeignKeys.Count > 0)
					{
						TreeNode nn = new TreeNode("ForeignKeys");
						nn.Tag = new NodeData(NodeType.FOREIGNKEYS, column.ForeignKeys);
						nn.SelectedImageIndex = nn.ImageIndex = 11;
						n.Nodes.Add(nn);
						nn.Nodes.Add(this.BlankNode);
					}
				}
			}
		}

		private void ExpandParameters(IParameters parameters, TreeNode node)
		{
			if(HasBlankNode(node))
			{
				foreach(IParameter parameter in parameters)
				{
					TreeNode n = new TreeNode(parameter.Name);
					n.Tag = new NodeData(NodeType.PARAMETER, parameter);
					n.SelectedImageIndex = n.ImageIndex = 18;
					node.Nodes.Add(n);
				}
			}
		}

		private void ExpandResultColumns(IResultColumns resultColumns, TreeNode node)
		{
			if(HasBlankNode(node))
			{
				foreach(IResultColumn resultColumn in resultColumns)
				{
					TreeNode n = new TreeNode(resultColumn.Name);
					n.Tag = new NodeData(NodeType.RESULTCOLUMN, resultColumn);
					n.SelectedImageIndex = n.ImageIndex = 20;
					node.Nodes.Add(n);
				}
			}
		}

		private void ExpandIndexes(IIndexes indexes, TreeNode node)
		{
			if(HasBlankNode(node))
			{
				foreach(IIndex index in indexes)
				{
					TreeNode indexNode = new TreeNode(index.Name);
					indexNode.Tag = new NodeData(NodeType.INDEX, index);
					indexNode.SelectedImageIndex = indexNode.ImageIndex = 15;
					node.Nodes.Add(indexNode);

					if(index.Columns.Count > 0)
					{
						TreeNode n = new TreeNode("Columns");
						n.Tag = new NodeData(NodeType.COLUMNS, index.Columns);
						n.SelectedImageIndex = n.ImageIndex = 9;
						indexNode.Nodes.Add(n);
						n.Nodes.Add(this.BlankNode);
					}
				}
			}
		}

		private void ExpandForeignKeys(IForeignKeys foreignKeys, TreeNode node)
		{
			if(HasBlankNode(node))
			{
				foreach(IForeignKey foreignKey in foreignKeys)
				{
					TreeNode n;
					TreeNode fkNode = new TreeNode(foreignKey.Name);
					fkNode.Tag = new NodeData(NodeType.FOREIGNKEY, foreignKey);
					fkNode.SelectedImageIndex = fkNode.ImageIndex = 12;
					node.Nodes.Add(fkNode);

					if(foreignKey.PrimaryColumns.Count > 0)
					{
						n = new TreeNode("PrimaryColumns");
						n.Tag = new NodeData(NodeType.COLUMNS, foreignKey.PrimaryColumns);
						n.SelectedImageIndex = n.ImageIndex = 9;
						fkNode.Nodes.Add(n);
						n.Nodes.Add(this.BlankNode);
					}

					if(foreignKey.ForeignColumns.Count > 0)
					{
						n = new TreeNode("ForeignColumns");
						n.Tag = new NodeData(NodeType.COLUMNS, foreignKey.ForeignColumns);
						n.SelectedImageIndex = n.ImageIndex = 9;
						fkNode.Nodes.Add(n);
						n.Nodes.Add(this.BlankNode);
					}
				}
			}
		}

		private void ExpandDomains(IDomains domains, TreeNode node)
		{
			if(HasBlankNode(node))
			{
				foreach(IDomain domain in domains)
				{
					TreeNode n = new TreeNode(domain.Name);
					n.Tag = new NodeData(NodeType.DOMAIN, domain);
					n.SelectedImageIndex = n.ImageIndex = 25;
					node.Nodes.Add(n);
				}
			}
		}

		public bool SaveUserMetaData()
        {
            if (IsTreeBusy) return false; 

			bool saved = StaticMyMeta.SaveUserMetaData();
			this.IsUserDataDirty = false;
			return saved;
		}

		public bool IsUserDataDirty
		{
			get
			{
				return _isUserDataDirty;
			}

			set
			{
				_isUserDataDirty = value;

				this.UserData.MetaDataStateChanged(_isUserDataDirty);
				this.GlobalUserData.MetaDataStateChanged(_isUserDataDirty);
			}
        }

        private void MetaDataBrowser_Enter(object sender, EventArgs e)
        {
            if (IsTreeBusy) return; 

            if (this.toolBar1.Visible == false)
            {
                this.toolBar1.Visible = true;
                this.chkSystem.Visible = true;
            }

            if ((this.mdi != null)
                && (this.mdi.DockPanel != null))
            {
                if ((this.mdi.DockPanel.ActiveDocument is IMyGenDocument) &&
                        (StaticMyMeta.IsConnected))
                {
                    IMyGenDocument doc = this.mdi.DockPanel.ActiveDocument as IMyGenDocument;
                    this.toolBarButtonExecute.Enabled = true;
                }
                else
                {
                    this.toolBarButtonExecute.Enabled = false;
                }
            }
        }

        #region IMyGenContent Members

        public ToolStrip ToolStrip
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            if (command.Equals("UpdateDefaultSettings", StringComparison.CurrentCultureIgnoreCase))
            {
                this.DefaultSettingsChanged(DefaultSettings.Instance);
            }
        }

        public bool CanClose(bool allowPrevent)
        {
            return true;
        }

        public DockContent DockContent
        {
            get { return this; }
        }

        #endregion
	}
}
