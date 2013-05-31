using System;using System.IO;using System.Windows.Forms;
namespace MyGeneration{	/// <summary>	/// Summary description for MyGenerationForm.	/// </summary>	public class MyGenerationForm : System.Windows.Forms.Form	{
		public MyGenerationForm()		{
		}
		virtual public void FileSave()		{
		}

		private void InitializeComponent()
		{
			// 
			// MyGenerationForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Name = "MyGenerationForm";
			this.Load += new System.EventHandler(this.MyGenerationForm_Load);

		}
		virtual public void FileOpen(string fileName)		{
		}
		public string FileName		{			get			{				return fileName;			}
			set			{				fileName = value;				this.Text = fileName;			}		}
		private string fileName = "";

		private void MyGenerationForm_Load(object sender, System.EventArgs e)
		{
		
		}	}}