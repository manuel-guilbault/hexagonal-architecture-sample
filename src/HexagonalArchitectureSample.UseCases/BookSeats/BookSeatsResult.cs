using HexagonalArchitectureSample.Domain;

namespace HexagonalArchitectureSample.UseCases.BookSeats
{
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

        public class NotFound: BookSeatsResult
        {
        }

        public class WouldBeOverbooked: BookSeatsResult
        {
        }

        public class Outdated : BookSeatsResult
        {
        }
    }
}
