using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WpfTool.Entity;
using WpfTool.Util;

namespace WpfTool.CloudService;

public static class BaiduCloudHelper
{
    private const string BaseUrl = "https://aip.baidubce.com";

    public static async Task<string> Ocr(Bitmap bmp, string? ocrType = null)
    {
        if (string.IsNullOrWhiteSpace(ocrType)) ocrType = GlobalConfig.Ocr.DefaultOcrType;
        try
        {
            var accessToken = await GetAccessToken();
            var url = BaseUrl + "/rest/2.0/ocr/v1/" + ocrType + "?access_token=" + accessToken;
            var base64 = Utils.BitmapToBase64String(bmp);

            var dict = new Dictionary<string, string>
            {
                { "image", base64 }
            };

            HttpContent content = new FormUrlEncodedContent(dict);

            var response = await HttpHelper.PostAsync(url, content);

            var jsonObj = JObject.Parse(response);
            if (jsonObj.TryGetValue("error", out var error)) return error + " " + jsonObj["error_description"];
            var jArray = jsonObj["words_result"]!.ToArray();
            return jArray.Aggregate("", (current, t) => current + t["words"]! + Environment.NewLine);
        }
        catch (Exception e)
        {
            return e.ToString();
        }
    }

    private static async Task<string> GetAccessToken()
    {
        if (DateTime.Now.AddDays(1) < GlobalConfig.Ocr.BaiduCloud.AccessTokenExpiresTime)
            return GlobalConfig.Ocr.BaiduCloud.AccessToken;

        const string url = BaseUrl + "/oauth/2.0/token";

        var dict = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", GlobalConfig.Ocr.BaiduCloud.ClientId },
            { "client_secret", GlobalConfig.Ocr.BaiduCloud.ClientSecret }
        };

        HttpContent content = new FormUrlEncodedContent(dict);

        var response = await HttpHelper.PostAsync(url, content);

        var jsonObj = JObject.Parse(response);
        var accessToken = jsonObj["access_token"]!.ToString();
        var expiresIn = jsonObj["expires_in"]!.ToString();

        GlobalConfig.Ocr.BaiduCloud.AccessToken = accessToken;
        GlobalConfig.Ocr.BaiduCloud.AccessTokenExpiresTime = DateTime.Now.AddSeconds(Convert.ToInt32(expiresIn));
        GlobalConfig.SaveConfig();

        return accessToken;
    }
}