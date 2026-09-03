using DOL.Models.enums;
namespace DOL.Models.Dtos;

public record SingularResponse(string Name, string Last4Number, CardProvider CardProvider);
public record SingularRequest(Guid PublicId);
public record CardRequest(string Name, string Creds, CardProvider CardProvider, Guid UserId, IssuerBanks Issuer);