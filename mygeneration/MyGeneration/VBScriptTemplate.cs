using System;using System.Drawing;using System.Collections;using System.ComponentModel;using System.Windows.Forms;using System.IO;
using Scintilla;using GreenwoodLib.Zeus;
using GreenwoodLib.Zeus.ScriptModel;
using GreenwoodLib.Zeus.Templates;
using GreenwoodLib.Zeus.ErrorHandling;
using GreenwoodLib.Zeus.UserInterface;using GreenwoodLib.Zeus.WinForms;
namespace MyGeneration{	/// <summary>	/// Summary description for VBScriptTemplate.	/// </summary>	public class VBScriptTemplate : ScriptWindow	{		/// <summary>		/// Required designer variable.		/// </summary>		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem Script;
		private System.Windows.Forms.MenuItem ScriptGenerate;		private ScintillaControl scintillaControl = null;
		public VBScriptTemplate()		{			InitializeComponent();			scintillaControl = new ZeusScintillaControl(ScriptLanguage.VBScript);			scintillaControl.AddShortcutsFromForm(this);						this.Controls.Add(scintillaControl);		}
		/// <summary>		/// Clean up any resources being used.		/// </summary>		protected override void Dispose( bool disposing )		{			if( disposing )			{				if(components != null)				{					components.Dispose();				}			}			base.Dispose( disposing );		}
		protected override ScintillaControl ScintillaControl		{			get			{				return scintillaControl;			}		}		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.Script = new System.Windows.Forms.MenuItem();
			this.ScriptGenerate = new System.Windows.Forms.MenuItem();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.Script});
			// 
			// Script
			// 
			this.Script.Index = 0;
			this.Script.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				   this.ScriptGenerate});
			this.Script.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
			this.Script.Text = "Script";
			// 
			// ScriptGenerate
			// 
			this.ScriptGenerate.Index = 0;
			this.ScriptGenerate.MergeOrder = 1;
			this.ScriptGenerate.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
			this.ScriptGenerate.Text = "ScriptGenerate";
			this.ScriptGenerate.Click += new System.EventHandler(this.ScriptGenerate_Click);
			// 
			// VBScriptTemplate
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(728, 494);
			this.Menu = this.mainMenu1;
			this.Name = "VBScriptTemplate";
			this.Text = "Unititled.vbgen";

		}
		#endregion
	}}