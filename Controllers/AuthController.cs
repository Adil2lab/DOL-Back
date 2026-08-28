using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DOL.Services;
using DOL.Data;
using DOL.Models;
using DOL.Models.Dtos;

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

    [HttpGet("byEmail/{email}")]
    public async Task<ActionResult<UserResponse>> GetUserByEmail([FromRoute] string email)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null) return NotFound(new { error = "No email found" });
        return Ok(new UserResponse(user.Id, user.Email, user.IsMerchant));
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

        var refreshToken = await _db.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.UserId == user.Id);
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
            refreshToken.HashedToken = _tokenService.HashToken(rawRefreshToken);
            refreshToken.ExpiredAt = DateTime.UtcNow.AddDays(30);   
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

    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponse>> Refresh()
    {
        if (!Request.Cookies.TryGetValue("RefreshToken", out var rawToken) || rawToken == null)
            return Unauthorized(new { error = "No refresh token provided" });

        string tokenHash = _tokenService.HashToken(rawToken);

        RefreshToken? storedToken = await _db.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.HashedToken == tokenHash && !t.Revoked);

        if (storedToken == null || storedToken.ExpiredAt < DateTime.UtcNow || storedToken.User == null)
            return Unauthorized(new { error = "Invalid or expired refresh token" });

        var (accessToken, expiresAt) = _tokenService.GenAccessToken(storedToken.User);

        return new LoginResponse(accessToken, expiresAt);
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> RegisterUser([FromBody] RegisterRequest data)
    {
        User? existing = await _db.Users.FirstOrDefaultAsync(u => u.Email == data.email || u.MobileNumber == data.number);

        if (existing != null || data.pass == null) return Conflict(new { error = "User could not be registered with the provided details" });

        User user = new() { Email = data.email, MobileNumber = data.number, Nid = data.nid};
        user.PassHash = _passService.HashPass(user, data.pass);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserByEmail), new { email = user.Email}, new UserResponse(user.Id, user.Email, user.IsMerchant));
    }
}
