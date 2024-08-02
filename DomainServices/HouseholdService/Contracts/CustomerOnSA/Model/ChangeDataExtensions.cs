using System.Text.Json;
using System.Text.Json.Serialization;
using DomainServices.HouseholdService.Contracts.Model;

namespace DomainServices.HouseholdService.Contracts;

public static class ChangeDataExtensions
{
    private static readonly JsonSerializerOptions _deserializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
    };

    public static CustomerChangeData? GetCustomerChangeDataObject(this CustomerOnSA customerOnSA)
    {
        if (string.IsNullOrWhiteSpace(customerOnSA.CustomerChangeData))
            return null;

        return customerOnSA.InternalCustomerChangeData ??= JsonSerializer.Deserialize<CustomerChangeData>(customerOnSA.CustomerChangeData, _deserializerOptions);
    }

    public static void UpdateCustomerChangeDataObject(this UpdateCustomerDetailRequest customerUpdate, CustomerChangeData? changeData)
    {
        if (changeData is null)
        {
            customerUpdate.CustomerChangeData = null;

            return;
        }

        customerUpdate.CustomerChangeData = JsonSerializer.Serialize(changeData, _serializerOptions);
    }
}