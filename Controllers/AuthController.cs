using Microsoft.AspNetCore.Mvc;
using DOL.Services;

namespace DOL.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IPassService _passService;

    public AuthController(IPassService passService)
    {
        _passService = passService;
    }

}
