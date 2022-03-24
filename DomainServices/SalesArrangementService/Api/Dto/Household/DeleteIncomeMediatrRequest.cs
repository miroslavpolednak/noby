namespace DomainServices.SalesArrangementService.Api.Dto;

internal record DeleteIncomeMediatrRequest(int IncomeId)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}
