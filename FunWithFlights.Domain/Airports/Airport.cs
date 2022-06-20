using FunWithFlights.Core.Entities;
using Newtonsoft.Json;

namespace FunWithFlights.Domain.Airports;

public record Airport(
    AirportCode Code, 
    string Name, 
    string City, 
    string Country, 
    decimal Latitude, 
    decimal Longitude, 
    decimal Altitude
    ): IEntity<AirportCode>
{
    [JsonIgnore]
    public AirportCode Key => Code;
}