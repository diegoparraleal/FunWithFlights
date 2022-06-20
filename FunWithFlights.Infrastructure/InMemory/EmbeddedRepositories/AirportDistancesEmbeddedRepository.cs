using FunWithFlights.Core.Extensions;
using FunWithFlights.Domain.Airports;
using FunWithFlights.Infrastructure.Contracts.Repositories;

namespace FunWithFlights.Infrastructure.InMemory.EmbeddedRepositories;

public class AirportDistancesEmbeddedRepository: CsvEmbeddedRepository<AirportDistanceKey, AirportDistance>, IAirportDistancesRepository
{
    public AirportDistancesEmbeddedRepository(string file) : base(file)
    {
    }

    protected override AirportDistance MapRecord(Dictionary<string, string> record)
        => new (
            record.MaybeGet("Source").ValueOrDefault(),
            record.MaybeGet("Destination").ValueOrDefault(),
            record.MaybeGet("Distance").Then(decimal.Parse).ValueOrDefault()
        );
}