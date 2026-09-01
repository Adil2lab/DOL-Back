using DOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace DOL.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly static User Dummy = new() { Email = "dummy@generic.com", PassHash = "hHt$uu3hgatv3b4" };

    [HttpGet("GetUserById")]
    public async Task<ActionResult<User>> GetUserById()
    {
        return await Task.FromResult(Ok(Dummy));
    }

    [HttpGet("GetRandomUser")]
    public async Task<ActionResult<User>> GetUser()
    {
        return await Task.FromResult(Ok(new User { Email = "dummy@generic.com", PassHash = "hHt$uu3hgatv3b4" }));
    }
}
