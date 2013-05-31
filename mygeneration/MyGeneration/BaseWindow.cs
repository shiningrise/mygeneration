using System;
using System.IO;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration
{
	public class BaseWindow :  DockContent
	{
		public BaseWindow() {}

		virtual public void DefaultSettingsChanged(DefaultSettings settings) {}

		virtual public bool CanClose(bool allowPrevent)
		{
			return true;
		}

		public virtual void ShowCatchingErrors(DockPanel dockManager) 
		{
			try 
			{
				this.Show(dockManager);
			}	
			catch (Exception ex) 
			{
				string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockManager.config");
				if (File.Exists(configFile)) 
				{
					try { File.Delete(configFile); } 
					catch {}
				}
				else 
				{
					throw ex;
				}
			}
		}

		virtual public void ResetMenu() {}
	}
}
