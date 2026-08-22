using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOL.Models;

/// <summary>
/// Represents a single ledger entry for a user.
/// A ledger entry records a monetary change (credit/debit), an optional link to a related payment token,
/// a human-readable description, and the creation timestamp.
/// </summary>
public class LedgerEntry
{
    /// <summary>
    /// Primary key: unique identifier for the ledger entry.
    /// Initialized with a new <see cref="Guid"/> by default.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Foreign key referencing the owning user.
    /// Use this to associate the entry with a <see cref="User"/>.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Navigation property for the owning <see cref="User"/>.
    /// Nullable to allow different loading strategies (explicit/eager/lazy).
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    /// <summary>
    /// Monetary amount for this ledger entry.
    /// Mapped to the database as <c>decimal(18,2)</c> to preserve monetary precision.
    /// Positive or negative values can represent credits or debits depending on application logic.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount {  get; set; }

    /// <summary>
    /// Optional reference to a related <see cref="PaymentToken"/> (if this entry was caused by a token).
    /// Nullable when there is no related token.
    /// </summary>
    public Guid? RelativeTokenId { get; set; }

    /// <summary>
    /// Short description or reason for the ledger entry (e.g., "Token redemption", "Manual adjustment").
    /// Defaults to an empty string to avoid nulls.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// UTC timestamp when the ledger entry was created.
    /// Initialized to <see cref="DateTime.UtcNow"/> by default.
    /// Consumers should compare this against UTC time.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}