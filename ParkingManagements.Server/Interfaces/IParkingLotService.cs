using ParkingManagements.Server.Common;
using ParkingManagements.Server.DTOs.ParkingLot;

namespace ParkingManagements.Server.Interfaces
{
    public interface IParkingLotService
    {
        Task<PagedResult<ParkingLotDTO>> GetAllLotsAsync(PaginationParams pagination);
        Task<ParkingLotDTO?> GetLotByIdAsync(Guid lotId);
        Task<ParkingLotDTO> CreateLotAsync(ParkingLotCreateDTO dto);
        Task<ParkingLotKpiDTO?> GetLotOccupancyMetricsAsync(Guid lotId);
    }
}
