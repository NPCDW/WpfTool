using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfTool
{
    public static class GlobalConfig
    {
        private static String configPath = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\Setting.json";

        public static class Common
        {
            public static OcrProvideEnum defaultOcrProvide;
            public static String defaultOcrType = "";
            public static TranslateProvideEnum defaultTranslateProvide;
            public static String defaultTranslateSourceLanguage;
            public static String defaultTranslateTargetLanguage;
            public static int wordSelectionInterval;
            public static bool autoStart = false;
        }
        public static class Local
        {
        }
        public static class BaiduCloud
        {
            public static String access_token = "";
            public static DateTime access_token_expires_time;
            public static String client_id = "";
            public static String client_secret = "";
            public enum OcrTypeEnum
            {
                general_basic,
                accurate_basic,
                handwriting
            }

        }
        public static class BaiduAI
        {
            public static String app_id = "";
            public static String app_secret = "";
        }
        public static class TencentCloud
        {
            public static String secret_id = "";
            public static String secret_key = "";
            public enum OcrTypeEnum
            {
                GeneralBasicOCR,
                GeneralAccurateOCR,
                GeneralHandwritingOCR
            }

        }
        public static class TencentCloudTranslate
        {
            public static String secret_id = "";
            public static String secret_key = "";
        }
        public static class HotKeys
        {
            public static class GetWordsTranslate
            {
                public static byte Modifiers = 0;
                public static int Key = 113;
                public static String Text = "F2";
                public static bool Conflict = false;
            }
            public static class Ocr
            {
                public static byte Modifiers = 0;
                public static int Key = 115;
                public static String Text = "F4";
                public static bool Conflict = false;
            }
            public static class ScreenshotTranslate
            {
                public static byte Modifiers = 2;
                public static int Key = 113;
                public static String Text = "Ctrl+F2";
                public static bool Conflict = false;
            }
        }

        public enum OcrProvideEnum
        {
            BaiduCloud,
            TencentCloud
        }

        public enum TranslateProvideEnum
        {
            BaiduAI,
            TencentCloud
        }

        public static void GetConfig()
        {
            string jsonStr;
            try
            {
                using (StreamReader sr = new StreamReader(configPath, false))
                {
                    jsonStr = sr.ReadToEnd().ToString();
                }
                JObject jsonObj = JObject.Parse(jsonStr);
                
                Common.defaultOcrProvide = (OcrProvideEnum)Enum.Parse(typeof(OcrProvideEnum), jsonObj["Common"]["defaultOcrProvide"].ToString());
                Common.defaultOcrType = jsonObj["Common"]["defaultOcrType"].ToString();
                Common.defaultTranslateProvide = (TranslateProvideEnum)Enum.Parse(typeof(TranslateProvideEnum), jsonObj["Common"]["defaultTranslateProvide"].ToString());
                Common.defaultTranslateSourceLanguage = jsonObj["Common"]["defaultTranslateSourceLanguage"].ToString();
                Common.defaultTranslateTargetLanguage = jsonObj["Common"]["defaultTranslateTargetLanguage"].ToString();
                Common.wordSelectionInterval = int.Parse(jsonObj["Common"]["WordSelectionInterval"].ToString());
                Common.autoStart = AutoStart.GetStatus();

                BaiduCloud.access_token = jsonObj["BaiduCloud"]["access_token"].ToString();
                BaiduCloud.access_token_expires_time = DateTime.Parse(jsonObj["BaiduCloud"]["access_token_expires_time"].ToString());
                BaiduCloud.client_id = jsonObj["BaiduCloud"]["client_id"].ToString();
                BaiduCloud.client_secret = jsonObj["BaiduCloud"]["client_secret"].ToString();

                BaiduAI.app_id = jsonObj["BaiduAI"]["app_id"].ToString();
                BaiduAI.app_secret = jsonObj["BaiduAI"]["app_secret"].ToString();

                TencentCloud.secret_id = jsonObj["TencentCloud"]["secret_id"].ToString();
                TencentCloud.secret_key = jsonObj["TencentCloud"]["secret_key"].ToString();

                TencentCloudTranslate.secret_id = jsonObj["TencentCloudTranslate"]["secret_id"].ToString();
                TencentCloudTranslate.secret_key = jsonObj["TencentCloudTranslate"]["secret_key"].ToString();

                HotKeys.Ocr.Modifiers = byte.Parse(jsonObj["HotKeys"]["Ocr"]["Modifiers"].ToString());
                HotKeys.Ocr.Key = int.Parse(jsonObj["HotKeys"]["Ocr"]["Key"].ToString());
                HotKeys.Ocr.Text = jsonObj["HotKeys"]["Ocr"]["Text"].ToString();
                HotKeys.GetWordsTranslate.Modifiers = byte.Parse(jsonObj["HotKeys"]["GetWordsTranslate"]["Modifiers"].ToString());
                HotKeys.GetWordsTranslate.Key = int.Parse(jsonObj["HotKeys"]["GetWordsTranslate"]["Key"].ToString());
                HotKeys.GetWordsTranslate.Text = jsonObj["HotKeys"]["GetWordsTranslate"]["Text"].ToString();
                HotKeys.ScreenshotTranslate.Modifiers = byte.Parse(jsonObj["HotKeys"]["ScreenshotTranslate"]["Modifiers"].ToString());
                HotKeys.ScreenshotTranslate.Key = int.Parse(jsonObj["HotKeys"]["ScreenshotTranslate"]["Key"].ToString());
                HotKeys.ScreenshotTranslate.Text = jsonObj["HotKeys"]["ScreenshotTranslate"]["Text"].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void SaveConfig()
        {
            JObject jsonObj = new JObject();

            jsonObj["Common"] = new JObject();
            jsonObj["Common"]["defaultOcrProvide"] = Common.defaultOcrProvide.ToString();
            jsonObj["Common"]["defaultOcrType"] = Common.defaultOcrType;
            jsonObj["Common"]["defaultTranslateProvide"] = Common.defaultTranslateProvide.ToString();
            jsonObj["Common"]["defaultTranslateSourceLanguage"] = Common.defaultTranslateSourceLanguage;
            jsonObj["Common"]["defaultTranslateTargetLanguage"] = Common.defaultTranslateTargetLanguage;
            jsonObj["Common"]["WordSelectionInterval"] = Common.wordSelectionInterval;

            jsonObj["BaiduCloud"] = new JObject();
            jsonObj["BaiduCloud"]["access_token"] = BaiduCloud.access_token;
            jsonObj["BaiduCloud"]["access_token_expires_time"] = BaiduCloud.access_token_expires_time.ToString();
            jsonObj["BaiduCloud"]["client_id"] = BaiduCloud.client_id;
            jsonObj["BaiduCloud"]["client_secret"] = BaiduCloud.client_secret;

            jsonObj["BaiduAI"] = new JObject();
            jsonObj["BaiduAI"]["app_id"] = BaiduAI.app_id;
            jsonObj["BaiduAI"]["app_secret"] = BaiduAI.app_secret;

            jsonObj["TencentCloud"] = new JObject();
            jsonObj["TencentCloud"]["secret_id"] = TencentCloud.secret_id;
            jsonObj["TencentCloud"]["secret_key"] = TencentCloud.secret_key;

            jsonObj["TencentCloudTranslate"] = new JObject();
            jsonObj["TencentCloudTranslate"]["secret_id"] = TencentCloudTranslate.secret_id;
            jsonObj["TencentCloudTranslate"]["secret_key"] = TencentCloudTranslate.secret_key;

            jsonObj["HotKeys"] = new JObject();
            jsonObj["HotKeys"]["Ocr"] = new JObject();
            jsonObj["HotKeys"]["Ocr"]["Modifiers"] = HotKeys.Ocr.Modifiers;
            jsonObj["HotKeys"]["Ocr"]["Key"] = HotKeys.Ocr.Key;
            jsonObj["HotKeys"]["Ocr"]["Text"] = HotKeys.Ocr.Text;
            jsonObj["HotKeys"]["GetWordsTranslate"] = new JObject();
            jsonObj["HotKeys"]["GetWordsTranslate"]["Modifiers"] = HotKeys.GetWordsTranslate.Modifiers;
            jsonObj["HotKeys"]["GetWordsTranslate"]["Key"] = HotKeys.GetWordsTranslate.Key;
            jsonObj["HotKeys"]["GetWordsTranslate"]["Text"] = HotKeys.GetWordsTranslate.Text;
            jsonObj["HotKeys"]["ScreenshotTranslate"] = new JObject();
            jsonObj["HotKeys"]["ScreenshotTranslate"]["Modifiers"] = HotKeys.ScreenshotTranslate.Modifiers;
            jsonObj["HotKeys"]["ScreenshotTranslate"]["Key"] = HotKeys.ScreenshotTranslate.Key;
            jsonObj["HotKeys"]["ScreenshotTranslate"]["Text"] = HotKeys.ScreenshotTranslate.Text;

            String jsonStr = jsonObj.ToString();
            using (StreamWriter sw = new StreamWriter(configPath))
            {
                sw.WriteLine(jsonStr);
            }
        }

    }
}
