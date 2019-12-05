using System;

namespace HexagonalArchitectureSample.WebApi.Models.Flights
{
    public class BookingViewModel
    {
        public Guid Id { get; set; }
        public ushort NumberOfSeats { get; set; }
    }
}
