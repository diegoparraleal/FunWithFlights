using FunWithFlights.Core.Extensions;
using FunWithFlights.Domain.Airlines;
using FunWithFlights.Infrastructure.Contracts.Repositories;

namespace FunWithFlights.Infrastructure.InMemory.EmbeddedRepositories;

public class AirlineEmbeddedRepository: CsvEmbeddedRepository<AirlineCode, Airline>, IAirlineRepository
{
    public AirlineEmbeddedRepository(string resource) : base(resource)
    {
    }

    protected override Airline MapRecord(Dictionary<string, string> record) 
        => new (
            record.MaybeGet("Code").ValueOrDefault(),
            record.MaybeGet("Name").ValueOrDefault(),
            record.MaybeGet("Iata").ValueOrDefault(),
            record.MaybeGet("Country").ValueOrDefault(),
            record.MaybeGet("Active").Then(x => x == "Y").ValueOrDefault()
        );
}