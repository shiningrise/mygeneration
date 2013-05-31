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
using ICSharpCode.SharpDevelop.Gui.XmlForms;
using MyGeneration;

namespace MyGenSharpDev22
{
public class MyUserControl : BaseSharpDevelopUserControl
{
    public MyUserControl()
    {
        SetupFromXmlStream(this.GetType().Assembly.GetManifestResourceStream("MyGenSharpDev22.Resources.MyUserControl.xfrm"));
        Get<Button>("test").Click += ButtonClick;
    }
    
    void ButtonClick(object sender, EventArgs e)
    {
        System.Windows.Forms.MessageBox.Show("The button was clicked!");
    }
}
}
