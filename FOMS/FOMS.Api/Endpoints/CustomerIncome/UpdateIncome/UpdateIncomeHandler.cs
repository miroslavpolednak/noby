using DomainServices.SalesArrangementService.Abstraction;

namespace FOMS.Api.Endpoints.CustomerIncome.UpdateIncome;

internal class UpdateIncomeHandler
    : AsyncRequestHandler<UpdateIncomeRequest>
{
    protected override async Task Handle(UpdateIncomeRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateIncomeHandler), request.IncomeId);

        ServiceCallResult.Resolve(await _customerService.UpdateIncome(new DomainServices.SalesArrangementService.Contracts.UpdateIncomeRequest
        {
            IncomeId = request.IncomeId,
            BaseData = new DomainServices.SalesArrangementService.Contracts.IncomeBaseData
            {
                CurrencyCode = request.CurrencyCode,
                Sum = request.Sum
            }
        }, cancellationToken));
    }

    private readonly ICustomerOnSAServiceAbstraction _customerService;
    private readonly ILogger<UpdateIncomeHandler> _logger;

    public UpdateIncomeHandler(
        ICustomerOnSAServiceAbstraction customerService,
        ILogger<UpdateIncomeHandler> logger)
    {
        _logger = logger;
        _customerService = customerService;
    }
}
