using Microsoft.AspNetCore.Mvc;
using ParkingManagements.Server.Common;
using ParkingManagements.Server.DTOs.ParkingLot;
using ParkingManagements.Server.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class ParkingLotController : ControllerBase
{
    private readonly IParkingLotService _lotService;

    public ParkingLotController(IParkingLotService lotService)
    {
        _lotService = lotService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ParkingLotDTO>>> GetAll([FromQuery] PaginationParams pagination)
    {
        var lots = await _lotService.GetAllLotsAsync(pagination);
        return Ok(lots);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParkingLotDTO>> GetById(Guid id)
    {
        var lot = await _lotService.GetLotByIdAsync(id);
        if (lot == null) return NotFound();
        return Ok(lot);
    }

    [HttpPost]
    public async Task<ActionResult<ParkingLotDTO>> Create([FromBody] ParkingLotCreateDTO dto)
    {
        var createdLot = await _lotService.CreateLotAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdLot.Id }, createdLot);
    }

    [HttpGet("{id}/occupancy")]
    public async Task<ActionResult<ParkingLotKpiDTO>> GetOccupancy(Guid id)
    {
        var kpi = await _lotService.GetLotOccupancyMetricsAsync(id);
        if (kpi == null) return NotFound();
        return Ok(kpi);
    }
}
