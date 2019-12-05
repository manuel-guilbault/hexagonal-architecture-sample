using HexagonalArchitectureSample.Domain;

namespace HexagonalArchitectureSample.UseCases.BookSeats
{
    public class BookSeatsCommand
    {
        public BookSeatsCommand(FlightId flightId, VersionTag? versionTag, ushort numberOfSeats)
        {
            FlightId = flightId;
            VersionTag = versionTag;
            NumberOfSeats = numberOfSeats;
        }

        public FlightId FlightId { get; }
        public VersionTag? VersionTag { get; }
        public ushort NumberOfSeats { get; }
    }
}
