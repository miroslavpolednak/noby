using System.Text.Json.Serialization;

namespace DomainServices.RiskIntegrationService.ExternalServices;

public static class C4mJsonOptions
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

    static C4mJsonOptions()
    {
        _jsonOptions.Converters.Add(new CIS.Infrastructure.Json.DateTimeOffsetConverterUsingDateTimeParse());
    }
}
