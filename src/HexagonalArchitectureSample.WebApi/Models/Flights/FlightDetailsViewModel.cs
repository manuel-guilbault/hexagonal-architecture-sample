using System;
using System.Collections.Generic;

namespace HexagonalArchitectureSample.WebApi.Models.Flights
{
    public class FlightDetailsViewModel
    {
        public Guid Id { get; set; }
        public ushort Capacity { get; set; }
        public IEnumerable<BookingViewModel> Bookings { get; set; }
    }
}
