using HGD_SSO_API.Data;
using HGD_SSO_API.Models.AuthManagement;
using HGD_SSO_API.Services.AuthManagement;
using Microsoft.AspNetCore.Mvc;

namespace HGD_SSO_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly PasswordService _passwordService;
    private readonly JwtService _jwtService;

    public AuthController(AppDbContext db, PasswordService passwordService, JwtService jwtService)
    {
        _db = db;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] SignInRequest request)
    {
        var user = _db.Users.SingleOrDefault(u => u.UserName == request.UserName);
        if (user == null || !_passwordService.VerifyPassword(request.Password, user.PasswordHash))
            return Unauthorized();

        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = Guid.NewGuid().ToString();

        _db.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        });
        _db.SaveChanges();

        return Ok(new SignInResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15)
        });
    }
}