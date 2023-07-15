using System;
using System.Collections.Generic;

namespace WpfTool.Entity;

internal enum TranslateLanguageEnum
{
    [TranslateLanguage("Language_auto", "auto", "auto", "auto")]
    Auto,

    [TranslateLanguage("Language_zh", "zh", "zh", "zh-cn")]
    Zh,

    [TranslateLanguage("Language_cht", "cht", "zh-TW", "zh-tw")]
    Cht,

    [TranslateLanguage("Language_en", "en", "en", "en")]
    En,

    [TranslateLanguage("Language_jp", "jp", "ja", "ja")]
    Jp,

    [TranslateLanguage("Language_kor", "kor", "ko", "ko")]
    Kor,

    [TranslateLanguage("Language_fra", "fra", "fr", "fr")]
    Fra,

    [TranslateLanguage("Language_spa", "spa", "es", "es")]
    Spa,

    [TranslateLanguage("Language_th", "th", "th", "th")]
    Th,

    [TranslateLanguage("Language_ara", "ara", "ar", "ar")]
    Ara,

    [TranslateLanguage("Language_ru", "ru", "ru", "ru")]
    Ru,

    [TranslateLanguage("Language_pt", "pt", "pt", "pt")]
    Pt,

    [TranslateLanguage("Language_de", "de", "de", "de")]
    De,

    [TranslateLanguage("Language_it", "it", "it", "it")]
    It,

    [TranslateLanguage("Language_vie", "vie", "vi", "vi")]
    Vie,

    [TranslateLanguage("Language_el", "el", "", "el")]
    El,

    [TranslateLanguage("Language_nl", "nl", "", "nl")]
    Nl,

    [TranslateLanguage("Language_pl", "pl", "", "pl")]
    Pl,

    [TranslateLanguage("Language_bul", "bul", "", "bg")]
    Bul,

    [TranslateLanguage("Language_est", "est", "", "et")]
    Est,

    [TranslateLanguage("Language_dan", "dan", "", "da")]
    Dan,

    [TranslateLanguage("Language_fin", "fin", "", "fi")]
    Fin,

    [TranslateLanguage("Language_cs", "cs", "", "cs")]
    Cs,

    [TranslateLanguage("Language_rom", "rom", "", "ro")]
    Som,

    [TranslateLanguage("Language_slo", "slo", "", "sl")]
    Slo,

    [TranslateLanguage("Language_swe", "swe", "", "sv")]
    Swe,

    [TranslateLanguage("Language_hu", "hu", "", "hu")]
    Hu,

    [TranslateLanguage("Language_tr", "", "tr", "tr")]
    Tr,

    [TranslateLanguage("Language_id", "", "id", "id")]
    Id,

    [TranslateLanguage("Language_ms", "", "ms", "ms")]
    Ms,

    [TranslateLanguage("Language_hi", "", "hi", "hi")]
    Hi,

    [TranslateLanguage("Language_yue", "yue", "", "")]
    Yue,

    [TranslateLanguage("Language_wyw", "wyw", "", "")]
    Wyw,

    [TranslateLanguage("Language_af", "", "", "af")]
    Af,

    [TranslateLanguage("Language_sq", "", "", "sq")]
    Sq,

    [TranslateLanguage("Language_am", "", "", "am")]
    Am,

    [TranslateLanguage("Language_hy", "", "", "hy")]
    Hy,

    [TranslateLanguage("Language_az", "", "", "az")]
    Az,

    [TranslateLanguage("Language_eu", "", "", "eu")]
    Eu,

    [TranslateLanguage("Language_be", "", "", "be")]
    Be,

    [TranslateLanguage("Language_bn", "", "", "bn")]
    Bn,

    [TranslateLanguage("Language_bs", "", "", "bs")]
    Bs,

    [TranslateLanguage("Language_ca", "", "", "ca")]
    Ca,

    [TranslateLanguage("Language_ceb", "", "", "ceb")]
    Ceb,

    [TranslateLanguage("Language_ny", "", "", "ny")]
    Ny,

    [TranslateLanguage("Language_co", "", "", "co")]
    Co,

    [TranslateLanguage("Language_hr", "", "", "hr")]
    Hr,

    [TranslateLanguage("Language_eo", "", "", "eo")]
    Eo,

    [TranslateLanguage("Language_tl", "", "", "tl")]
    Tl,

    [TranslateLanguage("Language_fy", "", "", "fy")]
    Fy,

    [TranslateLanguage("Language_gl", "", "", "gl")]
    Gl,

    [TranslateLanguage("Language_ka", "", "", "ka")]
    Ka,

    [TranslateLanguage("Language_gu", "", "", "gu")]
    Gu,

    [TranslateLanguage("Language_ht", "", "", "ht")]
    Ht,

    [TranslateLanguage("Language_ha", "", "", "ha")]
    Ha,

    [TranslateLanguage("Language_haw", "", "", "haw")]
    Haw,

    [TranslateLanguage("Language_iw", "", "", "iw")]
    Iw,

    [TranslateLanguage("Language_hmn", "", "", "hmn")]
    Hmn,

    [TranslateLanguage("Language_is", "", "", "is")]
    Icelandic,

    [TranslateLanguage("Language_ig", "", "", "ig")]
    Ig,

