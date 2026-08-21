using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOL.Models;

public class PaymentToken
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required] public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))] public User? User { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal Amount { get; set; }

    [MaxLength(3)] public string CurrencyCode { get; set; } = string.Empty;

    public bool Used { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime ExpiredAt { get; set; }
}
