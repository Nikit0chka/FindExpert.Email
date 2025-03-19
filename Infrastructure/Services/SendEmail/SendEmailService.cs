using Application.Contracts;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Domain.AggregateModels.EmailMessageAggregate;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.SendEmail;

/// <inheritdoc />
internal sealed class SendEmailService(IOptions<SendEmailServiceOptions> options):ISendEmailService
{
    public async Task SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken)
    {
        if (emailMessage.Recipients.Count == 0)
            throw new ArgumentException("No recipients specified", nameof(emailMessage));

        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress("MasterOK", options.Value.FromEmail));
        mailMessage.Subject = emailMessage.Subject;

        foreach (var recipient in emailMessage.Recipients)
        {
            mailMessage.To.Add(new MailboxAddress(recipient.Name, recipient.EmailAddress.FullAddress));
        }

        mailMessage.Body = new TextPart(TextFormat.Html)
                           {
                               Text = emailMessage.Body
                           };

        using var smtpClient = new SmtpClient();
        await smtpClient.ConnectAsync(options.Value.SmtpHost, options.Value.SmtpPort, options.Value.EnableSsl, cancellationToken);
        await smtpClient.AuthenticateAsync(options.Value.SmtpUsername, options.Value.SmtpPassword, cancellationToken);
        await smtpClient.SendAsync(mailMessage, cancellationToken);
        await smtpClient.DisconnectAsync(true, cancellationToken);
    }
}