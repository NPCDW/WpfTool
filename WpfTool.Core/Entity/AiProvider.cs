using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WpfTool.Entity;

/// <summary>
///     AI 翻译服务商配置（OpenAI 兼容接口），一个服务商下可配置多个模型
/// </summary>
public class AiProvider
{
    [JsonProperty("Id")] public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [JsonProperty("Name")] public string Name { get; set; } = "";

    [JsonProperty("Url")] public string Url { get; set; } = "";

    [JsonProperty("ApiKey")] public string ApiKey { get; set; } = "";

    [JsonProperty("Models")] public List<AiModel> Models { get; set; } = new();

    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(Name) ? "AI" : Name;
    }

    public class AiModel
    {
        [JsonProperty("Id")] public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [JsonProperty("Name")] public string Name { get; set; } = "";

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? "Model" : Name;
        }
    }
}
