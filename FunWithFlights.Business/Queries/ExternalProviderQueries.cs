using FunWithFlights.Core.Extensions;
using FunWithFlights.Core.Queries;
using FunWithFlights.Domain.ExternalProviders;
using FunWithFlights.Infrastructure.Contracts.Repositories;

namespace FunWithFlights.Business.Queries;

public interface IExternalProviderQueries: ICommonQuery<ExternalProvider> {}

public class ExternalProviderQueries: IExternalProviderQueries
{
    private readonly IExternalProviderRepository _repository;

    public ExternalProviderQueries(IExternalProviderRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IReadOnlyCollection<ExternalProvider>> GetAllAsync() => (await  _repository.GetAllAsync()).AsReadOnly();
}