using System.Threading.Tasks;
using HexagonalArchitectureSample.Domain;

namespace HexagonalArchitectureSample.UseCases.UpdateFlightCapacity
{
    public class UpdateFlightCapacityCommandHandler: ICommandHandler<UpdateFlightCapacityCommand, UpdateFlightCapacityResult>
    {
        private readonly IRepository<FlightId, Flight> _flightRepository;

        public UpdateFlightCapacityCommandHandler(IRepository<FlightId, Flight> flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public async Task<UpdateFlightCapacityResult> HandleAsync(UpdateFlightCapacityCommand command)
        {
            var aggregateRoot = await _flightRepository.GetByIdAsync(command.FlightId);
            if (aggregateRoot == null)
            {
                return new UpdateFlightCapacityResult.NotFound();
            }

            if (!aggregateRoot.IsUpToDate(command.VersionTag))
            {
                return new UpdateFlightCapacityResult.Outdated();
            }

            var flight = aggregateRoot.Entity;

            var result = flight.UpdateCapacity(command.Capacity);

            return result switch
            {
                Flight.UpdateCapacityResult.Updated _ => await UpdateAsync(aggregateRoot),
                Flight.UpdateCapacityResult.WouldBeOverbooked _ => new UpdateFlightCapacityResult.WouldBeOverbooked(),
            };

            async Task<UpdateFlightCapacityResult> UpdateAsync(AggregateRoot<Flight> ar)
            {
                await _flightRepository.UpdateAsync(ar);
                return new UpdateFlightCapacityResult.Updated();
            }
        }
    }
}
