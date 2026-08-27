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

        if (user == null) return await Task.FromResult(NotFound(new { error = "No email found" }));
        return Ok(new UserResponse(user.Id, user.Email, user.Nid, user.IsMerchant));
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> LoginRequest([FromBody] LoginRequest data)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == data.email);

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

        await _db.SaveChangesAsync();

        (string token, DateTime expiresAt) = _tokenService.GenAccessToken(user);

        return Ok(new LoginResponse(token, expiresAt));
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> RegisterUser([FromBody] RegisterRequest data)
    {
        var existing = await _db.Users.FirstOrDefaultAsync(u => u.Email == data.email);

        if (existing != null) return BadRequest(new { error = "Wrong Credential Given" });

        var user = new User { Email = data.email, MobileNumber = data.number, Nid = data.nid};
        user.PassHash = _passService.HashPass(user, data.pass);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(RegisterUser), new { email = user.Email}, new UserResponse(user.Id, user.Email, user.Nid, user.IsMerchant));
    }
}
