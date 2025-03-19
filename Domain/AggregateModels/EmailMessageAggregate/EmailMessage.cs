using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Domain.AggregateModels.EmailMessageAggregate.ValueObjects.RecipientAggregate;

namespace Domain.AggregateModels.EmailMessageAggregate;

/// <inheritdoc />
/// <summary>
/// Email message entity
/// </summary>
public sealed class EmailMessage(string subject, string body, Recipient recipient):IAggregateRoot
{
    public string Subject { get; private set; } = Guard.Against.NullOrEmpty(subject, nameof(subject), "Email subject is required.");
    public string Body { get; private set; } = Guard.Against.NullOrEmpty(body, nameof(body), "Email message body is required.");
    public readonly ICollection<Recipient> Recipients = new List<Recipient> { recipient };
}