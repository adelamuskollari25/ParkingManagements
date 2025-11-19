using Microsoft.AspNetCore.Mvc;
using ParkingManagements.Server.Common;

[ApiController]
[Route("api/reporting")]
public class ReportingController : ControllerBase
{
    private readonly IReportingService _reportingService;

    public ReportingController(IReportingService reportingService)
    {
        _reportingService = reportingService;
    }

    [HttpGet("lotsnapshot")]
    public async Task<IActionResult> GetLotSnapshot([FromQuery] PaginationParams pagination)
    {
        var result = await _reportingService.GetPerLotSnapshotAsync(pagination);
        return Ok(result);
    }

    [HttpGet("dailyrevenue")]
    public async Task<IActionResult> GetDailyRevenue([FromQuery] DateTime from, [FromQuery] DateTime to, [FromQuery] PaginationParams pagination)
    {
        var result = await _reportingService.GetDailyRevenueSummaryAsync(from, to, pagination);
        return Ok(result);
    }
}
