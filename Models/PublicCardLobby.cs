using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOL.Models;

public class PublicCardLobby
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Hashed { get; set; } = string.Empty;
}
