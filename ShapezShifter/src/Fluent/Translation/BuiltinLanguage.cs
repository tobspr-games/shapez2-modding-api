using System;

namespace ShapezShifter.Fluent
{
    public enum BuiltinLanguage
    {
        English,
        En = English,
        German,
        De = German,
        Spanish,
        Es = Spanish,
        French,
        Fr = French,
        Japanese,
        Ja = Japanese,
        Korean,
        Ko = Korean,
        Polish,
        Pl = Polish,
        BrazilianPortuguese,
        PtBr = BrazilianPortuguese,
        Russian,
        Ru = Russian,
        Thai,
        Th = Thai,
        Turkish,
        Tr = Turkish,
        TraditionalChinese,
        ZhTw = TraditionalChinese,
        SimplifiedChinese,
        ZhCn = SimplifiedChinese
    }

    public static class BuiltinLanguagesExtensions
    {
        public static string Code(this BuiltinLanguage language)
        {
            return language switch
            {
                BuiltinLanguage.English => "en-US",
                BuiltinLanguage.German => "de-DE",
                BuiltinLanguage.Spanish => "es-ES",
                BuiltinLanguage.French => "fr-FR",
                BuiltinLanguage.Japanese => "ja",
                BuiltinLanguage.Korean => "ko",
                BuiltinLanguage.Polish => "pl",
                BuiltinLanguage.BrazilianPortuguese => "pt-BR",
                BuiltinLanguage.Russian => "ru",
                BuiltinLanguage.Thai => "th",
                BuiltinLanguage.Turkish => "tr",
                BuiltinLanguage.TraditionalChinese => "zh-Hant",
                BuiltinLanguage.SimplifiedChinese => "zh-Hans",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            };
        }
    }
}