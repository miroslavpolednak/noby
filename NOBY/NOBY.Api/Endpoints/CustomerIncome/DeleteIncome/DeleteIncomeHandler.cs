using DomainServices.HouseholdService.Clients;
using NOBY.Services.FlowSwitchAtLeastOneIncomeMainHousehold;

namespace NOBY.Api.Endpoints.CustomerIncome.DeleteIncome;

internal sealed class DeleteIncomeHandler(
    ICustomerOnSAServiceClient _customerService, 
    FlowSwitchAtLeastOneIncomeMainHouseholdService _flowSwitchMainHouseholdService)
        : IRequestHandler<DeleteIncomeRequest>
{
    public async Task Handle(DeleteIncomeRequest request, CancellationToken cancellationToken)
    {
        await _customerService.DeleteIncome(request.IncomeId, cancellationToken);

        await _flowSwitchMainHouseholdService.SetFlowSwitchByCustomerOnSAId(request.CustomerOnSAId, cancellationToken: cancellationToken);
    }
}
