using System.Text.Json;
using DomainServices.HouseholdService.Contracts.Dto;

namespace DomainServices.HouseholdService.Clients.Services.Internal;

internal sealed class CustomerChangeData
{
    private readonly JsonDocument _jsonDocument;

    private JsonElement? _naturalPersonElement;

    private static JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };

    private CustomerChangeData(string customerChangeData)
    {
        _jsonDocument = JsonDocument.Parse(customerChangeData);

        Delta = ParseDelta();
    }

    public static CustomerChangeData? TryCreate(string? customerChangeData)
    {
        if (string.IsNullOrWhiteSpace(customerChangeData))
            return default;

        return new CustomerChangeData(customerChangeData);
    }

    public CustomerChangeDataDelta Delta { get; }

    public TValue? GetNaturalPersonAttributeOrDefault<TValue>(string attributeName, TValue defaultValue)
    {
        InitializeNaturalPersonElement();

        if (!_naturalPersonElement!.Value.TryGetProperty(attributeName, out var value))
            return defaultValue;

        return value.Deserialize<TValue>();
    }

    private CustomerChangeDataDelta ParseDelta()
    {
        return _jsonDocument.Deserialize<CustomerChangeDataDelta>(_options) ?? new CustomerChangeDataDelta();
    }

    private void InitializeNaturalPersonElement()
    {
        if (_jsonDocument.RootElement.TryGetProperty(nameof(CustomerChangeDataDelta.NaturalPerson), out var naturalPersonElement))
        {
            _naturalPersonElement = naturalPersonElement;

            return;
        }

        _naturalPersonElement = new JsonElement();
    }
}