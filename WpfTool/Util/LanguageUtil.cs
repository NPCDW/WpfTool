using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfTool.Util;

internal static class LanguageUtil
{
    public static void SwitchLanguage(string lang)
    {
        // var dictionaryList = new List<ResourceDictionary>();
        var dictionaryList = Application.Current.Resources.MergedDictionaries.Aggregate(new List<ResourceDictionary>(),
            (current, t) =>
            {
                current.Add(t);
                return current;
            });

        var resourceDictionary = dictionaryList.FirstOrDefault(d =>
            d.Source != null && d.Source.OriginalString.Equals(@"Lang\" + lang + ".xaml"));
        Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
        Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
    }
}