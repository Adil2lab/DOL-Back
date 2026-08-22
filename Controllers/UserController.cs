using DOL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DOL.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private static User Dummy = new User();

    [HttpGet]
    public async Task<ActionResult<User>> GetUserById()
    {
        return await Task.FromResult(Ok(Dummy));
    }
}
