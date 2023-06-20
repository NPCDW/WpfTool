using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WpfTool
{
    public class HttpHelper
    {
        private static String USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 Edg/108.0.1462.54";

        public static string Get(string url, Dictionary<String, String>? headers = null)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Date = DateTime.Now;
                request.Method = "GET";
                request.Timeout = 5000;
                request.UserAgent = USER_AGENT;
                if (headers != null && headers.Count > 0)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        request.Headers.Set(header.Key, header.Value);
                    }
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                if (response != null)
                {
                    Console.WriteLine("Error code: {0}", response.StatusCode);

                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
                    {
                        string text = reader.ReadToEnd();

                        throw new Exception(text);
                    }
                }
                else
                {
                    throw ex;
                }
            }
        }

        public static string Post(string url, string body, Dictionary<String, String>? headers = null)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = 5000;
                request.ContentType = "application/json";
                request.UserAgent = USER_AGENT;
                if (headers != null && headers.Count > 0)
                {
                    if (headers.ContainsKey("Content-Type"))
                    {
                        request.ContentType = headers["Content-Type"];
                        headers.Remove("Content-Type");
                    }
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        request.Headers.Set(header.Key, header.Value);
                    }
                }

                byte[] buffer = Encoding.UTF8.GetBytes(body);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                if (response != null)
                {
                    Console.WriteLine("Error code: {0}", response.StatusCode);

                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
                    {
                        string text = reader.ReadToEnd();

                        throw new Exception(text);
                    }
                }
                else
                {
                    throw ex;
                }
            }
        }

        public static string Upload(String url, String fileFieldName, byte[] fileContentBytes, String filename, Dictionary<String, String>? param = null, Dictionary<String, String>? headers = null)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            request.Timeout = 5000;

            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes(System.Environment.NewLine + "--" + boundary + System.Environment.NewLine);
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes(System.Environment.NewLine + "--" + boundary + "--" + System.Environment.NewLine);

            byte[] fileHeaderBytes = Encoding.UTF8.GetBytes(
                "Content-Disposition:form-data;name=\"" + fileFieldName + "\";filename=\""+ filename + "\"" + System.Environment.NewLine
                + "Content-Type:application/octet-stream" + System.Environment.NewLine
                + System.Environment.NewLine
            );

            Stream postStream = request.GetRequestStream();
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
            postStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);
            postStream.Write(fileContentBytes, 0, fileContentBytes.Length);
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            postStream.Close();

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

    }
}
