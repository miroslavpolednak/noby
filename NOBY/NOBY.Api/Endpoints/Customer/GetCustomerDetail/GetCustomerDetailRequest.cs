namespace NOBY.Api.Endpoints.Customer.GetCustomerDetail;

public record GetCustomerDetailRequest(long Id, CIS.Foms.Enums.IdentitySchemes Schema)
    : IRequest<GetCustomerDetailResponse>
{
}