namespace DomainServices.SalesArrangementService.Api.Dto;

internal sealed record DeleteSalesArrangementMediatrRequest(int SalesArrangementId)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}
