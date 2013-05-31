using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MyGeneration;
using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration.UI.Plugins.SqlTool
{
    public class SqlToolPluginManager : ISimplePluginManager
    {
        public string Name
        {
            get { return "Open in SqlTool"; }
        }

        public string Description
        {
            get { return "This will the current active edit control text in the SQL Tool plugin editor. - komma8.komma1"; }
        }

        public Uri AuthorUri
        {
            get
            {
                return new Uri("http://sourceforge.net/projects/mygeneration/");
            }
        }

        public bool AddToolbarIcon
        {
            get { return true; }
        }

        public Image MenuImage
        {
            get { return Properties.Resources.riskdb; }
        }

        public void Execute(IMyGenerationMDI mdi, params string[] args)
        {
            SqlToolForm stf = null;
            int cnt = 0;
            do
            {
                foreach (IDockContent d in mdi.DockPanel.Documents)
                {
                    if (d is SqlToolForm)
                    {
                        stf = d as SqlToolForm;
                        if (stf.IsNew && stf.IsEmpty)
                        {
                            break;
                        }
                        else
                        {
                            stf = null;
                        }
                    }
                }

                if (stf == null)
                {
                    mdi.CreateDocument(SqlToolEditorManager.SQL_FILE);
                }
                cnt++;
            } while (stf == null && cnt < 2);

            if (stf != null)
            {
                if (mdi.DockPanel.ActiveDocument != null)
                {
                    if (mdi.DockPanel.ActiveDocument is IMyGenDocument)
                    {
                        IMyGenDocument doc = mdi.DockPanel.ActiveDocument as IMyGenDocument;
                        stf.TextContent = doc.TextContent;
                    }
                }
                stf.Show();
                stf.Activate();
            }
        }
    }
}
