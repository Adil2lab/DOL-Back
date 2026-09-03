using Microsoft.EntityFrameworkCore;
using DOL.Models;

namespace DOL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<PaymentToken> PaymentTokens => Set<PaymentToken>();
    public DbSet<LedgerEntry> LedgerEntries => Set<LedgerEntry>();
    public DbSet<CreditCard> CreditCards => Set<CreditCard>();
    public DbSet<PublicCardLobby> PublicCards => Set<PublicCardLobby>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<WebAuthnCred> WebAuthnCreds => Set<WebAuthnCred>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<PaymentToken>()
            .HasIndex(t => t.Used);
    }
}
