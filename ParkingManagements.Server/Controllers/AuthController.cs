using AutoMapper.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParkingManagements.server.Data.Entities;
using ParkingManagements.Server.DTOs;
using ParkingManagements.Server.DTOs.Auth;
using ParkingManagements.Server.Interfaces;
using ParkingManagements.Server.Managers;
using ParkingManagements.Server.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AuthController> _logger;
    private readonly ITokenService _tokenService;
    private readonly JwtSettings _jwtSettings;


    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AuthController> logger, ITokenService tokenService, JwtSettings jwtSettings)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _logger = logger;
        _jwtSettings = jwtSettings;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Check if user already exists
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            return BadRequest(new { message = "User with this email already exists" });
        }

        var user = new User
        {
            UserName = model.Email,
            Email = model.Email 
        };

        // Create user with hashed password
        var result = await _userManager.CreateAsync(user, model.Password);


        if (result.Succeeded)
        {
            _logger.LogInformation("User {Email} registered successfully", model.Email);

            // Optionally assign default role
            await _userManager.AddToRoleAsync(user, "User");

            // Generate token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateToken(user, roles);

            return Ok(new AuthResponseDTO
            {
                Token = token,
                Email = user.Email,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes)
            });
        }

        return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            _logger.LogWarning("Login failed for email {Email}: user not found", model.Email);
            return Unauthorized(new { message = "Invalid email or password" });
        }

        // Check password
        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            _logger.LogInformation("User {Email} logged in successfully", model.Email);

            // Generate token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateToken(user, roles);

            return Ok(new AuthResponseDTO
            {
                Token = token,
                Email = user.Email,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes)
            });
        }
        else if (result.IsLockedOut)
        {
            _logger.LogWarning("User {Email} is locked out", model.Email);
            return Unauthorized(new { message = "Account locked. Please try again later." });
        }
        else
        {
            _logger.LogWarning("Login failed for email {Email}: invalid password", model.Email);
            return Unauthorized(new { message = "Invalid email or password" });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            userId = user.Id,
            email = user.Email
        });
    }
}
