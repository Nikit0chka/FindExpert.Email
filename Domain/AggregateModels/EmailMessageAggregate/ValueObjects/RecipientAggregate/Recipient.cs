using Domain.AggregateModels.EmailMessageAggregate.ValueObjects.EmailAddressAggregate;

namespace Domain.AggregateModels.EmailMessageAggregate.ValueObjects.RecipientAggregate;

/// <inheritdoc />
/// <summary>
/// Email recipient value object
/// </summary>
/// <param name="EmailAddress">Recipient address</param>
/// <param name="Name">Recipient name</param>
public readonly record struct Recipient(EmailAddress EmailAddress, string? Name = null);