using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingManagements.Data.Entities;
using ParkingManagements.Data.Entities.Enums;
using ParkingManagements.Server.Common;
using ParkingManagements.Server.DTOs.Auth;
using ParkingManagements.Server.Interfaces;
using ParkingManagements.Server.Services;

namespace ParkingManagements.Server.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var user = await _adminService.CreateUserAsync(model);
            return Ok(new
            {
                message = "User created successfully",
                userId = user.UserId,
                email = user.Email,
                role = user.Role
            });

        }

        [HttpPost("deactivate/{userId}")]
        public async Task<IActionResult> DeactivateUser(Guid userId)
        {

            await _adminService.DeactivateUserAsync(userId);
            return Ok(new { message = "User deactivated successfully" });

        }

        [HttpPost("reactivate/{userId}")]
        public async Task<IActionResult> ReactivateUser(Guid userId)
        {

            await _adminService.ReactivateUserAsync(userId);
            return Ok(new { message = "User reactivated successfully" });

        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            return Ok(users);

        }
    }

}
