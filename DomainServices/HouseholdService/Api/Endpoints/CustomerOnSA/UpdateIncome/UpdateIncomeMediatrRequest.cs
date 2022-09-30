using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateIncome;

internal record UpdateIncomeMediatrRequest(UpdateIncomeRequest Request)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
}
