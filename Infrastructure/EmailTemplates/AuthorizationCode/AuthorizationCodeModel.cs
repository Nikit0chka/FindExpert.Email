namespace Infrastructure.EmailTemplates.AuthorizationCode;

public sealed class AuthorizationCodeModel(string authorizationCode)
{
    public string AuthorizationCode { get; init; } = authorizationCode;
}