using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Ocr.V20181119;
using TencentCloud.Ocr.V20181119.Models;
using WpfTool.Entity;
using WpfTool.Util;

namespace WpfTool.CloudService;

public static class TencentCloudHelper
{
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
                var req = new GeneralBasicOCRRequest()
                {
                    ImageBase64 = base64,
                };
                var resp = await client.GeneralBasicOCR(req);
                jsonStr = AbstractModel.ToJsonString(resp);
            }
            else if (ocrType == GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralAccurateOcr)
            {
                var req = new GeneralAccurateOCRRequest()
                {
                    ImageBase64 = base64,
                };
                var resp = await client.GeneralAccurateOCR(req);
                jsonStr = AbstractModel.ToJsonString(resp);
            }
            else if (ocrType == GlobalConfig.Ocr.TencentCloud.OcrTypeEnum.GeneralHandwritingOcr)
            {
                var req = new GeneralHandwritingOCRRequest()
                {
                    ImageBase64 = base64,
                };
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
}
