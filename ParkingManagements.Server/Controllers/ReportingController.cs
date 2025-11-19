using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> GetLotSnapshot()
    {
        var result = await _reportingService.GetPerLotSnapshotAsync();
        return Ok(result);
    }

    [HttpGet("dailyrevenue")]
    public async Task<IActionResult> GetDailyRevenue([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var result = await _reportingService.GetDailyRevenueSummaryAsync(from, to);
        return Ok(result);
    }
}
