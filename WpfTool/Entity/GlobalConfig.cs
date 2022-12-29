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
        private static String REGEDIT_CONFIG_PATH_DIR = @"SOFTWARE\NPCDW\WpfTool";
        private static String REGEDIT_CONFIG_PATH_KEY = "config_path";

        public static String APP_DIR_CONFIG_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config\Setting.json");
        public static String USER_DIR_CONFIG_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"NPCDW\WpfTool\Config\Setting.json");

        public static class Common
        {
            public static int wordSelectionInterval;
            public static bool autoStart = false;
            public static String configPath = "";
            public static String language = "en_US";
        }
        public static class Ocr
        {
            public static OcrProvideEnum defaultOcrProvide;
            public static String defaultOcrType = "";
            public static String defaultOcrLanguage = "";
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
            public static class SpaceOCR
            {
                public static String apiKey = "";
                public enum OcrTypeEnum
                {
                    Engine1,
                    Engine2,
                    Engine3,
                    Engine5,
                }
            }
            public enum OcrProvideEnum
            {
                BaiduCloud,
                TencentCloud,
                SpaceOCR,
            }
        }
        public static class Translate
        {
            public static TranslateProvideEnum defaultTranslateProvide;
            public static String defaultTranslateSourceLanguage;
            public static String defaultTranslateTargetLanguage;
            public static class BaiduAI
            {
                public static String app_id = "";
                public static String app_secret = "";
            }
            public static class TencentCloud
            {
                public static String secret_id = "";
                public static String secret_key = "";
            }
            public enum TranslateProvideEnum
            {
                BaiduAI,
                TencentCloud,
                GoogleCloud
            }

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
            public static class TopMost
            {
                public static byte Modifiers = 0;
                public static int Key = 117;
                public static String Text = "F6";
                public static bool Conflict = false;
            }
        }


        public static void GetConfig()
        {
            string jsonStr;
            try
            {
                String configPath = RegeditUtil.GetValue(REGEDIT_CONFIG_PATH_DIR, REGEDIT_CONFIG_PATH_KEY);
                if (configPath == null)
                {
                    configPath = USER_DIR_CONFIG_PATH;
                    RegeditUtil.SetValue(REGEDIT_CONFIG_PATH_DIR, REGEDIT_CONFIG_PATH_KEY, configPath);
                }
                if (!File.Exists(configPath))
                {
                    Directory.GetParent(configPath).Create();
                    File.Copy(APP_DIR_CONFIG_PATH, configPath);
                }

                using (StreamReader sr = new StreamReader(configPath, false))
                {
                    jsonStr = sr.ReadToEnd().ToString();
                }
                JObject jsonObj = JObject.Parse(jsonStr);
                
                Common.wordSelectionInterval = int.Parse(jsonObj["Common"]["WordSelectionInterval"].ToString());
                Common.autoStart = AutoStart.GetStatus();
                Common.configPath = configPath;
                Common.language = jsonObj["Common"]["language"] == null ? "en_US" : jsonObj["Common"]["language"].ToString();

                Ocr.defaultOcrProvide = (Ocr.OcrProvideEnum)Enum.Parse(typeof(Ocr.OcrProvideEnum), jsonObj["Ocr"]["defaultOcrProvide"].ToString());
                Ocr.defaultOcrType = jsonObj["Ocr"]["defaultOcrType"].ToString();
                Ocr.defaultOcrLanguage = jsonObj["Ocr"]["defaultOcrLanguage"] == null ? "auto" : jsonObj["Ocr"]["defaultOcrLanguage"].ToString();
                Ocr.BaiduCloud.access_token = jsonObj["Ocr"]["BaiduCloud"]["access_token"].ToString();
                Ocr.BaiduCloud.access_token_expires_time = DateTime.Parse(jsonObj["Ocr"]["BaiduCloud"]["access_token_expires_time"].ToString());
                Ocr.BaiduCloud.client_id = jsonObj["Ocr"]["BaiduCloud"]["client_id"].ToString();
                Ocr.BaiduCloud.client_secret = jsonObj["Ocr"]["BaiduCloud"]["client_secret"].ToString();
                Ocr.TencentCloud.secret_id = jsonObj["Ocr"]["TencentCloud"]["secret_id"].ToString();
                Ocr.TencentCloud.secret_key = jsonObj["Ocr"]["TencentCloud"]["secret_key"].ToString();
                if (jsonObj["Ocr"]["SpaceOCR"] == null) 
                {
                    jsonObj["Ocr"]["SpaceOCR"] = new JObject();
                }
                Ocr.SpaceOCR.apiKey = jsonObj["Ocr"]["SpaceOCR"]["apiKey"] == null ? "" : jsonObj["Ocr"]["SpaceOCR"]["apiKey"].ToString();

                Translate.defaultTranslateProvide = (Translate.TranslateProvideEnum)Enum.Parse(typeof(Translate.TranslateProvideEnum), jsonObj["Translate"]["defaultTranslateProvide"].ToString());
                Translate.defaultTranslateSourceLanguage = jsonObj["Translate"]["defaultTranslateSourceLanguage"].ToString();
                Translate.defaultTranslateTargetLanguage = jsonObj["Translate"]["defaultTranslateTargetLanguage"].ToString();
                Translate.BaiduAI.app_id = jsonObj["Translate"]["BaiduAI"]["app_id"].ToString();
                Translate.BaiduAI.app_secret = jsonObj["Translate"]["BaiduAI"]["app_secret"].ToString();
                Translate.TencentCloud.secret_id = jsonObj["Translate"]["TencentCloud"]["secret_id"].ToString();
                Translate.TencentCloud.secret_key = jsonObj["Translate"]["TencentCloud"]["secret_key"].ToString();

                HotKeys.Ocr.Modifiers = byte.Parse(jsonObj["HotKeys"]["Ocr"]["Modifiers"].ToString());
                HotKeys.Ocr.Key = int.Parse(jsonObj["HotKeys"]["Ocr"]["Key"].ToString());
                HotKeys.Ocr.Text = jsonObj["HotKeys"]["Ocr"]["Text"].ToString();
                HotKeys.GetWordsTranslate.Modifiers = byte.Parse(jsonObj["HotKeys"]["GetWordsTranslate"]["Modifiers"].ToString());
                HotKeys.GetWordsTranslate.Key = int.Parse(jsonObj["HotKeys"]["GetWordsTranslate"]["Key"].ToString());
                HotKeys.GetWordsTranslate.Text = jsonObj["HotKeys"]["GetWordsTranslate"]["Text"].ToString();
                HotKeys.ScreenshotTranslate.Modifiers = byte.Parse(jsonObj["HotKeys"]["ScreenshotTranslate"]["Modifiers"].ToString());
                HotKeys.ScreenshotTranslate.Key = int.Parse(jsonObj["HotKeys"]["ScreenshotTranslate"]["Key"].ToString());
                HotKeys.ScreenshotTranslate.Text = jsonObj["HotKeys"]["ScreenshotTranslate"]["Text"].ToString();
                HotKeys.TopMost.Modifiers = byte.Parse(jsonObj["HotKeys"]["TopMost"]["Modifiers"].ToString());
                HotKeys.TopMost.Key = int.Parse(jsonObj["HotKeys"]["TopMost"]["Key"].ToString());
                HotKeys.TopMost.Text = jsonObj["HotKeys"]["TopMost"]["Text"].ToString();
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
            jsonObj["Common"]["WordSelectionInterval"] = Common.wordSelectionInterval;
            jsonObj["Common"]["language"] = Common.language;

            jsonObj["Ocr"] = new JObject();
            jsonObj["Ocr"]["defaultOcrProvide"] = Ocr.defaultOcrProvide.ToString();
            jsonObj["Ocr"]["defaultOcrType"] = Ocr.defaultOcrType;
            jsonObj["Ocr"]["defaultOcrLanguage"] = Ocr.defaultOcrLanguage;
            jsonObj["Ocr"]["BaiduCloud"] = new JObject();
            jsonObj["Ocr"]["BaiduCloud"]["access_token"] = Ocr.BaiduCloud.access_token;
            jsonObj["Ocr"]["BaiduCloud"]["access_token_expires_time"] = Ocr.BaiduCloud.access_token_expires_time.ToString();
            jsonObj["Ocr"]["BaiduCloud"]["client_id"] = Ocr.BaiduCloud.client_id;
            jsonObj["Ocr"]["BaiduCloud"]["client_secret"] = Ocr.BaiduCloud.client_secret;
            jsonObj["Ocr"]["TencentCloud"] = new JObject();
            jsonObj["Ocr"]["TencentCloud"]["secret_id"] = Ocr.TencentCloud.secret_id;
            jsonObj["Ocr"]["TencentCloud"]["secret_key"] = Ocr.TencentCloud.secret_key;
            jsonObj["Ocr"]["SpaceOCR"] = new JObject();
            jsonObj["Ocr"]["SpaceOCR"]["apiKey"] = Ocr.SpaceOCR.apiKey;

            jsonObj["Translate"] = new JObject();
            jsonObj["Translate"]["defaultTranslateProvide"] = Translate.defaultTranslateProvide.ToString();
            jsonObj["Translate"]["defaultTranslateSourceLanguage"] = Translate.defaultTranslateSourceLanguage;
            jsonObj["Translate"]["defaultTranslateTargetLanguage"] = Translate.defaultTranslateTargetLanguage;
            jsonObj["Translate"]["BaiduAI"] = new JObject();
            jsonObj["Translate"]["BaiduAI"]["app_id"] = Translate.BaiduAI.app_id;
            jsonObj["Translate"]["BaiduAI"]["app_secret"] = Translate.BaiduAI.app_secret;
            jsonObj["Translate"]["TencentCloud"] = new JObject();
            jsonObj["Translate"]["TencentCloud"]["secret_id"] = Translate.TencentCloud.secret_id;
            jsonObj["Translate"]["TencentCloud"]["secret_key"] = Translate.TencentCloud.secret_key;

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
            jsonObj["HotKeys"]["TopMost"] = new JObject();
            jsonObj["HotKeys"]["TopMost"]["Modifiers"] = HotKeys.TopMost.Modifiers;
            jsonObj["HotKeys"]["TopMost"]["Key"] = HotKeys.TopMost.Key;
            jsonObj["HotKeys"]["TopMost"]["Text"] = HotKeys.TopMost.Text;

            String jsonStr = jsonObj.ToString();
            using (StreamWriter sw = new StreamWriter(Common.configPath))
            {
                sw.WriteLine(jsonStr);
            }
            RegeditUtil.SetValue(REGEDIT_CONFIG_PATH_DIR, REGEDIT_CONFIG_PATH_KEY, Common.configPath);
        }

    }
}
