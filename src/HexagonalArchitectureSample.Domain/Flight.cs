using System.Collections.Generic;
using System.Linq;

namespace HexagonalArchitectureSample.Domain
{
    public class Flight
    {
        public static Flight CreateNew(ushort capacity)
        {
            return new Flight(
                FlightId.CreateNew(),
                capacity,
                Enumerable.Empty<Booking>());
        }

        private readonly List<Booking> _bookings;

        public Flight(
            FlightId id, 
            ushort capacity, 
            IEnumerable<Booking> bookings)
        {
            Id = id;
            Capacity = capacity;
            _bookings = new List<Booking>(bookings);
        }

        public FlightId Id { get; }
        public ushort Capacity { get; private set; }
        public IEnumerable<Booking> Bookings => _bookings.AsReadOnly();

        private uint NumberOfBookedSeats => (uint)Bookings.Sum(x => x.NumberOfSeats);

        private bool CanUpdateCapacity(ushort newCapacity)
        {
            return newCapacity >= NumberOfBookedSeats;
        }

        public UpdateCapacityResult UpdateCapacity(ushort newCapacity)
        {
            if (!CanUpdateCapacity(newCapacity))
            {
                return new UpdateCapacityResult.WouldBeOverbooked();
            }

            Capacity = newCapacity;
            return new UpdateCapacityResult.Updated();
        }

        public abstract class UpdateCapacityResult
        {
            public class Updated : UpdateCapacityResult
            {
            }

            public class WouldBeOverbooked : UpdateCapacityResult
            {
            }
        }

        private bool CanBookSeats(ushort numberOfSeats)
        {
            return (NumberOfBookedSeats + numberOfSeats) <= Capacity;
        }

        public BookSeatsResult BookSeats(ushort numberOfSeats)
        {
            if (!CanBookSeats(numberOfSeats))
            {
                return new BookSeatsResult.WouldBeOverbooked();
            }

            var bookingId = BookingId.CreateNew();
            _bookings.Add(new Booking(bookingId, numberOfSeats));
            return new BookSeatsResult.Booked(bookingId);
        }

        public abstract class BookSeatsResult
        {
            public class Booked: BookSeatsResult
            {
                public Booked(BookingId bookingId)
                {
                    BookingId = bookingId;
                }

                public BookingId BookingId { get; }
            }

            public class WouldBeOverbooked: BookSeatsResult
            {
            }
        }
    }
}
