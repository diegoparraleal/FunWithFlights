using FunWithFlights.Core.Entities;
using FunWithFlights.Domain.Airports;

namespace FunWithFlights.Infrastructure.Contracts.Repositories;

public interface IAirportDistancesRepository: IReadOnlyRepository<AirportDistanceKey, AirportDistance> { }