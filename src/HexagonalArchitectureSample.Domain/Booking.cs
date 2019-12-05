using System;

namespace HexagonalArchitectureSample.Domain
{
    public readonly struct Booking: IEquatable<Booking>
    {
        public Booking(BookingId id, ushort numberOfSeats)
        {
            Id = id;
            NumberOfSeats = numberOfSeats;
        }

        public BookingId Id { get; }
        public ushort NumberOfSeats { get; }

        public bool Equals(Booking other)
        {
            return Id.Equals(other.Id) && NumberOfSeats == other.NumberOfSeats;
        }

        public override bool Equals(object? obj)
        {
            return obj is Booking other && Equals(other);
        }

        public override int GetHashCode()
        {
            return unchecked((Id.GetHashCode() * 397) ^ NumberOfSeats.GetHashCode());
        }
    }
}
