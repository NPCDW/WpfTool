using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WpfTool
{
    public class BaiduCloudHelper
    {
        private static String baseUrl = "https://aip.baidubce.com";

        public static async Task<string> ocr(Bitmap bmp, String ocrType = null)
        {
            if (string.IsNullOrWhiteSpace(ocrType))
            {
                ocrType = GlobalConfig.Ocr.defaultOcrType;
            }
            try
            {
                string access_token = await GetAccessToken();
                String url = baseUrl + "/rest/2.0/ocr/v1/" + ocrType + "?access_token=" + access_token;
                String base64 = Utils.BitmapToBase64String(bmp);

                Dictionary<String, String> dict = new Dictionary<String, String>();
                dict.Add("image", base64);

                HttpContent content = new FormUrlEncodedContent(dict);

                String response = await HttpHelper.PostAsync(url, content);

                JObject jsonObj = JObject.Parse(response);
                if (jsonObj.ContainsKey("error"))
                {
                    return jsonObj["error"].ToString() + " " + jsonObj["error_description"].ToString();
                }
                JToken[] jArray = jsonObj["words_result"].ToArray();
                String text = "";
                foreach (JToken jToken in jArray)
                {
                    text += jToken["words"].ToString() + System.Environment.NewLine;
                }
                return text;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        private static async Task<string> GetAccessToken()
        {
            if (DateTime.Now.AddDays(1) < GlobalConfig.Ocr.BaiduCloud.access_token_expires_time)
            {
                return GlobalConfig.Ocr.BaiduCloud.access_token;
            }

            String url = baseUrl + "/oauth/2.0/token";

            Dictionary<String, String> dict = new Dictionary<String, String>();
            dict.Add("grant_type", "client_credentials");
            dict.Add("client_id", GlobalConfig.Ocr.BaiduCloud.client_id);
            dict.Add("client_secret", GlobalConfig.Ocr.BaiduCloud.client_secret);

            HttpContent content = new FormUrlEncodedContent(dict);

            String response = await HttpHelper.PostAsync(url, content);

            JObject jsonObj = JObject.Parse(response);
            String access_token = jsonObj["access_token"].ToString();
            String expires_in = jsonObj["expires_in"].ToString();

            GlobalConfig.Ocr.BaiduCloud.access_token = access_token;
            GlobalConfig.Ocr.BaiduCloud.access_token_expires_time = DateTime.Now.AddSeconds(Convert.ToInt32(expires_in));
            GlobalConfig.SaveConfig();

            return access_token;
        }

    }
}
