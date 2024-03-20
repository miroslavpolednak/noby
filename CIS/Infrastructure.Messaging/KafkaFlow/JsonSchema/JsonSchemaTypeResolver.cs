using KafkaFlow;
using KafkaFlow.Middlewares.Serializer.Resolvers;
using System.Collections.Concurrent;
using System.Text;

namespace CIS.Infrastructure.Messaging.KafkaFlow.JsonSchema;

internal sealed class JsonSchemaTypeResolver : IMessageTypeResolver
{
    private static readonly ConcurrentDictionary<string, Type?> _types = new();
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    public async ValueTask<Type?> OnConsumeAsync(IMessageContext context)
    {
        var payloadId = GetPayloadId(context);

        if (_types.TryGetValue(payloadId, out var type))
            return type;

        await _semaphore.WaitAsync();

        try
        {
            if (_types.TryGetValue(payloadId, out type))
                return type;

            return _types[payloadId] = AppDomain.CurrentDomain
                                                .GetAssemblies()
                                                .Select(a => a.GetType(payloadId))
                                                .FirstOrDefault(x => x != null);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public ValueTask OnProduceAsync(IMessageContext context) => default;

    private static string GetPayloadId(IMessageContext context)
    {
        var header = context.Headers.Single(h => h.Key == "messaging.kafka.payloadTypeId");

        return Encoding.Default.GetString(header.Value);
    }
}