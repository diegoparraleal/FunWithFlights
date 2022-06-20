using FunWithFlights.Business.Queries;
using FunWithFlights.Domain.Equipments;
using FunWithFlights.Infrastructure.Contracts.Loggers;
using Microsoft.AspNetCore.Mvc;

namespace FunWithFlights.Web.Controllers;

[ApiController]
[Route("/api/equipments")]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentQueries _equipmentQueries;
    private static readonly ILogger Log = Logger.Initialize<EquipmentController>();

    public EquipmentController(
        IEquipmentQueries equipmentQueries)
    {
        _equipmentQueries = equipmentQueries;
    }

    [HttpGet]
    public async Task<IEnumerable<Equipment>> GetAllAsync() => await _equipmentQueries.GetAllAsync();
}