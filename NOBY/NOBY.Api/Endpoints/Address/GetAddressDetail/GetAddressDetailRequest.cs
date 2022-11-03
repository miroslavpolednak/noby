using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.Address.GetAddressDetail;

public sealed class GetAddressDetailRequest
    : IRequest<GetAddressDetailResponse>
{
    /// <summary>
    /// SessionId generované přímo na frontendu (unikátní GUID)
    /// </summary>
    [Required]
    public string SessionId { get; set; } = string.Empty;

    /// <summary>
    /// Id adresy
    /// </summary>
    [Required]
    public string AddressId { get; set; } = string.Empty;

    /// <summary>
    /// Parametr title našeptávače adres
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Id státu z číselníku Country
    /// </summary>
    [Required]
    public int CountryId { get; set; }
}
