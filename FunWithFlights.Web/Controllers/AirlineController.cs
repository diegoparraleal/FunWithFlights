using FunWithFlights.Business.Queries;
using FunWithFlights.Domain.Airlines;
using FunWithFlights.Infrastructure.Contracts.Loggers;
using Microsoft.AspNetCore.Mvc;

namespace FunWithFlights.Web.Controllers;

[ApiController]
[Route("/api/airlines")]
public class AirlineController : ControllerBase
{
    private readonly IAirlineQueries _airlineQueries;
    private static readonly ILogger Log = Logger.Initialize<AirlineController>();

    public AirlineController(
        IAirlineQueries airlineQueries)
    {
        _airlineQueries = airlineQueries;
    }

    [HttpGet]
    public async Task<IEnumerable<Airline>> GetAllAsync() => await _airlineQueries.GetAllAsync();
}