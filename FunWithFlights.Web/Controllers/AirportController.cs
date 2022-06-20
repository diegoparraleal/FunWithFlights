using FunWithFlights.Business.Queries;
using FunWithFlights.Domain.Airports;
using FunWithFlights.Infrastructure.Contracts.Loggers;
using Microsoft.AspNetCore.Mvc;

namespace FunWithFlights.Web.Controllers;

[ApiController]
[Route("/api/airports")]
public class AirportController : ControllerBase
{
    private readonly IAirportQueries _airportQueries;
    private static readonly ILogger Log = Logger.Initialize<AirportController>();

    public AirportController(
        IAirportQueries airportQueries)
    {
        _airportQueries = airportQueries;
    }

    [HttpGet]
    public async Task<IEnumerable<Airport>> GetAllAsync() => await _airportQueries.GetAllAsync();
}