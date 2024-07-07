using System;
using System.Collections.Generic;

namespace WpfTool.Entity;

internal enum BaiduAiTranslateLanguageEnum
{
    [TranslateLanguage("Language_auto", "auto", true, false)]
    Auto,

    [TranslateLanguage("Language_zh", "zh", true, true)]
    Zh,

    [TranslateLanguage("Language_cht", "cht", true, true)]
    Cht,

    [TranslateLanguage("Language_en", "en", true, true)]
    En,

    [TranslateLanguage("Language_jp", "jp", true, true)]
    Jp,

    [TranslateLanguage("Language_kor", "kor", true, true)]
    Kor,

    [TranslateLanguage("Language_fra", "fra", true, true)]
    Fra,

    [TranslateLanguage("Language_spa", "spa", true, true)]
    Spa,

    [TranslateLanguage("Language_th", "th", true, true)]
    Th,

    [TranslateLanguage("Language_ara", "ara", true, true)]
    Ara,

    [TranslateLanguage("Language_ru", "ru", true, true)]
    Ru,

    [TranslateLanguage("Language_pt", "pt", true, true)]
    Pt,

    [TranslateLanguage("Language_de", "de", true, true)]
    De,

    [TranslateLanguage("Language_it", "it", true, true)]
    It,

    [TranslateLanguage("Language_vie", "vie", true, true)]
    Vie,

    [TranslateLanguage("Language_el", "el", true, true)]
    El,

    [TranslateLanguage("Language_nl", "nl", true, true)]
    Nl,

    [TranslateLanguage("Language_pl", "pl", true, true)]
    Pl,

    [TranslateLanguage("Language_bul", "bul", true, true)]
    Bul,

    [TranslateLanguage("Language_est", "est", true, true)]
    Est,

    [TranslateLanguage("Language_dan", "dan", true, true)]
    Dan,

    [TranslateLanguage("Language_fin", "fin", true, true)]
    Fin,

    [TranslateLanguage("Language_cs", "cs", true, true)]
    Cs,

    [TranslateLanguage("Language_rom", "rom", true, true)]
    Som,

    [TranslateLanguage("Language_slo", "slo", true, true)]
    Slo,

    [TranslateLanguage("Language_swe", "swe", true, true)]
    Swe,

    [TranslateLanguage("Language_hu", "hu", true, true)]
    Hu,

    [TranslateLanguage("Language_yue", "yue", true, true)]
    Yue,

    [TranslateLanguage("Language_wyw", "wyw", true, true)]
    Wyw,
}

internal enum TencentTranslateLanguageEnum
{
    [TranslateLanguage("Language_auto", "auto", true, false)]
    Auto,

    [TranslateLanguage("Language_zh", "zh", true, true)]
    Zh,

    [TranslateLanguage("Language_cht", "zh-TW", true, true)]
    Cht,

    [TranslateLanguage("Language_en", "en", true, true)]
    En,

    [TranslateLanguage("Language_jp", "ja", true, true)]
    Jp,

    [TranslateLanguage("Language_kor", "ko", true, true)]
    Kor,

    [TranslateLanguage("Language_fra", "fr", true, true)]
    Fra,

    [TranslateLanguage("Language_spa", "es", true, true)]
    Spa,

    [TranslateLanguage("Language_it", "it", true, true)]
    It,

    [TranslateLanguage("Language_de", "de", true, true)]
    De,

    [TranslateLanguage("Language_tr", "tr", true, true)]
    Tr,

    [TranslateLanguage("Language_ru", "ru", true, true)]
    Ru,

    [TranslateLanguage("Language_pt", "pt", true, true)]
    Pt,

    [TranslateLanguage("Language_vie", "vi", true, true)]
    Vie,

    [TranslateLanguage("Language_id", "id", true, true)]
    Id,

    [TranslateLanguage("Language_th", "th", true, true)]
    Th,

    [TranslateLanguage("Language_ms", "ms", true, true)]
    Ms,

    [TranslateLanguage("Language_ara", "ar", true, true)]
    Ara,

    [TranslateLanguage("Language_hi", "hi", true, true)]
    Hi,
}

internal enum GoogleTranslateLanguageEnum
{
    [TranslateLanguage("Language_auto", "auto", true, false)]
    Auto,

    [TranslateLanguage("Language_zh", "zh-cn", true, true)]
    Zh,

    [TranslateLanguage("Language_cht", "zh-tw", true, true)]
    Cht,

    [TranslateLanguage("Language_en", "en", true, true)]
    En,

    [TranslateLanguage("Language_jp", "ja", true, true)]
    Jp,

    [TranslateLanguage("Language_kor", "ko", true, true)]
    Kor,

    [TranslateLanguage("Language_fra", "fr", true, true)]
    Fra,

    [TranslateLanguage("Language_spa", "es", true, true)]
    Spa,

