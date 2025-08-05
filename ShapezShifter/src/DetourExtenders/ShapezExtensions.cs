using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace ShapezShifter
{
    [PublicAPI]
    public static class ShapezExtensions
    {
        internal static IReadOnlyCollection<IExtender> Extenders => ExtendersMap.Values;
        internal static int Version;

        private static readonly Dictionary<ExtenderHandle, IExtender> ExtendersMap = new();
        private static ExtenderHandle LastHandle = ExtenderHandle.Invalid;

        public static ExtenderHandle AddExtender<TExtender>(TExtender extender) where TExtender : IExtender
        {
            LastHandle = LastHandle.Next();
            ExtendersMap.Add(LastHandle, extender);
            Version++;
            return LastHandle;
        }

        public static void RemoveExtender(ExtenderHandle extenderHandle)
        {
            ExtendersMap.Remove(extenderHandle);
            Version++;
        }

        public static void RemoveExtender<TExtender>(ExtenderHandle extenderHandle, out TExtender extender)
        {
            var removed = ExtendersMap.Remove(extenderHandle, out var ext);
            if (!removed)
            {
                throw new KeyNotFoundException();
            }

            Version++;
            if (ext is not TExtender typedExtender)
            {
                throw new InvalidCastException();
            }

            extender = typedExtender;
        }
    }

    public interface IExtender
    {
    }
}