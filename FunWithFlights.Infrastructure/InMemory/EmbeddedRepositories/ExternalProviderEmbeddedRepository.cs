using FunWithFlights.Core.Extensions;
using FunWithFlights.Domain.ExternalProviders;
using FunWithFlights.Infrastructure.Contracts.Repositories;

namespace FunWithFlights.Infrastructure.InMemory.EmbeddedRepositories;

public class ExternalProviderEmbeddedRepository: CsvEmbeddedRepository<string, ExternalProvider>, IExternalProviderRepository
{
    public ExternalProviderEmbeddedRepository(string file) : base(file)
    {
    }

    protected override ExternalProvider MapRecord(Dictionary<string, string> record)
        => new (
            record.MaybeGet("Code").ValueOrDefault(),
            record.MaybeGet("Name").ValueOrDefault(),
            record.MaybeGet("Url").Then(x => new Uri(x)).ValueOrDefault()
        );
}