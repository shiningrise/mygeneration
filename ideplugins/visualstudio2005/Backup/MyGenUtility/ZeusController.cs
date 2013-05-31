using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Diagnostics;
using Zeus;
using Zeus.Configuration;
using Zeus.Projects;
using Zeus.Serializers;
using Zeus.UserInterface;
using MyGeneration;
using MyMeta;
using System.Xml;

namespace Zeus
{
    public static class SharedResources
    {
        public static System.Drawing.Bitmap Refresh16x16_1
        {
            get { return MyGeneration.Properties.Resources.Refresh16x16_1; }
        }
        public static System.Drawing.Bitmap Refresh16x16_2
        {
            get { return MyGeneration.Properties.Resources.Refresh16x16_2; }
        }
        public static System.Drawing.Bitmap Refresh16x16_3
        {
            get { return MyGeneration.Properties.Resources.Refresh16x16_3; }
        }
        public static System.Drawing.Bitmap Refresh16x16_4
        {
            get { return MyGeneration.Properties.Resources.Refresh16x16_4; }
        }
    }
    public class ZeusController : IZeusController
    {

        private static ZeusController _instance;
        public static ZeusController Instance 
        {
            get 
            {
                if (_instance == null) _instance = new ZeusController();
                return _instance;
            }
        }

        private ZeusController() { }

        public IZeusSavedInput CollectTemplateInput(IZeusContext context, string templatePath)
        {
            ZeusTemplate template = new ZeusTemplate(templatePath);
            ZeusSavedInput collectedInput = new ZeusSavedInput();
            DefaultSettings settings = DefaultSettings.Instance;

            settings.PopulateZeusContext(context);
            template.Collect(context, settings.ScriptTimeout, collectedInput.InputData.InputItems);
            collectedInput.Save();

            return collectedInput;
        }

        public IZeusSavedInput ExecuteTemplateAndCollectInput(IZeusContext context, string templatePath)
        {
            ZeusTemplate template = new ZeusTemplate(templatePath);
            ZeusSavedInput collectedInput = new ZeusSavedInput();
            DefaultSettings settings = DefaultSettings.Instance;

            settings.PopulateZeusContext(context);
            template.ExecuteAndCollect(context, settings.ScriptTimeout, collectedInput.InputData.InputItems);
            collectedInput.Save();

            return collectedInput;
        }

        public void ExecuteTemplate(IZeusContext context, string templateFilePath)
        {
            this.ExecuteTemplate(context, templateFilePath, null);
        }

        public void ExecuteTemplate(IZeusContext context, string templatePath, string inputFilePath)
        {
            ZeusTemplate template = new ZeusTemplate(templatePath);
            ZeusSavedInput savedInput = null;
            DefaultSettings settings = DefaultSettings.Instance;

            if (!string.IsNullOrEmpty(inputFilePath))
            {
                savedInput = new ZeusSavedInput(inputFilePath);
            }

            context.Log.Write("Executing: " + template.Title);
            if (savedInput != null)
            {
                context.Input.AddItems(savedInput.InputData.InputItems);
                template.Execute(context, settings.ScriptTimeout, true);
            }
            else
            {
                settings.PopulateZeusContext(context);
                template.Execute(context, settings.ScriptTimeout, false);
            }
        }

        public void ExecuteProject(IZeusContext context, string projectFilePath)
        {
            ExecuteProjectModule(context, projectFilePath);
        }

        public void ExecuteProjectModule(IZeusContext context, string projectFilePath, params string[] modules)
        {
            ZeusProject proj = new ZeusProject(projectFilePath);
            DefaultSettings settings = DefaultSettings.Instance;

            if (modules.Length == 0)
            {
                context.Log.Write("Executing: " + proj.Name);
                proj.Execute(settings.ScriptTimeout, context.Log);

            }
            else
            {
                foreach (string mod in modules)
                {
                    context.Log.Write("Executing: " + mod);
                    ExecuteModules(context, proj, new List<string>(modules), settings.ScriptTimeout);
                }
            }
        }

        private void ExecuteModules(IZeusContext context, ZeusModule parent, List<string> names, int timeout)
        {
            foreach (ZeusModule module in parent.ChildModules)
            {
                if (names.Contains(module.Name))
                {
                    module.Execute(timeout, context.Log);
                }
                else
                {
                    ExecuteModules(context, module, names, timeout);
                }
            }
        }

