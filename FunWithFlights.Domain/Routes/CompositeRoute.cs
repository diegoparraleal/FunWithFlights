using FunWithFlights.Domain.Airports;

namespace FunWithFlights.Domain.Routes;

public record CompositeRoute(AirportCode Source, AirportCode Destination, int NumStops, decimal Distance, IEnumerable<Route> Routes);