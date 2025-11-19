namespace ParkingManagements.Server.Services
{
    using global::ParkingManagements.server.Data.Entities;
    using global::ParkingManagements.Server.Common;
    using global::ParkingManagements.Server.DTOs.Auth;
    using global::ParkingManagements.Server.DTOs.UserDTOs;
    using global::ParkingManagements.Server.Interfaces;
    using global::ParkingManagements.Server.Managers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using System.Security.Claims;

    namespace ParkingManagements.Server.Services
    {
        public class AuthService : IAuthService
        {
            private readonly UserManager<User> _userManager;
            private readonly SignInManager<User> _signInManager;
            private readonly ITokenService _tokenService;
            private readonly ILogger<AuthService> _logger;
            private readonly JwtSettings _jwtSettings;

            public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, ILogger<AuthService> logger, JwtSettings jwtSettings)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _tokenService = tokenService;
                _logger = logger;
                _jwtSettings = jwtSettings;
            }

            public async Task<AuthResponseDTO> LoginAsync(LoginDTO model)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _logger.LogWarning("Login failed for email {Email}: user not found", model.Email);
                    throw new ServiceException("invalid_credentials", "Invalid email or password.", 401);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User {Email} is locked out", model.Email);
                    throw new ServiceException("account_locked", "Account locked. Please try again later.", 401);
                }

                if (!result.Succeeded)
                {
                    _logger.LogWarning("Login failed for email {Email}: invalid password", model.Email);
                    throw new ServiceException("invalid_credentials", "Invalid email or password.", 401);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var token = _tokenService.GenerateToken(user, roles);

                _logger.LogInformation("User {Email} logged in successfully", model.Email);

                return new AuthResponseDTO
                {
                    Token = token,
                    Email = user.Email,
                    UserId = user.Id,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes)
                };
            }

            public async Task<CurrentUserDTO> GetCurrentUserAsync(ClaimsPrincipal userClaims)
            {
                var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                    throw new ServiceException("unauthorized", "User is not authenticated.", 401);

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new ServiceException("not_found", "User not found.", 404);

                return new CurrentUserDTO
                {
                    UserId = user.Id,
                    Email = user.Email
                };
            }
        }

    }

}
