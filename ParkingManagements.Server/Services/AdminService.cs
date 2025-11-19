using Microsoft.AspNetCore.Identity;
using ParkingManagements.Data.Entities;
using ParkingManagements.Data.Entities.Enums;
using ParkingManagements.server.Data.Entities;
using ParkingManagements.Server.Common;
using ParkingManagements.Server.DTOs;
using ParkingManagements.Server.DTOs.Auth;
using ParkingManagements.Server.DTOs.UserDTOs;
using ParkingManagements.Server.Interfaces;

namespace ParkingManagements.Server.Services
{

    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;

        public AdminService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserDTO> CreateUserAsync(RegisterDTO model)
        {
            if (model.Role != UserRole.Attendant && model.Role != UserRole.Viewer)
                throw new ServiceException("invalid_role", "Admin can only create Attendant or Viewer.", 400);

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                throw new ServiceException("email_exists", "User with this email already exists.", 400);

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Role = model.Role,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new ServiceException("create_failed", errors, 400);
            }

            await _userManager.AddToRoleAsync(user, model.Role.ToString());

            return new UserDTO
            {
                UserId = user.Id,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        public async Task DeactivateUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new ServiceException("not_found", "User not found.", 404);

            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100); 
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new ServiceException("deactivate_failed", errors, 400);
            }
        }

        public async Task ReactivateUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new ServiceException("not_found", "User not found.", 404);

            user.LockoutEnabled = false;
            user.LockoutEnd = null;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new ServiceException("reactivate_failed", errors, 400);
            }
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<UserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserDTO
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Role = string.Join(",", roles),
                    IsLockedOut = user.LockoutEnabled && user.LockoutEnd > DateTimeOffset.UtcNow
                });
            }

            return userList;
        }
    }

}
