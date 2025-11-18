using ParkingManagements.Server.DTOs.ParkingLot;

namespace ParkingManagements.Server.Interfaces
{
    public interface IParkingLotService
    {
        Task<IEnumerable<ParkingLotDTO>> GetAllLotsAsync();
        Task<ParkingLotDTO?> GetLotByIdAsync(Guid lotId);
        Task<ParkingLotDTO> CreateLotAsync(ParkingLotCreateDTO dto);
        Task<ParkingLotKpiDTO?> GetLotOccupancyMetricsAsync(Guid lotId);
    }
}
