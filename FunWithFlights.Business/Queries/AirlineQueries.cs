using FunWithFlights.Core.Extensions;
using FunWithFlights.Core.Queries;
using FunWithFlights.Domain.Airlines;
using FunWithFlights.Infrastructure.Contracts.Repositories;

namespace FunWithFlights.Business.Queries;

public interface IAirlineQueries: ICommonQuery<Airline> {}

public class AirlineQueries: IAirlineQueries
{
    private readonly IAirlineRepository _repository;

    public AirlineQueries(IAirlineRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IReadOnlyCollection<Airline>> GetAllAsync() => (await  _repository.GetAllAsync()).AsReadOnly();
}