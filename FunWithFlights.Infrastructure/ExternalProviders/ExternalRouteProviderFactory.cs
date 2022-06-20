using FunWithFlights.Domain.ExternalProviders;
using FunWithFlights.Infrastructure.Contracts.ExternalProviders;

namespace FunWithFlights.Infrastructure.ExternalProviders;

public class ExternalRouteProviderFactory: IExternalRouteProviderFactory
{
    public IExternalRouteProvider Get(ExternalProvider externalProvider)
        => new ExternalRouteProvider(externalProvider);
}