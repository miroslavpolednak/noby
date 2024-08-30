namespace NOBY.Api.Endpoints.Customer.GetCustomerDetail;

public record GetCustomerDetailRequest(SharedTypesCustomerIdentity Identity)
    : IRequest<CustomerGetCustomer>
{
}