using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;
using WpfTool.Util;

namespace WpfTool.Entity;

public static class GlobalConfig
{
    private const string RegeditConfigPathDir = @"SOFTWARE\NPCDW\WpfTool";
    private const string RegeditConfigPathKey = "config_path";

    public static readonly string AppDirConfigPath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config\Setting.json");

    public static readonly string UserDirConfigPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            @"NPCDW\WpfTool\Config\Setting.json");


    public static void GetConfig()
    {
        try
        {
            var configPath = RegeditUtil.GetValue(RegeditConfigPathDir, RegeditConfigPathKey);
            if (configPath == null)
            {
                configPath = UserDirConfigPath;
                RegeditUtil.SetValue(RegeditConfigPathDir, RegeditConfigPathKey, configPath);
            }

            if (!File.Exists(configPath))
            {
                Directory.GetParent(configPath)!.Create();
                File.Copy(AppDirConfigPath, configPath);
            }
            
            string jsonStr;
            using (var sr = new StreamReader(configPath, false))
            {
                jsonStr = sr.ReadToEnd();
            }

            var jsonObj = JObject.Parse(jsonStr);

            Common.WordSelectionInterval = int.Parse(jsonObj["Common"]!["WordSelectionInterval"]!.ToString());
            Common.AutoStart = AutoStart.GetStatus();
            Common.ConfigPath = configPath;
            Common.Language = jsonObj["Common"]!["language"] == null
                ? "en_US"
                : jsonObj["Common"]!["language"]!.ToString();

            Ocr.DefaultOcrProvide = (Ocr.OcrProvideEnum)Enum.Parse(typeof(Ocr.OcrProvideEnum),
                jsonObj["Ocr"]!["defaultOcrProvide"]!.ToString());
            Ocr.DefaultOcrType = jsonObj["Ocr"]!["defaultOcrType"]!.ToString();
            Ocr.DefaultOcrLanguage = jsonObj["Ocr"]!["defaultOcrLanguage"] == null
                ? "auto"
                : jsonObj["Ocr"]!["defaultOcrLanguage"]!.ToString();
            Ocr.BaiduCloud.AccessToken = jsonObj["Ocr"]!["BaiduCloud"]!["access_token"]!.ToString();
            Ocr.BaiduCloud.AccessTokenExpiresTime =
                DateTime.Parse(jsonObj["Ocr"]!["BaiduCloud"]!["access_token_expires_time"]!.ToString());
            Ocr.BaiduCloud.ClientId = jsonObj["Ocr"]!["BaiduCloud"]!["client_id"]!.ToString();
            Ocr.BaiduCloud.ClientSecret = jsonObj["Ocr"]!["BaiduCloud"]!["client_secret"]!.ToString();
            Ocr.TencentCloud.SecretId = jsonObj["Ocr"]!["TencentCloud"]!["secret_id"]!.ToString();
            Ocr.TencentCloud.SecretKey = jsonObj["Ocr"]!["TencentCloud"]!["secret_key"]!.ToString();
            jsonObj["Ocr"]!["SpaceOCR"] ??= new JObject();
            Ocr.SpaceOcr.ApiKey = jsonObj["Ocr"]!["SpaceOCR"]!["apiKey"] == null
                ? ""
                : jsonObj["Ocr"]!["SpaceOCR"]!["apiKey"]!.ToString();

            Translate.DefaultTranslateProvide = (Translate.TranslateProvideEnum)Enum.Parse(
                typeof(Translate.TranslateProvideEnum), jsonObj["Translate"]!["defaultTranslateProvide"]!.ToString());
            Translate.DefaultTranslateSourceLanguage =
                jsonObj["Translate"]!["defaultTranslateSourceLanguage"]!.ToString();
            Translate.DefaultTranslateTargetLanguage =
                jsonObj["Translate"]!["defaultTranslateTargetLanguage"]!.ToString();
            Translate.BaiduAi.AppId = jsonObj["Translate"]!["BaiduAI"]!["app_id"]!.ToString();
            Translate.BaiduAi.AppSecret = jsonObj["Translate"]!["BaiduAI"]!["app_secret"]!.ToString();
            Translate.TencentCloud.SecretId = jsonObj["Translate"]!["TencentCloud"]!["secret_id"]!.ToString();
            Translate.TencentCloud.SecretKey = jsonObj["Translate"]!["TencentCloud"]!["secret_key"]!.ToString();

            HotKeys.OcrHotKey.Modifiers = byte.Parse(jsonObj["HotKeys"]!["Ocr"]!["Modifiers"]!.ToString());
            HotKeys.OcrHotKey.Key = int.Parse(jsonObj["HotKeys"]!["Ocr"]!["Key"]!.ToString());
            HotKeys.OcrHotKey.Text = jsonObj["HotKeys"]!["Ocr"]!["Text"]!.ToString();
            HotKeys.GetWordsTranslate.Modifiers =
                byte.Parse(jsonObj["HotKeys"]!["GetWordsTranslate"]!["Modifiers"]!.ToString());
            HotKeys.GetWordsTranslate.Key = int.Parse(jsonObj["HotKeys"]!["GetWordsTranslate"]!["Key"]!.ToString());
            HotKeys.GetWordsTranslate.Text = jsonObj["HotKeys"]!["GetWordsTranslate"]!["Text"]!.ToString();
            HotKeys.ScreenshotTranslate.Modifiers =
                byte.Parse(jsonObj["HotKeys"]!["ScreenshotTranslate"]!["Modifiers"]!.ToString());
            HotKeys.ScreenshotTranslate.Key = int.Parse(jsonObj["HotKeys"]!["ScreenshotTranslate"]!["Key"]!.ToString());
            HotKeys.ScreenshotTranslate.Text = jsonObj["HotKeys"]!["ScreenshotTranslate"]!["Text"]!.ToString();
            HotKeys.TopMost.Modifiers = byte.Parse(jsonObj["HotKeys"]!["TopMost"]!["Modifiers"]!.ToString());
            HotKeys.TopMost.Key = int.Parse(jsonObj["HotKeys"]!["TopMost"]!["Key"]!.ToString());
            HotKeys.TopMost.Text = jsonObj["HotKeys"]!["TopMost"]!["Text"]!.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void SaveConfig()
    {
        var jsonObj = new JObject();

        jsonObj["Common"] = new JObject();
        jsonObj["Common"]!["WordSelectionInterval"] = Common.WordSelectionInterval;
        jsonObj["Common"]!["language"] = Common.Language;

        jsonObj["Ocr"] = new JObject();
        jsonObj["Ocr"]!["defaultOcrProvide"] = Ocr.DefaultOcrProvide.ToString();
        jsonObj["Ocr"]!["defaultOcrType"] = Ocr.DefaultOcrType;
        jsonObj["Ocr"]!["defaultOcrLanguage"] = Ocr.DefaultOcrLanguage;
        jsonObj["Ocr"]!["BaiduCloud"] = new JObject();
        jsonObj["Ocr"]!["BaiduCloud"]!["access_token"] = Ocr.BaiduCloud.AccessToken;
        jsonObj["Ocr"]!["BaiduCloud"]!["access_token_expires_time"] = Ocr.BaiduCloud.AccessTokenExpiresTime.ToString(CultureInfo.CurrentCulture);
        jsonObj["Ocr"]!["BaiduCloud"]!["client_id"] = Ocr.BaiduCloud.ClientId;
        jsonObj["Ocr"]!["BaiduCloud"]!["client_secret"] = Ocr.BaiduCloud.ClientSecret;
        jsonObj["Ocr"]!["TencentCloud"] = new JObject();
        jsonObj["Ocr"]!["TencentCloud"]!["secret_id"] = Ocr.TencentCloud.SecretId;
        jsonObj["Ocr"]!["TencentCloud"]!["secret_key"] = Ocr.TencentCloud.SecretKey;
        jsonObj["Ocr"]!["SpaceOCR"] = new JObject();
        jsonObj["Ocr"]!["SpaceOCR"]!["apiKey"] = Ocr.SpaceOcr.ApiKey;

        jsonObj["Translate"] = new JObject();
        jsonObj["Translate"]!["defaultTranslateProvide"] = Translate.DefaultTranslateProvide.ToString();
        jsonObj["Translate"]!["defaultTranslateSourceLanguage"] = Translate.DefaultTranslateSourceLanguage;
        jsonObj["Translate"]!["defaultTranslateTargetLanguage"] = Translate.DefaultTranslateTargetLanguage;
        jsonObj["Translate"]!["BaiduAI"] = new JObject();
        jsonObj["Translate"]!["BaiduAI"]!["app_id"] = Translate.BaiduAi.AppId;
        jsonObj["Translate"]!["BaiduAI"]!["app_secret"] = Translate.BaiduAi.AppSecret;
        jsonObj["Translate"]!["TencentCloud"] = new JObject();
        jsonObj["Translate"]!["TencentCloud"]!["secret_id"] = Translate.TencentCloud.SecretId;
        jsonObj["Translate"]!["TencentCloud"]!["secret_key"] = Translate.TencentCloud.SecretKey;

        jsonObj["HotKeys"] = new JObject();
        jsonObj["HotKeys"]!["Ocr"] = new JObject();
        jsonObj["HotKeys"]!["Ocr"]!["Modifiers"] = HotKeys.OcrHotKey.Modifiers;
        jsonObj["HotKeys"]!["Ocr"]!["Key"] = HotKeys.OcrHotKey.Key;
        jsonObj["HotKeys"]!["Ocr"]!["Text"] = HotKeys.OcrHotKey.Text;
        jsonObj["HotKeys"]!["GetWordsTranslate"] = new JObject();
        jsonObj["HotKeys"]!["GetWordsTranslate"]!["Modifiers"] = HotKeys.GetWordsTranslate.Modifiers;
        jsonObj["HotKeys"]!["GetWordsTranslate"]!["Key"] = HotKeys.GetWordsTranslate.Key;
        jsonObj["HotKeys"]!["GetWordsTranslate"]!["Text"] = HotKeys.GetWordsTranslate.Text;
        jsonObj["HotKeys"]!["ScreenshotTranslate"] = new JObject();
        jsonObj["HotKeys"]!["ScreenshotTranslate"]!["Modifiers"] = HotKeys.ScreenshotTranslate.Modifiers;
        jsonObj["HotKeys"]!["ScreenshotTranslate"]!["Key"] = HotKeys.ScreenshotTranslate.Key;
        jsonObj["HotKeys"]!["ScreenshotTranslate"]!["Text"] = HotKeys.ScreenshotTranslate.Text;
        jsonObj["HotKeys"]!["TopMost"] = new JObject();
        jsonObj["HotKeys"]!["TopMost"]!["Modifiers"] = HotKeys.TopMost.Modifiers;
        jsonObj["HotKeys"]!["TopMost"]!["Key"] = HotKeys.TopMost.Key;
        jsonObj["HotKeys"]!["TopMost"]!["Text"] = HotKeys.TopMost.Text;

        var jsonStr = jsonObj.ToString();
        using (var sw = new StreamWriter(Common.ConfigPath))
        {
            sw.WriteLine(jsonStr);
        }

        RegeditUtil.SetValue(RegeditConfigPathDir, RegeditConfigPathKey, Common.ConfigPath);
    }

    public static class Common
    {
        public static int WordSelectionInterval;
        public static bool AutoStart;
        public static string ConfigPath = "";
        public static string Language = "en_US";
    }

    public static class Ocr
    {
        public enum OcrProvideEnum
        {
            BaiduCloud,
            TencentCloud,
            SpaceOcr
        }

        public static OcrProvideEnum DefaultOcrProvide;
        public static string DefaultOcrType = "";
        public static string DefaultOcrLanguage = "";

        public static class BaiduCloud
        {
            public enum OcrTypeEnum
            {
                GeneralBasic,
                AccurateBasic,
                Handwriting
            }

            public static string AccessToken = "";
            public static DateTime AccessTokenExpiresTime;
            public static string ClientId = "";
            public static string ClientSecret = "";
        }

        public static class TencentCloud
        {
            public enum OcrTypeEnum
            {
                GeneralBasicOcr,
                GeneralAccurateOcr,
                GeneralHandwritingOcr
            }

            public static string SecretId = "";
            public static string SecretKey = "";
        }

        public static class SpaceOcr
        {
            public enum OcrTypeEnum
            {
                Engine1,
                Engine2,
                Engine3,
                Engine5
            }

            public static string ApiKey = "";
        }
    }

    public static class Translate
    {
        public enum TranslateProvideEnum
        {
            BaiduAi,
            TencentCloud,
            GoogleCloud
        }

        public static TranslateProvideEnum DefaultTranslateProvide;
        public static string? DefaultTranslateSourceLanguage;
        public static string? DefaultTranslateTargetLanguage;

        public static class BaiduAi
        {
            public static string AppId = "";
            public static string AppSecret = "";
        }

        public static class TencentCloud
        {
            public static string SecretId = "";
            public static string SecretKey = "";
        }
    }

    public static class HotKeys
    {
        public static class GetWordsTranslate
        {
            public static byte Modifiers;
            public static int Key = 113;
            public static string Text = "F2";
            public static bool Conflict = false;
        }

        public static class OcrHotKey
        {
            public static byte Modifiers;
            public static int Key = 115;
            public static string Text = "F4";
            public static bool Conflict = false;
        }

        public static class ScreenshotTranslate
        {
            public static byte Modifiers = 2;
            public static int Key = 113;
            public static string Text = "Ctrl+F2";
            public static bool Conflict = false;
        }

        public static class TopMost
        {
            public static byte Modifiers;
            public static int Key = 117;
            public static string Text = "F6";
            public static bool Conflict = false;
        }
    }
}