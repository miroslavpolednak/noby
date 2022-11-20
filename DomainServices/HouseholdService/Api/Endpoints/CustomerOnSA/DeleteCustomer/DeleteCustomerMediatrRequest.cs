namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.DeleteCustomer;

internal record DeleteCustomerMediatrRequest(int CustomerOnSAId, bool HardDelete)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}