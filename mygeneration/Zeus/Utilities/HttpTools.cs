using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;

namespace Zeus
{
    public static class HttpTools
    {
        public static string GetTextFromUrl(string url, WebProxy proxy)
        {
            string s;

            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            if (proxy != null) request.Proxy = proxy;

            request.Timeout = 15 * 1000;

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
                s = reader.ReadToEnd();
                reader.Close();
            }
            else
            {
                s = string.Empty;
            }

            return s;
        }

        public static XmlDocument GetXmlFromUrl(string url, WebProxy proxy)
        {
            XmlDocument xmldoc = new XmlDocument();

            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            if (proxy != null) request.Proxy = proxy;

            request.Timeout = 15 * 1000;

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
                xmldoc.LoadXml(reader.ReadToEnd());
                reader.Close();
            }
            else
            {
                xmldoc = null;
                throw new Exception(response.StatusCode.ToString());
            }

            return xmldoc;
        }

        public static byte[] GetBytesFromUrl(string url, WebProxy proxy) 
		{
			byte[] bytes = null;

			HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            if (proxy != null)  request.Proxy = proxy;
            
			request.Timeout = 15 * 1000;

			HttpWebResponse response = request.GetResponse() as HttpWebResponse;

			if (response.StatusCode == HttpStatusCode.OK) 
			{
				BinaryReader reader = new BinaryReader(response.GetResponseStream());				
				bytes = reader.ReadBytes(Convert.ToInt32(response.ContentLength));
				reader.Close();
				reader = null;
			}
			else 
			{
				throw new Exception(response.StatusCode.ToString());
			}

			return bytes;
		}
	}
}
