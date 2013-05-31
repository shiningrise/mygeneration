using System;
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
	/// Summary description for AddEditSavedObject.
	/// </summary>
public class FormAddEditSavedObjectEx : System.Windows.Forms.Form
{
private System.Windows.Forms.Button buttonOK;
private System.Windows.Forms.Button buttonCancel;

private SavedTemplateInput _savedObject;
private ZeusModule _module = null;
private ArrayList _extensions = new ArrayList();
private bool _isActivated = false;

private System.Windows.Forms.Button buttonCollectInput;
private System.Windows.Forms.Label labelName;
private System.Windows.Forms.ListBox listBoxTemplates;
private System.Windows.Forms.TextBox textBoxName;
private System.Windows.Forms.Button buttonViewData;

private FormViewSavedInput formViewSavedInput = new FormViewSavedInput();
private System.Windows.Forms.ErrorProvider errorProviderRequiredFields;

private int _lastRecordedSelectedIndex = -1;
private System.Windows.Forms.Button buttonClearInput;

		/// <summary>
		/// Required designer variable.
		/// </summary>
private System.ComponentModel.Container components = null;

public FormAddEditSavedObjectEx()
{
			//
			// Required for Windows Form Designer support
			//
InitializeComponent();

_extensions.Add(".zeus");
_extensions.Add(".jgen");
_extensions.Add(".vbgen");
_extensions.Add(".csgen");

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
this.buttonCollectInput = new System.Windows.Forms.Button();
this.labelName = new System.Windows.Forms.Label();
this.listBoxTemplates = new System.Windows.Forms.ListBox();
this.buttonOK = new System.Windows.Forms.Button();
this.buttonCancel = new System.Windows.Forms.Button();
this.textBoxName = new System.Windows.Forms.TextBox();
this.buttonViewData = new System.Windows.Forms.Button();
this.errorProviderRequiredFields = new System.Windows.Forms.ErrorProvider();
this.buttonClearInput = new System.Windows.Forms.Button();
this.SuspendLayout();
			// 
			// buttonCollectInput
			// 
this.buttonCollectInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
this.buttonCollectInput.Location = new System.Drawing.Point(16, 360);
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
			// listBoxTemplates
			// 
this.listBoxTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
| System.Windows.Forms.AnchorStyles.Left) 
| System.Windows.Forms.AnchorStyles.Right)));
this.listBoxTemplates.Location = new System.Drawing.Point(16, 64);
this.listBoxTemplates.Name = "listBoxTemplates";
this.listBoxTemplates.Size = new System.Drawing.Size(560, 290);
this.listBoxTemplates.Sorted = true;
this.listBoxTemplates.TabIndex = 2;
this.listBoxTemplates.DoubleClick += new System.EventHandler(this.listBoxTemplates_DoubleClick);
this.listBoxTemplates.Validating += new System.ComponentModel.CancelEventHandler(this.listBoxTemplates_Validating);
			// 
			// buttonOK
			// 
this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
this.buttonOK.Location = new System.Drawing.Point(456, 360);
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
this.buttonCancel.Location = new System.Drawing.Point(520, 360);
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
this.textBoxName.Text = "";
this.textBoxName.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxName_Validating);
			// 
			// buttonViewData
			// 
this.buttonViewData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
this.buttonViewData.Location = new System.Drawing.Point(304, 360);
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
this.buttonClearInput.Location = new System.Drawing.Point(160, 360);
this.buttonClearInput.Name = "buttonClearInput";
this.buttonClearInput.Size = new System.Drawing.Size(136, 24);
this.buttonClearInput.TabIndex = 7;
this.buttonClearInput.Text = "Clear Template Input";
this.buttonClearInput.Click += new System.EventHandler(this.buttonClearInput_Click);
			// 
			// FormAddEditSavedObject
			// 
this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
this.ClientSize = new System.Drawing.Size(600, 390);
this.Controls.Add(this.buttonClearInput);
this.Controls.Add(this.buttonViewData);
this.Controls.Add(this.textBoxName);
this.Controls.Add(this.buttonCancel);
this.Controls.Add(this.buttonOK);
this.Controls.Add(this.listBoxTemplates);
this.Controls.Add(this.labelName);
this.Controls.Add(this.buttonCollectInput);
this.Name = "FormAddEditSavedObject";
this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
this.Text = "AddEditSavedObject";
this.Load += new System.EventHandler(this.FormAddEditSavedObject_Load);
this.Activated += new System.EventHandler(this.FormAddEditSavedObject_Activated);
this.ResumeLayout(false);

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

