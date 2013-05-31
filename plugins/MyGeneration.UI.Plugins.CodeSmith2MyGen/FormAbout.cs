using System;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using MyGeneration.CodeSmithConversion.Plugins;

namespace MyGeneration.CodeSmithConversion
{
	/// <summary>
	/// Summary description for FormAbout.
	/// </summary>
	public class FormAbout : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelWrittenBy;
		private System.Windows.Forms.LinkLabel linkLabelMyGen;
		private System.Windows.Forms.LinkLabel linkLabelCodeSmith;
		private System.Windows.Forms.TextBox textBoxPlugins;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormAbout()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private void FormAbout_Load(object sender, System.EventArgs e)
		{
			StringBuilder b = new StringBuilder();

			if (PluginController.Plugins.Length == 0) 
			{
				b.Append("There are no plugins available.");
				b.Append(Environment.NewLine);
			}
			else 
			{
				foreach (ICstProcessor cp in PluginController.Plugins) 
				{
					b.Append(cp.Name + " - Written by: " + cp.Author);
					b.Append(Environment.NewLine);
				}
			}

			this.textBoxPlugins.Text = b.ToString();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormAbout));
			this.label1 = new System.Windows.Forms.Label();
			this.labelWrittenBy = new System.Windows.Forms.Label();
			this.linkLabelMyGen = new System.Windows.Forms.LinkLabel();
			this.linkLabelCodeSmith = new System.Windows.Forms.LinkLabel();
			this.textBoxPlugins = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Navy;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(288, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "CodeSmith-2-MyGeneration";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label1.Click += new System.EventHandler(this.FormAbout_Click);
			// 
			// labelWrittenBy
			// 
			this.labelWrittenBy.ForeColor = System.Drawing.Color.Navy;
			this.labelWrittenBy.Location = new System.Drawing.Point(8, 32);
			this.labelWrittenBy.Name = "labelWrittenBy";
			this.labelWrittenBy.Size = new System.Drawing.Size(288, 16);
			this.labelWrittenBy.TabIndex = 1;
			this.labelWrittenBy.Text = "Copyright © 2005 MyGeneration Software";
			this.labelWrittenBy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelWrittenBy.Click += new System.EventHandler(this.FormAbout_Click);
			// 
			// linkLabelMyGen
			// 
			this.linkLabelMyGen.Location = new System.Drawing.Point(8, 96);
			this.linkLabelMyGen.Name = "linkLabelMyGen";
			this.linkLabelMyGen.Size = new System.Drawing.Size(240, 16);
			this.linkLabelMyGen.TabIndex = 2;
			this.linkLabelMyGen.TabStop = true;
			this.linkLabelMyGen.Text = "http://www.mygenerationsoftware.com/";
			this.linkLabelMyGen.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelMyGen_LinkClicked);
			// 
			// linkLabelCodeSmith
			// 
			this.linkLabelCodeSmith.Location = new System.Drawing.Point(8, 152);
			this.linkLabelCodeSmith.Name = "linkLabelCodeSmith";
			this.linkLabelCodeSmith.Size = new System.Drawing.Size(240, 16);
			this.linkLabelCodeSmith.TabIndex = 3;
			this.linkLabelCodeSmith.TabStop = true;
			this.linkLabelCodeSmith.Text = "http://www.codesmithtools.com/";
			this.linkLabelCodeSmith.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelCodeSmith_LinkClicked);
			// 
			// textBoxPlugins
			// 
			this.textBoxPlugins.Location = new System.Drawing.Point(8, 200);
			this.textBoxPlugins.Multiline = true;
			this.textBoxPlugins.Name = "textBoxPlugins";
			this.textBoxPlugins.ReadOnly = true;
			this.textBoxPlugins.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxPlugins.Size = new System.Drawing.Size(288, 56);
			this.textBoxPlugins.TabIndex = 4;
			this.textBoxPlugins.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 136);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(272, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Copyright © 2005 CodeSmithTools, LLC ";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(8, 120);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(280, 16);
			this.label3.TabIndex = 6;
			this.label3.Text = "CodeSmith ";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(8, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(280, 16);
			this.label4.TabIndex = 8;
			this.label4.Text = "MyGeneration";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 80);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(272, 16);
			this.label5.TabIndex = 7;
			this.label5.Text = "Copyright © 2005 MyGeneration Software";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label6.Location = new System.Drawing.Point(8, 184);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 16);
			this.label6.TabIndex = 9;
			this.label6.Text = "Dynamic Plugins";
			// 
			// FormAbout
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(306, 263);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBoxPlugins);
			this.Controls.Add(this.linkLabelCodeSmith);
			this.Controls.Add(this.linkLabelMyGen);
			this.Controls.Add(this.labelWrittenBy);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAbout";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About CodeSmith-2-MyGeneration";
			this.Click += new System.EventHandler(this.FormAbout_Click);
			this.Load += new System.EventHandler(this.FormAbout_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void linkLabelMyGen_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			LaunchBrowser(linkLabelMyGen.Text);
		}

		private void linkLabelCodeSmith_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			LaunchBrowser(linkLabelCodeSmith.Text);
		}

		private void FormAbout_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void LaunchBrowser(string url)
		{
			try 
			{
				System.Diagnostics.Process.Start( url );
			}
			catch 
			{
				Help.ShowHelp(this, url);
			}
		}

	}
}
