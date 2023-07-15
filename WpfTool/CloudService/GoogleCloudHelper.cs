using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using WpfTool.Util;

namespace WpfTool.CloudService;

internal static class GoogleCloudHelper
{
    private const string TranslateUrl = "https://translate.googleapis.com/translate_a/single";

    public static async Task<string> Translate(string text, string sourceLanguage, string targetLanguage)
    {
        try
        {
            var param = "?client=gtx&dt=t"
                        + "&sl=" + sourceLanguage
                        + "&tl=" + targetLanguage
                        + "&q=" + HttpUtility.UrlEncode(text, Encoding.UTF8);

            var response = await HttpHelper.GetAsync(TranslateUrl + param);

            var jsonArray = JArray.Parse(response);
            return jsonArray[0].Aggregate("", (current, t) => current + t[0]);
        }
        catch (Exception e)
        {
            return e.ToString();
        }
    }
}