using ParkingManagements.Data.Entities.Enums;
using ParkingManagements.Server.Common;
using ParkingManagements.Server.Common.Sortings;
using ParkingManagements.Server.DTOs.ParkingSpot;

namespace ParkingManagements.Server.Interfaces
{
    public interface IParkingSpotService
    {
        Task<PagedResult<ParkingSpotDTO>> GetSpotsByLotAsync(Guid lotId, ParkingSpotFilterParams filters);
        Task<ParkingSpotDTO?> GetSpotByIdAsync(Guid spotId);
        Task<ParkingSpotDTO> CreateSpotAsync(Guid lotId, ParkingSpotCreateDTO dto);
        Task<bool> UpdateSpotAsync(Guid spotId, ParkingSpotUpdateDTO dto);
        Task<bool> ChangeSpotStatusAsync(Guid spotId, SpotStatus newStatus);
    }
}
