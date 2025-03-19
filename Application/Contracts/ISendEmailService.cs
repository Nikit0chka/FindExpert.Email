using Domain.AggregateModels.EmailMessageAggregate;

namespace Application.Contracts;

/// <summary>
/// Logic for sending emails
/// </summary>
public interface ISendEmailService
{
    /// <summary>
    /// Send email logic
    /// </summary>
    /// <param name="emailMessage">Email message to sent</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    public Task SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken);
}