using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Diagnostics;

using MyGeneration.CodeSmithConversion.Parser;
using MyGeneration.CodeSmithConversion.Template;
using MyGeneration.CodeSmithConversion.Conversion;

namespace MyGeneration.CodeSmithConversion
{
	/// <summary>
	/// Summary description for FormConvertCodeSmith.
	/// </summary>
	public class FormConvertCodeSmith : System.Windows.Forms.Form, ILog
	{
		private System.Windows.Forms.Button buttonConvert;
		private System.Windows.Forms.TextBox textBoxConsole;
		private System.Windows.Forms.TextBox textBoxCodeSmithPath;
		private System.Windows.Forms.TextBox textBoxCSTFile;
		private System.Windows.Forms.Label labelCodeSmithPath;
		private System.Windows.Forms.Label labelCSTFile;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.Button buttonCodeSmithPath;
		private System.Windows.Forms.Button buttonCSTPath;
		private System.Windows.Forms.CheckedListBox checkedListBoxTemplates;
		private System.Windows.Forms.Label labelTemplates;
		private System.Windows.Forms.Button buttonOutPath;
		private System.Windows.Forms.Label labelOutFolder;
		private System.Windows.Forms.TextBox textBoxOutFolder;
		private System.Windows.Forms.GroupBox groupBoxCodeSmith;
		private System.Windows.Forms.GroupBox groupBoxMyGen;
		private System.Windows.Forms.MainMenu mainMenuConverter;
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.MenuItem menuItemHelp;
		private System.Windows.Forms.MenuItem menuItemAbout;
		private System.Windows.Forms.Button buttonExit;
		private System.Windows.Forms.Label labelConversionLog;
		private System.Windows.Forms.MenuItem menuItemConvert;
		private System.Windows.Forms.MenuItem menuItemFileSep01;
		private System.Windows.Forms.CheckBox checkBoxLaunch;
		private System.Windows.Forms.TextBox textBoxMyGenAppPath;
		private System.Windows.Forms.Label labelMyGenAppPath;
		private System.Windows.Forms.Button buttonMyGenAppPath;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Button buttonSaveLog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		[STAThread]
		static void Main() 
		{
			Application.Run(new FormConvertCodeSmith());
		}

		public FormConvertCodeSmith()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
		}

		private void FormConvertCodeSmith_Load(object sender, System.EventArgs e)
		{
			// Load settings from config file
			Config conf = Config.Current;
			this.textBoxCSTFile.Text = conf.CodeSmithTemplatePath;
			this.textBoxCodeSmithPath.Text = conf.CodeSmithAppPath;
			this.textBoxMyGenAppPath.Text = conf.MyGenExePath;
			this.textBoxOutFolder.Text = conf.MyGenTemplatePath;
			this.checkBoxLaunch.Checked = conf.Launch;

			this.textBoxCSTFile_Leave(sender, e);
		}

		private void FormConvertCodeSmith_Closed(object sender, System.EventArgs e)
		{
			// Save settings to config file
			Config conf = Config.Current;
			conf.CodeSmithTemplatePath = this.textBoxCSTFile.Text;
			conf.CodeSmithAppPath = this.textBoxCodeSmithPath.Text;
			conf.MyGenExePath = this.textBoxMyGenAppPath.Text;
			conf.MyGenTemplatePath = this.textBoxOutFolder.Text;
			conf.Launch = this.checkBoxLaunch.Checked;
			conf.Save();
		}

