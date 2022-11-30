using DomainServices.HouseholdService.Clients;

namespace NOBY.Api.Endpoints.CustomerIncome.DeleteIncome;

internal class DeleteIncomeHandler
    : AsyncRequestHandler<DeleteIncomeRequest>
{
    protected override async Task Handle(DeleteIncomeRequest request, CancellationToken cancellationToken)
    {
        await _customerService.DeleteIncome(request.IncomeId, cancellationToken);
    }

    private readonly ICustomerOnSAServiceClient _customerService;
    
    public DeleteIncomeHandler(ICustomerOnSAServiceClient customerService)
    {
        _customerService = customerService;
    }
}
