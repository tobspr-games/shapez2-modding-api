using System;
using System.Collections.Generic;
using System.Linq;
using Core.Collections.Scoped;
using Core.Logging;

namespace ShapezShifter.Hijack
{
    internal class CachedStaticallyAccessibleRewirerProvider : IRewirerProvider, IDisposable
    {
        private readonly ILogger Logger;
        private int VersionCacheIsBased;

        private readonly MultiValueDictionary<Type, IRewirer, ScopedList<IRewirer>> DataPerTypeCache =
            new(ScopedList<IRewirer>.Get, ScopedList<IRewirer>.Return);

        private readonly ScopedHashSet<Type> CachedTypes = ScopedHashSet<Type>.Get();

        public CachedStaticallyAccessibleRewirerProvider(ILogger logger)
        {
            Logger = logger;
        }

        public IEnumerable<TRewirer> RewirersOfType<TRewirer>()
            where TRewirer : IRewirer
        {
            if (VersionCacheIsBased != GameRewirers.Version)
            {
                CachedTypes.Clear();
                DataPerTypeCache.Clear();
                VersionCacheIsBased = GameRewirers.Version;
            }


            // LogDictionary();

            if (!CachedTypes.Contains(typeof(TRewirer)))
            {
                UpdateCacheEntryForType<TRewirer>();
                CachedTypes.Add(typeof(TRewirer));
            }

            // LogDictionary();

            IEnumerable<TRewirer> rewirersList =
                DataPerTypeCache.TryGetValuesForKey(typeof(TRewirer), out ScopedList<IRewirer> list)
                    ? list.Cast<TRewirer>()
                    : Array.Empty<TRewirer>();

            return rewirersList;
        }

        private void LogDictionary()
        {
            Logger.Info?.Log($"KeyCount: {DataPerTypeCache.KeyCount}");
            Logger.Info?.Log($"ValueCount: {DataPerTypeCache.ValueCount()}");

            foreach (Type type in DataPerTypeCache.Keys)
            {
                if (!DataPerTypeCache.TryGetValuesForKey(type, out ScopedList<IRewirer> list))
                {
                    Logger.Error?.Log("Huh?");
                }
                else
                {
                    Logger.Info?.Log($"Type {type} has {list.Count} entries");
                    foreach (IRewirer rewirer in list)
                    {
                        Logger.Info?.Log($"\t {rewirer}");
                    }
                }
            }
        }

        private void UpdateCacheEntryForType<TData>()
        {
            using ScopedList<object> data = ScopedList<object>.Get();
            foreach (IRewirer obj in GameRewirers.Rewirers)
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