using FunWithFlights.Core.Entities;
using FunWithFlights.Domain.Airlines;
using FunWithFlights.Domain.Airports;
using FunWithFlights.Domain.CodeShares;
using FunWithFlights.Domain.Equipments;
using Newtonsoft.Json;

namespace FunWithFlights.Domain.Routes;

public record Route(
    Airline Airline,
    Airport Source,
    Airport Destination,
    CodeShare CodeShare,
    int Stops,
    Equipment? Equipment): IEntity<RouteKey>
{
    [JsonIgnore]
    public RouteKey Key => new (Source.Key, Destination.Key, Airline.Code);
}