using JetBrains.Annotations;

namespace FunWithFlights.Domain.ExternalProviders;

[UsedImplicitly]
public record ExternalRoute(
    string Airline,
    string SourceAirport,
    string DestinationAirport,
    string CodeShare,
    int Stops,
    string Equipment);
