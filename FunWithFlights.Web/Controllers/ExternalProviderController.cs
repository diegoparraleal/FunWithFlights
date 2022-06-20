using FunWithFlights.Business.Queries;
using FunWithFlights.Domain.ExternalProviders;
using FunWithFlights.Infrastructure.Contracts.Loggers;
using Microsoft.AspNetCore.Mvc;

namespace FunWithFlights.Web.Controllers;

[ApiController]
[Route("/api/externalProviders")]
public class ExternalProviderController : ControllerBase
{
    private readonly IExternalProviderQueries _externalProviderQueries;
    private static readonly ILogger Log = Logger.Initialize<ExternalProviderController>();

    public ExternalProviderController(
        IExternalProviderQueries externalProviderQueries)
    {
        _externalProviderQueries = externalProviderQueries;
    }

    [HttpGet]
    public async Task<IEnumerable<ExternalProvider>> GetAllAsync() => await _externalProviderQueries.GetAllAsync();
}