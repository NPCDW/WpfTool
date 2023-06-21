using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WpfTool.CloudService
{
    internal class SpaceOCRHelper
    {
        private static String INVOKE_URL = "https://api.ocr.space/parse/image";

        public static async Task<string> ocr(Bitmap bmp, String ocrType = null, String ocrLanguage = null)
        {
            if (string.IsNullOrWhiteSpace(ocrType))
            {
                ocrType = GlobalConfig.Ocr.defaultOcrType;
            }
            if (string.IsNullOrWhiteSpace(ocrLanguage))
            {
                ocrType = GlobalConfig.Ocr.defaultOcrLanguage;
            }
            try
            {
                String base64 = Utils.BitmapToBase64String(bmp);

                Dictionary<String, String> dict = new Dictionary<String, String>();
                dict.Add("base64image", "data:image/jpeg;base64," + base64);
                dict.Add("apikey", GlobalConfig.Ocr.SpaceOCR.apiKey);
                dict.Add("language", ocrLanguage);
                dict.Add("OCREngine", ocrType.Replace("Engine", ""));

                HttpContent content = new FormUrlEncodedContent(dict);

                String response = await HttpHelper.PostAsync(INVOKE_URL, content);

                JObject jsonObj = JObject.Parse(response);
                if (jsonObj.ContainsKey("ErrorMessage"))
                {
                    return jsonObj["ErrorMessage"].ToString();
                }
                JToken[] jArray = jsonObj["ParsedResults"].ToArray();
                String text = "";
                foreach (JToken jToken in jArray)
                {
                    text += jToken["ParsedText"].ToString() + System.Environment.NewLine;
                }
                return text;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

    }
}
