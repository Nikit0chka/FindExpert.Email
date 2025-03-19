using Application.CQRS.Email.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Kafka.SendAuthorizationCode;

/// <inheritdoc />
/// <summary>
/// Send authorization code consumer message handler
/// <see cref="T:Infrastructure.Kafka.Consumer`1" />
/// </summary>
/// <param name="mediator">Mediator</param>
public sealed class SendAuthorizationCodeHandler(IServiceScopeFactory scopeFactory):IMessageHandler<SendAuthorizationCodeMessage>
{
    public Task HandleMessage(SendAuthorizationCodeMessage message, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return mediator.Send(new SendAuthorizationCodeCommand(message.Address, message.Code), cancellationToken);

    }
}