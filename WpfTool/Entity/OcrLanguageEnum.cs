using System;
using System.Collections.Generic;
using System.Reflection;

namespace WpfTool.Entity
{
    internal enum OcrLanguageEnum
    {
        [OcrLanguage("Language_en", "eng")]
        English,
        [OcrLanguage("Language_zh", "chs")]
        ChineseSimplified,
        [OcrLanguage("Language_cht", "cht")]
        ChineseTraditional,
        [OcrLanguage("Language_jp", "jpn")]
        Japanese,
        [OcrLanguage("Language_kor", "kor")]
        Korean,
        [OcrLanguage("Language_fra", "fre")]
        French,
        [OcrLanguage("Language_spa", "spa")]
        Spanish,
        [OcrLanguage("Language_th", "tai")]
        Thai,
        [OcrLanguage("Language_ara", "ara")]
        Arabic,
        [OcrLanguage("Language_ru", "rus")]
        Russian,
        [OcrLanguage("Language_bul", "bul")]
        Bulgarian,
        [OcrLanguage("Language_hr", "hrv")]
        Croatian,
        [OcrLanguage("Language_cs", "cze")]
        Czech,
        [OcrLanguage("Language_dan", "dan")]
        Danish,
        [OcrLanguage("Language_nl", "dut")]
        Dutch,
        [OcrLanguage("Language_it", "ita")]
        Italian,
        [OcrLanguage("Language_fin", "fin")]
        Finnish,
        [OcrLanguage("Language_de", "ger")]
        German,
        [OcrLanguage("Language_el", "gre")]
        Greek,
        [OcrLanguage("Language_hu", "hun")]
        Hungarian,
        [OcrLanguage("Language_pl", "pol")]
        Polish,
        [OcrLanguage("Language_pt", "por")]
        Portuguese,
        [OcrLanguage("Language_slo", "slv")]
        Slovenian,
        [OcrLanguage("Language_swe", "swe")]
        Swedish,
        [OcrLanguage("Language_tr", "tur")]
        Turkish,
        [OcrLanguage("Language_hi", "hin")]
        Hindi,
        [OcrLanguage("Language_kn", "kan")]
        Kannada,
        [OcrLanguage("Language_fa", "per")]
        PersianFari,
        [OcrLanguage("Language_te", "tel")]
        Telugu,
        [OcrLanguage("Language_ta", "tam")]
        Tamil,
        [OcrLanguage("Language_vie", "vie")]
        Vietnamese,
    }

    internal static class OcrLanguageExtension
    {
        public static readonly List<OcrLanguageAttribute> OcrLanguageAttributeList = new List<OcrLanguageAttribute>();

        private static OcrLanguageAttribute? GetAttribute(OcrLanguageEnum item)
        {
            MemberInfo[] mi = item.GetType().GetMember(item.ToString());
            return Attribute.GetCustomAttribute(mi[0], typeof(OcrLanguageAttribute)) as OcrLanguageAttribute;
        }

        static OcrLanguageExtension()
        {
            foreach (OcrLanguageEnum item in Enum.GetValues(typeof(OcrLanguageEnum)))
            {
                OcrLanguageAttributeList.Add(GetAttribute(item)!);
            }
        }
    }

    internal class OcrLanguageAttribute : Attribute
    {
        private readonly string _name;

        private readonly string _spaceOcr;

        public OcrLanguageAttribute(string name, string spaceOcr)
        {
            this._name = name;
            this._spaceOcr = spaceOcr;
        }

        public string GetName()
        {
            return this._name;
        }

        public string GetSpaceOcrCode()
        {
            return this._spaceOcr;
        }

    }

}
