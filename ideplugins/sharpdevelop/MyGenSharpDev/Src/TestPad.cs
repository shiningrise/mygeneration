/*
 * Created by SharpDevelop.
 * User: justin.greenwood
 * Date: 8/8/2008
 * Time: 12:11 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Windows.Forms;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Gui;
using MyGeneration;

namespace MyGenSharpDev22
{
/// <summary>
/// Description of the pad content
/// </summary>
public class TestPad : AbstractPadContent
{
    TemplateBrowserControl templateBrowser;
    
    /// <summary>
    /// Creates a new TestPad object
    /// </summary>
    public TestPad()
    {
    	 string appPath = null;
            try
            {
                object o = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\MyGeneration13").GetValue(@"Install_Dir");
                if (o != null) appPath = o.ToString();
            }
            catch
            {
                appPath = null;
            }

            if (appPath != null) 
            {
                if (System.IO.Directory.Exists(appPath))
                {
                    MyGeneration.DefaultSettings.ApplicationPath = appPath;
                    Zeus.FileTools.ApplicationPath = appPath;
                }
            }
            
        templateBrowser = new TemplateBrowserControl();
        templateBrowser.Initialize();
    }
    
    /// <summary>
    /// The <see cref="System.Windows.Forms.Control"/> representing the pad
    /// </summary>
    public override Control Control {
        get {
            return templateBrowser;
        }
    }
    
    /// <summary>
    /// Refreshes the pad
    /// </summary>
    public override void RedrawContent()
    {
        // TODO: Refresh the whole pad control here, renew all resource strings whatever
        //       Note that you do not need to recreate the control.
    }
    
    /// <summary>
    /// Cleans up all used resources
    /// </summary>
    public override void Dispose()
    {
        templateBrowser.Dispose();
    }
}
}
