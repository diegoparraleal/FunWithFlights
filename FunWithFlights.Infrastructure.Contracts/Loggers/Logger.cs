using Microsoft.Extensions.Logging;

namespace FunWithFlights.Infrastructure.Contracts.Loggers;

public static class Logger
{
    private static ILoggerFactory? _loggerFactory;
    
    public static void Configure(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }
    public static ILogger Initialize<T>()
    {
        if (_loggerFactory == null)
            throw new InvalidOperationException("Cannot create loggers, please use configure method first");

        return _loggerFactory.CreateLogger(typeof(T));
    }
}