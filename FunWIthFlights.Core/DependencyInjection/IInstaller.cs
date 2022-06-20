using Microsoft.Extensions.DependencyInjection;

namespace FunWithFlights.Core.DependencyInjection;

public interface IInstaller
{
    void RegisterDependencies(IServiceCollection services);
}