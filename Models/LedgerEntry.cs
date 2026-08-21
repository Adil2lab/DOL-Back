using System.ComponentModel.DataAnnotations.Schema;

namespace DOL.Models;

public class LedgerEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid AccountId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount {  get; set; }

    public Guid? RelativeTokenId { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