        public List<IAppRelease> ReleaseList
        {
            get
            {
                List<IAppRelease> releases = new List<IAppRelease>();
                XmlDocument xmldoc = Zeus.HttpTools.GetXmlFromUrl(DefaultSettings.Instance.VersionRSSUrl, DefaultSettings.Instance.WebProxy);
                XmlNodeList nodes = xmldoc.SelectNodes("//item");
                foreach (XmlNode node in nodes)
                {
                    AppRelease r = new AppRelease();

                    XmlNode titleNode = node.SelectSingleNode("./title");
                    XmlNode descriptionNode = node.SelectSingleNode("./description");
                    XmlNode linkNode = node.SelectSingleNode("./link");
                    XmlNode authorNode = node.SelectSingleNode("./author");
                    XmlNode pubDateNode = node.SelectSingleNode("./pubDate");

                    if (titleNode != null)
                    {
                        r.Title = titleNode.InnerText;
                    }
                    if (descriptionNode != null)
                    {
                        r.Description = descriptionNode.InnerText;
                    }
                    if (authorNode != null)
                    {
                        r.Author = authorNode.InnerText;
                    }
                    if (linkNode != null)
                    {
                        string versionInfo;
                        r.ReleaseNotesLink = new Uri(linkNode.InnerText);
                        r.DownloadLink = new Uri(FindSourceForgeDownloadUrl(r.Description, out versionInfo));
                        
                        int ii;
                        string verString = string.Empty;
                        if (int.TryParse(versionInfo, out ii))
                        {
                            //major.minor[.build[.revision]] 
                            if (versionInfo.Length > 0)
                            {
                                verString += versionInfo[0];
                                if (versionInfo.Length > 1)
                                {
                                    verString += "." + versionInfo[1];
                                    if (versionInfo.Length > 2)
                                    {
                                        verString += "." + versionInfo[2];
                                        if (versionInfo.Length > 3)
                                        {
                                            verString += "." + versionInfo.Substring(3);
                                        }
                                    }
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(verString))
                        {
                            r.AppVersion = new Version();
                        }
                        else
                        {
                            r.AppVersion = new Version(verString);
                        }
                    }
                    if (pubDateNode != null)
                    {
                        r.Date = DateTime.Parse(pubDateNode.InnerText);
                    }

                    releases.Add(r);
                }
                return releases;
            }
        }

        private string FindSourceForgeDownloadUrl(string description, out string version)
        {
            version = string.Empty;
            string newurl = string.Empty;
            //"<br />Released at Fri, 29 Aug 2008 07:55:10 GMT by komma8komma1<br />Includes files: mygeneration_1306_20080829.exe (4562185 bytes, 64 downloads to date)<br /><a href=\"http://sourceforge.net/project/showfiles.php?group_id=198893&package_id=249524&release_id=622773\">[Download]</a> <a href=\"http://sourceforge.net/project/shownotes.php?release_id=622773\">[Release Notes]</a>"
            int start = -1, end = description.IndexOf("\">[Download]</a>");
            if (end > 0)
            {
                start = (description.Substring(0, end)).LastIndexOf("<a href=\"") + 9;
            }
            if (start >= 9 && end > start)
            {
                newurl = description.Substring(start, (end - start));
            }

            string u = Zeus.HttpTools.GetTextFromUrl(newurl, DefaultSettings.Instance.WebProxy);
            int i = 0, i2 = 0, j, k, l;
            try
            {
                //<td ><a href="http://downloads.sourceforge.net/mygeneration/mygeneration_1306_20080829.exe?modtime=1219979144&amp;big_mirror=0" onClick="window.location='/project/downloading.php?group_id=198893&amp;use_mirror=voxel&amp;filename=mygeneration_1306_20080829.exe&amp;'+Math.floor(Math.random()*100000000); return false;">mygeneration_1306_20080829.exe</a>
                string tokenToFind = "http://downloads.sourceforge.net/mygeneration/";
                do
                {
                    i = u.IndexOf(tokenToFind, i);
                    if (i > 0)
                    {
                        j = u.IndexOf('\"', i);
                        if (j > i)
                        {
                            k = u.IndexOf(">", j);
                            if (k > j)
                            {
                                l = u.IndexOf("</a>", k);

                                string url = u.Substring(i, (j - i));
                                string filename = u.Substring(k + 1, (l - k-1));

                                i += tokenToFind.Length;
                                if (!filename.Contains("plugin") && filename.StartsWith("mygen") && filename.EndsWith(".exe"))
                                {
                                    newurl = url;

                                    string[] items = filename.Substring(0, filename.Length-4).Split('_');
                                    if ((items.Length == 3)|| (items.Length == 3))
                                    {
                                        version = items[1];
                                    }
                                    else if (items.Length == 2)
                                    {
                                        version = items[1];
                                    }
                                    else
                                    {
                                        version = filename;
                                    }

                                    break;
                                }
                            }
                        }
                    }
                } while (i >= 0);
            }
            catch (Exception ex)
            {
                // do something with the exception?
                throw ex;
            }
            return newurl;
        }

    }
    public class AppRelease : IAppRelease
    {
        private string _title, _description, _author;
        private Uri _downloadLink, _releaseNotesLink;
        private DateTime _date;
        private Version _version;

        public string Title { get { return _title;  } set { _title = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        public string Author { get { return _author; } set { _author = value; } }
        public Version AppVersion { get { return _version; } set { _version = value; } }
        public Uri DownloadLink { get { return _downloadLink; } set { _downloadLink = value; } }
        public Uri ReleaseNotesLink { get { return _releaseNotesLink; } set { _releaseNotesLink = value; } }
        public DateTime Date { get { return _date; } set { _date = value; } }
    }
}
