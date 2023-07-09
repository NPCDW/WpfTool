using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Ocr.V20181119;
using TencentCloud.Ocr.V20181119.Models;
using TencentCloud.Tmt.V20180321;
using TencentCloud.Tmt.V20180321.Models;
using WpfTool.Entity;
using WpfTool.Util;

namespace WpfTool.CloudService;

public static class TencentCloudHelper
{
    public static async Task<string> Translate(string text, string sourceLanguage, string targetLanguage)
    {
        try
        {
            var cred = new Credential
            {
                SecretId = GlobalConfig.Translate.TencentCloud.SecretId,
                SecretKey = GlobalConfig.Translate.TencentCloud.SecretKey
            };

            var clientProfile = new ClientProfile();
            var httpProfile = new HttpProfile
            {
                Endpoint = "tmt.tencentcloudapi.com"
            };
            clientProfile.HttpProfile = httpProfile;

            var client = new TmtClient(cred, "ap-beijing", clientProfile);
            var req = new TextTranslateRequest
            {
                SourceText = text,
                Source = sourceLanguage,
                Target = targetLanguage,
                ProjectId = 0
            };

            var resp = await client.TextTranslate(req);
            var jsonStr = AbstractModel.ToJsonString(resp);
            var jsonObj = JObject.Parse(jsonStr);
            return jsonObj["TargetText"]!.ToString();
        }
        catch (Exception e)
        {
            return e.ToString();
        }
    }

    public static async Task<string> Ocr(Bitmap bmp, string? ocrTypeStr = null)
    {
        try
        {
            GlobalConfig.Ocr.TencentCloud.OcrTypeEnum ocrType;
            if (string.IsNullOrWhiteSpace(ocrTypeStr))
                ocrType = (GlobalConfig.Ocr.TencentCloud.OcrTypeEnum)Enum.Parse(
                    typeof(GlobalConfig.Ocr.TencentCloud.OcrTypeEnum), GlobalConfig.Ocr.DefaultOcrType);
            else
                ocrType = (GlobalConfig.Ocr.TencentCloud.OcrTypeEnum)Enum.Parse(
                    typeof(GlobalConfig.Ocr.TencentCloud.OcrTypeEnum), ocrTypeStr);

            var cred = new Credential
            {
                SecretId = GlobalConfig.Ocr.TencentCloud.SecretId,
                SecretKey = GlobalConfig.Ocr.TencentCloud.SecretKey
            };

            var clientProfile = new ClientProfile();
            var httpProfile = new HttpProfile
            {
                Endpoint = "ocr.tencentcloudapi.com"
            };
            clientProfile.HttpProfile = httpProfile;

            var client = new OcrClient(cred, "ap-beijing", clientProfile);
            var jsonStr = "{}";
            var base64 = Utils.BitmapToBase64String(bmp);
            if (ocrType == GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralBasicOcr)
            {
                var req = new GeneralBasicOCRRequest();
                req.ImageBase64 = base64;
                var resp = await client.GeneralBasicOCR(req);
                jsonStr = AbstractModel.ToJsonString(resp);
            }
            else if (ocrType == GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralAccurateOcr)
            {
                var req = new GeneralAccurateOCRRequest();
                req.ImageBase64 = base64;
                var resp = await client.GeneralAccurateOCR(req);
                jsonStr = AbstractModel.ToJsonString(resp);
            }
            else if (ocrType == GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralHandwritingOcr)
            {
                var req = new GeneralHandwritingOCRRequest();
                req.ImageBase64 = base64;
                var resp = await client.GeneralHandwritingOCR(req);
                jsonStr = AbstractModel.ToJsonString(resp);
            }

            var jsonObj = JObject.Parse(jsonStr);
            var jArray = jsonObj["TextDetections"]!.ToArray();
            return jArray.Aggregate("", (current, t) => current + t["DetectedText"]! + Environment.NewLine);
        }
        catch (Exception e)
        {
            return e.ToString();
        }
    }

    public static async Task<Dictionary<string, string>> ScreenshotTranslate(Bitmap bmp)
    {
        try
        {
            var cred = new Credential
            {
                SecretId = GlobalConfig.Translate.TencentCloud.SecretId,
                SecretKey = GlobalConfig.Translate.TencentCloud.SecretKey
            };

            var clientProfile = new ClientProfile();
            var httpProfile = new HttpProfile
            {
                Endpoint = "tmt.tencentcloudapi.com"
            };
            clientProfile.HttpProfile = httpProfile;

            var client = new TmtClient(cred, "ap-beijing", clientProfile);
            var req = new ImageTranslateRequest
            {
                Data = Utils.BitmapToBase64String(bmp),
                SessionUuid = Guid.NewGuid().ToString(),
                Scene = "doc",
                Source = GlobalConfig.Translate.DefaultTranslateSourceLanguage,
                Target = GlobalConfig.Translate.DefaultTranslateTargetLanguage,
                ProjectId = 0
            };
            var resp = await client.ImageTranslate(req);
            var jsonStr = AbstractModel.ToJsonString(resp);
            var jsonObj = JObject.Parse(jsonStr);
            var jArray = jsonObj["ImageRecord"]!["Value"]!.ToArray();
            var ocrText = "";
            var translateText = "";
            foreach (var jToken in jArray)
            {
                ocrText += jToken["SourceText"] + Environment.NewLine;
                translateText += jToken["TargetText"] + Environment.NewLine;
            }

            var keyValues = new Dictionary<string, string>
            {
                { "ocrText", ocrText },
                { "translateText", translateText }
            };
            return keyValues;
        }
        catch (Exception e)
        {
            var keyValues = new Dictionary<string, string>
            {
                { "ocrText", e.ToString() },
                { "translateText", e.ToString() }
            };
            return keyValues;
        }
    }
}