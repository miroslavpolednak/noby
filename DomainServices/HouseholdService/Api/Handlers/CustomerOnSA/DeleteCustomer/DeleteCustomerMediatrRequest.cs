namespace DomainServices.HouseholdService.Api.Handlers.CustomerOnSA.DeleteCustomer;

internal record DeleteCustomerMediatrRequest(int CustomerOnSAId)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}