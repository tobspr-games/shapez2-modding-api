using System;
using System.Collections.Generic;
using System.Linq;
using Core.Collections.Scoped;
using Core.Logging;

namespace ShapezShifter
{
    internal class CachedStaticallyAccessibleExtendersProvider : IExtendersProvider, IDisposable
    {
        private readonly ILogger Logger;
        private int VersionCacheIsBased;

        private readonly MultiValueDictionary<Type, IExtender, ScopedList<IExtender>> DataPerTypeCache =
            new(ScopedList<IExtender>.Get, ScopedList<IExtender>.Return);

        private readonly ScopedHashSet<Type> CachedTypes = ScopedHashSet<Type>.Get();

        public CachedStaticallyAccessibleExtendersProvider(ILogger logger)
        {
            Logger = logger;
        }

        public IEnumerable<TExtender> ExtendersOfType<TExtender>()
        {
            if (VersionCacheIsBased != ShapezExtensions.Version)
            {
                CachedTypes.Clear();
                DataPerTypeCache.Clear();
                VersionCacheIsBased = ShapezExtensions.Version;
            }


            LogDictionary();

            if (!CachedTypes.Contains(typeof(TExtender)))
            {
                UpdateCacheEntryForType<TExtender>();
                CachedTypes.Add(typeof(TExtender));
            }

            LogDictionary();

            var extenderList = DataPerTypeCache.TryGetValuesForKey(typeof(TExtender), out var list)
                ? list.Cast<TExtender>()
                : Array.Empty<TExtender>();

            return extenderList;
        }

        private void LogDictionary()
        {
            Logger.Info?.Log($"KeyCount: {DataPerTypeCache.KeyCount}");
            Logger.Info?.Log($"ValueCount: {DataPerTypeCache.ValueCount()}");

            foreach (var type in DataPerTypeCache.Keys)
            {
                if (!DataPerTypeCache.TryGetValuesForKey(type, out var list))
                {
                    Logger.Error?.Log("Huh?");
                }
                else
                {
                    Logger.Info?.Log($"Type {type} has {list.Count} entries");
                    foreach (var extender in list)
                    {
                        Logger.Info?.Log($"\t {extender}");
                    }
                }
            }
        }

        private void UpdateCacheEntryForType<TData>()
        {
            using var data = ScopedList<object>.Get();
            foreach (var obj in ShapezExtensions.Extenders)
            {
                if (obj is TData)
                {
                    DataPerTypeCache.AddValue(typeof(TData), obj);
                }
            }
        }

        public void Dispose()
        {
            DataPerTypeCache.Dispose();
        }
    }
}