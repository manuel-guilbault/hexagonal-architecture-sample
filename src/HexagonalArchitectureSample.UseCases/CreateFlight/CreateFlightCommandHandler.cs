using System.Threading.Tasks;
using HexagonalArchitectureSample.Domain;

namespace HexagonalArchitectureSample.UseCases.CreateFlight
{
    public class CreateFlightCommandHandler: ICommandHandler<CreateFlightCommand, CreateFlightResult>
    {
        private readonly IRepository<FlightId, Flight> _flightRepository;

        public CreateFlightCommandHandler(IRepository<FlightId, Flight> flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public async Task<CreateFlightResult> HandleAsync(CreateFlightCommand command)
        {
            var flight = Flight.CreateNew(command.Capacity);
            await _flightRepository.CreateAsync(flight);
            return new CreateFlightResult.Created(flight.Id);
        }
    }
}
