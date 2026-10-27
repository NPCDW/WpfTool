using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WpfTool.Util.HappyEyeballsHttp;

namespace WpfTool.Util;

public static class HttpHelper
{
    private const string UserAgent =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 Edg/108.0.1462.54";

    private static readonly SocketsHttpHandler Handler = new()
    {
        AutomaticDecompression = DecompressionMethods.All,
        ConnectCallback = new HappyEyeballsCallback().ConnectCallback
    };

    private static readonly HttpClient Client = new(Handler)
    {
        Timeout = TimeSpan.FromSeconds(5)
    };

    public static async Task<string> GetAsync(string url, Dictionary<string, string>? headers = null)
    {
        var message = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url),
            Headers =
            {
                Date = DateTime.Now
            }
        };
        message.Headers.UserAgent.ParseAdd(UserAgent);
        if (headers?.Count > 0)
            foreach (var header in headers)
                message.Headers.Add(header.Key, header.Value);

        using var response = await Client.SendAsync(message);

        if (response.StatusCode != HttpStatusCode.OK)
            return "HttpClient response fail, " + response.StatusCode + response.ReasonPhrase;

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return jsonResponse;
    }

    public static async Task<string> PostAsync(string url, HttpContent content,
        Dictionary<string, string>? headers = null)
    {
        var message = new HttpRequestMessage();
        message.Method = HttpMethod.Post;
        message.RequestUri = new Uri(url);
        message.Headers.Date = DateTime.Now;
        message.Headers.UserAgent.ParseAdd(UserAgent);
        if (headers != null && headers.Count > 0)
            foreach (var header in headers)
                message.Headers.Add(header.Key, header.Value);
        message.Content = content;

        using var response = await Client.SendAsync(message);

        if (response.StatusCode != HttpStatusCode.OK)
            return "HttpClient response fail, " + response.StatusCode + response.ReasonPhrase;

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return jsonResponse;
    }
}