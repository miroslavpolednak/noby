using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateIncomeBaseData;

internal record UpdateIncomeBaseDataMediatrRequest(UpdateIncomeBaseDataRequest Request)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}
