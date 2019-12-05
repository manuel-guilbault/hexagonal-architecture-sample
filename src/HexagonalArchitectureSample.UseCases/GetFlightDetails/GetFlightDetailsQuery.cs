using HexagonalArchitectureSample.Domain;

namespace HexagonalArchitectureSample.UseCases.GetFlightDetails
{
    public class GetFlightDetailsQuery
    {
        public GetFlightDetailsQuery(FlightId flightId)
        {
            FlightId = flightId;
        }

        public FlightId FlightId { get; }
    }
}
