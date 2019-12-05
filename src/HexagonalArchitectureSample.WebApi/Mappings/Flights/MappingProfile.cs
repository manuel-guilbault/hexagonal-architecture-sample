using System;
using AutoMapper;
using HexagonalArchitectureSample.Domain;
using HexagonalArchitectureSample.UseCases.GetFlightDetails;
using HexagonalArchitectureSample.UseCases.ListFlights;
using HexagonalArchitectureSample.WebApi.Models.Flights;

namespace HexagonalArchitectureSample.WebApi.Mappings.Flights
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<FlightSummary, FlightSummaryViewModel>();
            CreateMap<FlightDetails, FlightDetailsViewModel>();
            CreateMap<Booking, BookingViewModel>();
            CreateMap<FlightId, Guid>().ConvertUsing(x => x.Value);
            CreateMap<BookingId, Guid>().ConvertUsing(x => x.Value);
        }
    }
}
