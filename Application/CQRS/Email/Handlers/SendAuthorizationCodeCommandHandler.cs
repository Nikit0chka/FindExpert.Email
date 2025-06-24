using Application.Contracts;
using Application.CQRS.Email.Commands;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Domain.AggregateModels.EmailMessageAggregate;
using Domain.AggregateModels.EmailMessageAggregate.ValueObjects.EmailAddressAggregate;
using Domain.AggregateModels.EmailMessageAggregate.ValueObjects.RecipientAggregate;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Email.Handlers;

internal class SendAuthorizationCodeCommandHandler(ILogger<SendAuthorizationCodeCommandHandler> logger, ISendEmailService sendEmailService, IEmailMessageTemplateService emailMessageTemplateService):ICommandHandler<SendAuthorizationCodeCommand, Result>
{
    public async Task<Result> Handle(SendAuthorizationCodeCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Command} for {Email}", nameof(SendAuthorizationCodeCommand), request.Email);

        try
        {
            logger.LogInformation("Generating email message for {Email}", request.Email);

            var emailAddress = new EmailAddress(request.Email);
            var recipient = new Recipient(emailAddress);

            var messageByTemplate = await emailMessageTemplateService.GetAuthorizationCodeMessageAsync(request.AuthorizationCode);

            var message = new EmailMessage("Confirmation code", messageByTemplate, recipient);

            logger.LogInformation("Sending authorization code to {Email}", request.Email);
            await sendEmailService.SendEmailAsync(message, cancellationToken);

            logger.LogInformation("{Command} handled successful for {Email}", nameof(SendAuthorizationCodeCommand), request.Email);
            return Result.Success();
        }

        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send authorization code for {Email}", request.Email);
            return Result.Error("An error occurred while processing your request.");
        }
    }
}

