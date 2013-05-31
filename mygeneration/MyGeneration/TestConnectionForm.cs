using System;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MyGeneration
{
	public enum ConnectionTestState 
	{
		Waiting,
		Error,
		Success
	}

	/// <summary>
	/// Summary description for TestConnectionForm.
	/// </summary>
	public class TestConnectionForm : System.Windows.Forms.Form
    {
        delegate void SetTextCallback(string text);
        delegate void SetColorCallback(Color color);
        delegate void SetCursorCallback(Cursor cursor);
        delegate void SetSizeCallback(Size size);
        delegate void SetCloseCallback();

		private static bool up = true;
		private static bool dotUp = true;
		private static int dotCount = 0;
		private static string connectionType = null;
		private static string connectionString = null;
		private static string info = null;
		private static ConnectionTestState state = ConnectionTestState.Waiting;
		private static Process process = null;
		private System.Timers.Timer timer = new System.Timers.Timer(100);
		private bool isSilent = false;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelState;
		private System.Windows.Forms.TextBox textBoxErrorMessage;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private int randNumber = 0;

		
		public TestConnectionForm(string connType, string connString, bool isSilent)
		{
			Random rand = new Random();
			randNumber = rand.Next(2);
			dotCount=0;

			this.isSilent = isSilent;
			this.Cursor = Cursors.WaitCursor;
			Test(connType, connString);

			timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
			timer.Enabled = true;

			InitializeComponent();
        }

        private void SetStateText(string text)
        {
            if (this.labelState.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetStateText);
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    try
                    {
                        this.Invoke(d, new object[] { text });
                    }
                    catch { }
                }
            }
            else
            {
                this.labelState.Text = text;
            }
        }

        private void SetStateColor(Color color)
        {
            if (this.labelState.InvokeRequired)
            {
                SetColorCallback d = new SetColorCallback(SetStateColor);
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    try
                    {
                        this.Invoke(d, new object[] { color });
                    }
                    catch { }
                }
            }
            else
            {
                this.labelState.ForeColor = color;
            }
        }

        private void SetDescriptionText(string text)
        {
            if (this.textBoxErrorMessage.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetDescriptionText);
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    try
                    {
                        this.Invoke(d, new object[] { text });
                    }
                    catch { }
                }
            }
            else
            {
                string nl = "@!!nl@!!";
                this.textBoxErrorMessage.Text = text
                    .Replace("\r\n", nl)
                    .Replace("\n", nl)
                    .Replace("\r", nl)
                    .Replace(nl, Environment.NewLine);
            }
        }

        private void SetButtonText(string text)
        {

            if (this.buttonCancel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetButtonText);
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    try
                    {
                        this.Invoke(d, new object[] { text });
                    }
                    catch { }
                }
            }
            else
            {
                this.buttonCancel.Text = text;
            }
        }

        private void ChangeCursor(Cursor cursor)
        {
            if (this.InvokeRequired)
            {
                SetCursorCallback d = new SetCursorCallback(ChangeCursor);
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    try
                    {
                        this.Invoke(d, new object[] { cursor });
                    }
                    catch { }
                }
            }
            else
            {
                this.Cursor = cursor;
            }
        }

        private void ChangeSize(Size size)
        {
            if (this.InvokeRequired)
            {
                SetSizeCallback d = new SetSizeCallback(ChangeSize);
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    try
                    {
                        this.Invoke(d, new object[] { size });
                    }
                    catch { }
                }
            }
            else
            {
                this.Size = size;

                this.MaximumSize = new Size(0, 0);
                this.FormBorderStyle = FormBorderStyle.Sizable;

            }
        }

        private void CloseForm()
        {
            if (this.InvokeRequired)
            {
                SetCloseCallback d = new SetCloseCallback(CloseForm);
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    try
                    {
                        this.Invoke(d, new object[] { });
                    }
                    catch { }
                }
            }
            else
            {
                this.Close();
            }
        }

		private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			ConnectionTestState s = State;
			string strState = "";

			if (s == ConnectionTestState.Waiting) 
			{
				byte rg = labelState.ForeColor.G;
				if (up) 
				{
					if (rg < 160) rg += 20;
					else 
					{
						up = false;
					}
				}
				else 
				{
					if (rg > 0) rg -= 20;
					else 
					{
						up = true;
					}
				}

				//labelState.ForeColor = Color.FromArgb(0, rg, 255 - rg);
                this.SetStateColor(Color.FromArgb(0, rg, 255 - rg));
                if (randNumber == 0) 
				{
					if (dotCount == 16) dotUp = false; 
					else if (dotCount == 0) dotUp = true; 
					if (dotUp) dotCount++;
					else dotCount--;
				
					strState = string.Empty;
					for (int i=0; i<dotCount; i++) strState += ".";
					strState += "Connecting";
					for (int i=0; i<16-dotCount; i++) strState+= ".";

					this.SetStateText(strState);
				}
				else 
				{
					if (dotCount >= 2) dotUp = false; 
					else if (dotCount <= 0) dotUp = true; 
					if (dotUp) dotCount++;
					else dotCount--;

					string text = "Connecting";
					foreach (char c in text) 
					{
						for (int i=0; i < dotCount; i++) 
						{
							strState += " ";
						}
						strState += c;
                    }

                    this.SetStateText(strState.Trim());
					//labelState.Text = strState.Trim();
				}
			}
			else 
			{
                ChangeCursor(Cursors.Default);
				this.timer.Enabled = false;
                if (s == ConnectionTestState.Error)
                {
                    this.SetStateText("Connection Error");
                    this.SetStateColor(Color.Red);
                    //labelState.Text = "Connection Error";
					labelState.ForeColor = Color.Red;

                    this.SetDescriptionText(info);
					//this.textBoxErrorMessage.Text = info;
                    this.ChangeSize(this.MaximumSize);
				}
				else if (s == ConnectionTestState.Success)
                {
                    this.SetStateText("Connection Successful!");
                    this.SetStateColor(Color.Green);
					//labelState.Text = "Connection Successful!";
					//labelState.ForeColor = Color.Green;

					if (this.isSilent)
                        this.CloseForm();
				}

                SetButtonText("&Close");
			}
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
					timer.Enabled = false;
					timer.Dispose();
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelState = new System.Windows.Forms.Label();
            this.textBoxErrorMessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Location = new System.Drawing.Point(292, 8);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(56, 24);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelState
            // 
            this.labelState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelState.BackColor = System.Drawing.Color.LemonChiffon;
            this.labelState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelState.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelState.ForeColor = System.Drawing.Color.Blue;
            this.labelState.Location = new System.Drawing.Point(8, 8);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(276, 24);
            this.labelState.TabIndex = 2;
            this.labelState.Text = "Connecting";
            this.labelState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxErrorMessage
            // 
            this.textBoxErrorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxErrorMessage.BackColor = System.Drawing.Color.LemonChiffon;
            this.textBoxErrorMessage.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxErrorMessage.ForeColor = System.Drawing.Color.Red;
            this.textBoxErrorMessage.Location = new System.Drawing.Point(8, 40);
            this.textBoxErrorMessage.Multiline = true;
            this.textBoxErrorMessage.Name = "textBoxErrorMessage";
            this.textBoxErrorMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxErrorMessage.Size = new System.Drawing.Size(340, 0);
            this.textBoxErrorMessage.TabIndex = 4;
            // 
            // TestConnectionForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(356, 46);
            this.ControlBox = false;
            this.Controls.Add(this.textBoxErrorMessage);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximumSize = new System.Drawing.Size(358, 178);
            this.MinimumSize = new System.Drawing.Size(358, 48);
            this.Name = "TestConnectionForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		
		public static void Test(string connectionType, string connectionString) 
		{
			TestConnectionForm.dotCount = 3;
			TestConnectionForm.connectionType = connectionType;
			TestConnectionForm.connectionString = connectionString;
			TestConnectionForm.state = ConnectionTestState.Waiting;
			TestConnectionForm.info = null;

			string file = Zeus.FileTools.RootFolder + @".\ZeusCmd.exe";
			if (!File.Exists(file)) 
			{
				file = Zeus.FileTools.RootFolder + @"\..\..\..\..\ZeusCmd\bin\debug\ZeusCmd.exe";
				
				if (!File.Exists(file)) 
				{
					file = Zeus.FileTools.RootFolder + @"\..\..\..\..\ZeusCmd\bin\release\ZeusCmd.exe";
				}
			}

			if (File.Exists(file)) 
			{
#if DEBUG
                string args = string.Format("-tc \"{0}\" \"{1}\" -l \"{2}\"", connectionType.Replace("\"", "\\\""), connectionString.Replace("\"", "\\\""), "ZeusCmd.log");
#else
                string args = string.Format("-tc \"{0}\" \"{1}\"", connectionType.Replace("\"", "\\\""), connectionString.Replace("\"", "\\\""));
#endif
                ProcessStartInfo psi = new ProcessStartInfo(file, args);
				psi.CreateNoWindow = true;
				psi.UseShellExecute = false;
				psi.RedirectStandardOutput = true;
				process = new Process();
				process.StartInfo = psi;
				process.Start();
			}
			else 
			{
				TestConnectionForm.state = ConnectionTestState.Error;
				process = null;
			}
		}

		public static ConnectionTestState State
		{
			get 
			{ 
				if (process != null) 
				{
					if (state == ConnectionTestState.Waiting) 
					{
						if (process.HasExited) 
						{
							if (process.ExitCode == 0) 
							{
								state = ConnectionTestState.Success;
							}
							else 
							{
								state = ConnectionTestState.Error;
								info = process.StandardOutput.ReadToEnd().TrimEnd();
								info = info.Substring(info.IndexOf("ERROR") + 6);
							}
						}
					}
				}
				return state; 
			}
		}

		public static string LastErrorMsg
		{
			get { return info; }
		}

		public static void Cancel() 
		{
			if (state == ConnectionTestState.Waiting)
			{
				state = ConnectionTestState.Error;
				info = "Connection Cancelled";
				if (process != null) 
				{
					process.Kill();
				}
			}
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			Cancel();
			this.Close();
		}
	}
}
