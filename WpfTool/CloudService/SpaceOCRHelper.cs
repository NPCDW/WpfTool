using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;

namespace WpfTool.CloudService
{
    internal class SpaceOCRHelper
    {
        private static String INVOKE_URL = "https://api.ocr.space/parse/image";

        public static String ocr(Bitmap bmp, String ocrType = null, String ocrLanguage = null)
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
                String body = "base64image=data:image/jpeg;base64," + HttpUtility.UrlEncode(base64, Encoding.UTF8)
                    + "&apikey=" + GlobalConfig.Ocr.SpaceOCR.apiKey
                    + "&language=" + ocrLanguage
                    + "&OCREngine=" + ocrType.Replace("Engine", "");
                Dictionary<String, String> headers = new Dictionary<String, String>();
                headers.Add("Content-Type", "application/x-www-form-urlencoded");

                String response = HttpHelper.Post(INVOKE_URL, body, headers);

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
