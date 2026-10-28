using System;
using System.Net.Http;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using wotool.Entity;

namespace wotool.CloudService;

public static class OpenAiHelper
{
    private static readonly HttpClient Client = new()
    {
        Timeout = TimeSpan.FromSeconds(60)
    };

    public static async Task<string> Translate(string text, string sourceLanguage, string targetLanguage,
        string? providerId = null, string? modelId = null)
    {
        try
        {
            var provider = GlobalConfig.Translate.GetAiProvider(providerId);
            var url = provider.Url;
            var apiKey = provider.ApiKey;
            var model = GlobalConfig.Translate.GetAiModel(provider, modelId).Name;

            var sourceLangText = sourceLanguage == "auto"
                ? "the original language (auto-detect)"
                : sourceLanguage;
            var prompt =
                $"You are a professional translation assistant. Translate the following text from {sourceLangText} to {targetLanguage}. Only output the translation, without any explanation or additional text.";

            var requestBody = new JObject
            {
                ["model"] = model,
                ["messages"] = new JArray
                {
                    new JObject
                    {
                        ["role"] = "system",
                        ["content"] = prompt
                    },
                    new JObject
                    {
                        ["role"] = "user",
                        ["content"] = text
                    }
                }
            };

            var content = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Content = content
            };
            request.Headers.Add("Authorization", "Bearer " + apiKey);

            using var response = await Client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return "AI API error: " + response.StatusCode + " " + response.ReasonPhrase;

            var responseStr = await response.Content.ReadAsStringAsync();
            var jsonObj = JObject.Parse(responseStr);
            var response_content = jsonObj["choices"]![0]!["message"]!["content"]?.ToString().Trim() == "" ? jsonObj["choices"]![0]!["message"]!["reasoning_content"] : jsonObj["choices"]![0]!["message"]!["content"];
            return response_content!.ToString().Trim();
        }
        catch (Exception e)
        {
            return e.ToString();
        }
    }

    /// <summary>
    /// Translate with OpenAI-compatible SSE streaming. A normal JSON response is also
    /// accepted for providers that ignore the stream option or do not implement SSE.
    /// </summary>
    public static async Task<string> TranslateStream(string text, string sourceLanguage, string targetLanguage,
        Action<string> onDelta, string? providerId = null, string? modelId = null,
        CancellationToken cancellationToken = default, Action? onThinking = null)
    {
        var provider = GlobalConfig.Translate.GetAiProvider(providerId);
        var model = GlobalConfig.Translate.GetAiModel(provider, modelId).Name;
        var sourceLangText = sourceLanguage == "auto"
            ? "the original language (auto-detect)"
            : sourceLanguage;
        var prompt =
            $"You are a professional translation assistant. Translate the following text from {sourceLangText} to {targetLanguage}. Only output the translation, without any explanation or additional text.";

        var requestBody = new JObject
        {
            ["model"] = model,
            ["stream"] = true,
            ["messages"] = new JArray
            {
                new JObject { ["role"] = "system", ["content"] = prompt },
                new JObject { ["role"] = "user", ["content"] = text }
            }
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(provider.Url))
        {
            Content = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json")
        };
        request.Headers.Add("Authorization", "Bearer " + provider.ApiKey);

        using var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            // Some OpenAI-compatible services reject stream=true. Retry those responses
            // with the existing non-stream request format for backwards compatibility.
            if (response.StatusCode is System.Net.HttpStatusCode.BadRequest
                or System.Net.HttpStatusCode.NotFound
                or System.Net.HttpStatusCode.MethodNotAllowed
                or System.Net.HttpStatusCode.UnprocessableEntity)
            {
                var fallback = await Translate(text, sourceLanguage, targetLanguage, providerId, modelId);
                if (!string.IsNullOrEmpty(fallback)) onDelta(fallback);
                return fallback;
            }

            return "AI API error: " + response.StatusCode + " " + response.ReasonPhrase;
        }

        if (response.Content.Headers.ContentType?.MediaType?.Contains("json", StringComparison.OrdinalIgnoreCase) == true)
        {
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var ordinaryResult = ParseNonStreamingResponse(json);
            if (!string.IsNullOrEmpty(ordinaryResult)) onDelta(ordinaryResult);
            return ordinaryResult;
        }

        var result = new StringBuilder();
        using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream, Encoding.UTF8);
        while (await reader.ReadLineAsync(cancellationToken) is { } line)
        {
            if (!line.StartsWith("data:", StringComparison.Ordinal)) continue;
            var data = line.Substring(5).Trim();
            if (data.Length == 0 || data == "[DONE]")
            {
                if (data == "[DONE]") break;
                continue;
            }

            var delta = ParseStreamingDelta(data, out var reasoning);
            if (!string.IsNullOrEmpty(reasoning)) onThinking?.Invoke();
            if (string.IsNullOrEmpty(delta)) continue;
            result.Append(delta);
            onDelta(delta);
        }

        return result.ToString();
    }

    private static string ParseNonStreamingResponse(string response)
    {
        var json = JObject.Parse(response);
        var message = json["choices"]?[0]?["message"];
        var content = message?["content"]?.ToString();
        if (string.IsNullOrWhiteSpace(content)) content = message?["reasoning_content"]?.ToString();
        return content?.Trim() ?? string.Empty;
    }

    private static string? ParseStreamingDelta(string data, out string? reasoning)
    {
        var json = JObject.Parse(data);
        var delta = json["choices"]?[0]?["delta"];
        reasoning = delta?["reasoning_content"]?.ToString();
        if (string.IsNullOrEmpty(reasoning)) reasoning = delta?["reasoning"]?.ToString();
        return delta?["content"]?.ToString();
    }
}
