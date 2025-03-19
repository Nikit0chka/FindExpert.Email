using System.Text.Json;
using Confluent.Kafka;

namespace Infrastructure.Kafka;

/// <inheritdoc />
/// <summary>
/// Kafka message deserializer
/// </summary>
/// <typeparam name="TValue">Deserialization type</typeparam>
internal sealed class MessageDeserializer<TValue>:IDeserializer<TValue>
{
    public TValue Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context) => JsonSerializer.Deserialize<TValue>(data) ?? throw new InvalidOperationException("Unable to deserialize Kafka value.");
}