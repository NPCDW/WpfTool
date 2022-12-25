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
    public class BaiduCloudHelper
    {
        private static String baseUrl = "https://aip.baidubce.com";

        public static String ocr(Bitmap bmp, String ocrType = null)
        {
            if (string.IsNullOrWhiteSpace(ocrType))
            {
                ocrType = GlobalConfig.Ocr.defaultOcrType;
            }
            try
            {
                String url = baseUrl + "/rest/2.0/ocr/v1/" + ocrType + "?access_token=" + GetAccessToken();
                String base64 = Utils.BitmapToBase64String(bmp);
                String body = "image=" + HttpUtility.UrlEncode(base64, Encoding.UTF8);
                Dictionary<String, String> headers = new Dictionary<String, String>();
                headers.Add("Content-Type", "application/x-www-form-urlencoded");

                String response = HttpHelper.Post(url, body, headers);

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

        private static String GetAccessToken()
        {
            if (DateTime.Now.AddDays(1) < GlobalConfig.Ocr.BaiduCloud.access_token_expires_time)
            {
                return GlobalConfig.Ocr.BaiduCloud.access_token;
            }

            String url = baseUrl + "/oauth/2.0/token";
            String body = "grant_type=client_credentials&client_id=" + GlobalConfig.Ocr.BaiduCloud.client_id
                + "&client_secret=" + GlobalConfig.Ocr.BaiduCloud.client_secret;
            Dictionary<String, String> headers = new Dictionary<String, String>();
            headers.Add("Content-Type", "application/x-www-form-urlencoded");
            
            String response = HttpHelper.Post(url, body, headers);

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
