using System.Text.Json.Serialization;

namespace DomainServices.RiskIntegrationService.ExternalServices;

internal static class HttpClientFactoryExtensions
{
    public static System.Text.Json.JsonSerializerOptions CustomJsonOptions
    {
        get => _jsonOptions;
    }

    private static System.Text.Json.JsonSerializerOptions _jsonOptions = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        NumberHandling = JsonNumberHandling.AllowReadingFromString //TODO odstranit az c4m opravi format cisel
    };

    static HttpClientFactoryExtensions()
    {
        _jsonOptions.Converters.Add(new CIS.Infrastructure.Json.DateTimeOffsetConverterUsingDateTimeParse());
    }
}
