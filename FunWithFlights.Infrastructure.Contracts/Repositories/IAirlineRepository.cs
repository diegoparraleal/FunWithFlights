using FunWithFlights.Core.Entities;
using FunWithFlights.Domain.Airlines;

namespace FunWithFlights.Infrastructure.Contracts.Repositories;

public interface IAirlineRepository: IReadOnlyRepository<AirlineCode, Airline> { }