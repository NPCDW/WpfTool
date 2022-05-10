using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WpfTool
{
    public class BaiduAIHelper
    {
        private static String translateUrl = "https://fanyi-api.baidu.com/api/trans/vip/translate";
        private static String imageTranslateUrl = "https://fanyi-api.baidu.com/api/trans/sdk/picture";

        public static String translate(String text, String sourceLanguage, String targetLanguage)
        {
            try
            {
                String salt = Guid.NewGuid().ToString("N");
                String signStr = GlobalConfig.BaiduAI.app_id + text + salt + GlobalConfig.BaiduAI.app_secret;
                String sign = Utils.Md5(signStr);

                String body = "q=" + HttpUtility.UrlEncode(text, Encoding.UTF8)
                    + "&from=" + sourceLanguage
                    + "&to=" + targetLanguage
                    + "&appid=" + GlobalConfig.BaiduAI.app_id
                    + "&salt=" + salt
                    + "&sign=" + sign;
                Dictionary<String, String> headers = new Dictionary<String, String>();
                headers.Add("Content-Type", "application/x-www-form-urlencoded");

                String response = HttpHelper.Post(translateUrl, body, headers);

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

        public static Dictionary<String, String> screenshotTranslate(Bitmap bmp)
        {
            Dictionary<String, String> keyValues = new Dictionary<String, String>();
            try
            {
                int salt = new Random().Next(1, 10000000);
                String cuid = "APICUID";
                String mac = "mac";
                byte[] fileByteArray = Utils.BitmapToByteArray(bmp);
                String signStr = GlobalConfig.BaiduAI.app_id + Utils.Md5(fileByteArray) + salt + cuid + mac + GlobalConfig.BaiduAI.app_secret;
                String sign = Utils.Md5(signStr);

                String url = imageTranslateUrl
                    + "?from=" + GlobalConfig.Common.defaultTranslateSourceLanguage
                    + "&to=" + GlobalConfig.Common.defaultTranslateTargetLanguage
                    + "&appid=" + GlobalConfig.BaiduAI.app_id
                    + "&salt=" + salt
                    + "&cuid=" + cuid
                    + "&mac=" + mac
                    + "&version=3"
                    + "&sign=" + sign;

                String response = HttpHelper.Upload(url, "image", fileByteArray, salt + ".jpg");

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
