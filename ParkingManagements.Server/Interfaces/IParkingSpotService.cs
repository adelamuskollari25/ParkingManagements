using ParkingManagements.Data.Entities.Enums;
using ParkingManagements.Server.DTOs.ParkingSpot;

namespace ParkingManagements.Server.Interfaces
{
    public interface IParkingSpotService
    {
        Task<IEnumerable<ParkingSpotDTO>> GetSpotsByLotAsync(Guid lotId);
        Task<ParkingSpotDTO?> GetSpotByIdAsync(Guid spotId);
        Task<ParkingSpotDTO> CreateSpotAsync(Guid lotId, ParkingSpotCreateDTO dto);
        Task<bool> UpdateSpotAsync(Guid spotId, ParkingSpotUpdateDTO dto);
        Task<bool> ChangeSpotStatusAsync(Guid spotId, SpotStatus newStatus);
    }
}
