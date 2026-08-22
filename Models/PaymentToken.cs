using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOL.Models;

/// <summary>
/// Represents a single payment token assigned to a user.
/// A payment token encapsulates the monetary amount, currency, creation and expiration timestamps,
/// and whether the token has been consumed. Tokens are identified by a GUID and are intended for
/// one-time or time-limited payment operations.
/// </summary>
public class PaymentToken
{
    /// <summary>
    /// Primary key: unique identifier for the payment token.
    /// Initialized with a new <see cref="Guid"/> by default.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Foreign key referencing the owning user.
    /// This field is required.
    /// </summary>
    [Required] public Guid UserId { get; set; }

    /// <summary>
    /// Navigation property for the owning <see cref="User"/>.
    /// Nullable to support lazy loading / optional loading scenarios.
    /// </summary>
    [ForeignKey(nameof(UserId))] public User? User { get; set; }

    /// <summary>
    /// Monetary amount associated with the token.
    /// Mapped to the database as <c>decimal(18,2)</c> for monetary precision.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")] public decimal Amount { get; set; }

    /// <summary>
    /// Three-letter ISO 4217 currency code (e.g. "USD", "EUR").
    /// Maximum length constrained to 3 characters.
    /// Defaults to an empty string to avoid nulls.
    /// </summary>
    [MaxLength(3)] public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the token has already been used/consumed.
    /// Defaults to <c>false</c>.
    /// </summary>
    public bool Used { get; set; } = false;

    /// <summary>
    /// UTC timestamp when the token was created.
    /// Initialized to the current UTC time by default.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// UTC timestamp when the token expires and becomes invalid.
    /// Consumers should validate this against the current UTC time.
    /// </summary>
    public DateTime ExpiredAt { get; set; }
}
