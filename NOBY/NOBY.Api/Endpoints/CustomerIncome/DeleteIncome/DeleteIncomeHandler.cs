using DomainServices.HouseholdService.Clients;
using NOBY.Services.FlowSwitchAtLeastOneIncomeMainHousehold;

namespace NOBY.Api.Endpoints.CustomerIncome.DeleteIncome;

internal sealed class DeleteIncomeHandler
    : IRequestHandler<DeleteIncomeRequest>
{
    public async Task Handle(DeleteIncomeRequest request, CancellationToken cancellationToken)
    {
        await _customerService.DeleteIncome(request.IncomeId, cancellationToken);

        await _flowSwitchMainHouseholdService.SetFlowSwitchByCustomerOnSAId(request.CustomerOnSAId, false, cancellationToken);
    }

    private readonly FlowSwitchAtLeastOneIncomeMainHouseholdService _flowSwitchMainHouseholdService;
    private readonly ICustomerOnSAServiceClient _customerService;
    
    public DeleteIncomeHandler(ICustomerOnSAServiceClient customerService, FlowSwitchAtLeastOneIncomeMainHouseholdService flowSwitchMainHouseholdService)
    {
        _flowSwitchMainHouseholdService = flowSwitchMainHouseholdService;
        _customerService = customerService;
    }
}
