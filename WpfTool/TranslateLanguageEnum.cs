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
        [TranslateLanguage("自动检测", "auto", "auto")]
        auto,
        [TranslateLanguage("简体中文", "zh", "zh")]
        zh,
        [TranslateLanguage("繁体中文", "cht", "zh-TW")]
        cht,
        [TranslateLanguage("英语", "en", "en")]
        en,
        [TranslateLanguage("日语", "jp", "ja")]
        jp,
        [TranslateLanguage("韩语", "kor", "ko")]
        kor,
        [TranslateLanguage("法语", "fra", "fr")]
        fra,
        [TranslateLanguage("西班牙语", "spa", "es")]
        spa,
        [TranslateLanguage("泰语", "th", "th")]
        th,
        [TranslateLanguage("阿拉伯语", "ara", "ar")]
        ara,
        [TranslateLanguage("俄语", "ru", "ru")]
        ru,
        [TranslateLanguage("葡萄牙语", "pt", "pt")]
        pt,
        [TranslateLanguage("德语", "de", "de")]
        de,
        [TranslateLanguage("意大利语", "it", "it")]
        it,
        [TranslateLanguage("越南语", "vie", "vi")]
        vie,
        [TranslateLanguage("希腊语", "el", "")]
        el,
        [TranslateLanguage("荷兰语", "nl", "")]
        nl,
        [TranslateLanguage("波兰语", "pl", "")]
        pl,
        [TranslateLanguage("保加利亚语", "bul", "")]
        bul,
        [TranslateLanguage("爱沙尼亚语", "est", "")]
        est,
        [TranslateLanguage("丹麦语", "dan", "")]
        dan,
        [TranslateLanguage("芬兰语", "fin", "")]
        fin,
        [TranslateLanguage("捷克语", "cs", "")]
        cs,
        [TranslateLanguage("罗马尼亚语", "rom", "")]
        rom,
        [TranslateLanguage("斯洛文尼亚语", "slo", "")]
        slo,
        [TranslateLanguage("瑞典语", "swe", "")]
        swe,
        [TranslateLanguage("匈牙利语", "hu", "")]
        hu,
        [TranslateLanguage("土耳其语", "", "tr")]
        tr,
        [TranslateLanguage("印尼语", "", "id")]
        id,
        [TranslateLanguage("马来西亚语", "", "ms")]
        ms,
        [TranslateLanguage("印地语", "", "hi")]
        hi,
        [TranslateLanguage("粤语", "yue", "")]
        yue,
        [TranslateLanguage("文言文", "wyw", "")]
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
