using KafkaFlow;
using System.Collections;
using System.Text;

namespace DomainServices.CaseService.Tests;

internal class MockMessageContext()
    : IMessageContext
{
    public Message Message => new(nameof(MockMessageContext), new MockMessageContext());

    public IMessageHeaders Headers => new MockMessageHeaders();

    public IConsumerContext ConsumerContext => throw new NotImplementedException();

    public IProducerContext ProducerContext => throw new NotImplementedException();

    public IDictionary<string, object> Items => throw new NotImplementedException();

    public IDependencyResolver DependencyResolver => throw new NotImplementedException();

    public IReadOnlyCollection<string> Brokers => throw new NotImplementedException();

    public IMessageContext SetMessage(object key, object value) => throw new NotImplementedException();

    public class MockMessageHeaders : IMessageHeaders
    {
        public byte[] this[string key] { get => []; set => throw new NotImplementedException(); }

        public void Add(string key, byte[] value) => throw new NotImplementedException();

        public IEnumerator<KeyValuePair<string, byte[]>> GetEnumerator() => throw new NotImplementedException();

        public string GetString(string key, Encoding encoding) => "1234";

        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }
}
