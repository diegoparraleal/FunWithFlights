using FunWithFlights.Core.Time;

namespace FunWithFlights.Web;

public static class BuilderExtensions
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IClock, Clock>();
        new Infrastructure.Installer().RegisterDependencies(builder.Services);
        new Business.Installer().RegisterDependencies(builder.Services);
    }
}