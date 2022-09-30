using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Dto;

internal record UpdateIncomeBaseDataMediatrRequest(UpdateIncomeBaseDataRequest Request)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}
