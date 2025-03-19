using Application.Contracts;
using Infrastructure.EmailTemplates.AuthorizationCode;
using RazorLight;

namespace Infrastructure.Services;

/// <inheritdoc />
/// <summary>
/// Email message template service implementation <see cref="IEmailMessageTemplateService"/>
/// </summary>
internal sealed class EmailMessageTemplateService:IEmailMessageTemplateService
{
    private const string EmailTemplatesDirectory = "Infrastructure\\EmailTemplates";

    public Task<string> GetAuthorizationCodeMessageAsync(string authorizationCode)
    {
        var authorizationCodeModel = new AuthorizationCodeModel(authorizationCode);

        var baseDirectory = Path.Combine(Environment.CurrentDirectory, "..");

        var engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(Path.Combine(baseDirectory, EmailTemplatesDirectory))
            .UseMemoryCachingProvider()
            .Build();

        return engine.CompileRenderAsync("AuthorizationCode/AuthorizationCodeTemplate.cshtml", authorizationCodeModel);
    }
}