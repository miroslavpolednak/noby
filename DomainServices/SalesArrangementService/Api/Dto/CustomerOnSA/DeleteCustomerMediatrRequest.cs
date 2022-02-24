namespace DomainServices.SalesArrangementService.Api.Dto;

internal record DeleteCustomerMediatrRequest(int CustomerOnSAId)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}