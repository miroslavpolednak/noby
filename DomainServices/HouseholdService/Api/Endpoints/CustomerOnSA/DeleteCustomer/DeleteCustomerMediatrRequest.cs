namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.DeleteCustomer;

internal record DeleteCustomerMediatrRequest(int CustomerOnSAId)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}