    [TranslateLanguage("Language_th", "th", true, true)]
    Th,

    [TranslateLanguage("Language_ara", "ar", true, true)]
    Ara,

    [TranslateLanguage("Language_ru", "ru", true, true)]
    Ru,

    [TranslateLanguage("Language_pt", "pt", true, true)]
    Pt,

    [TranslateLanguage("Language_de", "de", true, true)]
    De,

    [TranslateLanguage("Language_it", "it", true, true)]
    It,

    [TranslateLanguage("Language_vie", "vi", true, true)]
    Vie,

    [TranslateLanguage("Language_el", "el", true, true)]
    El,

    [TranslateLanguage("Language_nl", "nl", true, true)]
    Nl,

    [TranslateLanguage("Language_pl", "pl", true, true)]
    Pl,

    [TranslateLanguage("Language_bul", "bg", true, true)]
    Bul,

    [TranslateLanguage("Language_est", "et", true, true)]
    Est,

    [TranslateLanguage("Language_dan", "da", true, true)]
    Dan,

    [TranslateLanguage("Language_fin", "fi", true, true)]
    Fin,

    [TranslateLanguage("Language_cs", "cs", true, true)]
    Cs,

    [TranslateLanguage("Language_rom", "ro", true, true)]
    Som,

    [TranslateLanguage("Language_slo", "sl", true, true)]
    Slo,

    [TranslateLanguage("Language_swe", "sv", true, true)]
    Swe,

    [TranslateLanguage("Language_hu", "hu", true, true)]
    Hu,

    [TranslateLanguage("Language_tr", "tr", true, true)]
    Tr,

    [TranslateLanguage("Language_id", "id", true, true)]
    Id,

    [TranslateLanguage("Language_ms", "ms", true, true)]
    Ms,

    [TranslateLanguage("Language_hi", "hi", true, true)]
    Hi,

    [TranslateLanguage("Language_af", "af", true, true)]
    Af,

    [TranslateLanguage("Language_sq", "sq", true, true)]
    Sq,

    [TranslateLanguage("Language_am", "am", true, true)]
    Am,

    [TranslateLanguage("Language_hy", "hy", true, true)]
    Hy,

    [TranslateLanguage("Language_az", "az", true, true)]
    Az,

    [TranslateLanguage("Language_eu", "eu", true, true)]
    Eu,

    [TranslateLanguage("Language_be", "be", true, true)]
    Be,

    [TranslateLanguage("Language_bn", "bn", true, true)]
    Bn,

    [TranslateLanguage("Language_bs", "bs", true, true)]
    Bs,

    [TranslateLanguage("Language_ca", "ca", true, true)]
    Ca,

    [TranslateLanguage("Language_ceb", "ceb", true, true)]
    Ceb,

    [TranslateLanguage("Language_ny", "ny", true, true)]
    Ny,

    [TranslateLanguage("Language_co", "co", true, true)]
    Co,

    [TranslateLanguage("Language_hr", "hr", true, true)]
    Hr,

    [TranslateLanguage("Language_eo", "eo", true, true)]
    Eo,

    [TranslateLanguage("Language_tl", "tl", true, true)]
    Tl,

    [TranslateLanguage("Language_fy", "fy", true, true)]
    Fy,

    [TranslateLanguage("Language_gl", "gl", true, true)]
    Gl,

    [TranslateLanguage("Language_ka", "ka", true, true)]
    Ka,

    [TranslateLanguage("Language_gu", "gu", true, true)]
    Gu,

    [TranslateLanguage("Language_ht", "ht", true, true)]
    Ht,

    [TranslateLanguage("Language_ha", "ha", true, true)]
    Ha,

    [TranslateLanguage("Language_haw", "haw", true, true)]
    Haw,

    [TranslateLanguage("Language_iw", "iw", true, true)]
    Iw,

    [TranslateLanguage("Language_hmn", "hmn", true, true)]
    Hmn,

    [TranslateLanguage("Language_is", "is", true, true)]
    Icelandic,

    [TranslateLanguage("Language_ig", "ig", true, true)]
    Ig,

    [TranslateLanguage("Language_ga", "ga", true, true)]
    Ga,

    [TranslateLanguage("Language_jw", "jw", true, true)]
    Jw,

    [TranslateLanguage("Language_kn", "kn", true, true)]
    Kn,

    [TranslateLanguage("Language_kk", "kk", true, true)]
    Kk,

    [TranslateLanguage("Language_km", "km", true, true)]
    Km,

    [TranslateLanguage("Language_ku", "ku", true, true)]
    Ku,

    [TranslateLanguage("Language_ky", "ky", true, true)]
    Ky,

    [TranslateLanguage("Language_lo", "lo", true, true)]
    Lo,

