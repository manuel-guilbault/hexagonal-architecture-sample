using HexagonalArchitectureSample.Domain;

namespace HexagonalArchitectureSample.UseCases.ListFlights
{
    public class FlightSummary
    {
        public FlightSummary(FlightId id, ushort capacity, ushort numberOfBookedSeats)
        {
            Id = id;
            Capacity = capacity;
            NumberOfBookedSeats = numberOfBookedSeats;
        }

        public FlightId Id { get; }
        public ushort Capacity { get; }
        public ushort NumberOfBookedSeats { get; }
        public ushort NumberOfAvailableSeats => (ushort)(Capacity - NumberOfBookedSeats);
    }
}
