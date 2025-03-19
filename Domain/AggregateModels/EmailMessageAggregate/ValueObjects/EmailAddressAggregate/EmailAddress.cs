using Ardalis.GuardClauses;

namespace Domain.AggregateModels.EmailMessageAggregate.ValueObjects.EmailAddressAggregate;

/// <inheritdoc />
/// <summary>
/// Email address value object
/// </summary>
public readonly record struct EmailAddress
{
    private string Address { get; }
    private string Domain { get; }

    public string FullAddress => $"{Address}@{Domain}";

    public EmailAddress(string fullAddress)
    {
        Guard.Against.NullOrEmpty(fullAddress, nameof(fullAddress), "Email address is required.");
        Guard.Against.StringTooLong(fullAddress, EmailAddressConstants.MaxEmailAddressLength, nameof(fullAddress), "Email address is too long.");
        Guard.Against.StringTooShort(fullAddress, EmailAddressConstants.MinEmailAddressLength, nameof(fullAddress), "Email address is too short.");

        if (!IsValidEmail(fullAddress))
            throw new ArgumentException("Invalid email format.", nameof(fullAddress));

        var addressParts = fullAddress.Split('@');
        Address = addressParts[0];
        Domain = addressParts[1];

        if (!IsValidDomain(Domain))
            throw new ArgumentException("Invalid domain format.", nameof(fullAddress));
    }

    /// <summary>
    /// Logic of validating email address
    /// </summary>
    /// <param name="email">Address to validate</param>
    /// <returns></returns>
    private static bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Logic of validating domain address
    /// </summary>
    /// <param name="domain">Domain to validate</param>
    /// <returns></returns>
    private static bool IsValidDomain(string domain) => domain.Contains('.') && !domain.StartsWith('.') && !domain.EndsWith('.');
}