using System.Net;
using FunWithFlights.Core.Extensions;
using FunWithFlights.Domain.ExternalProviders;
using FunWithFlights.Infrastructure.Contracts.ExternalProviders;
using FunWithFlights.Infrastructure.Contracts.Loggers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;

namespace FunWithFlights.Infrastructure.ExternalProviders;

public class ExternalRouteProvider: IExternalRouteProvider
{
    private readonly ExternalProvider _externalProvider;
    private const int NumberOfRetries = 2;
    private const int TimeoutSeconds = 10;
    private static readonly ILogger Log = Logger.Initialize<ExternalRouteProvider>();
    
    private static readonly HttpStatusCode[] HttpStatusCodesWorthRetrying = {
        HttpStatusCode.RequestTimeout, // 408
        HttpStatusCode.InternalServerError, // 500
        HttpStatusCode.BadGateway, // 502
        HttpStatusCode.ServiceUnavailable, // 503
        HttpStatusCode.GatewayTimeout // 504
    };
   
    private static readonly AsyncRetryPolicy RetryPolicy = 
        Policy.Handle<KnownException>()
        .WaitAndRetryAsync(NumberOfRetries, x => TimeSpan.FromMilliseconds(Math.Pow(2, x)));

    public ExternalRouteProvider(ExternalProvider externalProvider)
    {
        _externalProvider = externalProvider;
    }
    
    public async Task<IReadOnlyCollection<ExternalRoute>> GetAllRoutesAsync() 
        => await RetryPolicy.ExecuteAsync(InternalGetAllRoutesAsync);

    private async Task<IReadOnlyCollection<ExternalRoute>> InternalGetAllRoutesAsync()
    {
        try
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(TimeoutSeconds);
            var stream = await client.GetStreamAsync(_externalProvider.Url.AbsoluteUri);
            
            using var reader = new JsonTextReader(new StreamReader(stream));
            var serializer = new JsonSerializer();
            var routes = serializer.Deserialize<IEnumerable<ExternalRoute>>(reader);
            if (routes == null) return Array.Empty<ExternalRoute>();

            return routes.AsReadOnly();
        }
        catch (TaskCanceledException ex)
        {
            Log.LogError(ex, "Timeout invoking external provider");
            throw new KnownException(HttpStatusCode.RequestTimeout);
        }
        catch (HttpRequestException ex)
        {
            Log.LogError(ex, "Error invoking external provider");
            var statusCode = ex.StatusCode ?? HttpStatusCode.RequestTimeout;
            if (HttpStatusCodesWorthRetrying.Contains(statusCode)) throw new KnownException(statusCode);

            throw;
        }
    }
}