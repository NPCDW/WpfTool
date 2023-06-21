using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfTool
{
    internal class LanguageUtil
    {
        public static void switchLanguage(string lang)
        {
            List<ResourceDictionary> dictionaryList = new List<ResourceDictionary>();
            foreach (ResourceDictionary dictionary in Application.Current.Resources.MergedDictionaries)
            {
                dictionaryList.Add(dictionary);
            }

            ResourceDictionary? resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Equals(@"Lang\" + lang + ".xaml"));
            Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }
    }
}
