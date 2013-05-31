using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using Zeus;
using Zeus.Projects;
using Zeus.Serializers;
using Zeus.UserInterface;
using Zeus.UserInterface.WinForms;
using MyMeta;
using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration
{
    public delegate void TextChangedEventHandler(string text, string tabText, string filename);
    public delegate void ProjectExecutionStatusHandler(bool isRunning, string message);
    public partial class ProjectBrowserControl : UserControl
    {
        private const int INDEX_CLOSED_FOLDER = 0;
        private const int INDEX_OPEN_FOLDER = 1;

        private ZeusProcessStatusDelegate _executionCallback;
        private ProjectTreeNode _rootNode;
        private FormAddEditModule _formEditModule = new FormAddEditModule();
        private FormAddEditSavedObject _formEditSavedObject;
        private bool _isDirty = false;
        private bool _collectInChildProcess = false;

        public event TextChangedEventHandler TabTextChanged;
        public event ProjectExecutionStatusHandler ExecutionStatusUpdate;
        public event EventHandler ErrorsOccurred;
        public event EventHandler ExecutionStarted;

        public ProjectBrowserControl()
        {
            InitializeComponent();
            _executionCallback = new ZeusProcessStatusDelegate(ExecutionCallback);
            _formEditSavedObject = new FormAddEditSavedObject(_collectInChildProcess);
        }
        protected void OnExecutionStatusUpdate(bool isRunning, string message)
        {
            if (ExecutionStatusUpdate != null)
            {
                ExecutionStatusUpdate(isRunning, message);
            }
        }
        protected void OnExecutionStarted()
        {
            if (ExecutionStarted != null)
            {
                ExecutionStarted(this, EventArgs.Empty);
            }
        }

        protected void OnErrorsOccurred(Exception ex)
        {
            if (ErrorsOccurred != null)
            {
                ErrorsOccurred(ex, EventArgs.Empty);
            }
        }

        protected void OnTextChanged(string text, string tabText, string filename)
        {
            if (TabTextChanged != null)
            {
                TabTextChanged(text, tabText, filename);
            }
        }

        public bool CollectInChildProcess
        {
            get { return _collectInChildProcess; }
            set { _collectInChildProcess = value; }
        }

        public string GetPersistString()
        {
            if (this._rootNode != null &&
                this._rootNode.Project != null &&
                this._rootNode.Project.FilePath != null)
            {
                return "file," + this._rootNode.Project.FilePath;
            }
            else
            {
                return "type," + ProjectEditorManager.MYGEN_PROJECT;
            }
        }

        public void SetToolTip(string str)
        {
            this.toolTipProjectBrowser.SetToolTip(treeViewProject, str);
        }

        public string DocumentIndentity
        {
            get
            {
                return (this.FileName == string.Empty) ?
                    this._rootNode.Project.Name :
                    this.FileName;
            }
        }

        public string FileName
        {
            get
            {
                if (_rootNode.Project.FilePath != null)
                {
                    return _rootNode.Project.FilePath;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string CompleteFilePath
        {
            get
            {
                string tmp = this.FileName;
                if (tmp != string.Empty)
                {
                    FileInfo attr = new FileInfo(tmp);
                    tmp = attr.FullName;
                }

                return tmp;
            }
        }

        public bool IsDirty
        {
            get
            {
                return this._isDirty;
            }
        }

        public bool CanClose(bool allowPrevent)
        {
            return PromptForSave(allowPrevent);
        }

        private bool PromptForSave(bool allowPrevent)
        {
            bool canClose = true;

            if (this.IsDirty)
            {
                DialogResult result;

                if (allowPrevent)
                {
                    result = MessageBox.Show("This project has been modified, Do you wish to save before closing?",
                        this.FileName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                }
                else
                {
                    result = MessageBox.Show("This project has been modified, Do you wish to save before closing?",
                        this.FileName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }

                switch (result)
                {
                    case DialogResult.Yes:
                        this.Save();
                        break;
                    case DialogResult.Cancel:
                        canClose = false;
                        break;
                }
            }

            return canClose;
        }

        #region TreeView Event Handlers
        private void treeViewProject_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if ((e.Node is ProjectTreeNode) || (e.Node is ModuleTreeNode))
            {
                e.Node.SelectedImageIndex = INDEX_OPEN_FOLDER;
                e.Node.ImageIndex = INDEX_OPEN_FOLDER;
            }
        }

        private void treeViewProject_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if ((e.Node is ProjectTreeNode) || (e.Node is ModuleTreeNode))
            {
                e.Node.SelectedImageIndex = INDEX_CLOSED_FOLDER;
                e.Node.ImageIndex = INDEX_CLOSED_FOLDER;
            }
        }


        private void treeViewProject_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            TreeNode node = (TreeNode)treeViewProject.GetNodeAt(e.X, e.Y);
            treeViewProject.SelectedNode = node;
        }

        //TODO: Add keypress events
        private void treeViewProject_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //------------------
            }
        }

        private void treeViewProject_DoubleClick(object sender, System.EventArgs e)
        {
            Edit(false);
        }

        private object lastObject = null;

        private void treeViewProject_MouseMove(object sender, MouseEventArgs e)
        {
            object obj = treeViewProject.GetNodeAt(e.X, e.Y);
            if (object.Equals(obj, lastObject) || (obj == null && lastObject == null))
            {
                return;
            }
            else
            {
                if (obj is SavedObjectTreeNode)
                {
                    this.toolTipProjectBrowser.SetToolTip(treeViewProject, ((SavedObjectTreeNode)obj).SavedObject.SavedObjectName);
                }
                else if (obj is ProjectTreeNode)
                {
                    this.toolTipProjectBrowser.SetToolTip(treeViewProject, ((ProjectTreeNode)obj).Project.Description);
                }
                else if (obj is ModuleTreeNode)
                {
                    this.toolTipProjectBrowser.SetToolTip(treeViewProject, ((ModuleTreeNode)obj).Module.Description);
                }
                else
                {
                    this.toolTipProjectBrowser.SetToolTip(treeViewProject, string.Empty);
                }

                lastObject = obj;
            }
        }
        #endregion

        #region ContextMenu Event Handlers
        private void contextMenuTree_Popup(object sender, System.EventArgs e)
        {
            this.contextItemAddModule.Visible =
                this.contextItemAddSavedObject.Visible =
                this.contextItemEdit.Visible =
                this.contextItemExecute.Visible =
                this.contextItemRemove.Visible =
                this.contextItemSave.Visible =
                this.contextItemCopy.Visible =
                this.contextItemSaveAs.Visible =
                this.contextItemCacheSettings.Visible =
                this.menuItemSep01.Visible =
                this.menuItemSep02.Visible =
                this.menuItemSep03.Visible = false;

            SortedProjectTreeNode node = this.treeViewProject.SelectedNode as SortedProjectTreeNode;
            if (node is ProjectTreeNode)
            {
                this.contextItemSave.Visible =
                    this.contextItemSaveAs.Visible =
                    this.contextItemAddModule.Visible =
                    this.contextItemAddSavedObject.Visible =
                    this.contextItemEdit.Visible =
                    this.contextItemExecute.Visible =
                    this.contextItemCacheSettings.Visible = true;

                this.menuItemSep01.Visible = true;
                this.menuItemSep02.Visible = true;
                this.menuItemSep03.Visible = true;
            }
            else if (node is ModuleTreeNode)
            {
                this.contextItemAddModule.Visible =
                    this.contextItemAddSavedObject.Visible =
                    this.contextItemEdit.Visible =
                    this.contextItemExecute.Visible =
                    this.contextItemRemove.Visible =
                    this.contextItemCacheSettings.Visible = true;

                this.menuItemSep01.Visible = true;
                this.menuItemSep02.Visible = true;
                this.menuItemSep03.Visible = false;
            }
            else if (node is SavedObjectTreeNode)
            {
                this.contextItemEdit.Visible =
                    this.contextItemExecute.Visible =
                    this.contextItemCopy.Visible =
                    this.contextItemRemove.Visible = true;

                this.menuItemSep01.Visible = true;
                this.menuItemSep02.Visible = false;
                this.menuItemSep03.Visible = false;
            }
        }

        private void contextItemEdit_Click(object sender, System.EventArgs e)
        {
            Edit();
        }

        private void contextItemAddModule_Click(object sender, System.EventArgs e)
        {
            SortedProjectTreeNode node = this.treeViewProject.SelectedNode as SortedProjectTreeNode;
            if ((node is ModuleTreeNode) || (node is ProjectTreeNode))
            {
                ZeusModule module = node.Tag as ZeusModule;

                ZeusModule newmodule = new ZeusModule();

                this._formEditModule.Module = newmodule;
                if (this._formEditModule.ShowDialog() == DialogResult.OK)
                {
                    this._isDirty = true;
                    module.ChildModules.Add(newmodule);

                    ModuleTreeNode newNode = new ModuleTreeNode(newmodule);
                    node.AddSorted(newNode);
                    node.Expand();
                }
            }
        }

        private void contextItemAddSavedObject_Click(object sender, System.EventArgs e)
        {
            SortedProjectTreeNode node = this.treeViewProject.SelectedNode as SortedProjectTreeNode;
            if ((node is ModuleTreeNode) || (node is ProjectTreeNode))
            {
                ZeusModule module = node.Tag as ZeusModule;

                SavedTemplateInput savedInput = new SavedTemplateInput();
                this._formEditSavedObject.Module = module;
                this._formEditSavedObject.SavedObject = savedInput;
                if (this._formEditSavedObject.ShowDialog() == DialogResult.OK)
                {
                    this._isDirty = true;
                    module.SavedObjects.Add(savedInput);

                    SavedObjectTreeNode newNode = new SavedObjectTreeNode(savedInput);
                    node.AddSorted(newNode);
                    node.Expand();
                    this.treeViewProject.SelectedNode = newNode;
                }
            }
        }

        private void contextItemCopy_Click(object sender, System.EventArgs e)
        {
            SortedProjectTreeNode node = this.treeViewProject.SelectedNode as SortedProjectTreeNode;
            if (node is SavedObjectTreeNode)
            {
                SavedTemplateInput input = node.Tag as SavedTemplateInput;
                SavedTemplateInput copy = input.Copy();

                SortedProjectTreeNode moduleNode = node.Parent as SortedProjectTreeNode;

                ZeusModule module = moduleNode.Tag as ZeusModule;

                string copyName = copy.SavedObjectName;
                string newName = copyName;
                int count = 1;
                bool found;
                do
                {
                    found = false;
                    foreach (SavedTemplateInput tmp in module.SavedObjects)
                    {
                        if (tmp.SavedObjectName == newName)
                        {
                            found = true;
                            newName = copyName + " " + count++;
                            break;
                        }
                    }
                } while (found);

                copy.SavedObjectName = newName;

                module.SavedObjects.Add(copy);

                SavedObjectTreeNode copiedNode = new SavedObjectTreeNode(copy);
                moduleNode.AddSorted(copiedNode);

                this._isDirty = true;
            }
        }

        private void contextItemCacheSettings_Click(object sender, System.EventArgs e)
        {
            SortedProjectTreeNode node = this.treeViewProject.SelectedNode as SortedProjectTreeNode;
            if ((node is ModuleTreeNode) || (node is ProjectTreeNode))
            {
                ZeusModule module = node.Tag as ZeusModule;
                ZeusContext context = new ZeusContext();
                DefaultSettings settings = DefaultSettings.Instance;
                settings.PopulateZeusContext(context);
                module.SavedItems.Add(context.Input);
            }
        }

        private void contextItemRemove_Click(object sender, System.EventArgs e)
        {
            SortedProjectTreeNode node = this.treeViewProject.SelectedNode as SortedProjectTreeNode;
            SortedProjectTreeNode parentnode;
            if (node is ModuleTreeNode)
            {
                parentnode = node.Parent as SortedProjectTreeNode;

                ZeusModule parentmodule = parentnode.Tag as ZeusModule;
                ZeusModule module = node.Tag as ZeusModule;

                parentmodule.ChildModules.Remove(module);
                parentnode.Nodes.Remove(node);
                this._isDirty = true;
            }
            else if (node is SavedObjectTreeNode)
            {
                parentnode = node.Parent as SortedProjectTreeNode;

                ZeusModule parentmodule = parentnode.Tag as ZeusModule;
                SavedTemplateInput savedobj = node.Tag as SavedTemplateInput;

                parentmodule.SavedObjects.Remove(savedobj);
                parentnode.Nodes.Remove(node);
                this._isDirty = true;
            }
        }

        private void contextItemSave_Click(object sender, System.EventArgs e)
        {
            Save();
        }

        private void contextItemSaveAs_Click(object sender, System.EventArgs e)
        {
            SaveAs();
        }

        private void contextItemExecute_Click(object sender, System.EventArgs e)
        {
            this.Execute();
        }
        #endregion

        #region Load Project Tree
        public void CreateNewProject()
        {
            this.treeViewProject.Nodes.Clear();

            ZeusProject proj = new ZeusProject();
            proj.DefaultSettingsDelegate = new GetDefaultSettingsDelegate(GetDefaultSettingsDictionary);
            proj.Name = "New Project";
            proj.Description = "New Zeus Project file";

            _rootNode = new ProjectTreeNode(proj);
            _rootNode.Expand();

            OnTextChanged("Project: " + proj.Name, proj.Name, null);

            this.treeViewProject.Nodes.Add(_rootNode);
        }

        public void LoadProject(string filename)
        {
            this.treeViewProject.Nodes.Clear();

            ZeusProject proj = new ZeusProject(filename);
            proj.DefaultSettingsDelegate = new GetDefaultSettingsDelegate(GetDefaultSettingsDictionary);
            if (proj.Load())
            {
                OnTextChanged("Project: " + proj.Name, proj.Name, filename);

                _rootNode = new ProjectTreeNode(proj);

                foreach (ZeusModule module in proj.ChildModules)
                {
                    LoadModule(_rootNode, module);
                }

                foreach (SavedTemplateInput input in proj.SavedObjects)
                {
                    _rootNode.AddSorted(new SavedObjectTreeNode(input));
                }
            }
            _rootNode.Expand();


            this.treeViewProject.Nodes.Add(_rootNode);
        }

        public Dictionary<string, string> GetDefaultSettingsDictionary()
        {
            Dictionary<string, string> ds = new Dictionary<string, string>();
            DefaultSettings.Instance.PopulateDictionary(ds);
            return ds;
        }

        private void LoadModule(SortedProjectTreeNode parentNode, ZeusModule childModule)
        {
            ModuleTreeNode childNode = new ModuleTreeNode(childModule);
            parentNode.AddSorted(childNode);

            foreach (ZeusModule grandchildModule in childModule.ChildModules)
            {
                LoadModule(childNode, grandchildModule);
            }

            foreach (SavedTemplateInput input in childModule.SavedObjects)
            {
                childNode.AddSorted(new SavedObjectTreeNode(input));
            }
        }
        #endregion

        #region Execute, Save, SaveAs, Edit
        public void Execute()
        {
            this.Save();
            ZeusProject proj = this._rootNode.Project;

            Cursor.Current = Cursors.WaitCursor;

            TreeNode node = this.treeViewProject.SelectedNode;
            DefaultSettings settings = DefaultSettings.Instance;

            if (node is ProjectTreeNode)
            {
                OnExecutionStarted();
                ZeusProcessManager.ExecuteProject(proj.FilePath, ExecutionCallback);
            }
            else if (node is ModuleTreeNode)
            {
                ZeusModule module = node.Tag as ZeusModule;
                OnExecutionStarted();
                ZeusProcessManager.ExecuteModule(proj.FilePath, module.ProjectPath, ExecutionCallback);
                //module.Execute(settings.ScriptTimeout, log);
            }
            else if (node is SavedObjectTreeNode)
            {
                SavedTemplateInput savedinput = node.Tag as SavedTemplateInput;
                ZeusModule module = node.Parent.Tag as ZeusModule;
                OnExecutionStarted();
                ZeusProcessManager.ExecuteProjectItem(proj.FilePath, module.ProjectPath + "/" + savedinput.SavedObjectName, ExecutionCallback);
                //savedinput.Execute(settings.ScriptTimeout, log);
            }
        }

        private void ExecutionCallback(ZeusProcessStatusEventArgs args)
        {
            if (args.Message != null)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(_executionCallback, args);
                }
                else
                {
                    this.OnExecutionStatusUpdate(args.IsRunning, args.Message);
                    if (!args.IsRunning)
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        public void Save()
        {
            if (this._rootNode.Project.FilePath != null)
            {
                _isDirty = false;

                this._rootNode.Project.Save();
            }
            else
            {
                this.SaveAs();
            }
        }

        public void SaveAs()
        {
            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Zeus Project (*.zprj)|*.zprj|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;

            ZeusProject proj = this._rootNode.Project;
            if (proj.FilePath != null)
            {
                saveFileDialog.FileName = proj.FilePath;
            }

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {

                myStream = saveFileDialog.OpenFile();

                if (null != myStream)
                {
                    _isDirty = false;

                    myStream.Close();
                    proj.FilePath = saveFileDialog.FileName;
                    proj.Save();
                }
            }
        }

        public void Edit()
        {
            Edit(true);
        }

        public void Edit(bool allowEditFolders)
        {
            SortedProjectTreeNode node = this.treeViewProject.SelectedNode as SortedProjectTreeNode;
            SortedProjectTreeNode parentnode;
            if ( allowEditFolders && ((node is ModuleTreeNode) || (node is ProjectTreeNode)) )
            {
                ZeusModule module = node.Tag as ZeusModule;
                this._formEditModule.Module = module;
                if (this._formEditModule.ShowDialog() == DialogResult.OK)
                {
                    this._isDirty = true;
                    node.Text = module.Name;
                    parentnode = node.Parent as SortedProjectTreeNode;
                    if (parentnode != null)
                    {
                        node.Remove();
                        parentnode.AddSorted(node);
                        this.treeViewProject.SelectedNode = node;
                    }

                    if (node is ProjectTreeNode)
                    {
                        OnTextChanged("Project: " + module.Name, module.Name, _rootNode.Project.FilePath);
                    }
                }
            }
            else if (node is SavedObjectTreeNode)
            {
                if (_collectInChildProcess) Save();

                SavedTemplateInput input = node.Tag as SavedTemplateInput;
                ZeusModule parentMod = node.Parent.Tag as ZeusModule;
                this._formEditSavedObject.Module = parentMod;
                this._formEditSavedObject.SavedObject = input;
                if (this._formEditSavedObject.ShowDialog() == DialogResult.OK)
                {
                    this._isDirty = true;
                    node.Text = input.SavedObjectName;
                    parentnode = node.Parent as SortedProjectTreeNode;
                    if (parentnode != null)
                    {
                        node.Remove();
                        parentnode.AddSorted(node);
                        this.treeViewProject.SelectedNode = node;
                    }
                }
            }
        }
        #endregion
    }

    #region Project Browser Tree Node Classes
    public abstract class SortedProjectTreeNode : TreeNode
    {
        public int AddSorted(TreeNode newnode)
        {
            int insertIndex = -1;
            for (int i = 0; i < this.Nodes.Count; i++)
            {
                TreeNode node = this.Nodes[i];
                if (node.GetType() == newnode.GetType())
                {
                    if (newnode.Text.CompareTo(node.Text) <= 0)
                    {
                        insertIndex = i;
                        break;
                    }
                }
                else if (newnode is SavedObjectTreeNode)
                {
                    continue;
                }
                else
                {
                    insertIndex = i;
                    break;
                }
            }

            if (insertIndex == -1)
            {
                insertIndex = this.Nodes.Add(newnode);
            }
            else
            {
                this.Nodes.Insert(insertIndex, newnode);
            }

            return insertIndex;
        }
    }

    public class ProjectTreeNode : SortedProjectTreeNode
    {
        public ProjectTreeNode(ZeusProject proj)
        {
            this.Tag = proj;

            this.Text = proj.Name;
            this.ImageIndex = 1;
            this.SelectedImageIndex = 1;
        }

        public ZeusProject Project
        {
            get
            {
                return this.Tag as ZeusProject;
            }
        }
    }

    public class ModuleTreeNode : SortedProjectTreeNode
    {
        public ModuleTreeNode(ZeusModule module)
        {
            this.Tag = module;

            this.Text = module.Name;
            this.ImageIndex = 0;
            this.SelectedImageIndex = 0;
        }

        public ZeusModule Module
        {
            get
            {
                return this.Tag as ZeusModule;
            }
        }
    }

    public class SavedObjectTreeNode : SortedProjectTreeNode
    {
        public SavedObjectTreeNode(SavedTemplateInput templateInput)
        {
            this.Tag = templateInput;

            this.ForeColor = Color.Blue;
            this.Text = templateInput.SavedObjectName;
            this.ImageIndex = 2;
            this.SelectedImageIndex = 2;
        }

        public SavedTemplateInput SavedObject
        {
            get
            {
                return this.Tag as SavedTemplateInput;
            }
        }
    }
    #endregion
}