this._lastRecordedSelectedIndex = this.listBoxTemplates.SelectedIndex;

this._isActivated = false;
}
}

private TemplateListItem SelectedTemplate 
{
get 
{
if (this.listBoxTemplates.SelectedIndex >= 0)
return this.listBoxTemplates.SelectedItem as TemplateListItem;
else 
return null;
}
}

private void buttonOK_Click(object sender, System.EventArgs e)
{
CancelEventArgs args = new CancelEventArgs();
this.textBoxName_Validating(this, args);
this.listBoxTemplates_Validating(this, args);

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

TemplateListItem item = this.SelectedTemplate;
if (item != null)
{
this.SavedObject.TemplatePath = item.Path;
this.SavedObject.TemplateUniqueID = item.ID;
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

public void LoadTemplates(string selectedid) 
{
DefaultSettings settings = DefaultSettings.Instance;
ArrayList filenames = new ArrayList();
filenames.Add(settings.DefaultTemplateDirectory);
ArrayList templatePaths = FileTools.GetFilenamesRecursive(filenames, this._extensions);
ZeusTemplate template;
TemplateListItem item;

this.listBoxTemplates.Items.Clear();

foreach (string path in templatePaths) 
{
				
try 
{
template = new ZeusTemplate(FileTools.ResolvePath(path));
string ns = template.NamespacePathString.Trim();
string title = template.Title.Trim();
title = (title.Length > 0) ? title : "<unnamed>";

if (ns.Length > 0)
{
ns = template.NamespacePathString + "." + title;
}
else
{
ns = title;
}

item = new TemplateListItem(template.UniqueID, path, ns);

this.listBoxTemplates.Items.Add(item);
if (item.ID == selectedid) 
{
this.listBoxTemplates.SelectedItem = item;
}
}
catch 
{
continue;
}
}

if ((this.listBoxTemplates.SelectedItem == null) && 
(this.listBoxTemplates.Items.Count > 0)) 
{
this.listBoxTemplates.SelectedIndex = 0;
}
}

private void SaveInput() 
{
try 
{
				/*if (_lastRecordedSelectedIndex != this.listBoxTemplates.SelectedIndex) 
				{
					this.SavedObject.InputItems.Clear();
				}*/

ZeusTemplate template = new ZeusTemplate(this.SelectedTemplate.Path);
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
}

if (template.Collect(context, settings.ScriptTimeout, SavedObject.InputItems)) 
{
_lastRecordedSelectedIndex = this.listBoxTemplates.SelectedIndex;
}
					
if (log.HasExceptions) 
{
throw log.Exceptions[0];
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
ZeusDisplayError formError = new ZeusDisplayError(ex);
formError.SetControlsFromException();			
formError.ShowDialog(this);
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
if (textBoxName.Text.Trim().Length == 0) 
{
e.Cancel = true;
this.errorProviderRequiredFields.SetError(this.textBoxName, "Name is Required!");
}
else if (this.Module.SavedObjects.Contains(this.textBoxName.Text) &&
(this.SavedObject.SavedObjectName.Trim() != this.textBoxName.Text.Trim()))
{
e.Cancel = true;
this.errorProviderRequiredFields.SetError(this.textBoxName, "This name has already been difined in this module");
}
else 
{
this.errorProviderRequiredFields.SetError(this.textBoxName, string.Empty);
}
}

private void listBoxTemplates_Validating(object sender, System.ComponentModel.CancelEventArgs e)
{
if (listBoxTemplates.SelectedIndex < 0) 
{
e.Cancel = true;
this.errorProviderRequiredFields.SetError(this.listBoxTemplates, "Template selection is Required!");
}
else 
{
this.errorProviderRequiredFields.SetError(this.listBoxTemplates, string.Empty);
}
}

private void FormAddEditSavedObject_Load(object sender, System.EventArgs e)
{
this.errorProviderRequiredFields.SetError(this.textBoxName, string.Empty);
this.errorProviderRequiredFields.SetIconAlignment(this.textBoxName, ErrorIconAlignment.TopRight);
this.errorProviderRequiredFields.SetError(this.listBoxTemplates, string.Empty);
this.errorProviderRequiredFields.SetIconAlignment(this.listBoxTemplates, ErrorIconAlignment.TopRight);

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

#region Inner Class TemplateListItem
class TemplateListItem 
{
public string ID;
public string Path;
public string Text;

public TemplateListItem(string id, string path, string text) 
{
ID = id;
Text = text;
Path = path;
}

public override string ToString()
{
return Text;
}
}
#endregion
}

	
}