    [TranslateLanguage("Language_la", "la", true, true)]
    La,

    [TranslateLanguage("Language_lv", "lv", true, true)]
    Lv,

    [TranslateLanguage("Language_lt", "lt", true, true)]
    Lt,

    [TranslateLanguage("Language_lb", "lb", true, true)]
    Lb,

    [TranslateLanguage("Language_mk", "mk", true, true)]
    Mk,

    [TranslateLanguage("Language_mg", "mg", true, true)]
    Mg,

    [TranslateLanguage("Language_ml", "ml", true, true)]
    Ml,

    [TranslateLanguage("Language_mt", "mt", true, true)]
    Mt,

    [TranslateLanguage("Language_mi", "mi", true, true)]
    Mi,

    [TranslateLanguage("Language_mr", "mr", true, true)]
    Mr,

    [TranslateLanguage("Language_mn", "mn", true, true)]
    Mn,

    [TranslateLanguage("Language_my", "my", true, true)]
    My,

    [TranslateLanguage("Language_ne", "ne", true, true)]
    Ne,

    [TranslateLanguage("Language_no", "no", true, true)]
    No,

    [TranslateLanguage("Language_ps", "ps", true, true)]
    Ps,

    [TranslateLanguage("Language_fa", "fa", true, true)]
    Fa,

    [TranslateLanguage("Language_ma", "ma", true, true)]
    Ma,

    [TranslateLanguage("Language_sm", "sm", true, true)]
    Sm,

    [TranslateLanguage("Language_gd", "gd", true, true)]
    Gd,

    [TranslateLanguage("Language_sr", "sr", true, true)]
    Sr,

    [TranslateLanguage("Language_st", "st", true, true)]
    St,

    [TranslateLanguage("Language_sn", "sn", true, true)]
    Sn,

    [TranslateLanguage("Language_sd", "sd", true, true)]
    Sd,

    [TranslateLanguage("Language_si", "si", true, true)]
    Si,

    [TranslateLanguage("Language_sk", "sk", true, true)]
    Sk,

    [TranslateLanguage("Language_so", "so", true, true)]
    So,

    [TranslateLanguage("Language_su", "su", true, true)]
    Su,

    [TranslateLanguage("Language_sw", "sw", true, true)]
    Sw,

    [TranslateLanguage("Language_tg", "tg", true, true)]
    Tg,

    [TranslateLanguage("Language_ta", "ta", true, true)]
    Ta,

    [TranslateLanguage("Language_te", "te", true, true)]
    Te,

    [TranslateLanguage("Language_uk", "uk", true, true)]
    Uk,

    [TranslateLanguage("Language_ur", "ur", true, true)]
    Ur,

    [TranslateLanguage("Language_uz", "uz", true, true)]
    Uz,

    [TranslateLanguage("Language_cy", "cy", true, true)]
    Cy,

    [TranslateLanguage("Language_xh", "xh", true, true)]
    Xh,

    [TranslateLanguage("Language_yi", "yi", true, true)]
    Yi,

    [TranslateLanguage("Language_yo", "yo", true, true)]
    Yo,

    [TranslateLanguage("Language_zu", "zu", true, true)]
    Zu
}

internal enum DeeplxTranslateLanguageEnum
{
    [TranslateLanguage("Language_zh", "ZH", true, true)]
    Zh,

    [TranslateLanguage("Language_en", "EN", true, true)]
    En,

    [TranslateLanguage("Language_en_gb", "EN-GB", false, true)]
    En_Gb,

    [TranslateLanguage("Language_en_gb", "EN-US", false, true)]
    En_Us,

    [TranslateLanguage("Language_jp", "JA", true, true)]
    Jp,

    [TranslateLanguage("Language_kor", "KO", true, true)]
    Kor,

    [TranslateLanguage("Language_fra", "FR", true, true)]
    Fra,

    [TranslateLanguage("Language_spa", "ES", true, true)]
    Spa,

    [TranslateLanguage("Language_ara", "AR", true, true)]
    Ara,

    [TranslateLanguage("Language_ru", "RU", true, true)]
    Ru,

    [TranslateLanguage("Language_pt", "PT", true, true)]
    Pt,

    [TranslateLanguage("Language_pt_br", "PT-BR", false, true)]
    Pt_Br,

    [TranslateLanguage("Language_pt_pt", "PT-PT", false, true)]
    Pt_Pt,

    [TranslateLanguage("Language_de", "DE", true, true)]
    De,

    [TranslateLanguage("Language_it", "IT", true, true)]
    It,

    [TranslateLanguage("Language_el", "EL", true, true)]
    El,

    [TranslateLanguage("Language_nl", "NL", true, true)]
    Nl,

    [TranslateLanguage("Language_pl", "PL", true, true)]
    Pl,

    [TranslateLanguage("Language_bul", "BG", true, true)]
    Bul,

