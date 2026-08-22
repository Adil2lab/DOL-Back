using DOL.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DOL.Models;

public class CreditCard
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    [Column(TypeName = "varchar(4)")]
    [StringLength(4, MinimumLength = 4)]
    [RegularExpression(@"^\d{}$")]
    public string Last4Number { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Balance { get; set; }

    public CardProvider CardProvider { get; set; }
}
