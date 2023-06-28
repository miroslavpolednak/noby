using DomainServices.HouseholdService.Clients;

namespace NOBY.Api.Endpoints.CustomerIncome.DeleteIncome;

internal sealed class DeleteIncomeHandler
    : IRequestHandler<DeleteIncomeRequest>
{
    public async Task Handle(DeleteIncomeRequest request, CancellationToken cancellationToken)
    {
        await _customerService.DeleteIncome(request.IncomeId, cancellationToken);
    }

    private readonly ICustomerOnSAServiceClient _customerService;
    
    public DeleteIncomeHandler(ICustomerOnSAServiceClient customerService)
    {
        _customerService = customerService;
    }
}
