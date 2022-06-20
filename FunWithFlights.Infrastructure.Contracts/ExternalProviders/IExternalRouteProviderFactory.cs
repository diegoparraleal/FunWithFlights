using FunWithFlights.Domain.ExternalProviders;

namespace FunWithFlights.Infrastructure.Contracts.ExternalProviders;

public interface IExternalRouteProviderFactory
{
    IExternalRouteProvider Get(ExternalProvider externalProvider);
}