using System;

namespace HexagonalArchitectureSample.UseCases
{
    public readonly struct VersionTag: IEquatable<VersionTag>
    {
        private readonly string _value;

        public VersionTag(string value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value;
        }

        public bool Equals(VersionTag other)
        {
            return _value == other._value;
        }

        public override bool Equals(object? obj)
        {
            return obj is VersionTag other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
