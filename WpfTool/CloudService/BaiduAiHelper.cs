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

public static class BaiduAiHelper
{
    private const string TranslateUrl = "https://fanyi-api.baidu.com/api/trans/vip/translate";
    private const string ImageTranslateUrl = "https://fanyi-api.baidu.com/api/trans/sdk/picture";

    public static async Task<string> Translate(string text, string sourceLanguage, string targetLanguage)
    {
        try
        {
            var salt = Guid.NewGuid().ToString("N");
            var signStr = GlobalConfig.Translate.BaiduAi.AppId + text + salt +
                          GlobalConfig.Translate.BaiduAi.AppSecret;
            var sign = Utils.Md5(signStr);

            var dict = new Dictionary<string, string>
            {
                { "q", text },
                { "from", sourceLanguage },
                { "to", targetLanguage },
                { "appid", GlobalConfig.Translate.BaiduAi.AppId },
                { "salt", salt },
                { "sign", sign }
            };

            HttpContent content = new FormUrlEncodedContent(dict);

            var response = await HttpHelper.PostAsync(TranslateUrl, content);

            var jsonObj = JObject.Parse(response);
            if (jsonObj.TryGetValue("error_code", out var errorCode)) return errorCode.ToString();

            var jArray = jsonObj["trans_result"]!.ToArray();
            return jArray.Aggregate("",
                (current, t) => current + t["dst"]! + Environment.NewLine);
        }
        catch (Exception e)
        {
            return e.ToString();
        }
    }

    public static async Task<Dictionary<string, string>> ScreenshotTranslate(Bitmap bmp)
    {
        var keyValues = new Dictionary<string, string>();
        try
        {
            var salt = new Random().Next(1, 10000000);
            var cuid = "APICUID";
            var mac = "mac";
            var fileByteArray = Utils.BitmapToByteArray(bmp);
            var signStr = GlobalConfig.Translate.BaiduAi.AppId + Utils.Md5(fileByteArray) + salt + cuid + mac +
                          GlobalConfig.Translate.BaiduAi.AppSecret;
            var sign = Utils.Md5(signStr);

            var content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(fileByteArray), "image", salt + ".jpg");
            content.Add(new StringContent(GlobalConfig.Translate.DefaultTranslateSourceLanguage), "from");
            content.Add(new StringContent(GlobalConfig.Translate.DefaultTranslateTargetLanguage), "to");
            content.Add(new StringContent(GlobalConfig.Translate.BaiduAi.AppId), "appid");
            content.Add(new StringContent(salt + ""), "salt");
            content.Add(new StringContent(cuid), "cuid");
            content.Add(new StringContent(mac), "mac");
            content.Add(new StringContent("3"), "version");
            content.Add(new StringContent(sign), "sign");

            var response = await HttpHelper.PostAsync(ImageTranslateUrl, content);

            var jsonObj = JObject.Parse(response);
            if (jsonObj.ContainsKey("error_code") && !jsonObj["error_code"]!.ToString().Equals("0"))
            {
                keyValues.Add("ocrText",
                    jsonObj["error_code"]! + " " + jsonObj["error_msg"]!);
                keyValues.Add("translateText", "");
                return keyValues;
            }

            var jArray = jsonObj["data"]!["content"]!.ToArray();
            var ocrText = "";
            var translateText = "";
            foreach (var jToken in jArray)
            {
                ocrText += jToken["src"]! + Environment.NewLine;
                translateText += jToken["dst"]! + Environment.NewLine;
            }

            keyValues.Add("ocrText", ocrText);
            keyValues.Add("translateText", translateText);
            return keyValues;
        }
        catch (Exception e)
        {
            keyValues.Add("ocrText", e.ToString());
            keyValues.Add("translateText", e.ToString());
            return keyValues;
        }
    }
}