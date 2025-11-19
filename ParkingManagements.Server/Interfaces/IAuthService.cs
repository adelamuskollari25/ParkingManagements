using ParkingManagements.Server.DTOs.Auth;
using ParkingManagements.Server.DTOs.UserDTOs;
using ParkingManagements.Server.Services.ParkingManagements.Server.Services;
using System.Security.Claims;

namespace ParkingManagements.Server.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginDTO model);
        Task<CurrentUserDTO> GetCurrentUserAsync(ClaimsPrincipal userClaims);
    }
}
