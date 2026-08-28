using System.ComponentModel.DataAnnotations.Schema;

namespace DOL.Models;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public string HashedToken { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool Revoked { get; set; }

    public DateTime ExpiredAt { get; set; }
}
