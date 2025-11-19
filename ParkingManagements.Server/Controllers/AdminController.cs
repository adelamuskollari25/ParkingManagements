using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParkingManagements.Data.Entities;
using ParkingManagements.Data.Entities.Enums;
using ParkingManagements.server.Data.Entities;
using ParkingManagements.Server.DTOs;
using ParkingManagements.Server.DTOs.Auth;
using System.ComponentModel.DataAnnotations;

namespace ParkingManagements.Server.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public AdminController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

    
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Role != UserRole.Attendant && model.Role != UserRole.Viewer)
                return BadRequest(new { message = "Admin can only create Attendant or Viewer." });

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                return BadRequest(new { message = "User with this email already exists." });

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Role = model.Role,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

            await _userManager.AddToRoleAsync(user, model.Role.ToString());

            return Ok(new
            {
                message = "User created successfully",
                userId = user.Id,
                email = user.Email,
                role = user.Role.ToString()
            });
        }


        [HttpPost("deactivate/{userId}")]
        public async Task<IActionResult> DeactivateUser(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound(new { message = "User not found" });

            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100); 
            await _userManager.UpdateAsync(user);

            return Ok(new { message = "User deactivated successfully" });
        }


        [HttpPost("reactivate/{userId}")]
        public async Task<IActionResult> ReactivateUser(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound(new { message = "User not found" });

            user.LockoutEnd = null;
            user.LockoutEnabled = false;
            await _userManager.UpdateAsync(user);

            return Ok(new { message = "User reactivated successfully" });
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users.ToList();

            var userList = new List<object>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new
                {
                    userId = user.Id,
                    email = user.Email,
                    roles = roles,
                    isLockedOut = user.LockoutEnabled && user.LockoutEnd > DateTimeOffset.UtcNow
                });
            }

            return Ok(userList);
        }
    }
}
