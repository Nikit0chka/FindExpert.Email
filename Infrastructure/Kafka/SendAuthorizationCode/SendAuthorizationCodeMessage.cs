using System.ComponentModel.DataAnnotations;
using Domain.AggregateModels.EmailMessageAggregate.ValueObjects.EmailAddressAggregate;

namespace Infrastructure.Kafka.SendAuthorizationCode;

/// <summary>
/// Kafka send authorization code message
/// </summary>
public sealed class SendAuthorizationCodeMessage
{
    [Required(ErrorMessage = "Email address is required")]
    [MinLength(EmailAddressConstants.MinEmailAddressLength, ErrorMessage = "Email address is too short")]
    [MaxLength(EmailAddressConstants.MaxEmailAddressLength, ErrorMessage = "Email address is too long")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Code is required")]
    [MinLength(1, ErrorMessage = "Confirmation code is too short")]
    [MaxLength(6, ErrorMessage = "Confirmation code is too long")]
    public string Code { get; set; }
}