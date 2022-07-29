namespace DomainServices.SalesArrangementService.Api.Dto;

internal record DeleteObligationMediatrRequest(int ObligationId)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}
