using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WpfTool.Entity;
using WpfTool.Util;

namespace WpfTool.CloudService;

public static class DeeplxHelper
{
    public static async Task<string> Translate(string text, string sourceLanguage, string targetLanguage)
    {
        try
        {
            var Url = GlobalConfig.Translate.Deeplx.Url;
            var Authorization = GlobalConfig.Translate.Deeplx.Authorization;

            var dict = new Dictionary<string, string>()
            {
                {"text", text},
                {"source_lang", sourceLanguage},
                {"target_lang", targetLanguage},
            };

            HttpContent content = JsonContent.Create(dict);

            Dictionary<string, string>? header = null;
            if (!string.IsNullOrEmpty(Authorization))
            {
                header = new Dictionary<string, string>()
                {
                    {"Authorization", Authorization},
                };
            }

            var response = await HttpHelper.PostAsync(Url, content, header);
            JObject jsonObj;
            try
            {
                jsonObj = JObject.Parse(response);
            }
            catch (Exception)
            {
                return response;
            }
            if (!jsonObj.ContainsKey("code") || !jsonObj["code"]!.ToString().Equals("200"))
            {
                return jsonObj.ToString();
            }

            return jsonObj["data"]!.ToString();
        }
        catch (Exception e)
        {
            return e.ToString();
        }
    }

}