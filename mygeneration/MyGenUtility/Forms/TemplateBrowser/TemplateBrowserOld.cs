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
    public class TemplateBrowserOld : DockContent, IMyGenContent
	{
		private const string UPDATE_URL = "http://www.mygenerationsoftware.com/update/";
		private const int INDEX_CLOSED_FOLDER = 3;
		private const int INDEX_OPEN_FOLDER = 4;

        private IMyGenerationMDI mdi;
        private System.ComponentModel.IContainer components;

        private WebTemplateLibrary templateLibrary;
        private TemplateTreeBuilder treeBuilder;

        public TemplateBrowserOld(IMyGenerationMDI mdi)
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

            TemplateBrowser_Enter(this, EventArgs.Empty);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateBrowser));
            this.SuspendLayout();
            // 
            // TemplateBrowser
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(384, 542);
            this.ControlBox = false;
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((((WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TemplateBrowser";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
            this.TabText = "Template Browser";
            this.Text = "Template Browser";
            this.Enter += new System.EventHandler(this.TemplateBrowser_Enter);
            this.MouseLeave += new System.EventHandler(this.TemplateBrowser_MouseLeave);
            this.Leave += new System.EventHandler(this.TemplateBrowser_Leave);
            this.ResumeLayout(false);

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
                mdi.ErrorsOccurred(ex);
				//ZeusDisplayError formError = new ZeusDisplayError(ex);
				//formError.SetControlsFromException();			
				//formError.ShowDialog(this);

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

				MessageBox.Show(this, "Input collected and saved to file: \r\n" + collectedInput.FilePath);
			}
			catch (Exception ex)
			{
                mdi.ErrorsOccurred(ex);
                //ZeusDisplayError formError = new ZeusDisplayError(ex);
				//formError.SetControlsFromException();			
				//formError.ShowDialog(this);
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
                mdi.ErrorsOccurred(ex);
                //ZeusDisplayError formError = new ZeusDisplayError(ex);
				//formError.SetControlsFromException();			
				//formError.ShowDialog(this);
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
                mdi.ErrorsOccurred(ex);
                //ZeusDisplayError formError = new ZeusDisplayError(ex);
				//formError.SetControlsFromException();			
				//formError.ShowDialog(this);
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

        private void TemplateBrowser_Enter(object sender, EventArgs e)
        {
            if (this.toolBarToolbar.Visible == false)
            {
                this.toolBarToolbar.Visible = true;
            }
        }

        private void TemplateBrowser_Leave(object sender, EventArgs e)
        {
            //this.toolBarToolbar.Visible = false;
        }

        #region IMyGenContent Members

        public ToolStrip ToolStrip
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            DefaultSettings settings = DefaultSettings.Instance;
            if (command.Equals("UpdateDefaultSettings", StringComparison.CurrentCultureIgnoreCase))
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
