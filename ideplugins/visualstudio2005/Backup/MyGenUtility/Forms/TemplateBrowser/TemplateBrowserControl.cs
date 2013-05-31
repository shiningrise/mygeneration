using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Zeus;
using Zeus.UserInterface;
using Zeus.UserInterface.WinForms;
using MyMeta;

namespace MyGeneration
{
    public partial class TemplateBrowserControl : UserControl
    {
        private const string UPDATE_URL = "http://www.mygenerationsoftware.com/update/";
        private const int INDEX_CLOSED_FOLDER = 3;
        private const int INDEX_OPEN_FOLDER = 4;

        private WebTemplateLibrary _templateLibrary;
        private TemplateTreeBuilder _treeBuilder;
        private object _lastObject = null;
        private ShowGUIEventHandler _guiHandler;

        public event EventHandler TemplateDelete;
        public event EventHandler TemplateOpen;
        public event EventHandler TemplateUpdate;
        public event EventHandler GeneratedFileSaved;
        public event EventHandler ErrorsOccurred;
        public ExecuteTemplateDelegate ExecuteTemplateOverride;

        public TemplateBrowserControl()
        {
            InitializeComponent();
            this._guiHandler = new ShowGUIEventHandler(this.DynamicGUI_Display);

        }

        public void Initialize()
        {
            _treeBuilder = new TemplateTreeBuilder(treeViewTemplates);
            _treeBuilder.LoadTemplates();
        }

        public TemplateTreeBuilder TreeBuilder
        {
            get
            {
                return _treeBuilder;
            }
        }

        public string GetPersistString()
        {
            return this.GetType().FullName;
        }

        protected void OnErrorsOccurred(Exception ex)
        {
            if (ErrorsOccurred != null)
            {
                ErrorsOccurred(ex, EventArgs.Empty);
            }
        }

        protected void OnTemplateUpdate(string uniqueId)
        {
            if (TemplateUpdate != null)
            {
                TemplateUpdate(uniqueId, EventArgs.Empty);
            }
        }

        protected void OnTemplateOpen(string path)
        {
            if (TemplateOpen != null)
            {
                TemplateOpen(path, EventArgs.Empty);
            }
        }


        protected void OnTemplateDelete(string path)
        {
            if (TemplateDelete != null)
            {
                TemplateDelete(path, EventArgs.Empty);
            }
        }

