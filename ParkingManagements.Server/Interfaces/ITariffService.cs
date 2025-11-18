using ParkingManagements.Server.DTOs.Tariff;

namespace ParkingManagements.Server.Interfaces
{
    public interface ITariffService
    {
        Task<TariffDTO?> GetCurrentTariffAsync(Guid lotId);
        Task<IEnumerable<TariffDTO>> GetTariffHistoryAsync(Guid lotId);
        Task<TariffDTO> UpdateTariffAsync(Guid lotId, TariffUpdateDTO dto);
    }
}
