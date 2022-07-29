namespace FOMS.Api.Endpoints.Customer.GetDetail;

public record GetDetailRequest(long Id, CIS.Foms.Enums.IdentitySchemes Schema)
    : IRequest<GetDetailResponse>
{
}