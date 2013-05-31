using System;
using System.Drawing;
using System.Collections;
using System.Text;
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
	/// Summary description for AddEditSavedObject.
	/// </summary>
	public class FormAddEditSavedObject : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;

		private SavedTemplateInput _savedObject;
		private ZeusModule _module = null;
		private ArrayList _extensions = new ArrayList();
		private bool _isActivated = false;
        private bool _collectInChildProcess = false;
        private bool _insideRecording = false;
        private ZeusProcessStatusDelegate _executionCallback;
        private StringBuilder _collectedInput = new StringBuilder();

		private System.Windows.Forms.Button buttonCollectInput;
		private System.Windows.Forms.Label labelName;
		private System.Windows.Forms.TextBox textBoxName;
		private System.Windows.Forms.Button buttonViewData;

		private FormViewSavedInput formViewSavedInput = new FormViewSavedInput();
		private System.Windows.Forms.ErrorProvider errorProviderRequiredFields;

		private TreeNode _lastRecordedSelectedNode = null;
		private string _lastRecordedSelectedId = string.Empty;
		private System.Windows.Forms.Button buttonClearInput;
		private System.Windows.Forms.TreeView treeViewTemplates;
		private TemplateTreeBuilder treeBuilder = null;
		private System.Windows.Forms.Label labelTemplate;
        private IMyGenerationMDI mdi;
        private IContainer components;

		//public FormAddEditSavedObject(IMyGenerationMDI mdi)
        public FormAddEditSavedObject(bool collectInChildProcess)
		{
			InitializeComponent();
			treeBuilder = new TemplateTreeBuilder(this.treeViewTemplates);

			_extensions.Add(".zeus");
			_extensions.Add(".jgen");
			_extensions.Add(".vbgen");
			_extensions.Add(".csgen");

            _executionCallback = new ZeusProcessStatusDelegate(ExecutionCallback);
            _collectInChildProcess = collectInChildProcess;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddEditSavedObject));
            this.buttonCollectInput = new System.Windows.Forms.Button();
            this.labelName = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.buttonViewData = new System.Windows.Forms.Button();
            this.errorProviderRequiredFields = new System.Windows.Forms.ErrorProvider(this.components);
            this.buttonClearInput = new System.Windows.Forms.Button();
            this.treeViewTemplates = new System.Windows.Forms.TreeView();
            this.labelTemplate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderRequiredFields)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCollectInput
            // 
            this.buttonCollectInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCollectInput.Location = new System.Drawing.Point(16, 431);
            this.buttonCollectInput.Name = "buttonCollectInput";
            this.buttonCollectInput.Size = new System.Drawing.Size(136, 24);
            this.buttonCollectInput.TabIndex = 0;
            this.buttonCollectInput.Text = "Record Template Input";
            this.buttonCollectInput.Click += new System.EventHandler(this.buttonCollectInput_Click);
            // 
            // labelName
            // 
            this.labelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelName.Location = new System.Drawing.Point(16, 8);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(564, 23);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "Name:";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(456, 431);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(56, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.Location = new System.Drawing.Point(520, 431);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(56, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(16, 32);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(560, 20);
            this.textBoxName.TabIndex = 5;
            this.textBoxName.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxName_Validating);
            // 
            // buttonViewData
            // 
            this.buttonViewData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonViewData.Location = new System.Drawing.Point(304, 431);
            this.buttonViewData.Name = "buttonViewData";
            this.buttonViewData.Size = new System.Drawing.Size(72, 23);
            this.buttonViewData.TabIndex = 6;
            this.buttonViewData.Text = "View Data";
            this.buttonViewData.Click += new System.EventHandler(this.buttonViewData_Click);
            // 
            // errorProviderRequiredFields
            // 
            this.errorProviderRequiredFields.ContainerControl = this;
            // 
            // buttonClearInput
            // 
            this.buttonClearInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClearInput.Location = new System.Drawing.Point(160, 431);
            this.buttonClearInput.Name = "buttonClearInput";
            this.buttonClearInput.Size = new System.Drawing.Size(136, 24);
            this.buttonClearInput.TabIndex = 7;
            this.buttonClearInput.Text = "Clear Template Input";
            this.buttonClearInput.Click += new System.EventHandler(this.buttonClearInput_Click);
            // 
            // treeViewTemplates
            // 
            this.treeViewTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewTemplates.Location = new System.Drawing.Point(16, 72);
            this.treeViewTemplates.Name = "treeViewTemplates";
            this.treeViewTemplates.Size = new System.Drawing.Size(560, 344);
            this.treeViewTemplates.TabIndex = 8;
            this.treeViewTemplates.Validating += new System.ComponentModel.CancelEventHandler(this.treeViewTemplates_Validating);
            this.treeViewTemplates.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewTemplates_AfterSelect);
            // 
            // labelTemplate
            // 
            this.labelTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTemplate.Location = new System.Drawing.Point(16, 56);
            this.labelTemplate.Name = "labelTemplate";
            this.labelTemplate.Size = new System.Drawing.Size(564, 16);
            this.labelTemplate.TabIndex = 10;
            this.labelTemplate.Text = "Template:";
            this.labelTemplate.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // FormAddEditSavedObject
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(600, 461);
            this.Controls.Add(this.labelTemplate);
            this.Controls.Add(this.treeViewTemplates);
            this.Controls.Add(this.buttonClearInput);
            this.Controls.Add(this.buttonViewData);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.buttonCollectInput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAddEditSavedObject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddEditSavedObject";
            this.Load += new System.EventHandler(this.FormAddEditSavedObject_Load);
            this.Activated += new System.EventHandler(this.FormAddEditSavedObject_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderRequiredFields)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
			}
		}


		public SavedTemplateInput SavedObject 
		{
			get 
			{
				return _savedObject;
			}
			set 
			{
				_savedObject = value;

				if(_savedObject.SavedObjectName != null) 
				{
					this.textBoxName.Text = _savedObject.SavedObjectName;
					this.Text = "Template Instance: " + _savedObject.SavedObjectName;
				}
				else 
				{
					this.textBoxName.Text = string.Empty;
					this.Text = "Template Instance: [New] ";
				}

				LoadTemplates(_savedObject.TemplateUniqueID);

				this._lastRecordedSelectedNode = this.treeViewTemplates.SelectedNode;

				this._isActivated = false;
                this._insideRecording = false;
			}
		}

		private TemplateTreeNode SelectedTemplate 
		{
			get 
			{
				if (this.treeViewTemplates.SelectedNode != null)
					return this.treeViewTemplates.SelectedNode as TemplateTreeNode;
				else 
					return null;
			}
		}

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			CancelEventArgs args = new CancelEventArgs();
			this.textBoxName_Validating(this, args);
			this.treeViewTemplates_Validating(this, args);

			if (!args.Cancel)
			{
				if ((this.SavedObject.InputItems.Count > 0) || 
					(MessageBox.Show("Are you sure you want to save without any recorded input?", "Warning: No Recorded Template Input", MessageBoxButtons.YesNo) == DialogResult.Yes) )
				{
					if (SavedObject.SavedObjectName != null) 
					{
						this.Module.SavedObjects.Remove(SavedObject.SavedObjectName);
					}

					this.SavedObject.SavedObjectName = this.textBoxName.Text;

					TemplateTreeNode item = this.SelectedTemplate;
					if (item != null)
					{
						this.SavedObject.TemplatePath = item.Tag.ToString();
						this.SavedObject.TemplateUniqueID = item.UniqueId;
					}

					this.Module.SavedObjects[SavedObject.SavedObjectName] = SavedObject;

					this.DialogResult = DialogResult.OK;
					this.Close();
				}
			}
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		public TemplateTreeNode FindNodeByID(string id, TreeNode parentNode, bool returnFirst) 
		{
			TemplateTreeNode selectedNode = null;
			foreach (TreeNode node in parentNode.Nodes) 
			{
				if (node is TemplateTreeNode) 
				{
					selectedNode = node as TemplateTreeNode;
					
					if (returnFirst) return selectedNode;

					if (selectedNode.UniqueId.ToLower() != id) 
					{
						selectedNode = null;
					}
					else 
					{
						return selectedNode;
					}
				}

				selectedNode = FindNodeByID(id, node, returnFirst);
				if (selectedNode != null) 
				{
					return selectedNode;
				}
			}
			return selectedNode;
		}

		public void LoadTemplates(string selectedid) 
		{
			treeViewTemplates.Nodes.Clear();
			treeViewTemplates.HideSelection = false;
			treeBuilder = new TemplateTreeBuilder(treeViewTemplates);
			treeBuilder.LoadTemplates();
			
			TemplateTreeNode selectedNode = null;
			string id = selectedid;
			if (id != null)  id = id.ToLower();
			selectedNode = FindNodeByID(id, treeViewTemplates.Nodes[0], false);

			if (selectedNode == null)
			{
				selectedNode = FindNodeByID(_lastRecordedSelectedId, treeViewTemplates.Nodes[0], false);

				if (selectedNode == null)
				{
					selectedNode = FindNodeByID(id, treeViewTemplates.Nodes[0], true);
				}
			}

			if (selectedNode != null) 
			{
				_lastRecordedSelectedId = selectedNode.UniqueId.ToLower();

				this.treeViewTemplates.SelectedNode = selectedNode;

				TreeNode parent = selectedNode.Parent;
				while (parent != null) 
				{
					parent.Expand();
					parent = parent.Parent;
				}
			}
		}

		private void SaveInput() 
		{
			try 
			{
                if (_collectInChildProcess)
                {
                    this.buttonCollectInput.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;
                    ZeusProcessManager.RecordProjectItem(this._module.RootProject.FilePath, _module.ProjectPath + "/" + SavedObject.SavedObjectName, this.SelectedTemplate.Tag.ToString(), _executionCallback);
                }
                else
                {
                    //RecordProjectItem
                    ZeusTemplate template = new ZeusTemplate(this.SelectedTemplate.Tag.ToString());
                    DefaultSettings settings = DefaultSettings.Instance;

                    ZeusSimpleLog log = new ZeusSimpleLog();
                    ZeusContext context = new ZeusContext();
                    context.Log = log;

                    SavedObject.TemplateUniqueID = template.UniqueID;
                    SavedObject.TemplatePath = template.FilePath + template.FileName;

                    settings.PopulateZeusContext(context);
                    if (_module != null)
                    {
                        _module.PopulateZeusContext(context);
                        _module.OverrideSavedData(SavedObject.InputItems);
                    }

                    if (template.Collect(context, settings.ScriptTimeout, SavedObject.InputItems))
                    {
                        this._lastRecordedSelectedNode = this.SelectedTemplate;
                    }

                    if (log.HasExceptions)
                    {
                        throw log.Exceptions[0];
                    }
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
                    /*if (_consoleWriteGeneratedDetails)
                    {
                        if (this._mdi.Console.DockContent.IsHidden) this._mdi.Console.DockContent.Show(_mdi.DockPanel);
                        if (!this._mdi.Console.DockContent.IsActivated) this._mdi.Console.DockContent.Activate();
                    }*/

                    if (args.Message.StartsWith(ZeusProcessManager.BEGIN_RECORDING_TAG)) 
                    {
                        _collectedInput = new StringBuilder();
                        _insideRecording = true;
                    }
                    else if (args.Message.StartsWith(ZeusProcessManager.END_RECORDING_TAG)) 
                    {
                        this._savedObject.XML = _collectedInput.ToString();
                        _insideRecording = false;
                    }
                    else if (_insideRecording)
                    {
                        _collectedInput.AppendLine(args.Message);
                    }

                    if (!args.IsRunning)
                    {
                        this.Cursor = Cursors.Default;
                        this.buttonCollectInput.Enabled = true;
                    }
                }
            }
        }

		private void ClearInput() 
		{
			if (MessageBox.Show(this, "Are you sure you want to clear the template's input collection?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) 
				== DialogResult.Yes)
			{
				SavedObject.InputItems.Clear();
			}
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

		private void buttonCollectInput_Click(object sender, System.EventArgs e)
		{
			this.SaveInput();
		}

		private void buttonClearInput_Click(object sender, System.EventArgs e)
		{
			this.ClearInput();
		}

		private void listBoxTemplates_DoubleClick(object sender, System.EventArgs e)
		{
			this.SaveInput();
		}

		private void buttonViewData_Click(object sender, System.EventArgs e)
		{
			formViewSavedInput.SavedObject = this.SavedObject;
			formViewSavedInput.ShowDialog();
		}

		private void textBoxName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string x = this.SavedObject.SavedObjectName;
			if (x == null) x = string.Empty;
			if (textBoxName.Text.Trim().Length == 0) 
			{
				e.Cancel = true;
				this.errorProviderRequiredFields.SetError(this.textBoxName, "Name is Required!");
			}
			else if (this.Module.SavedObjects.Contains(this.textBoxName.Text) &&
				(x.Trim() != this.textBoxName.Text.Trim()))
			{
				e.Cancel = true;
				this.errorProviderRequiredFields.SetError(this.textBoxName, "This name has already been difined in this folder");
			}
			else 
			{
				this.errorProviderRequiredFields.SetError(this.textBoxName, string.Empty);
			}
		}

		private void treeViewTemplates_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (this.SelectedTemplate == null) 
			{
				e.Cancel = true;
				this.errorProviderRequiredFields.SetError(this.treeViewTemplates, "Template selection is Required!");
			}
			else 
			{
				this.errorProviderRequiredFields.SetError(this.treeViewTemplates, string.Empty);
			}
		}

		private void FormAddEditSavedObject_Load(object sender, System.EventArgs e)
		{
			this.errorProviderRequiredFields.SetError(this.textBoxName, string.Empty);
			this.errorProviderRequiredFields.SetIconAlignment(this.textBoxName, ErrorIconAlignment.TopRight);
			this.errorProviderRequiredFields.SetError(this.treeViewTemplates, string.Empty);
			this.errorProviderRequiredFields.SetIconAlignment(this.treeViewTemplates, ErrorIconAlignment.TopRight);

			this.textBoxName.Focus();
		}

		private void FormAddEditSavedObject_Activated(object sender, System.EventArgs e)
		{
			if (!this._isActivated) 
			{
				this.textBoxName.Focus();
				this._isActivated = true;
			}
		}

		private void treeViewTemplates_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (this.SelectedTemplate != null) 
			{
				_lastRecordedSelectedId = SelectedTemplate.UniqueId.ToLower();
			}
		}
	}
}

