using ParkingManagements.Server.DTOs.Metrics___Reportings;

public interface IReportingService
{
    Task<IEnumerable<LotSnapshotDTO>> GetPerLotSnapshotAsync();
    Task<IEnumerable<DailyRevenueDTO>> GetDailyRevenueSummaryAsync(DateTime from, DateTime to);
}