        protected void OnGeneratedFileSaved(string path)
        {
            if (GeneratedFileSaved != null)
            {
                GeneratedFileSaved(path, EventArgs.Empty);
            }
        }

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
                OnTemplateOpen(this.treeViewTemplates.SelectedNode.Tag.ToString());
            }
        }

        public void RefreshTree()
        {
            _treeBuilder.Clear();

            if (this.toolStripButtonMode.Checked)
                this._treeBuilder.LoadTemplatesByFile();
            else
                this._treeBuilder.LoadTemplates();
        }

        public void ChangeMode()
        {
            if (this.toolStripButtonMode.Checked) this._treeBuilder.LoadTemplatesByFile();
            else this._treeBuilder.LoadTemplates();
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
                    TemplateWebUpdateHelper.WebUpdate(tnode.UniqueId, tnode.Tag.ToString(), false);
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
                    TemplateWebUpdateHelper.WebUpdate(tnode.UniqueId, tnode.Tag.ToString(), true);
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

        #region ExecuteTemplate, SaveInput, ExecuteLoadedInput, DynamicGUI_Display

        public void ExecuteTemplate(ZeusTemplate template)
        {
            bool overridden = false;
            if (ExecuteTemplateOverride != null)
            {
                overridden = ExecuteTemplateOverride(TemplateOperations.Execute, template, null, _guiHandler);
            }
            
            if (!overridden)
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
                    template.GuiSegment.ZeusScriptingEngine.ExecutionHelper.SetShowGuiHandler(_guiHandler);
                    result = template.GuiSegment.Execute(context);
                    template.GuiSegment.ZeusScriptingEngine.ExecutionHelper.Cleanup();

                    if (result)
                    {
                        template.BodySegment.ZeusScriptingEngine.ExecutionHelper.Timeout = settings.ScriptTimeout;
                        result = template.BodySegment.Execute(context);

                        foreach (string filePath in context.Output.SavedFiles)
                        {
                            this.OnGeneratedFileSaved(filePath);
                        }

                        template.BodySegment.ZeusScriptingEngine.ExecutionHelper.Cleanup();
                    }
                }
                catch (Exception ex)
                {
                    OnErrorsOccurred(ex);

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
        }

        public void SaveInput(ZeusTemplate template)
        {
            bool overridden = false;
            if (ExecuteTemplateOverride != null)
            {
                overridden = ExecuteTemplateOverride(TemplateOperations.SaveInput, template, null, _guiHandler);
            }

            if (!overridden)
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
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
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
                    OnErrorsOccurred(ex);
                }

                Cursor.Current = Cursors.Default;
            }
        }

        public void ExecuteLoadedInput()
        {
            bool overridden = false;
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Zues Input Files (*.zinp)|*.zinp";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ZeusSavedInput savedInput = null;
                ZeusTemplate template = null;
                if (ExecuteTemplateOverride != null)
                {
                    try
                    {
                        foreach (string filename in openFileDialog.FileNames)
                        {
                            savedInput = new ZeusSavedInput(filename);
                            if (savedInput.Load())
                            {
                                template = new ZeusTemplate(savedInput.InputData.TemplatePath);

                                overridden = ExecuteTemplateOverride(TemplateOperations.ExecuteLoadedInput, template, savedInput, _guiHandler);
                                if (!overridden) break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        OnErrorsOccurred(ex);
                    }
                }
                

                if (!overridden)
                {
                    try
                    {
                        ZeusSimpleLog log = new ZeusSimpleLog();
                        DefaultSettings settings = DefaultSettings.Instance;

                        Cursor.Current = Cursors.WaitCursor;

                        foreach (string filename in openFileDialog.FileNames)
                        {
                            savedInput = new ZeusSavedInput(filename);
                            if (savedInput.Load())
                            {
                                ZeusContext context = new ZeusContext();
                                context.Input.AddItems(savedInput.InputData.InputItems);
                                context.Log = log;

                                template = new ZeusTemplate(savedInput.InputData.TemplatePath);
                                template.Execute(context, settings.ScriptTimeout, true);

                                foreach (string filePath in context.Output.SavedFiles)
                                {
                                    this.OnGeneratedFileSaved(filePath);
                                }

                                if (log.HasExceptions)
                                {
                                    throw log.Exceptions[0];
                                }
                            }
                        }

                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(this, "Selected files have been executed.");

                    }
                    catch (Exception ex)
                    {
                        OnErrorsOccurred(ex);
                    }

                    Cursor.Current = Cursors.Default;
                }
            }
        }

        public void DynamicGUI_Display(IZeusGuiControl gui, IZeusFunctionExecutioner executioner)
        {
            this.Cursor = Cursors.Default;

            try
            {
                DynamicForm df = new DynamicForm(gui as GuiController, executioner);
                DialogResult result = df.ShowDialog(this);

                if (result == DialogResult.Cancel)
                {
                    gui.IsCanceled = true;
                }
            }
            catch (Exception ex)
            {
                OnErrorsOccurred(ex);
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region TemplateBrowserControl EventHandlers
        private void TemplateBrowserControl_MouseLeave(object sender, System.EventArgs e)
        {
            this.toolTipTemplateBrowser.SetToolTip(treeViewTemplates, string.Empty);
        }        
        #endregion

        #region TreeView EventHandlers
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
                this._treeBuilder.LoadTemplates();
                return;
            }
        }

        private void treeViewTemplates_DoubleClick(object sender, EventArgs e)
        {
            if (this.treeViewTemplates.SelectedNode is TemplateTreeNode)
            {
                if (((TemplateTreeNode)treeViewTemplates.SelectedNode).IsLocked)
                {
                    MessageBox.Show(this, "You cannot open a locked template.");
                }
                else
                {
                    OnTemplateOpen(this.treeViewTemplates.SelectedNode.Tag.ToString());
                }
            }
        }

        private void treeViewTemplates_MouseMove(object sender, MouseEventArgs e)
        {
            object obj = treeViewTemplates.GetNodeAt(e.X, e.Y);
            if (object.Equals(obj, _lastObject) || (obj == null && _lastObject == null))
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

                _lastObject = obj;
            }
        }
        #endregion

        #region Toolbar EventHandlers
        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            this.RefreshTree();
        }

        private void toolStripButtonMode_Click(object sender, EventArgs e)
        {
            this.ChangeMode();
        }

        private void toolStripButtonWebLibrary_Click(object sender, EventArgs e)
        {
            try
            {
                if (_templateLibrary == null)
                {
                    _templateLibrary = new WebTemplateLibrary(_treeBuilder.IdPathHash);
                }

                _templateLibrary.ShowDialog(this);
                foreach (string uniqueid in _templateLibrary.UpdatedTemplateIDs)
                {
                    this.OnTemplateUpdate(uniqueid);
                }

                this.RefreshTree();
            }
            catch
            {
                _templateLibrary = null;
                MessageBox.Show("The WebUpdate feature failed to connect to the server.");
            }
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            this.Open();
        }

        private void toolStripButtonRecord_Click(object sender, EventArgs e)
        {
            this.SaveInput();
        }

        private void toolStripButtonReplay_Click(object sender, EventArgs e)
        {
            this.ExecuteSavedInput();
        }

        private void toolStripButtonExecute_Click(object sender, EventArgs e)
        {
            this.Execute();
        }
        #endregion

        #region ContextMenu EventHandlers
        private void contextMenuTree_Popup(object sender, System.EventArgs e)
        {
            System.Drawing.Point point = this.treeViewTemplates.PointToClient(Cursor.Position);
            object node = this.treeViewTemplates.GetNodeAt(point.X, point.Y);

            // Everything is off by default
            this.menuItemExecute.Visible = false;
            this.menuItemOpen.Visible = false;
            this.menuItemWebUpdate.Visible = false;
            this.menuItemDelete.Visible = false;
            this.menuItemRecord.Visible = false;

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

        private void menuItemDelete_Click(object sender, EventArgs e)
        {
            this.Delete();
        }

        private void menuItemRecord_Click(object sender, EventArgs e)
        {
            this.SaveInput();
        }
        #endregion
    }

    public enum TemplateOperations
    {
        Execute = 0,
        SaveInput,
        ExecuteLoadedInput
    }

    public delegate bool ExecuteTemplateDelegate(TemplateOperations operation, ZeusTemplate template, ZeusSavedInput input, ShowGUIEventHandler guiEventHandler);

}
