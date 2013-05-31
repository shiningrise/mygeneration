using System;using System.Drawing;using System.Collections;using System.ComponentModel;using System.Windows.Forms;using System.IO;
using Scintilla;using GreenwoodLib.Zeus;
using GreenwoodLib.Zeus.ScriptModel;
using GreenwoodLib.Zeus.Templates;
using GreenwoodLib.Zeus.ErrorHandling;
using GreenwoodLib.Zeus.UserInterface;using GreenwoodLib.Zeus.WinForms;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for ScriptWindow.
	/// </summary>
	public class ScriptWindow : ScintillaNETForm
	{
		public ScriptWindow()
		{

		}
		
		public void ScriptGenerate_Click(object sender, System.EventArgs e)
		{
			ZeusParser parser = new ZeusParser();			ZeusOutput zout = null;			ZeusTemplate template;			try 			{				template = parser.LoadTemplate(this.ScriptStream.BaseStream, this.ScriptFilePath);				System.Console.WriteLine(template.ScriptBody);						ExecuteZeusTemplate ex = new ExecuteZeusTemplate();				ex.ShowGUI += new ShowGUIEventHandler(DynamicGUI_Display);				zout = ex.Execute(template);			}			catch (Exception x)			{				ZeusDisplayError formError = new ZeusDisplayError(x);				formError.ShowDialog(this);			}			if (zout != null)			{				SimpleOutputForm frm = new SimpleOutputForm();				frm.OutText = zout.text;				frm.ShowDialog(this);			}		
		}		private StreamWriter ScriptStream 
		{
			get 
			{
				MemoryStream stream = new MemoryStream();
				StreamWriter writer = new StreamWriter(stream);				writer.Write(this.EditorText);
				writer.Flush();
				writer.BaseStream.Position = 0;

				return writer;
			}
		}		private string ScriptFilePath
		{
			get 
			{
				if(null != this.FileName)				{					return this.FileName;
				}
				else 
				{
					return null;
				}
			}
		}
		public void DynamicGUI_Display(GuiController gui, IScriptExecutioner executioner) 
		{
			DynamicForm df = new DynamicForm(gui, executioner);
			df.ShowDialog(this);
		}
	}
}
