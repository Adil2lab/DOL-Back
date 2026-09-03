using System.ComponentModel.DataAnnotations.Schema;

namespace DOL.Models;

public class WebAuthnCred
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public byte[] CredentialId { get; set; } = [];
    public byte[] PublicKey { get; set; } = [];
    public uint SignatureCounter { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
