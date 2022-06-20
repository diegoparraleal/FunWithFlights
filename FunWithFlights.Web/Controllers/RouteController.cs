using FunWithFlights.Business.Aggregators;
using FunWithFlights.Business.Commands;
using FunWithFlights.Domain.ExternalProviders;
using FunWithFlights.Infrastructure.Contracts.Loggers;
using Microsoft.AspNetCore.Mvc;

namespace FunWithFlights.Web.Controllers;

[ApiController]
[Route("/api/routes")]
public class RouteController : ControllerBase
{
    private readonly ICachedExternalRouteAggregator _externalRouteAggregator;
    private readonly ISearchRouteCommand _searchRouteCommand;
    private static readonly ILogger Log = Logger.Initialize<AirlineController>();

    public RouteController(ICachedExternalRouteAggregator externalRouteAggregator, ISearchRouteCommand searchRouteCommand)
    {
        _externalRouteAggregator = externalRouteAggregator;
        _searchRouteCommand = searchRouteCommand;
    }

    [HttpGet]
    public async Task<IEnumerable<ExternalRoute>> GetAllAsync() => await _externalRouteAggregator.ExecuteAsync();
    
    [HttpGet]
    [Route("/api/routes/from/{from}/to/{to}")]
    public async Task<SearchRouteResult> SearchRoutesAsync([FromRoute] string from, [FromRoute] string to) 
        => await _searchRouteCommand.ExecuteAsync(new SearchRouteParameters(from, to));
}