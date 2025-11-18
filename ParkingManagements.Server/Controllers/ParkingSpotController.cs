using Microsoft.AspNetCore.Mvc;
using ParkingManagements.Data.Entities.Enums;
using ParkingManagements.Server.Interfaces;
using ParkingManagements.Server.DTOs.ParkingSpot;

[ApiController]
[Route("api/lots/{lotId}/[controller]")]
public class ParkingSpotController : ControllerBase
{
    private readonly IParkingSpotService _spotService;

    public ParkingSpotController(IParkingSpotService spotService)
    {
        _spotService = spotService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParkingSpotDTO>>> GetAll(Guid lotId)
    {
        var spots = await _spotService.GetSpotsByLotAsync(lotId);
        return Ok(spots);
    }

    [HttpGet("{spotId}")]
    public async Task<ActionResult<ParkingSpotDTO>> GetById(Guid lotId, Guid spotId)
    {
        var spot = await _spotService.GetSpotByIdAsync(spotId);
        if (spot == null) return NotFound();
        return Ok(spot);
    }

    [HttpPost]
    public async Task<ActionResult<ParkingSpotDTO>> Create(Guid lotId, [FromBody] ParkingSpotCreateDTO dto)
    {
        var createdSpot = await _spotService.CreateSpotAsync(lotId, dto);
        return CreatedAtAction(nameof(GetById), new { lotId, spotId = createdSpot.Id }, createdSpot);
    }

    [HttpPut("{spotId}")]
    public async Task<IActionResult> Update(Guid lotId, Guid spotId, [FromBody] ParkingSpotUpdateDTO dto)
    {
        var result = await _spotService.UpdateSpotAsync(spotId, dto);
        if (!result)
            return NotFound();

        return NoContent();
    }


    [HttpPatch("{spotId}/status")]
    public async Task<IActionResult> ChangeStatus(Guid lotId, Guid spotId, [FromQuery] SpotStatus newStatus)
    {
        var result = await _spotService.ChangeSpotStatusAsync(spotId, newStatus);
        if (!result) return NotFound();
        return NoContent();
    }
}
