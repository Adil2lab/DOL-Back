using DOL.Data;
using DOL.Models;
using DOL.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DOL.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CardController : ControllerBase
{
    private readonly AppDbContext _db;

    public CardController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("singularData")]
    public async Task<ActionResult<SingularResponse>> GetSingularData([FromBody] SingularRequest request)
    {
        var card = await _db.CreditCards.FirstOrDefaultAsync(c => c.PublicId == request.PublicId);
        if (card == null)
        {
            return NotFound();
        }

        return Ok(new SingularResponse(Name: card.Name, Last4Number: card.Last4Number, CardProvider: card.CardProvider));
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateCard([FromBody] CardRequest req)
    {
        
        return Ok();
    }
}
