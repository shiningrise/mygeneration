using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Zeus;
using Zeus.UserInterface;
using Zeus.UserInterface.WinForms;
using MyMeta;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for TemplateBrowser.
	/// </summary>
    public class TemplateBrowser : DockContent, IMyGenContent
	{
		private const string UPDATE_URL = "http://www.mygenerationsoftware.com/update/";
		private const int INDEX_CLOSED_FOLDER = 3;
		private const int INDEX_OPEN_FOLDER = 4;

        private IMyGenerationMDI mdi;
		private System.Windows.Forms.ToolBar toolBarToolbar;
		private System.Windows.Forms.ToolBarButton toolBarButtonOpen;
		private System.Windows.Forms.ToolBarButton toolBarSeparator2;
		private System.Windows.Forms.ToolBarButton toolBarButtonExecute;
		private System.Windows.Forms.ImageList imageListFormIcons;
		private System.Windows.Forms.TreeView treeViewTemplates;
		private System.Windows.Forms.ToolBarButton toolBarButtonRefresh;
		private System.Windows.Forms.ToolTip toolTipTemplateBrowser;
		private System.Windows.Forms.ToolBarButton toolBarButtonMode;
		private System.Windows.Forms.ContextMenu contextMenuTree;
		private System.Windows.Forms.MenuItem menuItemExecute;
		private System.Windows.Forms.MenuItem menuItemOpen;
		private System.Windows.Forms.MenuItem menuItemWebUpdate;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolBarButton toolBarButtonExecuteSavedInput;
		private System.Windows.Forms.ToolBarButton toolBarButtonSaveInput;
		private System.Windows.Forms.ToolBarButton toolBarSeparator1;
		private System.Windows.Forms.ToolBarButton toolBarSeparator3;
		private System.Windows.Forms.ToolBarButton toolBarButtonWeb;

		private TemplateTreeBuilder treeBuilder;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private System.Windows.Forms.MenuItem menuItemEncryptAs;
		private System.Windows.Forms.MenuItem menuItemCompileAs;
		private WebTemplateLibrary templateLibrary;

        public TemplateBrowser(IMyGenerationMDI mdi)
		{
			//
			// Required for Windows Form Designer support
			//
            InitializeComponent();
            this.mdi = mdi;
            this.DockPanel = mdi.DockPanel;

			treeViewTemplates.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewTemplates_MouseDown);
			treeViewTemplates.DoubleClick += new System.EventHandler(this.treeViewTemplates_OnDoubleClick);
			treeViewTemplates.AfterExpand += new TreeViewEventHandler(this.treeViewTemplates_AfterExpand);
			treeViewTemplates.AfterCollapse += new TreeViewEventHandler(this.treeViewTemplates_AfterCollapse);
			treeViewTemplates.KeyDown += new KeyEventHandler(this.treeViewTemplates_KeyDown);
			treeViewTemplates.MouseMove += new MouseEventHandler(treeViewTemplates_MouseMove);
			
			treeBuilder = new TemplateTreeBuilder(treeViewTemplates);

			treeBuilder.LoadTemplates();
        }

        protected override string GetPersistString()
        {
            return this.GetType().FullName;
        }

		//public event EventHandler TemplateOpen;
		//public event EventHandler TemplateUpdate;

		protected void OnTemplateUpdate(string uniqueId) 
		{
			this.mdi.SendAlert(this, "UpdateTemplate", uniqueId);
		}

		protected void OnTemplateOpen(string path) 
		{
            this.mdi.OpenDocuments(path);
            /*if (this.TemplateOpen != null) 
			{
                this.TemplateOpen(path, new EventArgs());
			}*/
		}

		public event EventHandler TemplateDelete;

		protected void OnTemplateDelete(string path) 
		{
			if (this.TemplateDelete != null) 
			{
				this.TemplateDelete(path, new EventArgs());
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

		/*public void DefaultSettingsChanged(DefaultSettings settings)
		{
			bool doRefresh = false;

			try 
			{
				if (this.treeBuilder.DefaultTemplatePath != settings.DefaultTemplateDirectory)
				{
					doRefresh = true;
				}
			}
			catch 
			{
				doRefresh = true;
			}
			
			if (doRefresh) 
				this.RefreshTree();
			
		}*/

		private void treeViewTemplates_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e) 
		{
			if (e.Node is FolderTreeNode)
			{
				e.Node.SelectedImageIndex = INDEX_OPEN_FOLDER;
				e.Node.ImageIndex = INDEX_OPEN_FOLDER;
			}
		}

		private void treeViewTemplates_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e) 
		{
			if (e.Node is FolderTreeNode)
			{
				e.Node.SelectedImageIndex = INDEX_CLOSED_FOLDER;
				e.Node.ImageIndex = INDEX_CLOSED_FOLDER;
			}
		}

		
		private void treeViewTemplates_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) 
		{
			TreeNode node = (TreeNode)treeViewTemplates.GetNodeAt(e.X, e.Y);
			treeViewTemplates.SelectedNode = node;
		}

		private void treeViewTemplates_KeyDown(object sender, KeyEventArgs e) 
		{
			if (e.KeyCode == Keys.F5) 
			{
				this.treeBuilder.LoadTemplates();
				return;
			}
		}

		private void treeViewTemplates_OnDoubleClick(object sender, System.EventArgs e) 
		{
			if (this.treeViewTemplates.SelectedNode is TemplateTreeNode)
			{
				if (((TemplateTreeNode)treeViewTemplates.SelectedNode).IsLocked) 
				{
					MessageBox.Show(this, "You cannot open a locked template.");
				}
				else 
				{
					OnTemplateOpen( this.treeViewTemplates.SelectedNode.Tag.ToString() );
				}
			}
		}

        private object lastObject = null;

		private void treeViewTemplates_MouseMove(object sender, MouseEventArgs e)
		{
			object obj = treeViewTemplates.GetNodeAt(e.X, e.Y);
            if (object.Equals(obj, lastObject) || (obj == null && lastObject == null))
            {
                return;
            }
            else
            {
                if (obj is TemplateTreeNode)
                {
                    this.toolTipTemplateBrowser.SetToolTip(treeViewTemplates, ((TemplateTreeNode)obj).Comments);
                }
                else if ((obj is RootTreeNode) && (DateTime.Now.Hour == 1))
                {
                    this.toolTipTemplateBrowser.SetToolTip(treeViewTemplates, "Worship me as I generate your code.");
                }
                else
                {
                    this.toolTipTemplateBrowser.SetToolTip(treeViewTemplates, string.Empty);
                }

                lastObject = obj;
            }
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateBrowser));
            this.toolBarToolbar = new System.Windows.Forms.ToolBar();
            this.toolBarButtonRefresh = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonMode = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonWeb = new System.Windows.Forms.ToolBarButton();
            this.toolBarSeparator2 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonOpen = new System.Windows.Forms.ToolBarButton();
            this.toolBarSeparator1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonSaveInput = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonExecuteSavedInput = new System.Windows.Forms.ToolBarButton();
            this.toolBarSeparator3 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonExecute = new System.Windows.Forms.ToolBarButton();
            this.imageListFormIcons = new System.Windows.Forms.ImageList(this.components);
            this.treeViewTemplates = new System.Windows.Forms.TreeView();
            this.contextMenuTree = new System.Windows.Forms.ContextMenu();
            this.menuItemExecute = new System.Windows.Forms.MenuItem();
            this.menuItemOpen = new System.Windows.Forms.MenuItem();
            this.menuItemWebUpdate = new System.Windows.Forms.MenuItem();
            this.menuItemEncryptAs = new System.Windows.Forms.MenuItem();
            this.menuItemCompileAs = new System.Windows.Forms.MenuItem();
            this.menuItemDelete = new System.Windows.Forms.MenuItem();
            this.toolTipTemplateBrowser = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // toolBarToolbar
            // 
            this.toolBarToolbar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBarToolbar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonRefresh,
            this.toolBarButtonMode,
            this.toolBarButtonWeb,
            this.toolBarSeparator2,
            this.toolBarButtonOpen,
            this.toolBarSeparator1,
            this.toolBarButtonSaveInput,
            this.toolBarButtonExecuteSavedInput,
            this.toolBarSeparator3,
            this.toolBarButtonExecute});
            this.toolBarToolbar.DropDownArrows = true;
            this.toolBarToolbar.ImageList = this.imageListFormIcons;
            this.toolBarToolbar.Location = new System.Drawing.Point(0, 0);
            this.toolBarToolbar.Name = "toolBarToolbar";
            this.toolBarToolbar.ShowToolTips = true;
            this.toolBarToolbar.Size = new System.Drawing.Size(384, 28);
            this.toolBarToolbar.TabIndex = 0;
            this.toolBarToolbar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarToolbar_ButtonClick);
            // 
            // toolBarButtonRefresh
            // 
            this.toolBarButtonRefresh.ImageIndex = 2;
            this.toolBarButtonRefresh.Name = "toolBarButtonRefresh";
            this.toolBarButtonRefresh.ToolTipText = "Refresh Template Browser";
            // 
            // toolBarButtonMode
            // 
            this.toolBarButtonMode.ImageIndex = 7;
            this.toolBarButtonMode.Name = "toolBarButtonMode";
            this.toolBarButtonMode.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarButtonMode.ToolTipText = "Browse Mode";
            // 
            // toolBarButtonWeb
            // 
            this.toolBarButtonWeb.ImageIndex = 10;
            this.toolBarButtonWeb.Name = "toolBarButtonWeb";
            this.toolBarButtonWeb.ToolTipText = "Online Template Library";
            // 
            // toolBarSeparator2
            // 
            this.toolBarSeparator2.Name = "toolBarSeparator2";
            this.toolBarSeparator2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButtonOpen
            // 
            this.toolBarButtonOpen.ImageIndex = 0;
            this.toolBarButtonOpen.Name = "toolBarButtonOpen";
            this.toolBarButtonOpen.ToolTipText = "Open Template";
            // 
            // toolBarSeparator1
            // 
            this.toolBarSeparator1.Name = "toolBarSeparator1";
            this.toolBarSeparator1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButtonSaveInput
            // 
            this.toolBarButtonSaveInput.ImageIndex = 8;
            this.toolBarButtonSaveInput.Name = "toolBarButtonSaveInput";
            this.toolBarButtonSaveInput.ToolTipText = "Record a Template";
            // 
            // toolBarButtonExecuteSavedInput
            // 
            this.toolBarButtonExecuteSavedInput.ImageIndex = 9;
            this.toolBarButtonExecuteSavedInput.Name = "toolBarButtonExecuteSavedInput";
            this.toolBarButtonExecuteSavedInput.ToolTipText = "Replay a Template";
            // 
            // toolBarSeparator3
            // 
            this.toolBarSeparator3.Name = "toolBarSeparator3";
            this.toolBarSeparator3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButtonExecute
            // 
            this.toolBarButtonExecute.ImageIndex = 1;
            this.toolBarButtonExecute.Name = "toolBarButtonExecute";
            this.toolBarButtonExecute.ToolTipText = "Execute Template";
            // 
            // imageListFormIcons
            // 
            this.imageListFormIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListFormIcons.ImageStream")));
            this.imageListFormIcons.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageListFormIcons.Images.SetKeyName(0, "");
            this.imageListFormIcons.Images.SetKeyName(1, "");
            this.imageListFormIcons.Images.SetKeyName(2, "");
            this.imageListFormIcons.Images.SetKeyName(3, "");
            this.imageListFormIcons.Images.SetKeyName(4, "");
            this.imageListFormIcons.Images.SetKeyName(5, "");
            this.imageListFormIcons.Images.SetKeyName(6, "");
            this.imageListFormIcons.Images.SetKeyName(7, "");
            this.imageListFormIcons.Images.SetKeyName(8, "");
            this.imageListFormIcons.Images.SetKeyName(9, "");
            this.imageListFormIcons.Images.SetKeyName(10, "");
            this.imageListFormIcons.Images.SetKeyName(11, "");
            // 
            // treeViewTemplates
            // 
            this.treeViewTemplates.ContextMenu = this.contextMenuTree;
            this.treeViewTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewTemplates.ImageIndex = 0;
            this.treeViewTemplates.ImageList = this.imageListFormIcons;
            this.treeViewTemplates.Location = new System.Drawing.Point(0, 28);
            this.treeViewTemplates.Name = "treeViewTemplates";
            this.treeViewTemplates.SelectedImageIndex = 0;
            this.treeViewTemplates.Size = new System.Drawing.Size(384, 514);
            this.treeViewTemplates.TabIndex = 1;
            // 
            // contextMenuTree
            // 
            this.contextMenuTree.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemExecute,
            this.menuItemOpen,
            this.menuItemWebUpdate,
            this.menuItemEncryptAs,
            this.menuItemCompileAs,
            this.menuItemDelete});
            this.contextMenuTree.Popup += new System.EventHandler(this.contextMenuTree_Popup);
            // 
            // menuItemExecute
            // 
            this.menuItemExecute.Index = 0;
            this.menuItemExecute.Text = "E&xecute";
            this.menuItemExecute.Click += new System.EventHandler(this.menuItemExecute_Click);
            // 
            // menuItemOpen
            // 
            this.menuItemOpen.Index = 1;
            this.menuItemOpen.Text = "&Open";
            this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
            // 
            // menuItemWebUpdate
            // 
            this.menuItemWebUpdate.Index = 2;
            this.menuItemWebUpdate.Text = "&Web Update";
            this.menuItemWebUpdate.Click += new System.EventHandler(this.menuItemWebUpdate_Click);
            // 
            // menuItemEncryptAs
            // 
            this.menuItemEncryptAs.Index = 3;
            this.menuItemEncryptAs.Text = "Ecr&ypt As...";
            this.menuItemEncryptAs.Visible = false;
            this.menuItemEncryptAs.Click += new System.EventHandler(this.menuItemEncryptAs_Click);
            // 
            // menuItemCompileAs
            // 
            this.menuItemCompileAs.Index = 4;
            this.menuItemCompileAs.Text = "&Compile As...";
            this.menuItemCompileAs.Visible = false;
            this.menuItemCompileAs.Click += new System.EventHandler(this.menuItemCompileAs_Click);
            // 
            // menuItemDelete
            // 
            this.menuItemDelete.Index = 5;
            this.menuItemDelete.Text = "&Delete";
            this.menuItemDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
            // 
            // TemplateBrowser
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(384, 542);
            this.ControlBox = false;
            this.Controls.Add(this.treeViewTemplates);
            this.Controls.Add(this.toolBarToolbar);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((((WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TemplateBrowser";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
            this.TabText = "Template Browser";
            this.Text = "Template Browser";
            this.MouseLeave += new System.EventHandler(this.TemplateBrowser_MouseLeave);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Execute, Open, RefreshTree, ChangeMode, WebUpdate, Delete
		public void Execute() 
		{
			if (this.treeViewTemplates.SelectedNode is TemplateTreeNode)
			{
				ZeusTemplate template = new ZeusTemplate(this.treeViewTemplates.SelectedNode.Tag.ToString());
				ExecuteTemplate(template);
			}
		}

		public void SaveInput() 
		{
			if (this.treeViewTemplates.SelectedNode is TemplateTreeNode)
			{
				ZeusTemplate template = new ZeusTemplate(this.treeViewTemplates.SelectedNode.Tag.ToString());
				SaveInput(template);
			}
		}

		public void ExecuteSavedInput() 
		{
			ExecuteLoadedInput();
		}

		public void Open() 
		{
			if (this.treeViewTemplates.SelectedNode is TemplateTreeNode)
			{
				OnTemplateOpen( this.treeViewTemplates.SelectedNode.Tag.ToString() );
			}
		}

		/*protected void EncryptAs() 
		{
			if (this.treeViewTemplates.SelectedNode is TemplateTreeNode)
			{
				DialogResult dr = MessageBox.Show(this,
					"Be careful not to overwrite your source template or you will lose your work!\r\n Are you ready to Encrypt?", 
					"Encryption Warning", 
					MessageBoxButtons.OKCancel, 
					MessageBoxIcon.Information);
				if (dr == DialogResult.OK) 
				{
					ZeusTemplate template = new ZeusTemplate( ((TemplateTreeNode)treeViewTemplates.SelectedNode).FullPath );
					if (template.SourceType != ZeusConstants.SourceTypes.SOURCE) 
					{
						template.Encrypt();
						SaveAs(template);
					}
				}
			}
		}

		protected void CompileAs() 
		{
			if (this.treeViewTemplates.SelectedNode is TemplateTreeNode)
			{
				DialogResult dr = MessageBox.Show(this,
					"In order to finish compiling a template, the template must be executed completely.\r\nBe careful not to overwrite your source template or you will lose your work!\r\n Are you ready to Compile?", 
					"Compilation Warning", 
					MessageBoxButtons.OKCancel, 
					MessageBoxIcon.Information);
				if (dr == DialogResult.OK) 
				{
					ZeusTemplate template = new ZeusTemplate( ((TemplateTreeNode)treeViewTemplates.SelectedNode).FullPath );
					if (template.SourceType != ZeusConstants.SourceTypes.SOURCE) 
					{
						template.Compile();
						this.Execute();
					}

					SaveAs(template);

					this.menuItemCompileAs.Enabled = false;
					this.menuItemEncryptAs.Enabled = false;
				}
			}
		}

		protected void SaveAs(ZeusTemplate template) 
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
       
			saveFileDialog.Filter = TemplateEditor.FILE_TYPES;
			saveFileDialog.FilterIndex = TemplateEditor.DEFAULT_SAVE_FILE_TYPE_INDEX;
			saveFileDialog.RestoreDirectory = true;

			saveFileDialog.FileName = this.FileName;

			if(saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.FileSaveAs(saveFileDialog.FileName);
			}
		}*/

		public void RefreshTree() 
		{
			treeBuilder.Clear();

			if (this.toolBarButtonMode.Pushed) 
				this.treeBuilder.LoadTemplatesByFile();
			else 
				this.treeBuilder.LoadTemplates();
		}

		public void ChangeMode() 
		{
			if (this.toolBarButtonMode.Pushed) this.treeBuilder.LoadTemplatesByFile();
			else this.treeBuilder.LoadTemplates();
		}

		public void WebUpdate() 
		{
			Cursor.Current = Cursors.WaitCursor;

			try 
			{
				TreeNode node = this.treeViewTemplates.SelectedNode;

				if (node is TemplateTreeNode) 
				{
					TemplateTreeNode tnode = node as TemplateTreeNode;
					TemplateWebUpdateHelper.WebUpdate( tnode.UniqueId, tnode.Tag.ToString(), false );
					OnTemplateUpdate(tnode.UniqueId);
				}
				else if (node != null) 
				{
					DialogResult result = result = MessageBox.Show("Are you sure you want to update all the templates\r\nrecursively contained in this node?", "Update Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (result == DialogResult.Yes) 
					{
						WebUpdate(node);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.GetType().Name + "\r\n" + ex.Message + "\r\n" + ex.StackTrace);
				MessageBox.Show("The WebUpdate feature is not functioning due to either server or firewall issues.");			
			}

			Cursor.Current = Cursors.Default;
		}

		public void WebUpdate(TreeNode parentNode) 
		{
			foreach (TreeNode node in parentNode.Nodes) 
			{
				if (node is TemplateTreeNode) 
				{
					TemplateTreeNode tnode = node as TemplateTreeNode;
					TemplateWebUpdateHelper.WebUpdate( tnode.UniqueId, tnode.Tag.ToString(), true );
					OnTemplateUpdate(tnode.UniqueId);
				}
				else 
				{
					WebUpdate(node);
				}
			}
		}

		public void Delete() 
		{
			TreeNode node = this.treeViewTemplates.SelectedNode;
			
			if (node is TemplateTreeNode) 
			{ 
				DialogResult result = MessageBox.Show(this, "Are you sure you want to delete this template?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				if (result == DialogResult.Yes)
				{
					Delete(node as TemplateTreeNode);
					node.Remove();
				}
			}
			else
			{ 
				DialogResult result = MessageBox.Show(this, "Are you sure you want to delete the contents of this folder?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				if (result == DialogResult.Yes)
				{
					DeleteRecursive(node);
				}
			}
		}

		public void DeleteRecursive(TreeNode parentNode)  
		{
			foreach (TreeNode node in parentNode.Nodes) 
			{
				if (node is TemplateTreeNode) 
				{
					Delete(node as TemplateTreeNode);
				}
				else 
				{
					DeleteRecursive(node);
				}
			}
			parentNode.Nodes.Clear();
		}

		public void Delete(TemplateTreeNode node) 
		{
			if (node != null)
			{
				FileInfo info = new FileInfo(node.Tag.ToString());
				if (info.Exists) 
				{
					OnTemplateDelete(node.Tag.ToString());

					info.Delete();
				}
			}
		}
		#endregion
		
		private void toolBarToolbar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == this.toolBarButtonRefresh)
			{
				this.RefreshTree();
			}
			else if (e.Button == this.toolBarButtonMode)
			{
				this.ChangeMode();
			}
			else if (e.Button == this.toolBarButtonOpen) 
			{
				this.Open();
			}
			else if (e.Button == this.toolBarButtonExecute) 
			{
				this.Execute();
			}
			else if (e.Button == this.toolBarButtonExecuteSavedInput) 
			{
				this.ExecuteSavedInput();
			}
			else if (e.Button == this.toolBarButtonSaveInput) 
			{
				this.SaveInput();
			}
			else if (e.Button == this.toolBarButtonWeb) 
			{
				try 
				{
					if (templateLibrary == null) 
					{
						templateLibrary = new WebTemplateLibrary(treeBuilder.IdPathHash);
					}

					templateLibrary.ShowDialog(this);
					foreach (string uniqueid in templateLibrary.UpdatedTemplateIDs) 
					{
						this.OnTemplateUpdate(uniqueid);
					}

					this.RefreshTree();
				}
				catch 
				{
					templateLibrary = null;
					MessageBox.Show("The WebUpdate feature failed to connect to the server.");
				}
			}
		}

		public void ExecuteTemplate(ZeusTemplate template) 
		{
			Cursor.Current = Cursors.WaitCursor;

            DefaultSettings settings = DefaultSettings.Instance;

			IZeusContext context = new ZeusContext();
			IZeusGuiControl guiController = context.Gui;
			IZeusOutput zout = context.Output;

			settings.PopulateZeusContext(context);

			bool exceptionOccurred = false;
			bool result = false;

			try 
			{	
				template.GuiSegment.ZeusScriptingEngine.ExecutionHelper.Timeout = settings.ScriptTimeout;
				template.GuiSegment.ZeusScriptingEngine.ExecutionHelper.SetShowGuiHandler(new ShowGUIEventHandler(DynamicGUI_Display));
				result = template.GuiSegment.Execute(context); 
				template.GuiSegment.ZeusScriptingEngine.ExecutionHelper.Cleanup();
				
				if (result) 
				{
					template.BodySegment.ZeusScriptingEngine.ExecutionHelper.Timeout = settings.ScriptTimeout;
					result = template.BodySegment.Execute(context);
					template.BodySegment.ZeusScriptingEngine.ExecutionHelper.Cleanup();
				}
			}
			catch (Exception ex)
			{
				ZeusDisplayError formError = new ZeusDisplayError(ex);
				formError.SetControlsFromException();			
				formError.ShowDialog(this);

				exceptionOccurred = true;
			}

			Cursor.Current = Cursors.Default;

			if (!exceptionOccurred && result)
			{
				if (settings.EnableClipboard) 
				{
					try 
					{
						Clipboard.SetDataObject(zout.text, true);
					}
					catch
					{
						// HACK: For some reason, Clipboard.SetDataObject throws an error on some systems. I'm cathhing it and doing nothing for now.
					}
				}

				MessageBox.Show("Successfully rendered Template: " + template.Title);
			}
		}

		public void SaveInput(ZeusTemplate template) 
		{
			try 
			{
                DefaultSettings settings = DefaultSettings.Instance;

				ZeusSimpleLog log = new ZeusSimpleLog();
				ZeusContext context = new ZeusContext();
				context.Log = log;

				ZeusSavedInput collectedInput = new ZeusSavedInput();
				collectedInput.InputData.TemplateUniqueID = template.UniqueID;
				collectedInput.InputData.TemplatePath = template.FilePath + template.FileName;

				settings.PopulateZeusContext(context);
				template.Collect(context, settings.ScriptTimeout, collectedInput.InputData.InputItems);
					
				if (log.HasExceptions) 
				{
					throw log.Exceptions[0];
				}
				else 
				{
					SaveFileDialog saveFileDialog = new SaveFileDialog();
					saveFileDialog.Filter = "Zues Input Files (*.zinp)|*.zinp";
					saveFileDialog.FilterIndex = 0;
					saveFileDialog.RestoreDirectory = true;
					if(saveFileDialog.ShowDialog() == DialogResult.OK)
					{
						Cursor.Current = Cursors.WaitCursor;

						collectedInput.FilePath = saveFileDialog.FileName;
						collectedInput.Save();
					}
				}

				MessageBox.Show(this, "Input collected and saved to file:" + "\r\n" + collectedInput.FilePath);
			}
			catch (Exception ex)
			{
				ZeusDisplayError formError = new ZeusDisplayError(ex);
				formError.SetControlsFromException();			
				formError.ShowDialog(this);
			}

			Cursor.Current = Cursors.Default;
		}

		public void ExecuteLoadedInput() 
		{
			try 
			{
                DefaultSettings settings = DefaultSettings.Instance;
				ZeusSimpleLog log = new ZeusSimpleLog();

				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "Zues Input Files (*.zinp)|*.zinp";
				openFileDialog.FilterIndex = 0;
				openFileDialog.RestoreDirectory = true;
				openFileDialog.Multiselect = true;
				if(openFileDialog.ShowDialog() == DialogResult.OK)
				{
					Cursor.Current = Cursors.WaitCursor;

					foreach (string filename in openFileDialog.FileNames) 
					{
						ZeusSavedInput savedInput = new ZeusSavedInput(filename);
						if (savedInput.Load()) 
						{
							ZeusContext context = new ZeusContext();
							context.Input.AddItems(savedInput.InputData.InputItems);
							context.Log = log;

							ZeusTemplate template = new ZeusTemplate(savedInput.InputData.TemplatePath);
							template.Execute(context, settings.ScriptTimeout, true);

							if (log.HasExceptions) 
							{
								throw log.Exceptions[0];
							}
						}
					}

					Cursor.Current = Cursors.Default;
					MessageBox.Show(this, "Selected files have been executed.");
				}
			}
			catch (Exception ex)
			{
				ZeusDisplayError formError = new ZeusDisplayError(ex);
				formError.SetControlsFromException();			
				formError.ShowDialog(this);
			}

			Cursor.Current = Cursors.Default;
		}

		public void DynamicGUI_Display(IZeusGuiControl gui, IZeusFunctionExecutioner executioner) 
		{
			this.Cursor = Cursors.Default;

			try 
			{
				DynamicForm df = new DynamicForm(gui as GuiController, executioner);
				DialogResult result = df.ShowDialog(this);
				
				if(result == DialogResult.Cancel) 
				{
					gui.IsCanceled = true;
				}
			}
			catch (Exception ex)
			{
				ZeusDisplayError formError = new ZeusDisplayError(ex);
				formError.SetControlsFromException();			
				formError.ShowDialog(this);
			}

			Cursor.Current = Cursors.Default;
		}

		private void contextMenuTree_Popup(object sender, System.EventArgs e)
		{
			System.Drawing.Point point = this.treeViewTemplates.PointToClient(Cursor.Position);
			object node = this.treeViewTemplates.GetNodeAt(point.X, point.Y);

			// Everything is off by default
			this.menuItemExecute.Visible = false;
			this.menuItemOpen.Visible = false;
			this.menuItemWebUpdate.Visible = false;
			this.menuItemDelete.Visible = false;

			if (node is TemplateTreeNode) 
			{
				foreach (MenuItem item in contextMenuTree.MenuItems) item.Visible = true;
				if (((TemplateTreeNode)node).IsLocked) 
				{
					this.menuItemOpen.Visible = false;
				}
			}
			else if (node is FolderTreeNode)
			{
				this.menuItemWebUpdate.Visible = true;
				this.menuItemDelete.Visible = true;
			}
			else if (node is RootTreeNode)
			{
				this.menuItemWebUpdate.Visible = true;
			}
			else 
			{
				// The root node is no longer updateable
				foreach (MenuItem item in contextMenuTree.MenuItems) item.Visible = false;
			}
			
			this.menuItemCompileAs.Visible = false;
			this.menuItemEncryptAs.Visible = false;
		}

		private void menuItemExecute_Click(object sender, System.EventArgs e)
		{
			this.Execute();
		}

		private void menuItemOpen_Click(object sender, System.EventArgs e)
		{
			this.Open();
		}

		private void menuItemWebUpdate_Click(object sender, System.EventArgs e)
		{
			this.WebUpdate();
		}

		private void TemplateBrowser_MouseLeave(object sender, System.EventArgs e)
		{
			this.toolTipTemplateBrowser.SetToolTip(treeViewTemplates, string.Empty);
		}

		private void menuItemDelete_Click(object sender, System.EventArgs e)
		{
			this.Delete();
		}

		private void menuItemEncryptAs_Click(object sender, System.EventArgs e)
		{
			//this.EncryptAs();
		}

		private void menuItemCompileAs_Click(object sender, System.EventArgs e)
		{
			//this.CompileAs();
		}

        #region IMyGenContent Members

        public ToolStrip ToolStrip
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            DefaultSettings settings = DefaultSettings.Instance;
            if (command == "UpdateDefaultSettings")
            {
                bool doRefresh = false;

                try
                {
                    if (this.treeBuilder.DefaultTemplatePath != settings.DefaultTemplateDirectory)
                    {
                        doRefresh = true;
                    }
                }
                catch
                {
                    doRefresh = true;
                }

                if (doRefresh)
                    this.RefreshTree();
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
