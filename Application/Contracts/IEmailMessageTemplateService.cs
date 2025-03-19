namespace Application.Contracts;

/// <summary>
/// Logic for email message templates
/// </summary>
public interface IEmailMessageTemplateService
{
    /// <summary>
    /// Get authorization code message template
    /// </summary>
    /// <param name="authorizationCode">Authorization code</param>
    /// <returns></returns>
    public Task<string> GetAuthorizationCodeMessageAsync(string authorizationCode);
}