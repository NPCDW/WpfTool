using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WpfTool
{
    public class BaiduAIHelper
    {
        private static String translateUrl = "https://fanyi-api.baidu.com/api/trans/vip/translate";
        private static String imageTranslateUrl = "https://fanyi-api.baidu.com/api/trans/sdk/picture";

        public static async Task<string> translate(String text, String sourceLanguage, String targetLanguage)
        {
            try
            {
                String salt = Guid.NewGuid().ToString("N");
                String signStr = GlobalConfig.Translate.BaiduAI.app_id + text + salt + GlobalConfig.Translate.BaiduAI.app_secret;
                String sign = Utils.Md5(signStr);

                Dictionary<String, String> dict = new Dictionary<String, String>();
                dict.Add("q", text);
                dict.Add("from", sourceLanguage);
                dict.Add("to", targetLanguage);
                dict.Add("appid", GlobalConfig.Translate.BaiduAI.app_id);
                dict.Add("salt", salt);
                dict.Add("sign", sign);

                HttpContent content = new FormUrlEncodedContent(dict);

                String response = await HttpHelper.PostAsync(translateUrl, content);

                JObject jsonObj = JObject.Parse(response);
                if (jsonObj.ContainsKey("error_code"))
                {
                    return jsonObj["error_code"].ToString();
                }
                JToken[] jArray = jsonObj["trans_result"].ToArray();
                String target = "";
                foreach (JToken jToken in jArray)
                {
                    target += jToken["dst"].ToString() + System.Environment.NewLine;
                }
                return target;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public static async Task<Dictionary<String, String>> screenshotTranslate(Bitmap bmp)
        {
            Dictionary<String, String> keyValues = new Dictionary<String, String>();
            try
            {
                int salt = new Random().Next(1, 10000000);
                String cuid = "APICUID";
                String mac = "mac";
                byte[] fileByteArray = Utils.BitmapToByteArray(bmp);
                String signStr = GlobalConfig.Translate.BaiduAI.app_id + Utils.Md5(fileByteArray) + salt + cuid + mac + GlobalConfig.Translate.BaiduAI.app_secret;
                String sign = Utils.Md5(signStr);

                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new ByteArrayContent(fileByteArray), "image", salt + ".jpg");
                content.Add(new StringContent(GlobalConfig.Translate.defaultTranslateSourceLanguage), "from");
                content.Add(new StringContent(GlobalConfig.Translate.defaultTranslateTargetLanguage), "to");
                content.Add(new StringContent(GlobalConfig.Translate.BaiduAI.app_id), "appid");
                content.Add(new StringContent(salt + ""), "salt");
                content.Add(new StringContent(cuid), "cuid");
                content.Add(new StringContent(mac), "mac");
                content.Add(new StringContent("3"), "version");
                content.Add(new StringContent(sign), "sign");

                String response = await HttpHelper.PostAsync(imageTranslateUrl, content);

                JObject jsonObj = JObject.Parse(response);
                if (jsonObj.ContainsKey("error_code") && !jsonObj["error_code"].ToString().Equals("0"))
                {
                    keyValues.Add("ocrText", jsonObj["error_code"].ToString() + " " + jsonObj["error_msg"].ToString());
                    keyValues.Add("translateText", "");
                    return keyValues;
                }
                JToken[] jArray = jsonObj["data"]["content"].ToArray();
                String ocrText = "";
                String translateText = "";
                foreach (JToken jToken in jArray)
                {
                    ocrText += jToken["src"].ToString() + System.Environment.NewLine;
                    translateText += jToken["dst"].ToString() + System.Environment.NewLine;
                }
                keyValues.Add("ocrText", ocrText);
                keyValues.Add("translateText", translateText);
                return keyValues;
            }
            catch (Exception e)
            {
                keyValues.Add("ocrText", e.ToString());
                keyValues.Add("translateText", "");
                return keyValues;
            }
        }

    }
}
