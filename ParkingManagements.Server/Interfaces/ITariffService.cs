using ParkingManagements.Server.Common;
using ParkingManagements.Server.DTOs.Tariff;

namespace ParkingManagements.Server.Interfaces
{
    public interface ITariffService
    {
        Task<TariffDTO?> GetCurrentTariffAsync(Guid lotId);
        Task<PagedResult<TariffDTO>> GetTariffHistoryAsync(Guid lotId, PaginationParams pagination);
        Task<TariffDTO> UpdateTariffAsync(Guid lotId, TariffUpdateDTO dto);
    }
}
