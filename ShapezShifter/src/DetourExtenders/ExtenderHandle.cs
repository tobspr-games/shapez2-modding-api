using System;
using System.Runtime.CompilerServices;

namespace ShapezShifter
{
    public readonly struct ExtenderHandle : IEquatable<ExtenderHandle>
    {
        internal static readonly ExtenderHandle Invalid = default;
        private readonly int Id;

        private ExtenderHandle(int id)
        {
            Id = id;
        }

        /// <inheritdoc />
        public bool Equals(ExtenderHandle other)
        {
            return Id == other.Id;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is ExtenderHandle other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Id.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ExtenderHandle lhs, ExtenderHandle rhs)
        {
            return lhs.Id == rhs.Id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ExtenderHandle lhs, ExtenderHandle rhs)
        {
            return lhs.Id != rhs.Id;
        }

        internal ExtenderHandle Next()
        {
            return new ExtenderHandle(Id + 1);
        }
    }
}