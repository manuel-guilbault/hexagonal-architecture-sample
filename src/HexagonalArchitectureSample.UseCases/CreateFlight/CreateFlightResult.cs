using HexagonalArchitectureSample.Domain;

namespace HexagonalArchitectureSample.UseCases.CreateFlight
{
    public abstract class CreateFlightResult
    {
        public class Created: CreateFlightResult
        {
            public Created(FlightId id)
            {
                Id = id;
            }

            public FlightId Id { get; }
        }
    }
}
