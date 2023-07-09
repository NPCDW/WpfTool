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

internal static class SpaceOcrHelper
{
    private const string InvokeUrl = "https://api.ocr.space/parse/image";

    public static async Task<string> Ocr(Bitmap bmp, string? ocrType = null, string? ocrLanguage = null)
    {
        if (string.IsNullOrWhiteSpace(ocrType)) ocrType = GlobalConfig.Ocr.DefaultOcrType;
        if (string.IsNullOrWhiteSpace(ocrLanguage)) ocrLanguage = GlobalConfig.Ocr.DefaultOcrLanguage;
        try
        {
            var base64 = Utils.BitmapToBase64String(bmp);

            var dict = new Dictionary<string, string>()
            {
                {"base64image", "data:image/jpeg;base64," + base64},
                {"apikey", GlobalConfig.Ocr.SpaceOcr.ApiKey},
                {"language", ocrLanguage},
                {"OCREngine", ocrType.Replace("Engine", "")},
            };

            var content = new FormUrlEncodedContent(dict);

            var response = await HttpHelper.PostAsync(InvokeUrl, content);

            var jsonObj = JObject.Parse(response);
            if (jsonObj.TryGetValue("ErrorMessage", out var errorMessage)) return errorMessage.ToString();
            var jArray = jsonObj["ParsedResults"]!.ToArray();
            return jArray.Aggregate("", (current, t) => current + t["ParsedText"]! + Environment.NewLine);
        }
        catch (Exception e)
        {
            return e.ToString();
        }
    }
}