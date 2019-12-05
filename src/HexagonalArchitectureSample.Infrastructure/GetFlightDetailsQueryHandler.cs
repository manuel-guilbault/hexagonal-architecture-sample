using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using HexagonalArchitectureSample.Domain;
using HexagonalArchitectureSample.UseCases;
using HexagonalArchitectureSample.UseCases.GetFlightDetails;
using Newtonsoft.Json;

namespace HexagonalArchitectureSample.Infrastructure
{
    public class GetFlightDetailsQueryHandler: IQueryHandler<GetFlightDetailsQuery, FlightDetails?>
    {
        private readonly SqlConnection _sqlConnection;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public GetFlightDetailsQueryHandler(SqlConnection sqlConnection, JsonSerializerSettings jsonSerializerSettings)
        {
            _sqlConnection = sqlConnection;
            _jsonSerializerSettings = jsonSerializerSettings;
        }

        public async Task<FlightDetails?> HandleAsync(GetFlightDetailsQuery query)
        {
            const string statement = @"
                SELECT
                    [Id],
	                [VersionTag],
	                JSON_VALUE([JsonDocument], '$.Capacity') as [Capacity],
	                JSON_QUERY([JsonDocument], '$.Bookings') as [Bookings]
                FROM [Flights]
                WHERE [Id] = @id
            ";

            var row = await _sqlConnection.QueryFirstOrDefaultAsync<FlightRow>(
                statement,
                new { id = query.FlightId.Value });

            return row?.Deserialize(_jsonSerializerSettings);
        }

        private class FlightRow
        {
            public Guid Id { get; set; }
            public byte[] VersionTag { get; set; }
            public ushort Capacity { get; set; }
            public string Bookings { get; set; }

            public FlightDetails Deserialize(JsonSerializerSettings jsonSerializerSettings)
            {
                var versionTag = VersionTagConverter.FromBytes(VersionTag);
                var bookings = JsonConvert.DeserializeObject<IEnumerable<Booking>>(Bookings, jsonSerializerSettings)
                    ?? throw new DataException($"JsonDocument is null.");
                return new FlightDetails(
                    new FlightId(Id),
                    versionTag,
                    Capacity,
                    bookings);
            }
        }
    }
}
