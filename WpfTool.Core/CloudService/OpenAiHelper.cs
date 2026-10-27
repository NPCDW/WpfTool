using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WpfTool.Entity;

namespace WpfTool.CloudService;

public static class OpenAiHelper
{
    private static readonly HttpClient Client = new()
    {
        Timeout = TimeSpan.FromSeconds(60)
    };

    public static async Task<string> Translate(string text, string sourceLanguage, string targetLanguage)
    {
        try
        {
            var url = GlobalConfig.Translate.OpenAi.Url;
            var apiKey = GlobalConfig.Translate.OpenAi.ApiKey;
            var model = GlobalConfig.Translate.OpenAi.Model;

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
                return "OpenAI API error: " + response.StatusCode + " " + response.ReasonPhrase;

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
}
