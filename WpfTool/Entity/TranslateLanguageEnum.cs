using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WpfTool
{
    internal enum TranslateLanguageEnum
    {
        [TranslateLanguage("Language_auto", "auto", "auto")]
        auto,
        [TranslateLanguage("Language_zh", "zh", "zh")]
        zh,
        [TranslateLanguage("Language_cht", "cht", "zh-TW")]
        cht,
        [TranslateLanguage("Language_en", "en", "en")]
        en,
        [TranslateLanguage("Language_jp", "jp", "ja")]
        jp,
        [TranslateLanguage("Language_kor", "kor", "ko")]
        kor,
        [TranslateLanguage("Language_fra", "fra", "fr")]
        fra,
        [TranslateLanguage("Language_spa", "spa", "es")]
        spa,
        [TranslateLanguage("Language_th", "th", "th")]
        th,
        [TranslateLanguage("Language_ara", "ara", "ar")]
        ara,
        [TranslateLanguage("Language_ru", "ru", "ru")]
        ru,
        [TranslateLanguage("Language_pt", "pt", "pt")]
        pt,
        [TranslateLanguage("Language_de", "de", "de")]
        de,
        [TranslateLanguage("Language_it", "it", "it")]
        it,
        [TranslateLanguage("Language_vie", "vie", "vi")]
        vie,
        [TranslateLanguage("Language_el", "el", "")]
        el,
        [TranslateLanguage("Language_nl", "nl", "")]
        nl,
        [TranslateLanguage("Language_pl", "pl", "")]
        pl,
        [TranslateLanguage("Language_bul", "bul", "")]
        bul,
        [TranslateLanguage("Language_est", "est", "")]
        est,
        [TranslateLanguage("Language_dan", "dan", "")]
        dan,
        [TranslateLanguage("Language_fin", "fin", "")]
        fin,
        [TranslateLanguage("Language_cs", "cs", "")]
        cs,
        [TranslateLanguage("Language_rom", "rom", "")]
        rom,
        [TranslateLanguage("Language_slo", "slo", "")]
        slo,
        [TranslateLanguage("Language_swe", "swe", "")]
        swe,
        [TranslateLanguage("Language_hu", "hu", "")]
        hu,
        [TranslateLanguage("Language_tr", "", "tr")]
        tr,
        [TranslateLanguage("Language_id", "", "id")]
        id,
        [TranslateLanguage("Language_ms", "", "ms")]
        ms,
        [TranslateLanguage("Language_hi", "", "hi")]
        hi,
        [TranslateLanguage("Language_yue", "yue", "")]
        yue,
        [TranslateLanguage("Language_wyw", "wyw", "")]
        wyw,
    }

    internal class TranslateLanguageExtension
    {
        public static List<TranslateLanguageAttribute> TranslateLanguageAttributeList = new List<TranslateLanguageAttribute>();

        public static TranslateLanguageAttribute GetAttribute(TranslateLanguageEnum item)
        {
            MemberInfo[] mi = item.GetType().GetMember(item.ToString());
            return Attribute.GetCustomAttribute(mi[0], typeof(TranslateLanguageAttribute)) as TranslateLanguageAttribute;
        }

        static TranslateLanguageExtension()
        {
            foreach (TranslateLanguageEnum item in Enum.GetValues(typeof(TranslateLanguageEnum)))
            {
                TranslateLanguageAttributeList.Add(GetAttribute(item));
            }
        }
    }

    internal class TranslateLanguageAttribute : Attribute
    {
        private string name;

        private string baiduAiCode;

        private string tencentCloudCode;

        public TranslateLanguageAttribute(string name, string baiduAiCode, string tencentCloudCode)
        {
            this.name = name;
            this.baiduAiCode = baiduAiCode;
            this.tencentCloudCode = tencentCloudCode;
        }

        public string getName()
        {
            return this.name;
        }

        public string getBaiduAiCode()
        {
            return this.baiduAiCode;
        }

        public string getTencentCloudCode()
        {
            return this.tencentCloudCode;
        }

    }

}
