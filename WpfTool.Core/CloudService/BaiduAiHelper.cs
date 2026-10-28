using System;
using System.Collections.Generic;
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

}
