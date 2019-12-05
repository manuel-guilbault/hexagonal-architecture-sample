using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using HexagonalArchitectureSample.Domain;
using HexagonalArchitectureSample.UseCases;
using Newtonsoft.Json;

namespace HexagonalArchitectureSample.Infrastructure
{
    public class FlightRepository: IRepository<FlightId, Flight>
    {
        private readonly SqlConnection _sqlConnection;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public FlightRepository(SqlConnection sqlConnection, JsonSerializerSettings jsonSerializerSettings)
        {
            _sqlConnection = sqlConnection;
            _jsonSerializerSettings = jsonSerializerSettings;
        }

        public async Task CreateAsync(Flight flight)
        {
            const string statement = @"INSERT INTO [Flights] ([JsonDocument]) VALUES (@document)";

            await _sqlConnection.ExecuteAsync(statement, new
            {
                document = JsonConvert.SerializeObject(flight, _jsonSerializerSettings)
            });
        }

        public async Task<AggregateRoot<Flight>?> GetByIdAsync(FlightId id)
        {
            const string statement = @"SELECT [JsonDocument], [VersionTag] FROM [Flights] WHERE [Id] = @id";

            var row = await _sqlConnection.QueryFirstOrDefaultAsync<FlightRow>(
                statement, 
                new { id = id.Value });

            return row?.Deserialize(_jsonSerializerSettings);
        }

        public async Task UpdateAsync(AggregateRoot<Flight> aggregateRoot)
        {
            const string statement = @"UPDATE [Flights] SET [JsonDocument] = @document WHERE [Id] = @id AND [VersionTag] = @versionTag";

            var affectedRows = await _sqlConnection.ExecuteAsync(statement, new
            {
                document = JsonConvert.SerializeObject(aggregateRoot.Entity, _jsonSerializerSettings),
                id = aggregateRoot.Entity.Id.Value,
                versionTag = VersionTagConverter.ToBytes(aggregateRoot.VersionTag),
            });
            if (affectedRows == 0)
            {
                throw new DBConcurrencyException();
            }
        }

        private class FlightRow
        {
            public FlightRow(string jsonDocument, byte[] versionTag)
            {
                JsonDocument = jsonDocument;
                VersionTag = versionTag;
            }

            public string JsonDocument { get; }
            public byte[] VersionTag { get; }

            public AggregateRoot<Flight> Deserialize(JsonSerializerSettings jsonSerializerSettings)
            {
                var flight = JsonConvert.DeserializeObject<Flight>(JsonDocument, jsonSerializerSettings)
                    ?? throw new DataException($"JsonDocument is null.");
                var versionTag = VersionTagConverter.FromBytes(VersionTag);
                return  AggregateRoot.Create(flight, versionTag);
            }
        }
    }
}
