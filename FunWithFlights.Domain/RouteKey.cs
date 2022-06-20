using FunWithFlights.Domain.Airlines;
using FunWithFlights.Domain.Airports;
using FunWithFlights.Domain.ExternalProviders;

namespace FunWithFlights.Domain;

public record RouteKey
{
    private readonly int _hashCode;
   
    public AirportCode Source { get; }
    public AirportCode Destination { get; }
    public AirlineCode Airline { get; }

    public RouteKey(AirportCode source, AirportCode destination, AirlineCode airline)
    {
        Source = source;
        Destination = destination;
        Airline = airline;
        _hashCode = CalculateHashCode();
    }

    public override int GetHashCode() => _hashCode;
    private int CalculateHashCode() => $"{Source}{Destination}{Airline}".GetHashCode();

    public static RouteKey FromExternalRoute(ExternalRoute externalRoute)
        => new(externalRoute.SourceAirport, externalRoute.DestinationAirport, externalRoute.Airline);
  
}