namespace FunWithFlights.Domain.Airports;

public record AirportDistanceKey(AirportCode Source, AirportCode Destination)
{
    public static AirportDistanceKey FromRouteKey(RouteKey key) => new(key.Source, key.Destination);
}