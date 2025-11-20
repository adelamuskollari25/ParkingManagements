using Microsoft.AspNetCore.Mvc;
using ParkingManagements.Server.Common;
using ParkingManagements.Server.DTOs.Tariff;
using ParkingManagements.Server.Interfaces;

[ApiController]
[Route("api/lots/{lotId}/[controller]")]
public class TariffController : ControllerBase
{
    private readonly ITariffService _tariffService;

    public TariffController(ITariffService tariffService)
    {
        _tariffService = tariffService;
    }

    [HttpGet("current")]
    public async Task<ActionResult<TariffDTO>> GetCurrent(Guid lotId)
    {
        var tariff = await _tariffService.GetCurrentTariffAsync(lotId);
        if (tariff == null) return NotFound();
        return Ok(tariff);
    }

    [HttpGet("history")]
    public async Task<ActionResult<PagedResult<TariffDTO>>> GetHistory(Guid lotId, [FromQuery] PaginationParams pagination)
    {
        var history = await _tariffService.GetTariffHistoryAsync(lotId, pagination);
        return Ok(history);
    }

    [HttpPost]
    public async Task<ActionResult<TariffDTO>> Update(Guid lotId, [FromBody] TariffUpdateDTO dto)
    {
        var updated = await _tariffService.UpdateTariffAsync(lotId, dto);
        return CreatedAtAction(nameof(GetCurrent), new { lotId }, updated);
    }
}
