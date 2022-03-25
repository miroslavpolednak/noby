using DomainServices.SalesArrangementService.Abstraction;

namespace FOMS.Api.Endpoints.CustomerIncome.DeleteIncome;

internal class DeleteIncomeHandler
    : AsyncRequestHandler<DeleteIncomeRequest>
{
    protected override async Task Handle(DeleteIncomeRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(DeleteIncomeHandler), request.IncomeId);

        ServiceCallResult.Resolve(await _customerService.DeleteIncome(request.IncomeId, cancellationToken));
    }

    private readonly ICustomerOnSAServiceAbstraction _customerService;
    private readonly ILogger<DeleteIncomeHandler> _logger;

    public DeleteIncomeHandler(
        ICustomerOnSAServiceAbstraction customerService,
        ILogger<DeleteIncomeHandler> logger)
    {
        _logger = logger;
        _customerService = customerService;
    }
}
