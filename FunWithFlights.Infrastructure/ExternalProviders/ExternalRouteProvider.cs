using FunWithFlights.Core.Extensions;
using FunWithFlights.Domain.ExternalProviders;
using FunWithFlights.Infrastructure.Contracts.ExternalProviders;
using Newtonsoft.Json;

namespace FunWithFlights.Infrastructure.ExternalProviders;

public class ExternalRouteProvider: IExternalRouteProvider
{
    private readonly ExternalProvider _externalProvider;

    public ExternalRouteProvider(ExternalProvider externalProvider)
    {
        _externalProvider = externalProvider;
    }
    
    public async Task<IReadOnlyCollection<ExternalRoute>> GetAllRoutesAsync()
    {
        using var client = new HttpClient();
        var stream = await client.GetStreamAsync(_externalProvider.Url.AbsoluteUri);
        using JsonTextReader reader = new JsonTextReader(new StreamReader(stream));
        JsonSerializer serializer = new JsonSerializer();
        var routes = serializer.Deserialize<IEnumerable<ExternalRoute>>(reader);
        return routes.AsReadOnly();
    }
}