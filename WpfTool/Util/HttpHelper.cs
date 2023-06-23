using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WpfTool.Util.HappyEyeballsHttp;

namespace WpfTool
{
    public class HttpHelper
    {
        private static SocketsHttpHandler handler = new SocketsHttpHandler
        {
            AutomaticDecompression = DecompressionMethods.All,
            ConnectCallback = new HappyEyeballsCallback().ConnectCallback,
        };
        private static HttpClient httpClient = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(5),
        };

        private static String USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 Edg/108.0.1462.54";

        public static async Task<string> GetAsync(string url, Dictionary<String, String>? headers = null)
        {
            HttpRequestMessage message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri(url);
            message.Headers.Date = DateTime.Now;
            message.Headers.UserAgent.ParseAdd(USER_AGENT);
            if (headers != null && headers.Count > 0)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    message.Headers.Add(header.Key, header.Value);
                }
            }

            using HttpResponseMessage response = await httpClient.SendAsync(message);

            if (response == null)
            {
                return "HttpClient get response is null";
            }
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return "HttpClient response fail, " + response.StatusCode + response.ReasonPhrase;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }

        public static async Task<string> PostAsync(string url, HttpContent content, Dictionary<String, String>? headers = null)
        {
            HttpRequestMessage message = new HttpRequestMessage();
            message.Method = HttpMethod.Post;
            message.RequestUri = new Uri(url);
            message.Headers.Date = DateTime.Now;
            message.Headers.UserAgent.ParseAdd(USER_AGENT);
            if (headers != null && headers.Count > 0)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    message.Headers.Add(header.Key, header.Value);
                }
            }
            message.Content = content;

            using HttpResponseMessage response = await httpClient.SendAsync(message);

            if (response == null)
            {
                return "HttpClient get response is null";
            }
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return "HttpClient response fail, " + response.StatusCode + response.ReasonPhrase;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }

    }
}
