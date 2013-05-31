using System;
using Scintilla;
using GreenwoodLib.Zeus.ScriptModel;
using GreenwoodLib.Zeus.Templates;
using GreenwoodLib.Zeus.ErrorHandling;
using GreenwoodLib.Zeus.UserInterface;
namespace MyGeneration
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem Script;
		private System.Windows.Forms.MenuItem ScriptGenerate;
		public VBScriptTemplate()
		/// <summary>
		protected override ScintillaControl ScintillaControl
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
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
	}