using System.ComponentModel.DataAnnotations;
namespace DOL.Models;

/// <summary>
/// Represents an application user account.
/// </summary>
/// <remarks>
/// Instances are created with sensible defaults (new Guid for <see cref="Id"/>,
/// current UTC time for <see cref="CreatedAt"/>, and empty collections/strings).
/// Persisted using the project's data access conventions; validation attributes
/// are applied for common constraints.
/// </remarks>
public class User
{
    /// <summary>
    /// Primary key: unique identifier for the user.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// User email address. Required and limited to 255 characters.
    /// </summary>
    [Required, MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hashed password used for authentication. Only store hashes, never plain text.
    /// </summary>
    [Required]
    public string PassHash { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether this user is registered as a merchant.
    /// </summary>
    public bool IsMerchant { get; set; } = false;

    /// <summary>
    /// UTC timestamp indicating when the account was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property: collection of payment tokens associated with the user
    /// (for example, saved payment methods or single-use tokens).
    /// </summary>
    public ICollection<PaymentToken> PaymentTokens { get; set; } = new List<PaymentToken>();
}
