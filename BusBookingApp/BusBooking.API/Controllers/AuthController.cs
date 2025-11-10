using BusBooking.Application.DTOs;
using BusBooking.Application.Interfaces;
using BusBooking.Shared.Constants;
using BusBooking.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusBooking.API.Controllers;

[ApiController]
[Route(APIConstants.Controller)]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IConfiguration configuration, IAuthService authService, ILogger<AuthController> logger)
    {
        _configuration = configuration;
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDto loginDto)
    {
        var user = _authService.Authenticate(loginDto.Email, loginDto.Password);
        if (user == null)
        {
            _logger.LogWarning(APIConstants.InvalidLoginLogTemplate, loginDto.Email);
            return Unauthorized(new { message = APIConstants.InvalidLoginMessage });
        }

        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto, [FromServices] IUserService userService)
    {
        var created = await userService.RegisterAsync(dto);
        if (created == null)
            return BadRequest(APIConstants.AlreadyExistError);

        return CreatedAtAction(nameof(Login), new { email = created.Email }, created);
    }

    private string GenerateJwtToken(UserDto user)
    {
        var jwtSettings = _configuration.GetSection(APIConstants.JwtSection);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings[APIConstants.JwtKey] ?? string.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(APIConstants.UserIdClaimType, user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings[APIConstants.JwtIssuer],
            audience: jwtSettings[APIConstants.JwtAudience],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
