namespace FOMS.Api.Endpoints.Customer.GetDetail;

public record GetDetailRequest(int Id, CIS.Foms.Enums.IdentitySchemes Schema)
    : IRequest<GetDetailResponse>
{
}