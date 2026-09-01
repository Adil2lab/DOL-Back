using DOL.Models.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOL.Models;

/// <summary>
/// Represents a credit card record owned by a <see cref="User"/>.
/// Stores metadata required for billing and display purposes while avoiding
/// storage of full card numbers (only the last 4 digits are persisted).
/// </summary>
public class CreditCard
{
    /// <summary>
    /// Primary key: unique identifier for the credit card.
    /// Initialized with a new <see cref="Guid"/> by default.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid PublicId { get; set; } = Guid.NewGuid();

    [ForeignKey(nameof(PublicId))]
    public PublicCardLobby? PublicCard { get; set; }

    /// <summary>
    /// Friendly name for the card (e.g., "Personal Visa", "Work Card").
    /// Required and defaults to an empty string to avoid nulls.
    /// </summary>
    [Required, MaxLength(25)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key referencing the owning user.
    /// This field is required.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Navigation property for the owning <see cref="User"/>.
    /// Nullable to support different loading strategies (explicit, eager, or lazy).
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    /// <summary>
    /// Only the last 4 digits of the card number are stored for display and identification.
    /// - Persisted as <c>varchar(4)</c> in the database.
    /// - Must be exactly 4 numeric digits (validated by <see cref="RegularExpressionAttribute"/>).
    /// </summary>
    [Column(TypeName = "varchar(4)"), StringLength(4, MinimumLength = 4), RegularExpression(@"^\d{4}$")]
    public string Last4Number { get; set; } = string.Empty;

    /// <summary>
    /// Monetary balance associated with this card (if used to track outstanding amounts or limits).
    /// Stored in the database as <c>decimal(18,2)</c> to preserve precision for money.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Balance { get; set; }

    /// <summary>
    /// The card provider (enum) indicating the network or issuing type (e.g., Visa, MasterCard).
    /// </summary>
    public CardProvider CardProvider { get; set; }

    public DateTime LastTransactionedAt { get; set; } = DateTime.UtcNow;

    public IssuerBanks Issuer { get; set; }

}