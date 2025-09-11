using System.Collections.Generic;
using Core.Localization;

namespace ShapezShifter.Fluent
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
            var database = (LocalizationDatabase)Globals.Localization.DatabaseProvider.CurrentDatabase;

            var newKeys = new Dictionary<string, string>();

            foreach (var entry in Entries)
            {
                newKeys.Add(entry.Key, entry.value.DefaultTranslation);
            }

            var translationParser = new TranslationParser().Parse(newKeys);
            foreach (var parsedTranslation in translationParser)
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