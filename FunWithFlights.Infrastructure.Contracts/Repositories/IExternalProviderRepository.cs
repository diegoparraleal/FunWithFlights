using FunWithFlights.Core.Entities;
using FunWithFlights.Domain.ExternalProviders;

namespace FunWithFlights.Infrastructure.Contracts.Repositories;

public interface IExternalProviderRepository: IReadOnlyRepository<string, ExternalProvider>
{
    public Task<IReadOnlyCollection<ExternalProvider>> GetAllBySourceAsync(string source);
}