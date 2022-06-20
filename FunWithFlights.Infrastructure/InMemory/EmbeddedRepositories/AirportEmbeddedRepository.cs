using FunWithFlights.Core.Extensions;
using FunWithFlights.Domain.Airports;
using FunWithFlights.Infrastructure.Contracts.Repositories;

namespace FunWithFlights.Infrastructure.InMemory.EmbeddedRepositories;

public class AirportEmbeddedRepository: CsvEmbeddedRepository<AirportCode, Airport>, IAirportRepository
{
    public AirportEmbeddedRepository(string file) : base(file)
    {
    }

    protected override Airport MapRecord(Dictionary<string, string> record)
        => new (
            record.MaybeGet("Code").ValueOrDefault(),
            record.MaybeGet("Name").ValueOrDefault(),
            record.MaybeGet("City").ValueOrDefault(),
            record.MaybeGet("Country").ValueOrDefault(),
            record.MaybeGet("Latitude").Then(decimal.Parse).ValueOrDefault(),
            record.MaybeGet("Longitude").Then(decimal.Parse).ValueOrDefault(),
            record.MaybeGet("Altitude").Then(decimal.Parse).ValueOrDefault()
        );
}