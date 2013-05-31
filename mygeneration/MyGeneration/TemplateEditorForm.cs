using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Scintilla;
using GreenwoodLib.Zeus.Templates;


namespace MyGeneration
{
	/// <summary>
	/// Summary description for TemplateEditorForm.
	/// </summary>
	public class TemplateEditorForm : Form
	{
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabProperties;
		private System.Windows.Forms.TabPage tabCode;
		private System.Windows.Forms.TabPage tabInterface;
		private ScintillaControl scintillaTemplateCode = null;		private ScintillaControl scintillaGUICode = null;
		private System.Windows.Forms.Label labelTemplateTitle;		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox textBoxTitle;
		private System.Windows.Forms.Label labelTitle;
		private ZeusTemplate template;

		public TemplateEditorForm(ZeusTemplate template)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.template = template;

			scintillaTemplateCode = new ZeusScintillaControl(ScriptLanguage.JScript);			scintillaTemplateCode.AddShortcutsFromForm(this);
			scintillaGUICode = new ZeusScintillaControl(ScriptLanguage.JScript);			scintillaGUICode.AddShortcutsFromForm(this);
			this.tabCode.Controls.Add(this.scintillaTemplateCode);
			this.tabInterface.Controls.Add(this.scintillaGUICode);
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabProperties = new System.Windows.Forms.TabPage();
			this.labelTitle = new System.Windows.Forms.Label();
			this.textBoxTitle = new System.Windows.Forms.TextBox();
			this.tabCode = new System.Windows.Forms.TabPage();
			this.tabInterface = new System.Windows.Forms.TabPage();
			this.labelTemplateTitle = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.tabProperties.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabProperties);
			this.tabControl1.Controls.Add(this.tabCode);
			this.tabControl1.Controls.Add(this.tabInterface);
			this.tabControl1.Location = new System.Drawing.Point(0, 32);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(536, 312);
			this.tabControl1.TabIndex = 0;
			// 
			// tabProperties
			// 
			this.tabProperties.Controls.Add(this.labelTitle);
			this.tabProperties.Controls.Add(this.textBoxTitle);
			this.tabProperties.Location = new System.Drawing.Point(4, 22);
			this.tabProperties.Name = "tabProperties";
			this.tabProperties.Size = new System.Drawing.Size(528, 286);
			this.tabProperties.TabIndex = 0;
			this.tabProperties.Text = "Properties";
			// 
			// labelTitle
			// 
			this.labelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.labelTitle.Location = new System.Drawing.Point(16, 8);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(496, 24);
			this.labelTitle.TabIndex = 1;
			this.labelTitle.Text = "Template Name";
			this.labelTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textBoxTitle
			// 
			this.textBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxTitle.Location = new System.Drawing.Point(16, 32);
			this.textBoxTitle.Name = "textBoxTitle";
			this.textBoxTitle.Size = new System.Drawing.Size(496, 20);
			this.textBoxTitle.TabIndex = 0;
			this.textBoxTitle.Text = "";
			// 
			// tabCode
			// 
			this.tabCode.DockPadding.Top = 6;
			this.tabCode.Location = new System.Drawing.Point(4, 22);
			this.tabCode.Name = "tabCode";
			this.tabCode.Size = new System.Drawing.Size(528, 286);
			this.tabCode.TabIndex = 1;
			this.tabCode.Text = "Template Code";
			// 
			// tabInterface
			// 
			this.tabInterface.DockPadding.Top = 6;
			this.tabInterface.Location = new System.Drawing.Point(4, 22);
			this.tabInterface.Name = "tabInterface";
			this.tabInterface.Size = new System.Drawing.Size(528, 286);
			this.tabInterface.TabIndex = 2;
			this.tabInterface.Text = "Interface Code";
			// 
			// labelTemplateTitle
			// 
			this.labelTemplateTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.labelTemplateTitle.Location = new System.Drawing.Point(0, 0);
			this.labelTemplateTitle.Name = "labelTemplateTitle";
			this.labelTemplateTitle.Size = new System.Drawing.Size(536, 23);
			this.labelTemplateTitle.TabIndex = 0;
			this.labelTemplateTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// TemplateEditorForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(536, 342);
			this.Controls.Add(this.labelTemplateTitle);
			this.Controls.Add(this.tabControl1);
			this.Name = "TemplateEditorForm";
			this.Text = "TemplateEditorForm";
			this.Load += new System.EventHandler(this.TemplateEditorForm_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabProperties.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void TemplateEditorForm_Load(object sender, System.EventArgs e)
		{
			RefreshControl();
		}

		private void RefreshControl() 
		{
			this.labelTemplateTitle.Text = this.template.Title;
			this.textBoxTitle.Text = this.template.Title;
			this.scintillaTemplateCode.Text = this.template.ScriptBodyUnparsed;
			this.scintillaGUICode.Text = this.template.ScriptInterface;
		}
		
		private void RefreshTemplate() 
		{
			this.template.Title = this.textBoxTitle.Text;
			this.template.ScriptBodyUnparsed = this.scintillaTemplateCode.Text;
			this.template.ScriptInterface = this.scintillaGUICode.Text;
		}
	}
}
