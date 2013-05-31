using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

namespace MyGeneration
{
    public abstract class ContentManager : IContentManager
    {
        #region Static factory type members
        public static void AddNewContentMenuItems(EventHandler onClickEvent, ToolStripMenuItem pluginsMenuItem, ToolStrip toolStrip)
        {
            pluginsMenuItem.DropDownItems.Clear();
            List<ToolStripItem> itemsToRemove = new List<ToolStripItem>();
            foreach (ToolStripItem tsi in toolStrip.Items) if (tsi.Name.Contains("ContentPlugin")) itemsToRemove.Add(tsi);
            foreach (ToolStripItem itr in itemsToRemove) toolStrip.Items.Remove(itr);

            int pluginCount = 0;
            if (PluginManager.ContentManagers.Count > 0)
            {
                pluginsMenuItem.Visible = true;
                int i = 0;
                foreach (IContentManager cm in PluginManager.ContentManagers.Values)
                {
                    pluginCount++;
                    string id = "ContentPlugin" + (++i).ToString();
                    ToolStripMenuItem item = new ToolStripMenuItem(cm.Name, cm.MenuImage, onClickEvent, "toolStripItem" + id);
                    item.ToolTipText = cm.Name;
                    item.ImageTransparentColor = Color.Magenta;
                    item.Tag = typeof(IContentManager);
                    pluginsMenuItem.DropDownItems.Add(item);

                    if ((cm.MenuImage != null) && cm.AddToolbarIcon)
                    {
                        ToolStripButton b = new ToolStripButton(null, cm.MenuImage, onClickEvent, "toolStripButton" + id);
                        b.ImageTransparentColor = Color.Magenta;
                        b.ToolTipText = cm.Name;
                        b.Tag = typeof(IContentManager);
                        toolStrip.Items.Add(b);
                    }
                }
            }

            if (PluginManager.SimplePluginManagers.Count > 0)
            {
                pluginsMenuItem.Visible = true;
                int i = 0;
                foreach (ISimplePluginManager spm in PluginManager.SimplePluginManagers.Values)
                {
                    pluginCount++;
                    string id = "SimplePlugin" + (++i).ToString();
                    ToolStripMenuItem item = new ToolStripMenuItem(spm.Name, spm.MenuImage, onClickEvent, "toolStripItem" + id);
                    item.ToolTipText = spm.Name;
                    item.ImageTransparentColor = Color.Magenta;
                    item.Tag = typeof(ISimplePluginManager);
                    pluginsMenuItem.DropDownItems.Add(item);

                    if ((spm.MenuImage != null) && spm.AddToolbarIcon)
                    {
                        ToolStripButton b = new ToolStripButton(null, spm.MenuImage, onClickEvent, "toolStripButton" + id);
                        b.ImageTransparentColor = Color.Magenta;
                        b.ToolTipText = spm.Name;
                        b.Tag = typeof(ISimplePluginManager);
                        toolStrip.Items.Add(b);
                    }
                }
            }
            
            if (pluginCount == 0)
            {
                pluginsMenuItem.Visible = false;
            }
        }

        public static IMyGenContent CreateContent(IMyGenerationMDI mdi, string key)
        {
            IMyGenContent mygencontent = null;
            if (PluginManager.ContentManagers.ContainsKey(key)) 
            {
                mygencontent = PluginManager.ContentManagers[key].Create(mdi);
            }
            return mygencontent;
        }
        #endregion

        public abstract string Name { get; }
        public abstract Uri AuthorUri { get; }
        public abstract string Description { get; }
        public abstract bool AddToolbarIcon { get; }
        public abstract IMyGenContent Create(IMyGenerationMDI mdi, params string[] args);

        public virtual Image MenuImage
        {
            get { return null; }
        }
    }
}
