using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Ocr.V20181119;
using TencentCloud.Ocr.V20181119.Models;
using TencentCloud.Tmt.V20180321;
using TencentCloud.Tmt.V20180321.Models;

namespace WpfTool
{
    public class TencentCloudHelper
    {
        public static async Task<string> translate(String text, String sourceLanguage, String targetLanguage)
        {
            try
            {
                Credential cred = new Credential
                {
                    SecretId = GlobalConfig.Translate.TencentCloud.secret_id,
                    SecretKey = GlobalConfig.Translate.TencentCloud.secret_key
                };

                ClientProfile clientProfile = new ClientProfile();
                HttpProfile httpProfile = new HttpProfile();
                httpProfile.Endpoint = ("tmt.tencentcloudapi.com");
                clientProfile.HttpProfile = httpProfile;

                TmtClient client = new TmtClient(cred, "ap-beijing", clientProfile);
                TextTranslateRequest req = new TextTranslateRequest();
                req.SourceText = text;
                req.Source = sourceLanguage;
                req.Target = targetLanguage;
                req.ProjectId = 0;

                TextTranslateResponse resp = await client.TextTranslate(req);
                String jsonStr = AbstractModel.ToJsonString(resp);
                JObject jsonObj = JObject.Parse(jsonStr);
                return jsonObj["TargetText"].ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public static async Task<string> ocr(Bitmap bmp, String ocrTypeStr = null)
        {
            try
            {
                GlobalConfig.Ocr.TencentCloud.OcrTypeEnum ocrType;
                if (string.IsNullOrWhiteSpace(ocrTypeStr))
                {
                    ocrType = (GlobalConfig.Ocr.TencentCloud.OcrTypeEnum)Enum.Parse(typeof(GlobalConfig.Ocr.TencentCloud.OcrTypeEnum), GlobalConfig.Ocr.defaultOcrType);
                }
                else
                {
                    ocrType = (GlobalConfig.Ocr.TencentCloud.OcrTypeEnum)Enum.Parse(typeof(GlobalConfig.Ocr.TencentCloud.OcrTypeEnum), ocrTypeStr);
                }

                Credential cred = new Credential
                {
                    SecretId = GlobalConfig.Ocr.TencentCloud.secret_id,
                    SecretKey = GlobalConfig.Ocr.TencentCloud.secret_key
                };

                ClientProfile clientProfile = new ClientProfile();
                HttpProfile httpProfile = new HttpProfile();
                httpProfile.Endpoint = ("ocr.tencentcloudapi.com");
                clientProfile.HttpProfile = httpProfile;

                OcrClient client = new OcrClient(cred, "ap-beijing", clientProfile);
                String jsonStr = "{}";
                String base64 = Utils.BitmapToBase64String(bmp);
                if (ocrType == GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralBasicOCR)
                {
                    GeneralBasicOCRRequest req = new GeneralBasicOCRRequest();
                    req.ImageBase64 = base64;
                    GeneralBasicOCRResponse resp = await client.GeneralBasicOCR(req);
                    jsonStr = AbstractModel.ToJsonString(resp);
                }
                else if (ocrType == GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralAccurateOCR)
                {
                    GeneralAccurateOCRRequest req = new GeneralAccurateOCRRequest();
                    req.ImageBase64 = base64;
                    GeneralAccurateOCRResponse resp = await client.GeneralAccurateOCR(req);
                    jsonStr = AbstractModel.ToJsonString(resp);
                }
                else if (ocrType == GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralHandwritingOCR)
                {
                    GeneralHandwritingOCRRequest req = new GeneralHandwritingOCRRequest();
                    req.ImageBase64 = base64;
                    GeneralHandwritingOCRResponse resp = await client.GeneralHandwritingOCR(req);
                    jsonStr = AbstractModel.ToJsonString(resp);
                }
                JObject jsonObj = JObject.Parse(jsonStr);
                JToken[] jArray = jsonObj["TextDetections"].ToArray();
                String text = "";
                foreach (JToken jToken in jArray)
                {
                    text += jToken["DetectedText"].ToString() + System.Environment.NewLine;
                }
                return text;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public static async Task<Dictionary<String, String>> screenshotTranslate(Bitmap bmp)
        {
            try
            {
                Credential cred = new Credential
                {
                    SecretId = GlobalConfig.Translate.TencentCloud.secret_id,
                    SecretKey = GlobalConfig.Translate.TencentCloud.secret_key
                };

                ClientProfile clientProfile = new ClientProfile();
                HttpProfile httpProfile = new HttpProfile();
                httpProfile.Endpoint = ("tmt.tencentcloudapi.com");
                clientProfile.HttpProfile = httpProfile;

                TmtClient client = new TmtClient(cred, "ap-beijing", clientProfile);
                ImageTranslateRequest req = new ImageTranslateRequest();
                req.Data = Utils.BitmapToBase64String(bmp);
                req.SessionUuid = System.Guid.NewGuid().ToString();
                req.Scene = "doc";
                req.Source = GlobalConfig.Translate.defaultTranslateSourceLanguage;
                req.Target = GlobalConfig.Translate.defaultTranslateTargetLanguage;
                req.ProjectId = 0;
                ImageTranslateResponse resp = await client.ImageTranslate(req);
                String jsonStr = AbstractModel.ToJsonString(resp);
                JObject jsonObj = JObject.Parse(jsonStr);
                JToken[] jArray = jsonObj["ImageRecord"]["Value"].ToArray();
                String ocrText = "";
                String translateText = "";
                foreach (JToken jToken in jArray)
                {
                    ocrText += jToken["SourceText"].ToString() + System.Environment.NewLine;
                    translateText += jToken["TargetText"].ToString() + System.Environment.NewLine;
                }
                Dictionary<String, String> keyValues = new Dictionary<String, String>();
                keyValues.Add("ocrText", ocrText);
                keyValues.Add("translateText", translateText);
                return keyValues;
            }
            catch (Exception e)
            {
                Dictionary<String, String> keyValues = new Dictionary<String, String>();
                keyValues.Add("ocrText", e.ToString());
                keyValues.Add("translateText", "");
                return keyValues;
            }
        }

    }
}
