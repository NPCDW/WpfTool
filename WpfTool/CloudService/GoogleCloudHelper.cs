using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WpfTool.CloudService
{
    internal class GoogleCloudHelper
    {
        private static String TRANSLATE_URL = "https://translate.googleapis.com/translate_a/single";

        public static String translate(String text, String sourceLanguage, String targetLanguage)
        {
            try
            {
                String param = "?client=gtx&dt=t"
                    + "&sl=" + sourceLanguage
                    + "&tl=" + targetLanguage
                    + "&q=" + HttpUtility.UrlEncode(text, Encoding.UTF8);

                String response = HttpHelper.Get(TRANSLATE_URL + param);

                JArray jsonArray = JArray.Parse(response);
                return jsonArray[0][0][0].ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

    }
}
