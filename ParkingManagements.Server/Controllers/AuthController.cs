using AutoMapper.Execution;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParkingManagement.server.Data.Entities;
using ParkingManagement.server.DTOs;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AuthController> _logger;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            _logger.LogWarning("Login failed for email {Email}: user not found", model.Email);
            return Unauthorized();
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
        if (result.Succeeded)
        {
            _logger.LogInformation("User {Email} logged in successfully", model.Email);
            return Ok(new { message = "Login successful" });
        }
        else
        {
            _logger.LogWarning("Login failed for email {Email}: invalid password", model.Email);
            return Unauthorized();
        }
    }
}
