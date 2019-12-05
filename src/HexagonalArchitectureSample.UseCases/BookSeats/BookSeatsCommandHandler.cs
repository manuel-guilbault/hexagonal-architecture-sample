using System;
using System.Threading.Tasks;
using HexagonalArchitectureSample.Domain;

namespace HexagonalArchitectureSample.UseCases.BookSeats
{
    public class BookSeatsCommandHandler: ICommandHandler<BookSeatsCommand, BookSeatsResult>
    {
        private readonly IRepository<FlightId, Flight> _flightRepository;

        public BookSeatsCommandHandler(IRepository<FlightId, Flight> flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public async Task<BookSeatsResult> HandleAsync(BookSeatsCommand command)
        {
            var aggregateRoot = await _flightRepository.GetByIdAsync(command.FlightId);
            if (aggregateRoot == null)
            {
                return new BookSeatsResult.NotFound();
            }

            if (!aggregateRoot.IsUpToDate(command.VersionTag))
            {
                return new BookSeatsResult.Outdated();
            }

            var flight = aggregateRoot.Entity;

            var result = flight.BookSeats(command.NumberOfSeats);
            return result switch
            {
                Flight.BookSeatsResult.Booked booked => await UpdateAsync(aggregateRoot, booked.BookingId),
                Flight.BookSeatsResult.WouldBeOverbooked _ => new BookSeatsResult.WouldBeOverbooked(),
            };

            async Task<BookSeatsResult> UpdateAsync(AggregateRoot<Flight> ar, BookingId bookingId)
            {
                await _flightRepository.UpdateAsync(ar);
                return new BookSeatsResult.Booked(bookingId);
            }
        }
    }
}
