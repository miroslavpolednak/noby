namespace DomainServices.SalesArrangementService.Api.Dto;

internal sealed record DeleteSalesArrangementMediatrRequest(int SalesArrangementId, bool HardDelete)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}
