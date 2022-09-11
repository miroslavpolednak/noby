using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace FOMS.Api.Endpoints.Address.GetAddressDetail;

[SwaggerSchema(Required = new[] { "SessionId", "AddressId" })]
public sealed class GetAddressDetailRequest
    : IRequest<GetAddressDetailResponse>
{
    /// <summary>
    /// SessionId generované přímo na frontendu (unikátní GUID)
    /// </summary>
    public string SessionId { get; set; } = string.Empty;

    /// <summary>
    /// Id adresy
    /// </summary>
    public long AddressId { get; set; }
}
