using System;using System.IO;
using Scintilla;

namespace MyGeneration{	/// <summary>	/// Summary description for ScintillaNETForm.	/// </summary>
	public class ScintillaNETForm : MyGenerationForm	{		public ScintillaNETForm()		{
		}
		override public void FileSave()		{			StreamWriter writer = new StreamWriter(this.FileName);			writer.Write(this.EditorText);			writer.Close();		}
		override public void FileOpen(string fileName)		{			StreamReader reader = new StreamReader(fileName);			this.EditorText = reader.ReadToEnd();			reader.Close();			this.FileName = fileName;		}
		virtual protected ScintillaControl ScintillaControl		{			get			{				return null;			}		}		virtual public string EditorText		{			get			{				return this.ScintillaControl.Text;			}			set			{				this.ScintillaControl.InsertText(0, value);			}		}	}}
