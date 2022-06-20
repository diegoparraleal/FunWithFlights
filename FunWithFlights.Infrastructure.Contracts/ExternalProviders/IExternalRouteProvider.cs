using FunWithFlights.Domain.ExternalProviders;

namespace FunWithFlights.Infrastructure.Contracts.ExternalProviders;

public interface IExternalRouteProvider
{
    Task<IReadOnlyCollection<ExternalRoute>> GetAllRoutesAsync();
}