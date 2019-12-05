using System;

namespace HexagonalArchitectureSample.Domain
{
    public readonly struct FlightId : IEquatable<FlightId>
    {
        public static FlightId CreateNew()
        {
            return new FlightId(Guid.NewGuid());
        }

        public FlightId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public bool Equals(FlightId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return obj is FlightId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
