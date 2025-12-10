using ParkingManagements.Server.DTOs.Auth;
using ParkingManagements.Server.DTOs.UserDTOs;

namespace ParkingManagements.Server.Interfaces
{
    public interface IAdminService
    {
        Task<UserDTO> CreateUserAsync(RegisterDTO model);
        Task DeactivateUserAsync(Guid userId);
        Task ReactivateUserAsync(Guid userId);
        Task<List<UserDTO>> GetAllUsersAsync();
    }
}
