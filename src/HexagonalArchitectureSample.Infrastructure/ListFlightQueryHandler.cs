using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using HexagonalArchitectureSample.Domain;
using HexagonalArchitectureSample.UseCases;
using HexagonalArchitectureSample.UseCases.ListFlights;

namespace HexagonalArchitectureSample.Infrastructure
{
    public class ListFlightQueryHandler: IQueryHandler<ListFlightsQuery, IEnumerable<FlightSummary>>
    {
        private readonly SqlConnection _sqlConnection;

        public ListFlightQueryHandler(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<IEnumerable<FlightSummary>> HandleAsync(ListFlightsQuery query)
        {
            const string statement = @"
                SELECT 
                    [Id], 
                    JSON_VALUE([JsonDocument], '$.Capacity') AS [Capacity],
	                (
		                SELECT COALESCE(SUM([NumberOfBookedSeats]), 0)
		                FROM OPENJSON(JSON_QUERY([JsonDocument], '$.Bookings')) WITH (
			                [NumberOfBookedSeats] INT '$.NumberOfSeats'
		                )
	                ) AS [NumberOfBookedSeats]
                FROM [Flights]
            ";

            var rows = await _sqlConnection.QueryAsync<FlightSummaryRow>(statement);
            return rows.Select(x => x.ToFlightSummary());
        }

        private class FlightSummaryRow
        {
            public Guid Id { get; set; }
            public ushort Capacity { get; set; }
            public ushort NumberOfBookedSeats { get; set; }

            public FlightSummary ToFlightSummary()
            {
                return new FlightSummary(
                    new FlightId(Id),
                    Capacity,
                    NumberOfBookedSeats);
            }
        }
    }
}
