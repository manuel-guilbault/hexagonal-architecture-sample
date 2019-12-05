using System;

namespace HexagonalArchitectureSample.WebApi.Models.Flights
{
    public class FlightSummaryViewModel
    {
        public Guid Id { get; set; }
        public ushort Capacity { get; set; }
        public ushort NumberOfBookedSeats { get; set; }
        public ushort NumberOfAvailableSeats { get; set; }
    }
}
