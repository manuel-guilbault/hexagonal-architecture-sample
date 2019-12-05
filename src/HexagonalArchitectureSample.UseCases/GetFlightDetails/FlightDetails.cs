using System.Collections.Generic;
using HexagonalArchitectureSample.Domain;

namespace HexagonalArchitectureSample.UseCases.GetFlightDetails
{
    public class FlightDetails
    {
        public FlightDetails(
            FlightId id,
            VersionTag versionTag,  
            ushort capacity,
            IEnumerable<Booking> bookings)
        {
            Id = id;
            VersionTag = versionTag;
            Capacity = capacity;
            Bookings = bookings;
        }

        public FlightId Id { get; }
        public VersionTag VersionTag { get; }
        public ushort Capacity { get; }
        public IEnumerable<Booking> Bookings { get; }
    }
}
