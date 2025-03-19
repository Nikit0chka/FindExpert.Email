namespace Infrastructure.Services.SendEmail;

/// <summary>
/// Options for send email service <see cref="SendEmailService"/>
/// </summary>
internal sealed class SendEmailServiceOptions
{
    public string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; }
    public string SmtpPassword { get; set; }
    public bool EnableSsl { get; set; }
    public string FromEmail { get; set; }
}