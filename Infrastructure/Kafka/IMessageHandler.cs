namespace Infrastructure.Kafka;

/// <summary>
/// Consumer message handler
/// </summary>
/// <typeparam name="T">Handled message type</typeparam>
public interface IMessageHandler<in T>
{
    /// <summary>
    /// Logic for handling message from consumer
    /// </summary>
    /// <param name="message">Message to handle</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    public Task HandleMessage(T message, CancellationToken cancellationToken);
}