    [TranslateLanguage("Language_ga", "", "", "ga")]
    Ga,

    [TranslateLanguage("Language_jw", "", "", "jw")]
    Jw,

    [TranslateLanguage("Language_kn", "", "", "kn")]
    Kn,

    [TranslateLanguage("Language_kk", "", "", "kk")]
    Kk,

    [TranslateLanguage("Language_km", "", "", "km")]
    Km,

    [TranslateLanguage("Language_ku", "", "", "ku")]
    Ku,

    [TranslateLanguage("Language_ky", "", "", "ky")]
    Ky,

    [TranslateLanguage("Language_lo", "", "", "lo")]
    Lo,

    [TranslateLanguage("Language_la", "", "", "la")]
    La,

    [TranslateLanguage("Language_lv", "", "", "lv")]
    Lv,

    [TranslateLanguage("Language_lt", "", "", "lt")]
    Lt,

    [TranslateLanguage("Language_lb", "", "", "lb")]
    Lb,

    [TranslateLanguage("Language_mk", "", "", "mk")]
    Mk,

    [TranslateLanguage("Language_mg", "", "", "mg")]
    Mg,

    [TranslateLanguage("Language_ml", "", "", "ml")]
    Ml,

    [TranslateLanguage("Language_mt", "", "", "mt")]
    Mt,

    [TranslateLanguage("Language_mi", "", "", "mi")]
    Mi,

    [TranslateLanguage("Language_mr", "", "", "mr")]
    Mr,

    [TranslateLanguage("Language_mn", "", "", "mn")]
    Mn,

    [TranslateLanguage("Language_my", "", "", "my")]
    My,

    [TranslateLanguage("Language_ne", "", "", "ne")]
    Ne,

    [TranslateLanguage("Language_no", "", "", "no")]
    No,

    [TranslateLanguage("Language_ps", "", "", "ps")]
    Ps,

    [TranslateLanguage("Language_fa", "", "", "fa")]
    Fa,

    [TranslateLanguage("Language_ma", "", "", "ma")]
    Ma,

    [TranslateLanguage("Language_sm", "", "", "sm")]
    Sm,

    [TranslateLanguage("Language_gd", "", "", "gd")]
    Gd,

    [TranslateLanguage("Language_sr", "", "", "sr")]
    Sr,

    [TranslateLanguage("Language_st", "", "", "st")]
    St,

    [TranslateLanguage("Language_sn", "", "", "sn")]
    Sn,

    [TranslateLanguage("Language_sd", "", "", "sd")]
    Sd,

    [TranslateLanguage("Language_si", "", "", "si")]
    Si,

    [TranslateLanguage("Language_sk", "", "", "sk")]
    Sk,

    [TranslateLanguage("Language_so", "", "", "so")]
    So,

    [TranslateLanguage("Language_su", "", "", "su")]
    Su,

    [TranslateLanguage("Language_sw", "", "", "sw")]
    Sw,

    [TranslateLanguage("Language_tg", "", "", "tg")]
    Tg,

    [TranslateLanguage("Language_ta", "", "", "ta")]
    Ta,

    [TranslateLanguage("Language_te", "", "", "te")]
    Te,

    [TranslateLanguage("Language_uk", "", "", "uk")]
    Uk,

    [TranslateLanguage("Language_ur", "", "", "ur")]
    Ur,

    [TranslateLanguage("Language_uz", "", "", "uz")]
    Uz,

    [TranslateLanguage("Language_cy", "", "", "cy")]
    Cy,

    [TranslateLanguage("Language_xh", "", "", "xh")]
    Xh,

    [TranslateLanguage("Language_yi", "", "", "yi")]
    Yi,

    [TranslateLanguage("Language_yo", "", "", "yo")]
    Yo,

    [TranslateLanguage("Language_zu", "", "", "zu")]
    Zu
}

internal static class TranslateLanguageExtension
{
    public static readonly List<TranslateLanguageAttribute> TranslateLanguageAttributeList = new();

    static TranslateLanguageExtension()
    {
        foreach (TranslateLanguageEnum item in Enum.GetValues(typeof(TranslateLanguageEnum)))
            TranslateLanguageAttributeList.Add(GetAttribute(item)!);
    }

    private static TranslateLanguageAttribute? GetAttribute(TranslateLanguageEnum item)
    {
        var mi = item.GetType().GetMember(item.ToString());
        return Attribute.GetCustomAttribute(mi[0], typeof(TranslateLanguageAttribute)) as TranslateLanguageAttribute;
    }
}

internal class TranslateLanguageAttribute : Attribute
{
    private readonly string _baiduAiCode;

    private readonly string _googleCloudCode;
    private readonly string _name;

    private readonly string _tencentCloudCode;

    public TranslateLanguageAttribute(string name, string baiduAiCode, string tencentCloudCode, string googleCloudCode)
    {
        this._name = name;
        this._baiduAiCode = baiduAiCode;
        this._tencentCloudCode = tencentCloudCode;
        this._googleCloudCode = googleCloudCode;
    }

    public string GetName()
    {
        return _name;
    }

    public string GetBaiduAiCode()
    {
        return _baiduAiCode;
    }

    public string GetTencentCloudCode()
    {
        return _tencentCloudCode;
    }

    public string GetGoogleCloudCode()
    {
        return _googleCloudCode;
    }
}