    [TranslateLanguage("Language_est", "ET", true, true)]
    Est,

    [TranslateLanguage("Language_dan", "DA", true, true)]
    Dan,

    [TranslateLanguage("Language_fin", "FI", true, true)]
    Fin,

    [TranslateLanguage("Language_cs", "CS", true, true)]
    Cs,

    [TranslateLanguage("Language_rom", "RO", true, true)]
    Som,

    [TranslateLanguage("Language_slo", "SL", true, true)]
    Slo,

    [TranslateLanguage("Language_swe", "SV", true, true)]
    Swe,

    [TranslateLanguage("Language_hu", "HU", true, true)]
    Hu,

    [TranslateLanguage("Language_tr", "TR", true, true)]
    Tr,

    [TranslateLanguage("Language_id", "ID", true, true)]
    Id,

    [TranslateLanguage("Language_lv", "LV", true, true)]
    Lv,

    [TranslateLanguage("Language_lt", "LT", true, true)]
    Lt,

    [TranslateLanguage("Language_nb", "NB", true, true)]
    Nb,

    [TranslateLanguage("Language_sk", "SK", true, true)]
    Sk,

    [TranslateLanguage("Language_uk", "UK", true, true)]
    Uk,
}

internal static class BaiduAiTranslateLanguageExtension
{
    public static readonly List<TranslateLanguageAttribute> TranslateLanguageAttributeList = new();

    static BaiduAiTranslateLanguageExtension()
    {
        foreach (BaiduAiTranslateLanguageEnum item in Enum.GetValues(typeof(BaiduAiTranslateLanguageEnum)))
            TranslateLanguageAttributeList.Add(GetAttribute(item)!);
    }

    private static TranslateLanguageAttribute? GetAttribute(BaiduAiTranslateLanguageEnum item)
    {
        var mi = item.GetType().GetMember(item.ToString());
        return Attribute.GetCustomAttribute(mi[0], typeof(TranslateLanguageAttribute)) as TranslateLanguageAttribute;
    }
}

internal static class TencentTranslateLanguageExtension
{
    public static readonly List<TranslateLanguageAttribute> TranslateLanguageAttributeList = new();

    static TencentTranslateLanguageExtension()
    {
        foreach (TencentTranslateLanguageEnum item in Enum.GetValues(typeof(TencentTranslateLanguageEnum)))
            TranslateLanguageAttributeList.Add(GetAttribute(item)!);
    }

    private static TranslateLanguageAttribute? GetAttribute(TencentTranslateLanguageEnum item)
    {
        var mi = item.GetType().GetMember(item.ToString());
        return Attribute.GetCustomAttribute(mi[0], typeof(TranslateLanguageAttribute)) as TranslateLanguageAttribute;
    }
}

internal static class GoogleTranslateLanguageExtension
{
    public static readonly List<TranslateLanguageAttribute> TranslateLanguageAttributeList = new();

    static GoogleTranslateLanguageExtension()
    {
        foreach (GoogleTranslateLanguageEnum item in Enum.GetValues(typeof(GoogleTranslateLanguageEnum)))
            TranslateLanguageAttributeList.Add(GetAttribute(item)!);
    }

    private static TranslateLanguageAttribute? GetAttribute(GoogleTranslateLanguageEnum item)
    {
        var mi = item.GetType().GetMember(item.ToString());
        return Attribute.GetCustomAttribute(mi[0], typeof(TranslateLanguageAttribute)) as TranslateLanguageAttribute;
    }
}

internal static class DeeplxTranslateLanguageExtension
{
    public static readonly List<TranslateLanguageAttribute> TranslateLanguageAttributeList = new();

    static DeeplxTranslateLanguageExtension()
    {
        foreach (DeeplxTranslateLanguageEnum item in Enum.GetValues(typeof(DeeplxTranslateLanguageEnum)))
            TranslateLanguageAttributeList.Add(GetAttribute(item)!);
    }

    private static TranslateLanguageAttribute? GetAttribute(DeeplxTranslateLanguageEnum item)
    {
        var mi = item.GetType().GetMember(item.ToString());
        return Attribute.GetCustomAttribute(mi[0], typeof(TranslateLanguageAttribute)) as TranslateLanguageAttribute;
    }
}

internal class TranslateLanguageAttribute : Attribute
{
    private readonly string _name;

    private readonly string _code;

    private readonly bool _source;

    private readonly bool _target;

    public TranslateLanguageAttribute(string name, string code, bool source, bool target)
    {
        this._name = name;
        this._code = code;
        this._source = source;
        this._target = target;
    }

    public string GetName()
    {
        return _name;
    }

    public string GetCode()
    {
        return _code;
    }
    
    public bool GetSource()
    {
        return _source;
    }

    public bool GetTarget()
    {
        return _target;
    }
}