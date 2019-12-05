using HexagonalArchitectureSample.Domain;

namespace HexagonalArchitectureSample.UseCases.UpdateFlightCapacity
{
    public class UpdateFlightCapacityCommand
    {
        public UpdateFlightCapacityCommand(FlightId flightId, VersionTag? versionTag, ushort capacity)
        {
            FlightId = flightId;
            VersionTag = versionTag;
            Capacity = capacity;
        }

        public FlightId FlightId { get; }
        public VersionTag? VersionTag { get; }
        public ushort Capacity { get; }
    }
}
