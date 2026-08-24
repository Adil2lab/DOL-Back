using DOL.Models;
using Microsoft.AspNetCore.Identity;

namespace DOL.Services;

public interface IPassService
{
    string HashPass(User user, string password);
    (bool success, bool needsRehash) VerifyPass(User user, string hashedPassword, string password);
}

public class PassService : IPassService
{
    private readonly PasswordHasher<User> _hasher = new();
    public string HashPass(User user, string password)
    {
        return _hasher.HashPassword(user, password);
    }

    public (bool success, bool needsRehash) VerifyPass(User user, string hashedPassword, string password)
    {
        var result = _hasher.VerifyHashedPassword(user, hashedPassword, password);
        return (
            success: result != PasswordVerificationResult.Failed,
            needsRehash: result == PasswordVerificationResult.SuccessRehashNeeded
            );
    }
}
