using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HexagonalArchitectureSample.Domain;
using HexagonalArchitectureSample.UseCases;
using HexagonalArchitectureSample.UseCases.BookSeats;
using HexagonalArchitectureSample.UseCases.CreateFlight;
using HexagonalArchitectureSample.UseCases.GetFlightDetails;
using HexagonalArchitectureSample.UseCases.ListFlights;
using HexagonalArchitectureSample.UseCases.UpdateFlightCapacity;
using HexagonalArchitectureSample.WebApi.ActionResults;
using HexagonalArchitectureSample.WebApi.Models.Flights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HexagonalArchitectureSample.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(IEnumerable<FlightSummaryViewModel>), StatusCodes.Status200OK)]
    public class FlightsController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<FlightSummaryViewModel>> GetAllFlights(
            [FromServices] IQueryHandler<ListFlightsQuery, IEnumerable<FlightSummary>> queryHandler,
            [FromServices] IMapper mapper)
        {
            var query = new ListFlightsQuery();

            var flightSummaries = await queryHandler.HandleAsync(query);

            var viewModels = flightSummaries.Select(mapper.Map<FlightSummaryViewModel>);
            return viewModels;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateFlight(
            [FromServices] ICommandHandler<CreateFlightCommand, CreateFlightResult> commandHandler,
            [FromServices] IQueryHandler<GetFlightDetailsQuery, FlightDetails?> queryHandler,
            [FromServices] IMapper mapper,
            [FromBody] CreateFlightInputModel inputModel)
        {
            var command = new CreateFlightCommand(inputModel.Capacity);

            var result = await commandHandler.HandleAsync(command);

            return result switch
            {
                CreateFlightResult.Created created => CreatedAtAction(
                    nameof(GetFlightById),
                    new {id = created.Id.Value},
                    null),
            };
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FlightDetailsViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFlightById(
            [FromServices] IQueryHandler<GetFlightDetailsQuery, FlightDetails?> queryHandler,
            [FromServices] IMapper mapper,
            [FromRoute] Guid id)
        {
            var query = new GetFlightDetailsQuery(new FlightId(id));

            var flightDetails = await queryHandler.HandleAsync(query);
            if (flightDetails == null)
            {
                return NotFound();
            }

            var viewModel = mapper.Map<FlightDetailsViewModel>(flightDetails);
            return new EntityResult(viewModel, flightDetails.VersionTag);
        }

        [HttpPut("{id}/capacity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateFlightCapacity(
            [FromServices] ICommandHandler<UpdateFlightCapacityCommand, UpdateFlightCapacityResult> commandHandler,
            [FromRoute] Guid id,
            [FromHeader(Name = "if-match")] string? ifMatch,
            [FromBody] UpdateFlightCapacityInputModel inputModel)
        {
            var command = new UpdateFlightCapacityCommand(
                new FlightId(id),
                ConvertETagToVersionTag(ifMatch),
                inputModel.Capacity);

            var result = await commandHandler.HandleAsync(command);

            return result switch
            {
                UpdateFlightCapacityResult.Updated _ => Ok(),
                UpdateFlightCapacityResult.NotFound _ => NotFound(),
                UpdateFlightCapacityResult.Outdated _ => StatusCode(StatusCodes.Status412PreconditionFailed),
                UpdateFlightCapacityResult.WouldBeOverbooked _ => Conflict(),
            };
        }

        [HttpPost("{id}/bookings")]
        [ProducesResponseType(typeof(SeatsBookedViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> BookSeats(
            [FromServices] ICommandHandler<BookSeatsCommand, BookSeatsResult> commandHandler,
            [FromRoute] Guid id,
            [FromHeader(Name = "if-match")] string? ifMatch,
            [FromBody] BookSeatsInputModel inputModel)
        {
            var command = new BookSeatsCommand(
                new FlightId(id),
                ConvertETagToVersionTag(ifMatch),
                inputModel.NumberOfSeats);

            var result = await commandHandler.HandleAsync(command);

            return result switch
            {
                BookSeatsResult.Booked booked => Ok(new SeatsBookedViewModel(booked.BookingId.Value)),
                BookSeatsResult.NotFound _ => NotFound() as IActionResult,
                BookSeatsResult.WouldBeOverbooked _ => Conflict(),
            };
        }

        private static VersionTag? ConvertETagToVersionTag(string? ifMatch)
        {
            return ifMatch != null
                ? new VersionTag(ifMatch.Substring(1, ifMatch.Length - 2))
                : (VersionTag?)null;
        }
    }
}
