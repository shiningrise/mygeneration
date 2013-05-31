using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using System.Text;
using System.Diagnostics;
using System.Security.Principal;
using System.CodeDom;
using System.CodeDom.Compiler;

using Zeus.ErrorHandling;

namespace Zeus.UserInterface.WinForms.CrazyErrors
{
	/// <summary>
	/// Summary description for ZeusDisplayError.
	/// </summary>
	public class ZeusDisplayError : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label labelExceptionType;
		private System.Windows.Forms.TextBox textBoxExceptionType;
		private System.Windows.Forms.TextBox textBoxSource;
		private System.Windows.Forms.Label labelSource;
		private System.Windows.Forms.TextBox textBoxStackTrace;
		private System.Windows.Forms.Label labelMessage;
		private System.Windows.Forms.Label labelStackTrace;
		private System.Windows.Forms.TextBox textBoxMessage;
		private System.Windows.Forms.TextBox textBoxLine;
		private System.Windows.Forms.TextBox textBoxColumn;
		private System.Windows.Forms.Label labelLine;
		private System.Windows.Forms.Label labelColumn;
		private System.Windows.Forms.TextBox textBoxMethod;
		private System.Windows.Forms.Label labelMethod;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Label labelTitle;
		private Exception exception;
		
		private string lastFileName = string.Empty;
		private bool isTemplate = true;
		private int lineNumber = -1, _index = 0, _max = 0;
		private System.Windows.Forms.Label labelErrorIndex;
		private System.Windows.Forms.Button buttonNext;
		private System.Windows.Forms.Button buttonPrev;
		private System.Windows.Forms.Panel panelErrors;
		private System.Windows.Forms.TextBox textBoxFile;
		private System.Windows.Forms.Label labelFile;

		ArrayList lastErrors = new ArrayList();

		public event EventHandler ErrorIndexChange;

		protected void OnErrorIndexChange()
		{
			if (this.ErrorIndexChange != null) 
			{
				this.ErrorIndexChange(this, new EventArgs());
			}
		}

