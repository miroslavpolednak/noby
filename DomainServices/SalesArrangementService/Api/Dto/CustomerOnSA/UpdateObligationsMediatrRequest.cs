namespace DomainServices.SalesArrangementService.Api.Dto;

internal record UpdateObligationsMediatrRequest(Contracts.UpdateObligationsRequest Request)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}
