namespace NOBY.Api.Endpoints.Customer.ProfileCheck;

public sealed record ProfileCheckRequest(SharedTypesCustomerIdentity Identity)
    : IRequest<CustomerProfileCheckResponse>
{
}
