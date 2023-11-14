using Serilog.Core;
using Serilog.Events;
using System.Text;

namespace CIS.Infrastructure.Telemetry.Enrichers;

/// <summary>
/// Enricher, ktery kontroluje delku vlastnosti Payload (pokud je v LogEvent) a byte length presahne trashold, trimuje Payload value na pozadovany max length
/// </summary>
internal sealed class PayloadMaxLengthEnricher
    : ILogEventEnricher
{
    private const string _payloadKey = "Payload";
    private readonly int _trashold;

    public PayloadMaxLengthEnricher(int trashold)
    {
        _trashold = trashold;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (logEvent.Properties.ContainsKey(_payloadKey))
        {
            if (logEvent.Properties[_payloadKey] is ScalarValue eventValue && eventValue.Value != null)
            {
                var payload = eventValue.ToString("l", null);
                CIS.Core.StringExtensions.TrimUtf8String(ref payload, _trashold);
                logEvent.AddOrUpdateProperty(new LogEventProperty("Payload", new ScalarValue(payload)));
            }
        }
    }
}
