namespace NOBY.Api.Endpoints.Customer.GetCustomerDetail;

public record GetCustomerDetailRequest(long Id, SharedTypes.Enums.IdentitySchemes Schema)
    : IRequest<GetCustomerDetailResponse>
{
}