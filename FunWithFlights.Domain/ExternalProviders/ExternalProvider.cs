using FunWithFlights.Core.Entities;

namespace FunWithFlights.Domain.ExternalProviders;

public record ExternalProvider(string Code, string Source, Uri Url): IEntity<string>
{
    public string Key => Code;
}
