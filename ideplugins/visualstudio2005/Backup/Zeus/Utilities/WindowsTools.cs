using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;

namespace Zeus
{
    public static class WindowsTools
    {

        #region Browser launch code
        public static void LaunchFile(string filename)
        {
            Process p = new Process();
            p.StartInfo.FileName = filename;
            p.Start();
        }

        public static void LaunchBrowser(string url)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = DefaultBrowser;
                p.StartInfo.Arguments = url;
                p.Start();
            }
            catch
            {
                System.Diagnostics.Process.Start(url);
            }
        }

        public static void LaunchBrowser(string url, ProcessWindowStyle windowStyle, bool createNoWindow)
        {
            try
            {
                try
                {
                    Process p = new Process();
                    p.StartInfo.FileName = DefaultBrowser;
                    p.StartInfo.Arguments = "\"" + url + "\"";
                    p.StartInfo.CreateNoWindow = createNoWindow;
                    p.StartInfo.WindowStyle = windowStyle;
                    p.Start();
                }
                catch { }
            }
            catch
            {
                try
                {
                    Process p = new Process();
                    p.StartInfo.FileName = url;
                    p.StartInfo.CreateNoWindow = createNoWindow;
                    p.StartInfo.WindowStyle = windowStyle;
                    p.Start();
                }
                catch { }
            }
        }

        public static void LaunchHelpFile(string uri, ProcessWindowStyle windowStyle, bool createNoWindow)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = uri;
                p.StartInfo.CreateNoWindow = createNoWindow;
                p.StartInfo.WindowStyle = windowStyle;
                p.Start();
            }
            catch { }
        }

        private static string s_browser = string.Empty;

        private static string DefaultBrowser
        {
            get
            {
                if (s_browser == string.Empty)
                {
                    s_browser = getDefaultBrowser();
                }
                return s_browser;
            }
        }

        private static string getDefaultBrowser()
        {
            string browser = string.Empty;
            RegistryKey key = null;
            try
            {
                key = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false);

                //trim off quotes
                browser = key.GetValue(null).ToString().ToLower().Replace("\"", "");
                if (!browser.EndsWith("exe"))
                {
                    //get rid of everything after the ".exe"
                    browser = browser.Substring(0, browser.LastIndexOf(".exe") + 4);
                }
            }
            finally
            {
                if (key != null) key.Close();
            }
            return browser;
        }
        #endregion
    }
}
