using System;

namespace HexagonalArchitectureSample.Domain
{
    public readonly struct BookingId: IEquatable<BookingId>
    {
        public static BookingId CreateNew()
        {
            return new BookingId(Guid.NewGuid());
        }

        public BookingId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public bool Equals(BookingId other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is BookingId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