		private bool IsFormValid 
		{
			get 
			{
				bool isValid = true;
				string message = "Form Failed Validation:";
				FileInfo finfo;
				DirectoryInfo dinfo;
			
				finfo = new FileInfo(this.textBoxMyGenAppPath.Text);
				if (!finfo.Exists) 
				{
					isValid = false;
					message += "\r\n -> The MyGeneration Application Path is Invalid.";
				}

				dinfo = new DirectoryInfo(this.textBoxCSTFile.Text);
				if (!dinfo.Exists) 
				{
					isValid = false;
					message += "\r\n -> The CodeSmith Template Folder is Invalid.";
				}

				dinfo = new DirectoryInfo(this.textBoxCodeSmithPath.Text);
				if (!dinfo.Exists) 
				{
					isValid = false;
					message += "\r\n -> The CodeSmith Application Path is Invalid.";
				}

				dinfo = new DirectoryInfo(this.textBoxOutFolder.Text);
				if (!dinfo.Exists) 
				{
					try 
					{
						dinfo.Create();
					}
					catch 
					{
						isValid = false;
						message += "\r\n -> The MyGeneration Template Ouput Path is Invalid.";
					}
				}

				if (!isValid) 
				{
					MessageBox.Show(this, message);
				}
			
				return isValid;
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		private void buttonConvert_Click(object sender, System.EventArgs e)
		{
			ConvertTemplates();
		}
		
		private void WriteToTextBox(string text)
		{
			this.textBoxConsole.Text = this.textBoxConsole.Text + text + Environment.NewLine;
			this.textBoxConsole.SelectionStart = this.textBoxConsole.Text.Length;
			this.textBoxConsole.ScrollToCaret();
			this.textBoxConsole.Invalidate();
			this.textBoxConsole.Refresh();
		}

		private void ConvertTemplates() 
		{
			if (IsFormValid) 
			{
				Cursor.Current = Cursors.WaitCursor;

				string filename, tmp;
				ArrayList templates = new ArrayList();
				foreach (string file in this.checkedListBoxTemplates.CheckedItems) 
				{
					StreamWriter writer = null;
					try 
					{
						CstParser parser = new CstParser(this);
						CstTemplate template = parser.Parse(this.textBoxCSTFile.Text + "\\" + file);
						LanguageHelper h = LanguageHelper.CreateInstance(template.Language);
					
						tmp = h.BuildTemplate(template, this);
						filename = this.textBoxOutFolder.Text + "\\" + file.Substring(0, file.LastIndexOf(".")) + ".zeus";

						writer = File.CreateText(filename);
						writer.Write(tmp);
						writer.Flush();
						writer.Close();
						writer = null;

						templates.Add(filename);
					}
					catch (Exception ex)
					{
						if (writer != null) 
						{
							writer.Close();
							writer = null;
						}
						this.AddEntry(ex);
					}
				}

				if (templates.Count > 0)
				{
					string fileList = string.Empty;
					foreach (string fname in templates) 
					{
						if (fileList.Length > 0)
							fileList += " ";

						fileList += "\"" + fname + "\"";
					}

					if (this.checkBoxLaunch.Checked) 
					{
						try 
						{
							FileInfo finfo = new FileInfo(textBoxMyGenAppPath.Text);
							ProcessStartInfo info = new ProcessStartInfo(finfo.FullName, fileList);
							info.WorkingDirectory = finfo.DirectoryName;
							System.Diagnostics.Process.Start(info);
						}
						catch {}
					}
				}

				Cursor.Current = Cursors.Default;
			}
		}

		private void buttonExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
			Application.Exit();
		}

		private void buttonMyGenAppPath_Click(object sender, System.EventArgs e)
		{
			PickFile(this.textBoxMyGenAppPath);
		}

		private void buttonOutPath_Click(object sender, System.EventArgs e)
		{
			PickPath(this.textBoxOutFolder);
		}

		private void buttonCodeSmithPath_Click(object sender, System.EventArgs e)
		{
			PickPath(this.textBoxCodeSmithPath);
		}

		private void buttonCSTPath_Click(object sender, System.EventArgs e)
		{
			PickPath(this.textBoxCSTFile);
			this.textBoxCSTFile_Leave(sender, e);
		}

		private void buttonSaveLog_Click(object sender, System.EventArgs e)
		{
			this.saveFileDialog.RestoreDirectory = true;

			this.saveFileDialog.FileName = "log.txt";

			DialogResult r = this.saveFileDialog.ShowDialog(this);
			if (r == DialogResult.OK) 
			{
				StreamWriter writer = null;
				try 
				{
					writer = File.CreateText(saveFileDialog.FileName);
					writer.Write(textBoxConsole.Text);
					writer.Flush();
					writer.Close();
					writer = null;
				}
				catch (Exception ex)
				{
					if (writer != null) 
					{
						writer.Close();
						writer = null;
					}
					throw ex;
				}
			}
		}

		private void textBoxCSTFile_Leave(object sender, System.EventArgs e)
		{
			DirectoryInfo d = new DirectoryInfo(textBoxCSTFile.Text);
			if (d.Exists) 
			{
				this.checkedListBoxTemplates.Items.Clear();

				foreach (FileInfo file in d.GetFiles()) 
				{
					if (file.Extension == ".cst")
						this.checkedListBoxTemplates.Items.Add(file.Name);
				}
			}
		}

		private void PickPath(TextBox txt) 
		{
			if (txt.Text != string.Empty) 
			{
				this.folderBrowserDialog.SelectedPath = txt.Text;
			}

			DialogResult r = this.folderBrowserDialog.ShowDialog(this);
			if (r == DialogResult.OK) 
			{
				txt.Text = this.folderBrowserDialog.SelectedPath;
			}
		}

		private void PickFile(TextBox txt) 
		{
			this.openFileDialog.RestoreDirectory = true;

			if (txt.Text != string.Empty) 
			{
				this.openFileDialog.FileName = txt.Text;
			}

			DialogResult r = this.openFileDialog.ShowDialog(this);
			if (r == DialogResult.OK) 
			{
				txt.Text = this.openFileDialog.FileName;
			}
		}

		private void LaunchProcess(string url)
		{
			try 
			{
				System.Diagnostics.Process.Start( url );
			}
			catch {}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormConvertCodeSmith));
			this.buttonConvert = new System.Windows.Forms.Button();
			this.textBoxConsole = new System.Windows.Forms.TextBox();
			this.textBoxCodeSmithPath = new System.Windows.Forms.TextBox();
			this.textBoxCSTFile = new System.Windows.Forms.TextBox();
			this.labelCodeSmithPath = new System.Windows.Forms.Label();
			this.labelCSTFile = new System.Windows.Forms.Label();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.buttonCodeSmithPath = new System.Windows.Forms.Button();
			this.buttonCSTPath = new System.Windows.Forms.Button();
			this.checkedListBoxTemplates = new System.Windows.Forms.CheckedListBox();
			this.labelTemplates = new System.Windows.Forms.Label();
			this.buttonOutPath = new System.Windows.Forms.Button();
			this.labelOutFolder = new System.Windows.Forms.Label();
			this.textBoxOutFolder = new System.Windows.Forms.TextBox();
			this.groupBoxCodeSmith = new System.Windows.Forms.GroupBox();
			this.groupBoxMyGen = new System.Windows.Forms.GroupBox();
			this.buttonMyGenAppPath = new System.Windows.Forms.Button();
			this.textBoxMyGenAppPath = new System.Windows.Forms.TextBox();
			this.labelMyGenAppPath = new System.Windows.Forms.Label();
			this.checkBoxLaunch = new System.Windows.Forms.CheckBox();
			this.mainMenuConverter = new System.Windows.Forms.MainMenu();
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemConvert = new System.Windows.Forms.MenuItem();
			this.menuItemFileSep01 = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.menuItemHelp = new System.Windows.Forms.MenuItem();
			this.menuItemAbout = new System.Windows.Forms.MenuItem();
			this.labelConversionLog = new System.Windows.Forms.Label();
			this.buttonExit = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.buttonSaveLog = new System.Windows.Forms.Button();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.groupBoxCodeSmith.SuspendLayout();
			this.groupBoxMyGen.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonConvert
			// 
			this.buttonConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonConvert.Location = new System.Drawing.Point(248, 536);
			this.buttonConvert.Name = "buttonConvert";
			this.buttonConvert.Size = new System.Drawing.Size(64, 23);
			this.buttonConvert.TabIndex = 19;
			this.buttonConvert.Text = "&Convert";
			this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
			// 
			// textBoxConsole
			// 
			this.textBoxConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxConsole.BackColor = System.Drawing.Color.Black;
			this.textBoxConsole.Enabled = false;
			this.textBoxConsole.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(192)), ((System.Byte)(0)));
			this.textBoxConsole.Location = new System.Drawing.Point(8, 384);
			this.textBoxConsole.MaxLength = 999999;
			this.textBoxConsole.Multiline = true;
			this.textBoxConsole.Name = "textBoxConsole";
			this.textBoxConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxConsole.Size = new System.Drawing.Size(376, 144);
			this.textBoxConsole.TabIndex = 18;
			this.textBoxConsole.Text = "";
			this.textBoxConsole.WordWrap = false;
			// 
			// textBoxCodeSmithPath
			// 
			this.textBoxCodeSmithPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxCodeSmithPath.Location = new System.Drawing.Point(16, 40);
			this.textBoxCodeSmithPath.Name = "textBoxCodeSmithPath";
			this.textBoxCodeSmithPath.Size = new System.Drawing.Size(320, 20);
			this.textBoxCodeSmithPath.TabIndex = 10;
			this.textBoxCodeSmithPath.Text = "C:\\Program Files\\CodeSmith";
			// 
			// textBoxCSTFile
			// 
			this.textBoxCSTFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxCSTFile.Location = new System.Drawing.Point(16, 80);
			this.textBoxCSTFile.Name = "textBoxCSTFile";
			this.textBoxCSTFile.Size = new System.Drawing.Size(320, 20);
			this.textBoxCSTFile.TabIndex = 13;
			this.textBoxCSTFile.Text = "C:\\Program Files\\CodeSmith\\Samples\\Collections";
			this.textBoxCSTFile.Leave += new System.EventHandler(this.textBoxCSTFile_Leave);
			// 
			// labelCodeSmithPath
			// 
			this.labelCodeSmithPath.ForeColor = System.Drawing.Color.Black;
			this.labelCodeSmithPath.Location = new System.Drawing.Point(16, 24);
			this.labelCodeSmithPath.Name = "labelCodeSmithPath";
			this.labelCodeSmithPath.Size = new System.Drawing.Size(184, 16);
			this.labelCodeSmithPath.TabIndex = 9;
			this.labelCodeSmithPath.Text = "Application Path:";
			this.labelCodeSmithPath.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelCSTFile
			// 
			this.labelCSTFile.ForeColor = System.Drawing.Color.Black;
			this.labelCSTFile.Location = new System.Drawing.Point(16, 64);
			this.labelCSTFile.Name = "labelCSTFile";
			this.labelCSTFile.Size = new System.Drawing.Size(208, 16);
			this.labelCSTFile.TabIndex = 12;
			this.labelCSTFile.Text = "CTS Templates File Path:";
			this.labelCSTFile.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// buttonCodeSmithPath
			// 
			this.buttonCodeSmithPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCodeSmithPath.ForeColor = System.Drawing.Color.Black;
			this.buttonCodeSmithPath.Location = new System.Drawing.Point(336, 40);
			this.buttonCodeSmithPath.Name = "buttonCodeSmithPath";
			this.buttonCodeSmithPath.Size = new System.Drawing.Size(24, 23);
			this.buttonCodeSmithPath.TabIndex = 11;
			this.buttonCodeSmithPath.Text = "...";
			this.buttonCodeSmithPath.Click += new System.EventHandler(this.buttonCodeSmithPath_Click);
			// 
			// buttonCSTPath
			// 
			this.buttonCSTPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCSTPath.ForeColor = System.Drawing.Color.Black;
			this.buttonCSTPath.Location = new System.Drawing.Point(336, 80);
			this.buttonCSTPath.Name = "buttonCSTPath";
			this.buttonCSTPath.Size = new System.Drawing.Size(24, 23);
			this.buttonCSTPath.TabIndex = 14;
			this.buttonCSTPath.Text = "...";
			this.buttonCSTPath.Click += new System.EventHandler(this.buttonCSTPath_Click);
			// 
			// checkedListBoxTemplates
			// 
			this.checkedListBoxTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.checkedListBoxTemplates.Location = new System.Drawing.Point(16, 120);
			this.checkedListBoxTemplates.Name = "checkedListBoxTemplates";
			this.checkedListBoxTemplates.Size = new System.Drawing.Size(344, 79);
			this.checkedListBoxTemplates.TabIndex = 16;
			// 
			// labelTemplates
			// 
			this.labelTemplates.ForeColor = System.Drawing.Color.Black;
			this.labelTemplates.Location = new System.Drawing.Point(16, 104);
			this.labelTemplates.Name = "labelTemplates";
			this.labelTemplates.Size = new System.Drawing.Size(128, 16);
			this.labelTemplates.TabIndex = 15;
			this.labelTemplates.Text = "CodeSmith Templates:";
			this.labelTemplates.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// buttonOutPath
			// 
			this.buttonOutPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOutPath.ForeColor = System.Drawing.Color.Black;
			this.buttonOutPath.Location = new System.Drawing.Point(336, 40);
			this.buttonOutPath.Name = "buttonOutPath";
			this.buttonOutPath.Size = new System.Drawing.Size(24, 23);
			this.buttonOutPath.TabIndex = 3;
			this.buttonOutPath.Text = "...";
			this.buttonOutPath.Click += new System.EventHandler(this.buttonOutPath_Click);
			// 
			// labelOutFolder
			// 
			this.labelOutFolder.ForeColor = System.Drawing.Color.Black;
			this.labelOutFolder.Location = new System.Drawing.Point(16, 24);
			this.labelOutFolder.Name = "labelOutFolder";
			this.labelOutFolder.Size = new System.Drawing.Size(288, 16);
			this.labelOutFolder.TabIndex = 1;
			this.labelOutFolder.Text = "Generated Output Folder Path:";
			this.labelOutFolder.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textBoxOutFolder
			// 
			this.textBoxOutFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxOutFolder.Location = new System.Drawing.Point(16, 40);
			this.textBoxOutFolder.Name = "textBoxOutFolder";
			this.textBoxOutFolder.Size = new System.Drawing.Size(320, 20);
			this.textBoxOutFolder.TabIndex = 2;
			this.textBoxOutFolder.Text = "C:\\Program Files\\MyGeneration\\Templates\\CodeSmith";
			// 
			// groupBoxCodeSmith
			// 
			this.groupBoxCodeSmith.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxCodeSmith.Controls.Add(this.textBoxCSTFile);
			this.groupBoxCodeSmith.Controls.Add(this.textBoxCodeSmithPath);
			this.groupBoxCodeSmith.Controls.Add(this.labelTemplates);
			this.groupBoxCodeSmith.Controls.Add(this.checkedListBoxTemplates);
			this.groupBoxCodeSmith.Controls.Add(this.buttonCSTPath);
			this.groupBoxCodeSmith.Controls.Add(this.buttonCodeSmithPath);
			this.groupBoxCodeSmith.Controls.Add(this.labelCSTFile);
			this.groupBoxCodeSmith.Controls.Add(this.labelCodeSmithPath);
			this.groupBoxCodeSmith.ForeColor = System.Drawing.Color.Blue;
			this.groupBoxCodeSmith.Location = new System.Drawing.Point(8, 152);
			this.groupBoxCodeSmith.Name = "groupBoxCodeSmith";
			this.groupBoxCodeSmith.Size = new System.Drawing.Size(376, 208);
			this.groupBoxCodeSmith.TabIndex = 8;
			this.groupBoxCodeSmith.TabStop = false;
			this.groupBoxCodeSmith.Text = "CodeSmith Settings";
			// 
			// groupBoxMyGen
			// 
			this.groupBoxMyGen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxMyGen.Controls.Add(this.buttonMyGenAppPath);
			this.groupBoxMyGen.Controls.Add(this.textBoxMyGenAppPath);
			this.groupBoxMyGen.Controls.Add(this.labelMyGenAppPath);
			this.groupBoxMyGen.Controls.Add(this.checkBoxLaunch);
			this.groupBoxMyGen.Controls.Add(this.textBoxOutFolder);
			this.groupBoxMyGen.Controls.Add(this.labelOutFolder);
			this.groupBoxMyGen.Controls.Add(this.buttonOutPath);
			this.groupBoxMyGen.ForeColor = System.Drawing.Color.Blue;
			this.groupBoxMyGen.Location = new System.Drawing.Point(8, 8);
			this.groupBoxMyGen.Name = "groupBoxMyGen";
			this.groupBoxMyGen.Size = new System.Drawing.Size(376, 136);
			this.groupBoxMyGen.TabIndex = 0;
			this.groupBoxMyGen.TabStop = false;
			this.groupBoxMyGen.Text = "MyGeneration Settings";
			// 
			// buttonMyGenAppPath
			// 
			this.buttonMyGenAppPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonMyGenAppPath.ForeColor = System.Drawing.Color.Black;
			this.buttonMyGenAppPath.Location = new System.Drawing.Point(336, 80);
			this.buttonMyGenAppPath.Name = "buttonMyGenAppPath";
			this.buttonMyGenAppPath.Size = new System.Drawing.Size(24, 23);
			this.buttonMyGenAppPath.TabIndex = 6;
			this.buttonMyGenAppPath.Text = "...";
			this.buttonMyGenAppPath.Click += new System.EventHandler(this.buttonMyGenAppPath_Click);
			// 
			// textBoxMyGenAppPath
			// 
			this.textBoxMyGenAppPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxMyGenAppPath.Location = new System.Drawing.Point(16, 80);
			this.textBoxMyGenAppPath.Name = "textBoxMyGenAppPath";
			this.textBoxMyGenAppPath.Size = new System.Drawing.Size(320, 20);
			this.textBoxMyGenAppPath.TabIndex = 5;
			this.textBoxMyGenAppPath.Text = "C:\\Program Files\\MyGeneration\\MyGeneration.exe";
			// 
			// labelMyGenAppPath
			// 
			this.labelMyGenAppPath.ForeColor = System.Drawing.Color.Black;
			this.labelMyGenAppPath.Location = new System.Drawing.Point(16, 64);
			this.labelMyGenAppPath.Name = "labelMyGenAppPath";
			this.labelMyGenAppPath.Size = new System.Drawing.Size(288, 16);
			this.labelMyGenAppPath.TabIndex = 4;
			this.labelMyGenAppPath.Text = "MyGeneration Application Path";
			this.labelMyGenAppPath.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// checkBoxLaunch
			// 
			this.checkBoxLaunch.Checked = true;
			this.checkBoxLaunch.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxLaunch.ForeColor = System.Drawing.Color.Black;
			this.checkBoxLaunch.Location = new System.Drawing.Point(16, 112);
			this.checkBoxLaunch.Name = "checkBoxLaunch";
			this.checkBoxLaunch.Size = new System.Drawing.Size(376, 16);
			this.checkBoxLaunch.TabIndex = 7;
			this.checkBoxLaunch.Text = "Launch Templates After Conversion?";
			// 
			// mainMenuConverter
			// 
			this.mainMenuConverter.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							  this.menuItemFile,
																							  this.menuItemHelp});
			// 
			// menuItemFile
			// 
			this.menuItemFile.Index = 0;
			this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemConvert,
																						 this.menuItemFileSep01,
																						 this.menuItemExit});
			this.menuItemFile.Text = "&File";
			// 
			// menuItemConvert
			// 
			this.menuItemConvert.Index = 0;
			this.menuItemConvert.Text = "&Convert";
			this.menuItemConvert.Click += new System.EventHandler(this.menuItemConvert_Click);
			// 
			// menuItemFileSep01
			// 
			this.menuItemFileSep01.Index = 1;
			this.menuItemFileSep01.Text = "-";
			// 
			// menuItemExit
			// 
			this.menuItemExit.Index = 2;
			this.menuItemExit.Text = "E&xit";
			this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
			// 
			// menuItemHelp
			// 
			this.menuItemHelp.Index = 1;
			this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemAbout});
			this.menuItemHelp.Text = "&Help";
			// 
			// menuItemAbout
			// 
			this.menuItemAbout.Index = 0;
			this.menuItemAbout.Text = "&About";
			this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
			// 
			// labelConversionLog
			// 
			this.labelConversionLog.Location = new System.Drawing.Point(8, 368);
			this.labelConversionLog.Name = "labelConversionLog";
			this.labelConversionLog.Size = new System.Drawing.Size(100, 16);
			this.labelConversionLog.TabIndex = 17;
			this.labelConversionLog.Text = "Conversion Log:";
			// 
			// buttonExit
			// 
			this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonExit.Location = new System.Drawing.Point(320, 536);
			this.buttonExit.Name = "buttonExit";
			this.buttonExit.Size = new System.Drawing.Size(64, 23);
			this.buttonExit.TabIndex = 20;
			this.buttonExit.Text = "E&xit";
			this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
			// 
			// buttonSaveLog
			// 
			this.buttonSaveLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonSaveLog.Location = new System.Drawing.Point(8, 536);
			this.buttonSaveLog.Name = "buttonSaveLog";
			this.buttonSaveLog.Size = new System.Drawing.Size(72, 23);
			this.buttonSaveLog.TabIndex = 21;
			this.buttonSaveLog.Text = "&Save Log";
			this.buttonSaveLog.Click += new System.EventHandler(this.buttonSaveLog_Click);
			// 
			// FormConvertCodeSmith
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(392, 561);
			this.Controls.Add(this.buttonSaveLog);
			this.Controls.Add(this.buttonExit);
			this.Controls.Add(this.labelConversionLog);
			this.Controls.Add(this.groupBoxMyGen);
			this.Controls.Add(this.groupBoxCodeSmith);
			this.Controls.Add(this.buttonConvert);
			this.Controls.Add(this.textBoxConsole);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenuConverter;
			this.Name = "FormConvertCodeSmith";
			this.Text = "CodeSmith-2-MyGeneration Converter";
			this.Load += new System.EventHandler(this.FormConvertCodeSmith_Load);
			this.Closed += new System.EventHandler(this.FormConvertCodeSmith_Closed);
			this.groupBoxCodeSmith.ResumeLayout(false);
			this.groupBoxMyGen.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Menu Event Handlers
		private void menuItemExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
			Application.Exit();
		}

		private void menuItemAbout_Click(object sender, System.EventArgs e)
		{
			FormAbout about = new FormAbout();
			about.ShowDialog(this);
		}

		private void menuItemConvert_Click(object sender, System.EventArgs e)
		{
			ConvertTemplates();
		}
		#endregion 

		#region ILog Members

		public void AddEntry(Exception ex)
		{
			AddEntry("EXCEPTION [{0}] - Message=\"{1}\" Source=\"{2}\" StackTrace=\"{3}\" ", 
				ex.GetType().Name, ex.Message, ex.Source, ex.StackTrace);
		}

		public void AddEntry(string message, params object[] args)
		{
			string item;
			if (args.Length > 0)
				item = DateTime.Now.ToString() + " - " + string.Format(message, args);
			else
				item = DateTime.Now.ToString() + " - " + message;

			WriteToTextBox(item);
		}
		#endregion
	}
}
