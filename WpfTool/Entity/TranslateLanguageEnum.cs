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
        [TranslateLanguage("Language_auto", "auto", "auto", "auto")]
        auto,
        [TranslateLanguage("Language_zh", "zh", "zh", "zh-cn")]
        zh,
        [TranslateLanguage("Language_cht", "cht", "zh-TW", "zh-tw")]
        cht,
        [TranslateLanguage("Language_en", "en", "en", "en")]
        en,
        [TranslateLanguage("Language_jp", "jp", "ja", "ja")]
        jp,
        [TranslateLanguage("Language_kor", "kor", "ko", "ko")]
        kor,
        [TranslateLanguage("Language_fra", "fra", "fr", "fr")]
        fra,
        [TranslateLanguage("Language_spa", "spa", "es", "es")]
        spa,
        [TranslateLanguage("Language_th", "th", "th", "th")]
        th,
        [TranslateLanguage("Language_ara", "ara", "ar", "ar")]
        ara,
        [TranslateLanguage("Language_ru", "ru", "ru", "ru")]
        ru,
        [TranslateLanguage("Language_pt", "pt", "pt", "pt")]
        pt,
        [TranslateLanguage("Language_de", "de", "de", "de")]
        de,
        [TranslateLanguage("Language_it", "it", "it", "it")]
        it,
        [TranslateLanguage("Language_vie", "vie", "vi", "vi")]
        vie,
        [TranslateLanguage("Language_el", "el", "", "el")]
        el,
        [TranslateLanguage("Language_nl", "nl", "", "nl")]
        nl,
        [TranslateLanguage("Language_pl", "pl", "", "pl")]
        pl,
        [TranslateLanguage("Language_bul", "bul", "", "bg")]
        bul,
        [TranslateLanguage("Language_est", "est", "", "et")]
        est,
        [TranslateLanguage("Language_dan", "dan", "", "da")]
        dan,
        [TranslateLanguage("Language_fin", "fin", "", "fi")]
        fin,
        [TranslateLanguage("Language_cs", "cs", "", "cs")]
        cs,
        [TranslateLanguage("Language_rom", "rom", "", "ro")]
        rom,
        [TranslateLanguage("Language_slo", "slo", "", "sl")]
        slo,
        [TranslateLanguage("Language_swe", "swe", "", "sv")]
        swe,
        [TranslateLanguage("Language_hu", "hu", "", "hu")]
        hu,
        [TranslateLanguage("Language_tr", "", "tr", "tr")]
        tr,
        [TranslateLanguage("Language_id", "", "id", "id")]
        id,
        [TranslateLanguage("Language_ms", "", "ms", "ms")]
        ms,
        [TranslateLanguage("Language_hi", "", "hi", "hi")]
        hi,
        [TranslateLanguage("Language_yue", "yue", "", "")]
        yue,
        [TranslateLanguage("Language_wyw", "wyw", "", "")]
        wyw,
        [TranslateLanguage("Language_af", "", "", "af")]
        af,
        [TranslateLanguage("Language_sq", "", "", "sq")]
        sq,
        [TranslateLanguage("Language_am", "", "", "am")]
        am,
        [TranslateLanguage("Language_hy", "", "", "hy")]
        hy,
        [TranslateLanguage("Language_az", "", "", "az")]
        az,
        [TranslateLanguage("Language_eu", "", "", "eu")]
        eu,
        [TranslateLanguage("Language_be", "", "", "be")]
        be,
        [TranslateLanguage("Language_bn", "", "", "bn")]
        bn,
        [TranslateLanguage("Language_bs", "", "", "bs")]
        bs,
        [TranslateLanguage("Language_ca", "", "", "ca")]
        ca,
        [TranslateLanguage("Language_ceb", "", "", "ceb")]
        ceb,
        [TranslateLanguage("Language_ny", "", "", "ny")]
        ny,
        [TranslateLanguage("Language_co", "", "", "co")]
        co,
        [TranslateLanguage("Language_hr", "", "", "hr")]
        hr,
        [TranslateLanguage("Language_eo", "", "", "eo")]
        eo,
        [TranslateLanguage("Language_tl", "", "", "tl")]
        tl,
        [TranslateLanguage("Language_fy", "", "", "fy")]
        fy,
        [TranslateLanguage("Language_gl", "", "", "gl")]
        gl,
        [TranslateLanguage("Language_ka", "", "", "ka")]
        ka,
        [TranslateLanguage("Language_gu", "", "", "gu")]
        gu,
        [TranslateLanguage("Language_ht", "", "", "ht")]
        ht,
        [TranslateLanguage("Language_ha", "", "", "ha")]
        ha,
        [TranslateLanguage("Language_haw", "", "", "haw")]
        haw,
        [TranslateLanguage("Language_iw", "", "", "iw")]
        iw,
        [TranslateLanguage("Language_hmn", "", "", "hmn")]
        hmn,
        [TranslateLanguage("Language_is", "", "", "is")]
        Icelandic,
        [TranslateLanguage("Language_ig", "", "", "ig")]
        ig,
        [TranslateLanguage("Language_ga", "", "", "ga")]
        ga,
        [TranslateLanguage("Language_jw", "", "", "jw")]
        jw,
        [TranslateLanguage("Language_kn", "", "", "kn")]
        kn,
        [TranslateLanguage("Language_kk", "", "", "kk")]
        kk,
        [TranslateLanguage("Language_km", "", "", "km")]
        km,
        [TranslateLanguage("Language_ku", "", "", "ku")]
        ku,
        [TranslateLanguage("Language_ky", "", "", "ky")]
        ky,
        [TranslateLanguage("Language_lo", "", "", "lo")]
        lo,
        [TranslateLanguage("Language_la", "", "", "la")]
        la,
        [TranslateLanguage("Language_lv", "", "", "lv")]
        lv,
        [TranslateLanguage("Language_lt", "", "", "lt")]
        lt,
        [TranslateLanguage("Language_lb", "", "", "lb")]
        lb,
        [TranslateLanguage("Language_mk", "", "", "mk")]
        mk,
        [TranslateLanguage("Language_mg", "", "", "mg")]
        mg,
        [TranslateLanguage("Language_ml", "", "", "ml")]
        ml,
        [TranslateLanguage("Language_mt", "", "", "mt")]
        mt,
        [TranslateLanguage("Language_mi", "", "", "mi")]
        mi,
        [TranslateLanguage("Language_mr", "", "", "mr")]
        mr,
        [TranslateLanguage("Language_mn", "", "", "mn")]
        mn,
        [TranslateLanguage("Language_my", "", "", "my")]
        my,
        [TranslateLanguage("Language_ne", "", "", "ne")]
        ne,
        [TranslateLanguage("Language_no", "", "", "no")]
        no,
        [TranslateLanguage("Language_ps", "", "", "ps")]
        ps,
        [TranslateLanguage("Language_fa", "", "", "fa")]
        fa,
        [TranslateLanguage("Language_ma", "", "", "ma")]
        ma,
        [TranslateLanguage("Language_sm", "", "", "sm")]
        sm,
        [TranslateLanguage("Language_gd", "", "", "gd")]
        gd,
        [TranslateLanguage("Language_sr", "", "", "sr")]
        sr,
        [TranslateLanguage("Language_st", "", "", "st")]
        st,
        [TranslateLanguage("Language_sn", "", "", "sn")]
        sn,
        [TranslateLanguage("Language_sd", "", "", "sd")]
        sd,
        [TranslateLanguage("Language_si", "", "", "si")]
        si,
        [TranslateLanguage("Language_sk", "", "", "sk")]
        sk,
        [TranslateLanguage("Language_so", "", "", "so")]
        so,
        [TranslateLanguage("Language_su", "", "", "su")]
        su,
        [TranslateLanguage("Language_sw", "", "", "sw")]
        sw,
        [TranslateLanguage("Language_tg", "", "", "tg")]
        tg,
        [TranslateLanguage("Language_ta", "", "", "ta")]
        ta,
        [TranslateLanguage("Language_te", "", "", "te")]
        te,
        [TranslateLanguage("Language_uk", "", "", "uk")]
        uk,
        [TranslateLanguage("Language_ur", "", "", "ur")]
        ur,
        [TranslateLanguage("Language_uz", "", "", "uz")]
        uz,
        [TranslateLanguage("Language_cy", "", "", "cy")]
        cy,
        [TranslateLanguage("Language_xh", "", "", "xh")]
        xh,
        [TranslateLanguage("Language_yi", "", "", "yi")]
        yi,
        [TranslateLanguage("Language_yo", "", "", "yo")]
        yo,
        [TranslateLanguage("Language_zu", "", "", "zu")]
        zu,
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

        private string googleCloudCode;

        public TranslateLanguageAttribute(string name, string baiduAiCode, string tencentCloudCode, string googleCloudCode)
        {
            this.name = name;
            this.baiduAiCode = baiduAiCode;
            this.tencentCloudCode = tencentCloudCode;
            this.googleCloudCode = googleCloudCode;
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

        public string getGoogleCloudCode()
        {
            return this.googleCloudCode;
        }

    }

}
