using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Address.GetAddressDetail;

public sealed class GetAddressDetailResponse
    : ExternalServices.AddressWhisperer.Dto.AddressDetail
{
    /// <summary>
    /// Stát - AddressWhispererBEService|country
    /// </summary>
    public int? CountryId { get; set; }

    // skryt aby nebyl videt v API
    [JsonIgnore]
    public new string? Country { get; set; } = null!;
}