		public ZeusDisplayError(Exception exception)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.exception = exception;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ZeusDisplayError));
			this.labelExceptionType = new System.Windows.Forms.Label();
			this.textBoxExceptionType = new System.Windows.Forms.TextBox();
			this.textBoxSource = new System.Windows.Forms.TextBox();
			this.labelSource = new System.Windows.Forms.Label();
			this.textBoxStackTrace = new System.Windows.Forms.TextBox();
			this.labelMessage = new System.Windows.Forms.Label();
			this.labelStackTrace = new System.Windows.Forms.Label();
			this.textBoxMessage = new System.Windows.Forms.TextBox();
			this.textBoxLine = new System.Windows.Forms.TextBox();
			this.textBoxColumn = new System.Windows.Forms.TextBox();
			this.labelLine = new System.Windows.Forms.Label();
			this.labelColumn = new System.Windows.Forms.Label();
			this.textBoxMethod = new System.Windows.Forms.TextBox();
			this.labelMethod = new System.Windows.Forms.Label();
			this.buttonClose = new System.Windows.Forms.Button();
			this.labelTitle = new System.Windows.Forms.Label();
			this.labelErrorIndex = new System.Windows.Forms.Label();
			this.buttonNext = new System.Windows.Forms.Button();
			this.buttonPrev = new System.Windows.Forms.Button();
			this.panelErrors = new System.Windows.Forms.Panel();
			this.textBoxFile = new System.Windows.Forms.TextBox();
			this.labelFile = new System.Windows.Forms.Label();
			this.panelErrors.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelExceptionType
			// 
			this.labelExceptionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.labelExceptionType.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelExceptionType.Location = new System.Drawing.Point(8, 72);
			this.labelExceptionType.Name = "labelExceptionType";
			this.labelExceptionType.Size = new System.Drawing.Size(472, 16);
			this.labelExceptionType.TabIndex = 0;
			this.labelExceptionType.Text = "Exception Type:";
			this.labelExceptionType.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textBoxExceptionType
			// 
			this.textBoxExceptionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxExceptionType.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBoxExceptionType.Location = new System.Drawing.Point(8, 88);
			this.textBoxExceptionType.Name = "textBoxExceptionType";
			this.textBoxExceptionType.ReadOnly = true;
			this.textBoxExceptionType.Size = new System.Drawing.Size(472, 20);
			this.textBoxExceptionType.TabIndex = 1;
			this.textBoxExceptionType.Text = "";
			// 
			// textBoxSource
			// 
			this.textBoxSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxSource.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBoxSource.Location = new System.Drawing.Point(8, 128);
			this.textBoxSource.Name = "textBoxSource";
			this.textBoxSource.ReadOnly = true;
			this.textBoxSource.Size = new System.Drawing.Size(472, 20);
			this.textBoxSource.TabIndex = 3;
			this.textBoxSource.Text = "";
			// 
			// labelSource
			// 
			this.labelSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.labelSource.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelSource.Location = new System.Drawing.Point(8, 112);
			this.labelSource.Name = "labelSource";
			this.labelSource.Size = new System.Drawing.Size(472, 16);
			this.labelSource.TabIndex = 2;
			this.labelSource.Text = "Source:";
			this.labelSource.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textBoxStackTrace
			// 
			this.textBoxStackTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxStackTrace.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBoxStackTrace.Location = new System.Drawing.Point(8, 304);
			this.textBoxStackTrace.Multiline = true;
			this.textBoxStackTrace.Name = "textBoxStackTrace";
			this.textBoxStackTrace.ReadOnly = true;
			this.textBoxStackTrace.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxStackTrace.Size = new System.Drawing.Size(472, 64);
			this.textBoxStackTrace.TabIndex = 5;
			this.textBoxStackTrace.Text = "";
			// 
			// labelMessage
			// 
			this.labelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.labelMessage.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelMessage.Location = new System.Drawing.Point(8, 232);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Size = new System.Drawing.Size(472, 16);
			this.labelMessage.TabIndex = 4;
			this.labelMessage.Text = "Message:";
			this.labelMessage.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelStackTrace
			// 
			this.labelStackTrace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.labelStackTrace.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelStackTrace.Location = new System.Drawing.Point(8, 288);
			this.labelStackTrace.Name = "labelStackTrace";
			this.labelStackTrace.Size = new System.Drawing.Size(472, 16);
			this.labelStackTrace.TabIndex = 7;
			this.labelStackTrace.Text = "Stack Trace:";
			this.labelStackTrace.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textBoxMessage
			// 
			this.textBoxMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxMessage.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBoxMessage.Location = new System.Drawing.Point(8, 248);
			this.textBoxMessage.Multiline = true;
			this.textBoxMessage.Name = "textBoxMessage";
			this.textBoxMessage.ReadOnly = true;
			this.textBoxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxMessage.Size = new System.Drawing.Size(472, 40);
			this.textBoxMessage.TabIndex = 8;
			this.textBoxMessage.Text = "";
			// 
			// textBoxLine
			// 
			this.textBoxLine.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBoxLine.Location = new System.Drawing.Point(8, 208);
			this.textBoxLine.Name = "textBoxLine";
			this.textBoxLine.ReadOnly = true;
			this.textBoxLine.Size = new System.Drawing.Size(72, 20);
			this.textBoxLine.TabIndex = 9;
			this.textBoxLine.Text = "";
			// 
			// textBoxColumn
			// 
			this.textBoxColumn.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBoxColumn.Location = new System.Drawing.Point(88, 208);
			this.textBoxColumn.Name = "textBoxColumn";
			this.textBoxColumn.ReadOnly = true;
			this.textBoxColumn.Size = new System.Drawing.Size(72, 20);
			this.textBoxColumn.TabIndex = 10;
			this.textBoxColumn.Text = "";
			// 
			// labelLine
			// 
			this.labelLine.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelLine.Location = new System.Drawing.Point(8, 192);
			this.labelLine.Name = "labelLine";
			this.labelLine.Size = new System.Drawing.Size(72, 16);
			this.labelLine.TabIndex = 11;
			this.labelLine.Text = "Line:";
			this.labelLine.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelColumn
			// 
			this.labelColumn.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelColumn.Location = new System.Drawing.Point(88, 192);
			this.labelColumn.Name = "labelColumn";
			this.labelColumn.Size = new System.Drawing.Size(72, 16);
			this.labelColumn.TabIndex = 12;
			this.labelColumn.Text = "Column:";
			this.labelColumn.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textBoxMethod
			// 
			this.textBoxMethod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxMethod.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBoxMethod.Location = new System.Drawing.Point(8, 168);
			this.textBoxMethod.Name = "textBoxMethod";
			this.textBoxMethod.ReadOnly = true;
			this.textBoxMethod.Size = new System.Drawing.Size(472, 20);
			this.textBoxMethod.TabIndex = 14;
			this.textBoxMethod.Text = "";
			// 
			// labelMethod
			// 
			this.labelMethod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.labelMethod.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelMethod.Location = new System.Drawing.Point(8, 152);
			this.labelMethod.Name = "labelMethod";
			this.labelMethod.Size = new System.Drawing.Size(472, 16);
			this.labelMethod.TabIndex = 13;
			this.labelMethod.Text = "Method:";
			this.labelMethod.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// buttonClose
			// 
			this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonClose.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonClose.Location = new System.Drawing.Point(400, 384);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.TabIndex = 15;
			this.buttonClose.Text = "Close";
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// labelTitle
			// 
			this.labelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.labelTitle.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelTitle.Location = new System.Drawing.Point(8, 8);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(472, 24);
			this.labelTitle.TabIndex = 16;
			this.labelTitle.Text = "System Exception";
			this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelErrorIndex
			// 
			this.labelErrorIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelErrorIndex.Location = new System.Drawing.Point(72, 16);
			this.labelErrorIndex.Name = "labelErrorIndex";
			this.labelErrorIndex.Size = new System.Drawing.Size(48, 24);
			this.labelErrorIndex.TabIndex = 0;
			this.labelErrorIndex.Text = "1 of 5";
			this.labelErrorIndex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// buttonNext
			// 
			this.buttonNext.Location = new System.Drawing.Point(120, 16);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(56, 23);
			this.buttonNext.TabIndex = 18;
			this.buttonNext.Text = "Next";
			this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
			// 
			// buttonPrev
			// 
			this.buttonPrev.Location = new System.Drawing.Point(8, 16);
			this.buttonPrev.Name = "buttonPrev";
			this.buttonPrev.Size = new System.Drawing.Size(56, 23);
			this.buttonPrev.TabIndex = 19;
			this.buttonPrev.Text = "Prev";
			this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
			// 
			// panelErrors
			// 
			this.panelErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.panelErrors.Controls.Add(this.buttonPrev);
			this.panelErrors.Controls.Add(this.labelErrorIndex);
			this.panelErrors.Controls.Add(this.buttonNext);
			this.panelErrors.Location = new System.Drawing.Point(0, 368);
			this.panelErrors.Name = "panelErrors";
			this.panelErrors.Size = new System.Drawing.Size(208, 48);
			this.panelErrors.TabIndex = 20;
			// 
			// textBoxFile
			// 
			this.textBoxFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxFile.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBoxFile.Location = new System.Drawing.Point(8, 48);
			this.textBoxFile.Name = "textBoxFile";
			this.textBoxFile.ReadOnly = true;
			this.textBoxFile.Size = new System.Drawing.Size(472, 20);
			this.textBoxFile.TabIndex = 22;
			this.textBoxFile.Text = "";
			// 
			// labelFile
			// 
			this.labelFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.labelFile.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelFile.Location = new System.Drawing.Point(8, 32);
			this.labelFile.Name = "labelFile";
			this.labelFile.Size = new System.Drawing.Size(472, 16);
			this.labelFile.TabIndex = 21;
			this.labelFile.Text = "File:";
			this.labelFile.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// ZeusDisplayError
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(488, 414);
			this.Controls.Add(this.textBoxFile);
			this.Controls.Add(this.labelFile);
			this.Controls.Add(this.panelErrors);
			this.Controls.Add(this.labelTitle);
			this.Controls.Add(this.buttonClose);
			this.Controls.Add(this.textBoxMethod);
			this.Controls.Add(this.textBoxColumn);
			this.Controls.Add(this.textBoxLine);
			this.Controls.Add(this.textBoxMessage);
			this.Controls.Add(this.textBoxStackTrace);
			this.Controls.Add(this.textBoxSource);
			this.Controls.Add(this.textBoxExceptionType);
			this.Controls.Add(this.labelMethod);
			this.Controls.Add(this.labelColumn);
			this.Controls.Add(this.labelLine);
			this.Controls.Add(this.labelStackTrace);
			this.Controls.Add(this.labelMessage);
			this.Controls.Add(this.labelSource);
			this.Controls.Add(this.labelExceptionType);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ZeusDisplayError";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Execption Occurred";
			this.Load += new System.EventHandler(this.ZeusDisplayError_Load);
			this.panelErrors.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		public void SetControlsFromException() 
		{
			lineNumber = -1;
			isTemplate = true;

			lastErrors.Clear();

			if (exception is ZeusExecutionException)
			{
				ZeusExecutionException executionException = exception as ZeusExecutionException;

				int numErrors = executionException.Errors.Length;
				if (numErrors > 1) 
				{
					this.panelErrors.Visible = true;
					this._max = executionException.Errors.Length - 1;
					this.UpdateErrorLabel();
					
				}
				else 
				{
					this.panelErrors.Visible = false;
				}

				this.labelFile.Text = "Template Filename";
				this.labelTitle.Text = "Scripting Error";
				this.labelSource.Text = "Source";
				this.labelMessage.Text = "Error Text";
				this.labelMethod.Text = "Error Number";
				this.labelStackTrace.Text = "Scripting Errors and Warnings:";
				this.textBoxExceptionType.Text = "Scripting Error"; 
						
				IZeusExecutionError error = executionException.Errors[this._index];

				if (!error.IsWarning) 
				{
					this.textBoxFile.Text = error.FileName;
					this.textBoxLine.Text = error.Line.ToString();
					this.textBoxColumn.Text = error.Column.ToString();
					this.textBoxSource.Text = error.Source;					
					this.textBoxMessage.Text = error.Message + " " + error.Description;
					this.textBoxMethod.Text = error.Number;
						
					this.lastFileName = error.FileName;
					this.lineNumber = error.Line;
				}
				
				foreach (IZeusExecutionError tmperror in executionException.Errors) 
				{
					// Create error string for the log
					StringBuilder builder = new StringBuilder();
					builder.Append(tmperror.IsWarning ? "[Warning] " : "[Error] ");
					builder.Append(tmperror.Source);
					builder.Append(" (" + tmperror.Line + ", " + tmperror.Column + ") ");
					builder.Append(tmperror.Number + ": ");
					builder.Append(tmperror.Message);

					if (tmperror.StackTrace != string.Empty) 
					{
						builder.Append("\r\nStack Trace");
						builder.Append(tmperror.StackTrace);
					}

					if (tmperror == error) 
					{
						this.textBoxStackTrace.Text = builder.ToString();
					}

					lastErrors.Add(builder.ToString());
				}

				isTemplate = executionException.IsTemplateScript;

			}
			else if (exception is ZeusDynamicException)
			{
				ZeusDynamicException zde = exception as ZeusDynamicException;

				this.textBoxFile.Text = string.Empty;
				this.textBoxLine.Text = string.Empty;
				this.textBoxColumn.Text = string.Empty;

				this.textBoxSource.Text = string.Empty;	
				this.textBoxMessage.Text = zde.Message;
				this.textBoxMethod.Text = string.Empty;		
				this.textBoxStackTrace.Text = string.Empty;
				this.textBoxExceptionType.Text = zde.GetType().Name;

				this.labelTitle.Text = zde.DynamicExceptionTypeString;
				this.labelMethod.Text = string.Empty;

				// Create error string for the log
				lastErrors.Add("[" + this.textBoxExceptionType.Text + "] " + zde.DynamicExceptionTypeString + " - " + zde.Message);
			}
			else 
			{
				StackTrace stackTrace = null;
				StackFrame stackFrame = null;
			
				Exception baseException = exception.GetBaseException();

				try
				{
					stackTrace = new StackTrace(baseException, true);

					if (stackTrace.FrameCount > 0) 
					{
						stackFrame = stackTrace.GetFrame(stackTrace.FrameCount - 1);
					}
				} 
				catch 
				{
					stackTrace = null;
				}

				if (stackFrame != null) 
				{
					this.textBoxFile.Text = stackFrame.GetFileName();
					this.textBoxLine.Text = stackFrame.GetFileLineNumber().ToString();
					this.textBoxColumn.Text = stackFrame.GetFileColumnNumber().ToString();
				}

				this.textBoxSource.Text = baseException.Source;					
				this.textBoxMessage.Text = baseException.Message;
				this.textBoxMethod.Text = baseException.TargetSite.ToString();					
				this.textBoxStackTrace.Text = baseException.StackTrace;			
				this.textBoxExceptionType.Text = baseException.GetType().Name;			

				this.labelTitle.Text = "System Exception";
				this.labelMethod.Text = "Method";

				// Create error string for the log
				lastErrors.Add( "[" + this.textBoxExceptionType.Text + "] " + this.textBoxSource.Text +
					"(" + this.textBoxLine.Text + ", " + this.textBoxColumn.Text + ") " +
					this.textBoxMessage.Text);
			}
		}

		private void ZeusDisplayError_Load(object sender, System.EventArgs e)
		{
			_index = 0;
			SetControlsFromException();
			UpdateErrorLabel();

			OnErrorIndexChange();
		}

		public void UpdateErrorLabel() 
		{
			int index = _index + 1;
			int max = _max + 1;
			this.labelErrorIndex.Text = index.ToString() + " of " + max.ToString();

			this.buttonNext.Enabled = (_index != _max);
			this.buttonPrev.Enabled = (_index != 0);
		}

		private void buttonNext_Click(object sender, System.EventArgs e)
		{
			if (_index < _max)
			{
				_index++;
			}
			UpdateErrorLabel();
			SetControlsFromException();

			OnErrorIndexChange();
		}

		private void buttonPrev_Click(object sender, System.EventArgs e)
		{
			if (_index > 0)
			{
				_index--;
			}
			UpdateErrorLabel();
			SetControlsFromException();

			OnErrorIndexChange();
		}

		public bool LastErrorIsTemplate
		{
			get 
			{
				return isTemplate;
			}
		}

		public string LastErrorFileName 
		{
			get 
			{
				return this.lastFileName;
			}
		}

		public bool LastErrorIsScript
		{
			get 
			{
				return (lineNumber >= 0);
			}
		}
		
		public int LastErrorLineNumber
		{
			get 
			{
				return lineNumber;
			}
		}
		
		public ArrayList LastErrorMessages 
		{
			get 
			{
				return lastErrors;
			}
		}

		public string LastErrorMessage 
		{
			get 
			{
				if (lastErrors.Count > 0)
					return lastErrors[lastErrors.Count - 1].ToString();
				else
					return string.Empty;
			}
		}
	}
}
