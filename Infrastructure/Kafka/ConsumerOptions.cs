namespace Infrastructure.Kafka;

/// <summary>
/// Options for kafka consumer <see cref="Consumer{TValue}"/>
/// </summary>
internal sealed class ConsumerOptions
{
    public string BootstrapServers { get; set; }
    public string GroupId { get; set; }
    public string Topic { get; set; }
}