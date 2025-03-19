using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Kafka;

/// <inheritdoc />
/// <summary>
/// Kafka consumer base implementation
/// </summary>
/// <typeparam name="TValue">Type of message</typeparam>
internal sealed class Consumer<TValue>:BackgroundService
{
    private readonly string _topic;
    private readonly IConsumer<Ignore, TValue> _consumer;
    private readonly IMessageHandler<TValue> _messageHandler;
    private readonly ILogger<Consumer<TValue>> _logger;

    public Consumer(IOptions<ConsumerOptions> configuration, IMessageHandler<TValue> messageHandler, ILogger<Consumer<TValue>> logger)
    {
        _messageHandler = messageHandler;
        _logger = logger;

        var config = new ConsumerConfig
                     {
                         BootstrapServers = configuration.Value.BootstrapServers,
                         GroupId = configuration.Value.GroupId,
                         AutoOffsetReset = AutoOffsetReset.Earliest
                     };

        _topic = configuration.Value.Topic;
        _consumer = new ConsumerBuilder<Ignore, TValue>(config).SetValueDeserializer(new MessageDeserializer<TValue>()).Build();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) { return Task.Run(() => ConsumeAsync(stoppingToken), stoppingToken); }

    private async Task ConsumeAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topic);
        _logger.LogInformation("Consumer subscribed to Topic: {Topic}", _topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var res = _consumer.Consume(stoppingToken);

                    _logger.LogInformation("Consumer consumed: {Topic}, Value: {Value}", _topic, res.Message.Value);

                    if (res.Message.Value is null)
                    {
                        _logger.LogWarning("Received null message: {Topic}", _topic);
                        continue;
                    }

                    if (!ValidationHelper.TryValidateObject(res.Message.Value, out var validationResults))
                    {
                        _logger.LogWarning("Invalid message received: {Topic}, Value: {Value}, Errors: {Errors}",
                                           _topic,
                                           res.Message.Value,
                                           string.Join(", ", validationResults.Select(static v => v.ErrorMessage)));

                        continue;
                    }

                    try
                    {
                        await _messageHandler.HandleMessage(res.Message.Value, stoppingToken);
                        _logger.LogInformation("Message handled: {Topic}, Value: {Value}", _topic, res.Message.Value);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unexpected error handling consumed message {Topic}, Value: {Value}", _topic, res.Message.Value);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Error consuming message from topic: {Topic}", _topic);
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error consuming topic: {Topic}", _topic);
                    throw;
                }
            }
        }
        finally
        {
            _consumer.Unsubscribe();
            _consumer.Close();
            _logger.LogInformation("Consumer stopped for Topic: {Topic}", _topic);
        }
    }
}