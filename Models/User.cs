using System.ComponentModel.DataAnnotations;
    
namespace DOL.Models;

public class User
{
    public Guid Id { get; set; } = new Guid();

    [Required, MaxLength(255)] public string Email { get; set; } = string.Empty;

    [Required] public string PassHash {  get; set; } = string.Empty;

    public bool IsMerchant { get; set; } = false;

    public DateTime CheckedAt { get; set; } = DateTime.UtcNow;

    public ICollection<PaymentToken> PaymentTokens { get; set; } = new List<PaymentToken>();
}
