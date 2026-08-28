using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DOL.Services;
using DOL.Data;
using DOL.Models;
using DOL.Models.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace DOL.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IPassService _passService;
    private readonly ITokenService _tokenService;

    public AuthController(AppDbContext db, IPassService passService, ITokenService tokenService)
    {
        _db = db;
        _passService = passService;
        _tokenService = tokenService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("byEmail/{email}")]
    public async Task<ActionResult<UserResponse>> GetUserByEmail([FromRoute] string email)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null) return NotFound(new { error = "No email found" });
        return Ok(new UserResponse(user.Id, user.Email, user.Nid, user.IsMerchant));
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LoginRequest([FromBody] LoginRequest data)
    {
        if (string.IsNullOrEmpty(data.email) && string.IsNullOrEmpty(data.number))
        {
            return BadRequest(new {error = "There's no Email or Phone number."});
        }

        var user = await _db.Users.FirstOrDefaultAsync(u => (!string.IsNullOrEmpty(data.email) && u.Email == data.email) || (!string.IsNullOrEmpty(data.number) && u.MobileNumber == data.number));

        if (user == null) return Unauthorized(new { error = "Problem in the email or password" });

        (bool success, bool needsRehash) pass = _passService.VerifyPass(user, user.PassHash, data.pass);

        if (!pass.success)
        {
            return Unauthorized(new { error = "Problem in the email or password" });
        }

        user.LoggedInAt = DateTime.UtcNow;

        if (pass.needsRehash)
        {
            user.PassHash = _passService.HashPass(user, data.pass);
        }

        (string token, DateTime expiresAt) = _tokenService.GenAccessToken(user);
        var rawRefreshToken = _tokenService.GenRefreshTokenRaw();

        var refreshToken = await _db.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == user.Id);
        if (refreshToken == null)
        {
            _db.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.Id,
                HashedToken = _tokenService.HashToken(rawRefreshToken),
                ExpiredAt = DateTime.UtcNow.AddDays(30)
            });
        }
        else
        {
            if (refreshToken.ExpiredAt <= DateTime.UtcNow.AddDays(5))
            {
                refreshToken.HashedToken = _tokenService.HashToken(rawRefreshToken);
                refreshToken.ExpiredAt = DateTime.UtcNow.AddDays(30);
            }
        }
        await _db.SaveChangesAsync();

        Response.Cookies.Append("RefreshToken", rawRefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(30),
            Path = "/api/auth/refresh"
        });

        return Ok(new LoginResponse(token, expiresAt));
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> RegisterUser([FromBody] RegisterRequest data)
    {
        var existing = await _db.Users.FirstOrDefaultAsync(u => u.Email == data.email);

        if (existing != null) return Conflict(new { error = "User could not be registered with the provided details" });

        var user = new User { Email = data.email, MobileNumber = data.number, Nid = data.nid};
        user.PassHash = _passService.HashPass(user, data.pass);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserByEmail), new { email = user.Email}, new UserResponse(user.Id, user.Email, user.Nid, user.IsMerchant));
    }
}
