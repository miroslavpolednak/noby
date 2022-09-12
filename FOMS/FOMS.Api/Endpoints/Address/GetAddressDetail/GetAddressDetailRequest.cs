using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FOMS.Api.Endpoints.Address.GetAddressDetail;

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
}
