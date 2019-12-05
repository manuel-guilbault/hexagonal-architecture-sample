using System;

namespace HexagonalArchitectureSample.WebApi.Models.Flights
{
    public class SeatsBookedViewModel
    {
        public SeatsBookedViewModel(Guid bookingId)
        {
            BookingId = bookingId;
        }

        public Guid BookingId { get; }
    }
}
