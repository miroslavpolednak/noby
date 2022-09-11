namespace FOMS.Api.Endpoints.Address.GetAddressDetail;

internal sealed class GetAddressDetailHandler
    : IRequestHandler<GetAddressDetailRequest, GetAddressDetailResponse>
{
    public async Task<GetAddressDetailResponse> Handle(GetAddressDetailRequest request, CancellationToken cancellationToken)
    {
        return new GetAddressDetailResponse
        {
        };
    }

    public GetAddressDetailHandler()
    {

    }
}
