namespace FOMS.Api.Endpoints.Address.GetAddressDetail;

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
