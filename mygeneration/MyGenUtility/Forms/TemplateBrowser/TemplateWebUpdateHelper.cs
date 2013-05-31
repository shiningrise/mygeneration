using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;

using Zeus;
using Zeus.Configuration;
using Zeus.UserInterface;
using Zeus.UserInterface.WinForms;
using MyMeta;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for TemplateWebUpdateHelper.
	/// </summary>
    public class TemplateWebUpdateHelper
    {
        static public ArrayList GetTempates(DirectoryInfo templatePathInfo)
        {
            string path = templatePathInfo.FullName;
            path += (path.EndsWith("\\") ? "" : "\\");

            string url = ZeusConfig.Current.WebUpdateUrl + "?action=list";

            XmlDocument xmldoc = Zeus.HttpTools.GetXmlFromUrl(url, DefaultSettings.Instance.WebProxy);
            XmlNodeList nodes = xmldoc.GetElementsByTagName("template");

            ArrayList templates = new ArrayList();
            foreach (XmlNode node in nodes)
            {
                string ns = node.Attributes["namespace"].Value;
                string pth = BuildFolder(path, ns) + node.Attributes["filename"].Value;

                string[] sa = new string[] {
											pth,
											node.Attributes["id"].Value,
											node.Attributes["title"].Value,
											node.Attributes["url"].Value,
											ns,
											node.Attributes["username"].Value,
											ZeusConstants.SourceTypes.SOURCE
				};

                templates.Add(sa);
            }

            return templates;
        }

        static public void WebUpdate(string id, string path, bool isGrouped)
        {
            CultureInfo culture = new CultureInfo("en-US");

            string url = ZeusConfig.Current.WebUpdateUrl + "?action=update&id=" + id;

            XmlDocument xmldoc = Zeus.HttpTools.GetXmlFromUrl(url, DefaultSettings.Instance.WebProxy);
            XmlNodeList nodes = xmldoc.GetElementsByTagName("template");

            if (nodes.Count > 0)
            {
                XmlNode node = nodes[0];
                DateTime updateDate = DateTime.MinValue;
                try
                {
                    updateDate = Convert.ToDateTime(node.Attributes["lastupdated"].Value, culture);
                }
                catch
                {
                    try
                    {
                        updateDate = Convert.ToDateTime(node.Attributes["lastupdated"].Value);
                    }
                    catch
                    {
                        updateDate = DateTime.MinValue;
                    }
                }

                long length = Convert.ToInt64(node.Attributes["length"].Value) + 3;
                byte[] bytes = Convert.FromBase64String(node.InnerText);

                FileInfo info = new FileInfo(path);
                long localFileSize = 0;
                DateTime localFileUpdateDate = DateTime.MinValue;
                if (info.Exists)
                {
                    localFileSize = info.Length;
                    localFileUpdateDate = info.LastWriteTime;
                }

                DialogResult result = DialogResult.Yes;
                if (!isGrouped && info.Exists)
                {
                    string msg = "The remote template was last updated on {0}\r\nand the local template was last updated on {1}.\r\nAre you sure you want to update?";

                    result = MessageBox.Show(String.Format(msg, updateDate, localFileUpdateDate),
                        "Update Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }

                if (result == DialogResult.Yes)
                {
                    MemoryStream memorystream = new MemoryStream(bytes);
                    ZeusTemplate template = new ZeusTemplate(new StreamReader(memorystream), path);
                    template.Save();

                    // Save related files with the template
                    FileInfo finfo = new FileInfo(path);
                    GetRelatedFiles(id, template.FilePath);
                }
            }
            else
            {
                nodes = xmldoc.GetElementsByTagName("error");

                if (nodes.Count > 0)
                {
                    XmlNode node = nodes[0];
                    string errorText = node.InnerText;

                    if (!isGrouped) MessageBox.Show(errorText);
                }
            }
        }

        static private void GetRelatedFiles(string id, string folder)
        {
            string filesurl = ZeusConfig.Current.WebUpdateUrl + "?action=listtemplatefiles&id=" + id;
            XmlDocument xmldoc = Zeus.HttpTools.GetXmlFromUrl(filesurl, DefaultSettings.Instance.WebProxy);
            XmlNodeList nodes = xmldoc.GetElementsByTagName("file");
            bool fileExistsAndIsDLL = false;
            string fileurl = string.Empty, filename = string.Empty, newfilename = string.Empty;
            FileInfo info = null;

            BinaryWriter writer = null;
            foreach (XmlNode node in nodes)
            {
                try
                {
                    fileurl = node.Attributes["url"].Value;
                    filename = node.Attributes["filename"].Value;

                    info = null;
                    if (filename.ToLower().EndsWith(".dll"))
                    {
                        info = new FileInfo(FileTools.ApplicationPath + "\\" + filename);
                        if (info.Exists) fileExistsAndIsDLL = true;
                    }
                    else
                    {
                        info = new FileInfo(folder + "\\" + filename);
                    }

                    FileStream stream = info.Create();
                    writer = new BinaryWriter(stream);
                    writer.Write(Zeus.HttpTools.GetBytesFromUrl(fileurl, DefaultSettings.Instance.WebProxy));
                    writer.Flush();
                    writer.Close();
                    writer = null;
                }
                catch (Exception ex)
                {
                    if (writer != null)
                    {
                        writer.Close();
                        writer = null;
                    }

                    if (fileExistsAndIsDLL)
                    {
                        try
                        {
                            string prefix = filename.Substring(0, filename.LastIndexOf("."));

                            newfilename = prefix + "$REPLACEMENT$.dll";

                            info = new FileInfo(FileTools.ApplicationPath + "\\" + filename);

                            FileStream stream = info.Create();
                            writer = new BinaryWriter(stream);
                            writer.Write(Zeus.HttpTools.GetBytesFromUrl(fileurl, DefaultSettings.Instance.WebProxy));
                            writer.Flush();
                            writer.Close();
                            writer = null;

                            MessageBox.Show(
                                string.Format("Error downloading file \"{0}\" from Template Library (Renamed new file to \"{1}\". Replacement will happen on application restart.): " + ex.Message, filename)
                                );
                        }
                        catch
                        {
                            if (writer != null)
                            {
                                writer.Close();
                                writer = null;
                            }

                            MessageBox.Show(
                                string.Format("Error downloading file \"{0}\" from Template Library (Tried rename with no luck): " + ex.Message, filename)
                                );
                        }

                    }
                    else
                    {
                        MessageBox.Show(
                            string.Format("Error downloading file \"{0}\" from Template Library: " + ex.Message, filename)
                            );
                    }
                }
            }
        }

        static private string BuildFolder(string basePath, string ns)
        {
            string returnVal = basePath;
            if (ns.Trim() != string.Empty)
            {
                returnVal = returnVal.TrimEnd('\\');

                string[] items = ns.Split('.');
                foreach (string item in items)
                {
                    item.Replace("\r", "").Replace("\n", "").Replace("\t", " ").Replace("\\", "_").Replace("/", "_")
                        .Replace(":", "_").Replace("?", "_").Replace(",", "_").Replace(";", "_").Replace(":", "_")
                        .Replace("<", "(").Replace(">", ")").Replace("\"", "_").Replace("'", "_").Replace("%", "")
                        .Replace("*", "-").Replace("&", "");

                    returnVal += "\\" + item;
                }
                returnVal += "\\";

                DirectoryInfo info = new DirectoryInfo(returnVal);
                if (!info.Exists)
                {
                    info.Create();
                }
            }
            return returnVal;
        }
    }
}
