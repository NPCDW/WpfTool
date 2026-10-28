using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using wotool.Util;

namespace wotool.Entity;

public static class GlobalConfig
{
    private const string RegeditConfigPathDir = @"SOFTWARE\NPCDW\wotool";
    private const string LegacyRegeditConfigPathDir = @"SOFTWARE\NPCDW\wotool";
    private const string RegeditConfigPathKey = "config_path";

    public static readonly string AppDirConfigPath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config\Setting.json");

    public static readonly string UserDirConfigPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            @"NPCDW\wotool\Config\Setting.json");

    private static readonly string LegacyUserDirConfigPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            @"NPCDW\wotool\Config\Setting.json");


    public static void GetConfig()
    {
        try
        {
            var configPath = RegeditUtil.GetValue(RegeditConfigPathDir, RegeditConfigPathKey);
            if (configPath == null)
            {
                configPath = RegeditUtil.GetValue(LegacyRegeditConfigPathDir, RegeditConfigPathKey)
                    ?? (File.Exists(LegacyUserDirConfigPath) ? LegacyUserDirConfigPath : UserDirConfigPath);
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
            Common.Theme = jsonObj["Common"]!["theme"] == null
                ? "Default"
                : jsonObj["Common"]!["theme"]!.ToString();

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

            var translateProvideStr = jsonObj["Translate"]!["defaultTranslateProvide"]?.ToString();
            if (!Enum.TryParse<Translate.TranslateProvideEnum>(translateProvideStr, out var translateProvide))
                translateProvide = Translate.TranslateProvideEnum.Ai;
            Translate.DefaultTranslateProvide = translateProvide;
            Translate.DefaultAiProvider = jsonObj["Translate"]!["defaultAiProvider"] == null
                ? ""
                : jsonObj["Translate"]!["defaultAiProvider"]!.ToString();
            Translate.DefaultAiModel = jsonObj["Translate"]!["defaultAiModel"] == null
                ? ""
                : jsonObj["Translate"]!["defaultAiModel"]!.ToString();
            Translate.DefaultAiStreaming = jsonObj["Translate"]!["defaultAiStreaming"]?.Value<bool>() ?? true;
            Translate.DefaultTranslateSourceLanguage =
                jsonObj["Translate"]!["defaultTranslateSourceLanguage"]!.ToString();
            Translate.DefaultTranslateTargetLanguage =
                jsonObj["Translate"]!["defaultTranslateTargetLanguage"]!.ToString();
            Translate.BaiduAi.AppId = jsonObj["Translate"]!["BaiduAI"]!["app_id"]!.ToString();
            Translate.BaiduAi.AppSecret = jsonObj["Translate"]!["BaiduAI"]!["app_secret"]!.ToString();
            Translate.Deeplx.Url = jsonObj["Translate"]!["Deeplx"] == null ? "" : jsonObj["Translate"]!["Deeplx"]!["Url"] == null ? "" : jsonObj["Translate"]!["Deeplx"]!["Url"]!.ToString();
            Translate.Deeplx.Authorization = jsonObj["Translate"]!["Deeplx"] == null ? "" : jsonObj["Translate"]!["Deeplx"]!["Authorization"] == null ? "" : jsonObj["Translate"]!["Deeplx"]!["Authorization"]!.ToString();
            Translate.AiProviders.Clear();
            var aiProviders = (jsonObj["Translate"]!["AiProviders"] as JArray)?.ToObject<List<AiProvider>>();
            if (aiProviders != null) Translate.AiProviders.AddRange(aiProviders);
            if (Translate.AiProviders.Count == 0) Translate.LegacyOpenAiToAiProvider(jsonObj["Translate"]!);
            Translate.NormalizeAiSelection();

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
        jsonObj["Common"]!["theme"] = Common.Theme;

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
        jsonObj["Translate"]!["defaultAiProvider"] = Translate.DefaultAiProvider;
        jsonObj["Translate"]!["defaultAiModel"] = Translate.DefaultAiModel;
        jsonObj["Translate"]!["defaultAiStreaming"] = Translate.DefaultAiStreaming;
        jsonObj["Translate"]!["defaultTranslateSourceLanguage"] = Translate.DefaultTranslateSourceLanguage;
        jsonObj["Translate"]!["defaultTranslateTargetLanguage"] = Translate.DefaultTranslateTargetLanguage;
        jsonObj["Translate"]!["BaiduAI"] = new JObject();
        jsonObj["Translate"]!["BaiduAI"]!["app_id"] = Translate.BaiduAi.AppId;
        jsonObj["Translate"]!["BaiduAI"]!["app_secret"] = Translate.BaiduAi.AppSecret;
        jsonObj["Translate"]!["Deeplx"] = new JObject();
        jsonObj["Translate"]!["Deeplx"]!["Url"] = Translate.Deeplx.Url;
        jsonObj["Translate"]!["Deeplx"]!["Authorization"] = Translate.Deeplx.Authorization;
        jsonObj["Translate"]!["AiProviders"] = Translate.ParseJson(Translate.AiProviders);

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
        public static string Theme = "Default";
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
            GoogleCloud,
            Deeplx,
            Ai
        }

        public static TranslateProvideEnum DefaultTranslateProvide;
        public static string? DefaultTranslateSourceLanguage;
        public static string? DefaultTranslateTargetLanguage;

        /// <summary>
        ///     默认 AI 服务商 ID，为空表示未选择
        /// </summary>
        public static string DefaultAiProvider = "";

        /// <summary>
        ///     默认 AI 模型 ID
        /// </summary>
        public static string DefaultAiModel = "";

        /// <summary>Whether AI translations stream output by default.</summary>
        public static bool DefaultAiStreaming = true;

        /// <summary>
        ///     已配置的 AI 翻译服务商，每个服务商下可配置多个模型
        /// </summary>
        public static List<AiProvider> AiProviders = new();

        public static class BaiduAi
        {
            public static string AppId = "";
            public static string AppSecret = "";
        }

        public static class Deeplx
        {
            public static string Url = "";
            public static string Authorization = "";
        }

        public static JArray ParseJson(object obj)
        {
            return JArray.FromObject(obj, JsonSerializer.CreateDefault(new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));
        }

        public static AiProvider GetAiProvider(string? providerId)
        {
            var id = string.IsNullOrWhiteSpace(providerId) ? DefaultAiProvider : providerId;
            return AiProviders.Find(p => p.Id == id) ?? AiProviders.FirstOrDefault() ?? new AiProvider();
        }

        public static AiProvider.AiModel GetAiModel(AiProvider provider, string? modelId)
        {
            var useDefault = string.IsNullOrWhiteSpace(modelId) && provider.Id == DefaultAiProvider;
            var id = useDefault ? DefaultAiModel : modelId;
            return provider.Models.Find(m => m.Id == id) ?? provider.Models.FirstOrDefault() ??
                new AiProvider.AiModel();
        }

        /// <summary>
        ///     让默认服务商 / 模型指向实际存在的配置
        /// </summary>
        public static void NormalizeAiSelection()
        {
            if (!AiProviders.Exists(p => p.Id == DefaultAiProvider))
                DefaultAiProvider = AiProviders.Count > 0 ? AiProviders[0].Id : "";

            var provider = GetAiProvider(DefaultAiProvider);
            if (!provider.Models.Exists(m => m.Id == DefaultAiModel))
                DefaultAiModel = provider.Models.Count > 0 ? provider.Models[0].Id : "";
        }

        /// <summary>
        ///     兼容旧配置：把单个 OpenAi 配置迁移成一个 AI 服务商
        /// </summary>
        public static void LegacyOpenAiToAiProvider(JToken translateObj)
        {
            if (translateObj["OpenAi"] is not JObject openAi) return;

            var url = openAi["Url"]?.ToString() ?? "";
            var apiKey = openAi["ApiKey"]?.ToString() ?? "";
            var model = openAi["Model"]?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(url) && string.IsNullOrWhiteSpace(apiKey)) return;

            var provider = new AiProvider { Name = "OpenAI", Url = url, ApiKey = apiKey };
            if (!string.IsNullOrWhiteSpace(model)) provider.Models.Add(new AiProvider.AiModel { Name = model });
            AiProviders.Add(provider);
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
