namespace CIS.Core.Exceptions.ExternalServices;

/// <summary>
/// Chyba, která vzniká při volání API třetích stran. Pokud se nepodaří deserializovat response volaného API, vyhodíme tuto vyjímku.
/// </summary>
/// <remarks>
/// Vznikne, pokud API třetí strany vrátí jiný JSON, než jaký očekáváme my.
/// </remarks>
public sealed class CisExtServiceResponseDeserializationException
    : BaseCisException
{
    public CisExtServiceResponseDeserializationException(int exceptionCode, string serviceName, string url, string responseModelType)
        : base(exceptionCode, $"{serviceName} response from {url} can not be deserialized to {responseModelType}")
    { }
}