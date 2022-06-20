using FunWithFlights.Core.Entities;
using FunWithFlights.Domain.Equipments;

namespace FunWithFlights.Infrastructure.Contracts.Repositories;

public interface IEquipmentRepository: IReadOnlyRepository<EquipmentCode, Equipment> { }