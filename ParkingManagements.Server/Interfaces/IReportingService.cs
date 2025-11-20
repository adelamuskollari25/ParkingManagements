using ParkingManagements.Server.Common;
using ParkingManagements.Server.DTOs.Metrics___Reportings;

public interface IReportingService
{
    Task<PagedResult<LotSnapshotDTO>> GetPerLotSnapshotAsync(PaginationParams pagination);
    Task<PagedResult<DailyRevenueDTO>> GetDailyRevenueSummaryAsync(DateTime from, DateTime to, PaginationParams pagination);
}
