using ParkingManagements.Server.DTOs.Auth;
using ParkingManagements.Server.DTOs.UserDTOs;
using System.Security.Claims;

namespace ParkingManagements.Server.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginDTO model);
        Task<CurrentUserDTO> GetCurrentUserAsync(ClaimsPrincipal userClaims);
    }
}
