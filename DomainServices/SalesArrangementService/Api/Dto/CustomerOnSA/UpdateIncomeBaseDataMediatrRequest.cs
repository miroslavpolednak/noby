using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal record UpdateIncomeBaseDataMediatrRequest(UpdateIncomeBaseDataRequest Request)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}
