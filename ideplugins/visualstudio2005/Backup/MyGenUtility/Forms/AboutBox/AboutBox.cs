using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.Text;

using Zeus.Templates;
using MyMeta;
using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for NewAbout.
	/// </summary>
	public class NewAbout : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox lstBoxProducts;
        private System.Windows.Forms.TextBox txtProductInfo;
		private System.Windows.Forms.LinkLabel lnkURL;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private AboutBoxLogo fun1;
        private System.Collections.Generic.Dictionary<int, IMyMetaPlugin> plugins = new System.Collections.Generic.Dictionary<int, IMyMetaPlugin>();
        private System.Collections.Generic.Dictionary<int, IEditorManager> emPlugins = new System.Collections.Generic.Dictionary<int, IEditorManager>();
        private System.Collections.Generic.Dictionary<int, IContentManager> cmPlugins = new System.Collections.Generic.Dictionary<int, IContentManager>();
        private System.Collections.Generic.Dictionary<int, ISimplePluginManager> smPlugins = new System.Collections.Generic.Dictionary<int, ISimplePluginManager>();
        /// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NewAbout()
		{
			InitializeComponent();
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
            this.lstBoxProducts = new System.Windows.Forms.ListBox();
            this.txtProductInfo = new System.Windows.Forms.TextBox();
            this.lnkURL = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.fun1 = new MyGeneration.AboutBoxLogo();
            this.SuspendLayout();
            // 
            // lstBoxProducts
            // 
            this.lstBoxProducts.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstBoxProducts.ItemHeight = 16;
            this.lstBoxProducts.Location = new System.Drawing.Point(1, 67);
            this.lstBoxProducts.Name = "lstBoxProducts";
            this.lstBoxProducts.Size = new System.Drawing.Size(449, 148);
            this.lstBoxProducts.TabIndex = 0;
            this.lstBoxProducts.SelectedIndexChanged += new System.EventHandler(this.lstBoxProducts_SelectedIndexChanged);
            this.lstBoxProducts.SelectedValueChanged += new System.EventHandler(this.lstBoxProducts_SelectedValueChanged);
            // 
            // txtProductInfo
            // 
            this.txtProductInfo.AcceptsReturn = true;
            this.txtProductInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProductInfo.Location = new System.Drawing.Point(1, 247);
            this.txtProductInfo.Multiline = true;
            this.txtProductInfo.Name = "txtProductInfo";
            this.txtProductInfo.ReadOnly = true;
            this.txtProductInfo.Size = new System.Drawing.Size(449, 136);
            this.txtProductInfo.TabIndex = 1;
            // 
            // lnkURL
            // 
            this.lnkURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkURL.Location = new System.Drawing.Point(1, 218);
            this.lnkURL.Name = "lnkURL";
            this.lnkURL.Size = new System.Drawing.Size(449, 26);
            this.lnkURL.TabIndex = 3;
            this.lnkURL.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lnkURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkURL_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(-2, 360);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(371, 23);
            this.linkLabel1.TabIndex = 3;
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // fun1
            // 
            this.fun1.BackColor = System.Drawing.Color.White;
            this.fun1.Location = new System.Drawing.Point(1, 1);
            this.fun1.Name = "fun1";
            this.fun1.Size = new System.Drawing.Size(449, 68);
            this.fun1.TabIndex = 5;
            this.fun1.Text = "fun1";
            // 
            // NewAbout
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(451, 382);
            this.Controls.Add(this.lnkURL);
            this.Controls.Add(this.txtProductInfo);
            this.Controls.Add(this.lstBoxProducts);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.fun1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewAbout";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About MyGeneration";
            this.Load += new System.EventHandler(this.NewAbout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void NewAbout_Load(object sender, System.EventArgs e)
		{
            this.fun1.Start();
            Assembly asmblyMyGen = System.Reflection.Assembly.GetEntryAssembly();
			Assembly asmblyZeus = System.Reflection.Assembly.GetAssembly(typeof(Zeus.ZeusTemplate));
			Assembly asmblyPlugins = System.Reflection.Assembly.GetAssembly(typeof(Zeus.IZeusCodeSegment));
			Assembly asmblyMyMeta = System.Reflection.Assembly.GetAssembly(typeof(MyMeta.Database));
			Assembly asmblyScintilla = System.Reflection.Assembly.GetAssembly(typeof(Scintilla.ScintillaControl));
			Assembly asmblyWinFormsUI = System.Reflection.Assembly.GetAssembly(typeof(DockContent));

            lstBoxProducts.Items.Add("MyGeneration".PadRight(29) + asmblyMyGen.GetName().Version.ToString());
			lstBoxProducts.Items.Add("MyMeta".PadRight(29) + asmblyMyMeta.GetName().Version.ToString());
			lstBoxProducts.Items.Add("Zeus Parser".PadRight(29) + asmblyZeus.GetName().Version.ToString());
			lstBoxProducts.Items.Add("Plug-in Interface".PadRight(29) + asmblyPlugins.GetName().Version.ToString());
			lstBoxProducts.Items.Add("ScintillaNet".PadRight(29) + asmblyScintilla.GetName().Version.ToString());
			lstBoxProducts.Items.Add("Scintilla".PadRight(29) + "1.60");
			lstBoxProducts.Items.Add("DockPanel Suite".PadRight(29) + asmblyWinFormsUI.GetName().Version.ToString());
			lstBoxProducts.Items.Add("Npgsql".PadRight(29) + GetAssemblyVersion("Npgsql", "1.0.0.0"));
			lstBoxProducts.Items.Add("Firebird .Net Data Provider".PadRight(29) + GetAssemblyVersion("FirebirdSql.Data.Firebird", "1.7.1.0"));
			lstBoxProducts.Items.Add("System.Data.SQLite".PadRight(29) + GetAssemblyVersion("System.Data.SQLite", "1.0.38.0"));
#if !IGNORE_VISTA
			lstBoxProducts.Items.Add("VistaDB 2.0 ADO.NET Provider".PadRight(29) + "2.0.16");
#else
            lstBoxProducts.Items.Add(""); // number of items must match
#endif
			lstBoxProducts.Items.Add("Dnp.Utils".PadRight(29) + GetAssemblyVersion("Dnp.Utils", "1.0.0.0"));

            foreach (string pluginName in MyMeta.dbRoot.Plugins.Keys)
            {
                IMyMetaPlugin plugin = dbRoot.Plugins[pluginName] as IMyMetaPlugin;
                int index = lstBoxProducts.Items.Add(plugin.ProviderName.PadRight(29) + plugin.GetType().Assembly.GetName().Version.ToString());
                plugins[index] = plugin;
            }

            foreach (string pluginName in PluginManager.ContentManagers.Keys)
            {
                IContentManager plugin = PluginManager.ContentManagers[pluginName] as IContentManager;
                int index = lstBoxProducts.Items.Add(plugin.Name.PadRight(29) + plugin.GetType().Assembly.GetName().Version.ToString());
                cmPlugins[index] = plugin;
            }

            foreach (string pluginName in PluginManager.SimplePluginManagers.Keys)
            {
                ISimplePluginManager plugin = PluginManager.SimplePluginManagers[pluginName] as ISimplePluginManager;
                int index = lstBoxProducts.Items.Add(plugin.Name.PadRight(29) + plugin.GetType().Assembly.GetName().Version.ToString());
                smPlugins[index] = plugin;
            }

            foreach (string pluginName in PluginManager.EditorManagers.Keys)
            {
                IEditorManager plugin = PluginManager.EditorManagers[pluginName] as IEditorManager;
                int index = lstBoxProducts.Items.Add(plugin.Name.PadRight(29) + plugin.GetType().Assembly.GetName().Version.ToString());
                emPlugins[index] = plugin;
            }

			lstBoxProducts.SelectedIndex = 0;
		}

		private string GetAssemblyVersion(string dllName, string revertToVersion) 
		{
			string returnValue;
			try
            {
                string an = dllName;
                if (an.ToLower().EndsWith(".dll"))
                    an = an.Substring(0, an.Length - 4);
                AssemblyName name = new AssemblyName(an);
                Assembly assembly = Assembly.Load(name);

				returnValue = assembly.GetName().Version.ToString();
			}
			catch 
			{
				returnValue = revertToVersion;
			}
			return returnValue;
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void lstBoxProducts_SelectedIndexChanged(object sender, System.EventArgs e)
		{

		}

		private void lstBoxProducts_SelectedValueChanged(object sender, System.EventArgs e)
		{
			int product = (int)lstBoxProducts.SelectedIndex;

			switch(product)
			{
				case 0:
					txtProductInfo.Text =
                        @"MyGeneration is written in C# and is a tool that combines database meta-data with the power of scripting to generate stored procedures, data objects, business objects, user interfaces, and more.

Copyright © 2004-2008 by Mike Griffin and Justin Greenwood
All Rights Reserved";
					lnkURL.Text = @"http://www.mygenerationsoftware.com";
					break;

				case 1:
					txtProductInfo.Text =
                        @"MyMeta is C# COM object that serves up database meta-data with plug-in support.

Copyright © 2004-2008 by Mike Griffin and Justin Greenwood
All Rights Reserved";
					lnkURL.Text = @"http://www.mygenerationsoftware.com";
					break;

				case 2:
					txtProductInfo.Text =
                        @"The Zeus Parser contains the template parser and interpreter for MyGeneration.

Copyright © 2004-2008 by Mike Griffin and Justin Greenwood
All Rights Reserved";
					lnkURL.Text = @"http://www.mygenerationsoftware.com";
					break;

				case 3:
					txtProductInfo.Text =
                        @"The plug-in Interface contains interfaces allowing third parties to develop MyGeneration plug-ins.

Copyright © 2004-2008 by Mike Griffin and Justin Greenwood
All Rights Reserved";
					lnkURL.Text = @"http://www.mygenerationsoftware.com";
					break;

				case 4:

					txtProductInfo.Text = 
						@"ScintillaNET is an encapsulation of Scintilla for use within the .NET framework

The ScintillaNET bindings are Copyright © 2002-2007 by Garrett Serack";
					lnkURL.Text = @"http://sourceforge.net/projects/scide/";
					break;

				case 5:
					txtProductInfo.Text = 
						@"Scintilla is a free source code editing component. It comes with complete source code and a license that permits use in any free project or commercial product.

Copyright 1998-2007 by Neil Hodgson <neilh@scintilla.org>
All Rights Reserved";
					lnkURL.Text = @"http://www.scintilla.org";
					break;

				case 6:
					txtProductInfo.Text = 
						@"DockPanel suite is designed to achieve docking capability for MDI forms. It can be used to develop applications with Visual Studio .Net style user interface. DockPanel suite is a 100%-native .Net windows forms control, written in C#.


Copyright © 2007 Weifen Luo, All Rights Reserved.";

					lnkURL.Text = @"http://sourceforge.net/projects/dockpanelsuite/";
					break;

				case 7:
					txtProductInfo.Text =
						@"Npgsql is a .Net data provider for Postgresql. It allows any program developed for the .Net framework to access the Postgresql versions 7.x and 8.x. It is implemented in 100% C# code.

Copyright © 2002-2007  The Npgsql Development Team";


					lnkURL.Text = @"http://pgfoundry.org/projects/npgsql";
					break;

				case 8:
					txtProductInfo.Text =
						@"The .NET Data provider/Driver is written in C# and provides a high-performance, native implementation of the Firebird API.";

					lnkURL.Text = @"http://www.firebirdsql.org/index.php?op=files&id=netprovider";
					break;

				case 9:
					txtProductInfo.Text =
						@"System.Data.SQLite is an enhanced version of the original SQLite database engine. It is a complete drop-in replacement for the original sqlite3.dll (you can even rename it to sqlite3.dll).  It has no linker dependency on the .NET runtime so it can be distributed independently of .NET, yet embedded in the binary is a complete ADO.NET 2.0 provider for full managed development.

The C# provider, the very minor C code modifications to SQLite, documentation and etc were written by Robert Simpson.";
					lnkURL.Text = @"http://sqlite.phxsoftware.com/";
					break;

				case 10:
					txtProductInfo.Text = 
						@"VistaDB is a true RDBMS specifically designed for .NET to give developers a robust, high-speed embedded database solution with minimal overhead.

©1999-2007 Vista Software. All rights reserved.";

					lnkURL.Text = @"http://www.vistadb.net/";
					break;

				case 11:
					txtProductInfo.Text = 
						@"The DnpUtils plug-in for MyGeneration by David Parsons (dnparsons). This plug-in contains all kinds of useful functionality. See the Help menu for more details.";
					lnkURL.Text = @"http://www.mygenerationsoftware.com/TemplateLibrary/Archive/?guid=4a285a9a-4dd2-4655-b7ca-996b5516a5a1";
					break;

                default:
                    if (plugins.ContainsKey(product))
                    {
                        txtProductInfo.Text = this.plugins[product].ProviderAuthorInfo;
                        if (this.plugins[product].ProviderAuthorUri != null)
                        {
                            lnkURL.Text = plugins[product].ProviderAuthorUri.ToString();
                        }
                        else
                        {
                            lnkURL.Text = "http://www.mygenerationsoftware.com/";
                        }
                    }
                    else if (cmPlugins.ContainsKey(product))
                    {
                        txtProductInfo.Text = this.cmPlugins[product].Description;
                        if (this.cmPlugins[product].AuthorUri != null)
                        {
                            lnkURL.Text = cmPlugins[product].AuthorUri.ToString();
                        }
                        else
                        {
                            lnkURL.Text = "http://www.mygenerationsoftware.com/";
                        }
                    }
                    else if (emPlugins.ContainsKey(product))
                    {
                        txtProductInfo.Text = this.emPlugins[product].Description;
                        if (this.emPlugins[product].AuthorUri != null)
                        {
                            lnkURL.Text = emPlugins[product].AuthorUri.ToString();
                        }
                        else
                        {
                            lnkURL.Text = "http://www.mygenerationsoftware.com/";
                        }
                    }
                    else if (smPlugins.ContainsKey(product))
                    {
                        txtProductInfo.Text = this.smPlugins[product].Description;
                        if (this.smPlugins[product].AuthorUri != null)
                        {
                            lnkURL.Text = smPlugins[product].AuthorUri.ToString();
                        }
                        else
                        {
                            lnkURL.Text = "http://www.mygenerationsoftware.com/";
                        }
                    }
                    break;
			}

		}

		private void lnkURL_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			LaunchBrowser((lnkURL.Text == "http://www.mygenerationsoftware.com" ? "http://www.mygenerationsoftware.com/home/" : lnkURL.Text));
		}

		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			LaunchBrowser("http://www.mygenerationsoftware.com/");
		}

		private void pictureBox1_Click(object sender, System.EventArgs e)
		{
		
		}

		private void LaunchBrowser(string url)
		{
			try 
			{
                Zeus.WindowsTools.LaunchBrowser(url);
			}
			catch 
			{
				Help.ShowHelp(this, url);
			}
		}
	}
}
