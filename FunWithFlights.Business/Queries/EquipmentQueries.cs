using FunWithFlights.Core.Extensions;
using FunWithFlights.Core.Queries;
using FunWithFlights.Domain.Equipments;
using FunWithFlights.Infrastructure.Contracts.Repositories;

namespace FunWithFlights.Business.Queries;

public interface IEquipmentQueries: ICommonQuery<Equipment> {}

public class EquipmentQueries: IEquipmentQueries
{
    private readonly IEquipmentRepository _repository;

    public EquipmentQueries(IEquipmentRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IReadOnlyCollection<Equipment>> GetAllAsync() => (await  _repository.GetAllAsync()).AsReadOnly();
}