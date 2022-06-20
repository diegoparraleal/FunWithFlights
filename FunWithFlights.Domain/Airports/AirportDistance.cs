using FunWithFlights.Core.Entities;

namespace FunWithFlights.Domain.Airports;

public record AirportDistance(AirportCode Source, AirportCode Destination, decimal Distance) : IEntity<AirportDistanceKey>
{ 
    public AirportDistanceKey Key => new (Source, Destination);
}