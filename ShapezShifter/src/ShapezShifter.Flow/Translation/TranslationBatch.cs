using System.Collections.Generic;
using Core.Localization;

namespace ShapezShifter.Flow
{
    public static class TranslationBatch
    {
        public static ITranslationBatchBuilder Begin()
        {
            return new TranslationBatchBuilder();
        }
    }

    public static class TranslationEntry
    {
        public static ITranslationEntryBuilder WithDefault(string text)
        {
            return new TranslationEntryBuilder(text);
        }
    }

    public class TranslationBatchBuilder : ITranslationBatchBuilder
    {
        private readonly Dictionary<string, ITranslationEntryBuilder> Entries = new();

        public ITranslationBatchBuilder AddEntry(string id, ITranslationEntryBuilder translationEntry)
        {
            Entries.Add(id, translationEntry);
            return this;
        }

        public void Flush()
        {
            LocalizationDatabase database = (LocalizationDatabase)Globals.Localization.DatabaseProvider.CurrentDatabase;

            var newKeys = new Dictionary<string, string>();

            foreach (KeyValuePair<string, ITranslationEntryBuilder> entry in Entries)
            {
                newKeys.Add(entry.Key, entry.value.DefaultTranslation);
            }

            Dictionary<TranslationId, ParsedTranslation> translationParser = new TranslationParser().Parse(newKeys);
            foreach (KeyValuePair<TranslationId, ParsedTranslation> parsedTranslation in translationParser)
            {
                database.Entries.Add(parsedTranslation.Key, parsedTranslation.Value);
            }
        }
    }

    public interface ITranslationBatchBuilder
    {
        ITranslationBatchBuilder AddEntry(string id, ITranslationEntryBuilder translationEntry);
        void Flush();
    }

    public class TranslationEntryBuilder : ITranslationEntryBuilder
    {
        public string DefaultTranslation { get; }
        private readonly Dictionary<BuiltinLanguage, string> _Translations = new();

        public IReadOnlyDictionary<BuiltinLanguage, string> Translations => _Translations;

        public TranslationEntryBuilder(string defaultTranslation)
        {
            DefaultTranslation = defaultTranslation;
        }

        public ITranslationEntryBuilder AppendTranslation(BuiltinLanguage language, string translation)
        {
            _Translations.Add(language, translation);
            return this;
        }
    }

    public interface ITranslationEntryBuilder
    {
        string DefaultTranslation { get; }
        ITranslationEntryBuilder AppendTranslation(BuiltinLanguage language, string translation);

        IReadOnlyDictionary<BuiltinLanguage, string> Translations { get; }
    }
}