namespace DOL.Models.Dtos;

public record RegisterRequest(string email, string pass, string nid, string number);
public record LoginRequest(string? email, string pass, string? number);
public record LoginResponse(string token, DateTime expiresAt);
public record UserResponse(Guid id, string email, bool isMerchant);