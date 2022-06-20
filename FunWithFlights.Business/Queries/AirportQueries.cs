using FunWithFlights.Core.Extensions;
using FunWithFlights.Core.Queries;
using FunWithFlights.Domain.Airports;
using FunWithFlights.Infrastructure.Contracts.Repositories;

namespace FunWithFlights.Business.Queries;

public interface IAirportQueries: ICommonQuery<Airport> {}

public class AirportQueries: IAirportQueries
{
    private readonly IAirportRepository _repository;

    public AirportQueries(IAirportRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IReadOnlyCollection<Airport>> GetAllAsync() => (await  _repository.GetAllAsync()).AsReadOnly();
}