using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;

namespace MGR.PortableObject.Parsing;

internal static class PluralForms
{
    private static readonly Dictionary<string, IPluralForm> Rules;
    private static readonly ConcurrentDictionary<CultureInfo, IPluralForm> Caches = new();

    /// <summary>
    /// Gets the default plural form for the specified culture.
    /// </summary>
    /// <param name="culture">The culture.</param>
    /// <returns>The default plural form for the specified culture.</returns>
    public static IPluralForm For(CultureInfo culture)
    {
        IPluralForm pluralForm = Caches.GetOrAdd(culture, cultureKey =>
        {
            while (true)
            {
                if (Rules.ContainsKey(cultureKey.Name))
                {
                    return Rules[cultureKey.Name];
                }

                cultureKey = cultureKey.Parent;
            }
        });
        return pluralForm;
    }
    static PluralForms()
    {
        Rules = [];
        AddRule([""], 1, n => 0);
        AddRule(["ay", "bo", "cgg", "dz", "fa", "id", "ja", "jbo", "ka", "kk", "km", "ko", "ky", "lo", "ms", "my", "sah", "su", "th", "tt", "ug", "vi", "wo", "zh"], 1, n => 0);
        AddRule(["ach", "ak", "am", "arn", "br", "fil", "fr", "gun", "ln", "mfe", "mg", "mi", "oc", "pt-BR", "tg", "ti", "tr", "uz", "wa"], 2, n => n > 1 ? 1 : 0);
        AddRule(["af", "an", "anp", "as", "ast", "az", "bg", "bn", "brx", "ca", "da", "de", "doi", "el", "en", "eo", "es", "es-AR", "et", "eu", "ff", "fi", "fo", "fur", "fy", "gl", "gu", "ha", "he", "hi", "hne", "hu", "hy", "ia", "it", "kl", "kn", "ku", "lb", "mai", "ml", "mn", "mni", "mr", "nah", "nap", "nb", "ne", "nl", "nn", "no", "nso", "or", "pa", "pap", "pms", "ps", "pt", "rm", "rw", "sat", "sco", "sd", "se", "si", "so", "son", "sq", "sv", "sw", "ta", "te", "tk", "ur", "yo"], 2, n => n != 1 ? 1 : 0);
        AddRule(["is"], 2, n => n % 10 != 1 || n % 100 == 11 ? 1 : 0);
        AddRule(["jv"], 2, n => n != 0 ? 1 : 0);
        AddRule(["mk"], 2, n => n == 1 || n % 10 == 1 ? 0 : 1);
        AddRule(["be", "bs", "hr", "lt"], 3, n => n % 10 == 1 && n % 100 != 11 ? 0 : n % 10 >= 2 && n % 10 <= 4 && (n % 100 < 10 || n % 100 >= 20) ? 1 : 2);
        AddRule(["cs"], 3, n => n == 1 ? 0 : n >= 2 && n <= 4 ? 1 : 2);
        AddRule(["csb", "pl"], 3, n => n == 1 ? 0 : n % 10 >= 2 && n % 10 <= 4 && (n % 100 < 10 || n % 100 >= 20) ? 1 : 2);
        AddRule(["lv"], 3, n => n % 10 == 1 && n % 100 != 11 ? 0 : n != 0 ? 1 : 2);
        AddRule(["mnk"], 3, n => n == 0 ? 0 : n == 1 ? 1 : 2);
        AddRule(["ro"], 3, n => n == 1 ? 0 : n == 0 || n % 100 > 0 && n % 100 < 20 ? 1 : 2);
        AddRule(["cy"], 4, n => n == 1 ? 0 : n == 2 ? 1 : n != 8 && n != 11 ? 2 : 3);
        AddRule(["gd"], 4, n => n == 1 || n == 11 ? 0 : n == 2 || n == 12 ? 1 : n > 2 && n < 20 ? 2 : 3);
        AddRule(["kw"], 4, n => n == 1 ? 0 : n == 2 ? 1 : n == 3 ? 2 : 3);
        AddRule(["mt"], 4, n => n == 1 ? 0 : n == 0 || n % 100 > 1 && n % 100 < 11 ? 1 : n % 100 > 10 && n % 100 < 20 ? 2 : 3);
        AddRule(["sl"], 4, n => n % 100 == 1 ? 1 : n % 100 == 2 ? 2 : n % 100 == 3 || n % 100 == 4 ? 3 : 0);
        AddRule(["ru", "sr", "uk"], 3, n => n % 10 == 1 && n % 100 != 11 ? 0 : n % 10 >= 2 && n % 10 <= 4 && (n % 100 < 10 || n % 100 >= 20) ? 1 : 2);
        AddRule(["sk"], 3, n => n == 1 ? 0 : n >= 2 && n <= 4 ? 1 : 2);
        AddRule(["ga"], 5, n => n == 1 ? 0 : n == 2 ? 1 : n > 2 && n < 7 ? 2 : n > 6 && n < 11 ? 3 : 4);
        AddRule(["ar"], 6, n => n == 0 ? 0 : n == 1 ? 1 : n == 2 ? 2 : n % 100 >= 3 && n % 100 <= 10 ? 3 : n % 100 >= 11 ? 4 : 5);
    }

    private static void AddRule(string[] cultures,int numberOfPluralForms, Func<int, int> rule)
    {
        var pluralForm = new FuncBasedPluralForm(numberOfPluralForms, rule);
        foreach (var culture in cultures)
        {
            Rules.Add(culture, pluralForm);
        }
    }
}