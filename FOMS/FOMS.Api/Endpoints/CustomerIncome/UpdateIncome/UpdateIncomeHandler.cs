using DomainServices.SalesArrangementService.Abstraction;

namespace FOMS.Api.Endpoints.CustomerIncome.UpdateIncome;

internal class UpdateIncomeHandler
    : IRequestHandler<UpdateIncomeRequest, int>
{
    public async Task<int> Handle(UpdateIncomeRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateIncomeHandler), request.IncomeId);

        ServiceCallResult.Resolve(await _householdService.UpdateIncome(new DomainServices.SalesArrangementService.Contracts.UpdateIncomeRequest
        {
            IncomeId = request.IncomeId,
            BaseData = new DomainServices.SalesArrangementService.Contracts.IncomeBaseData
            {
                CurrencyCode = request.CurrencyCode,
                Sum = request.Sum
            }
        }, cancellationToken));

        return request.IncomeId;
    }

    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ILogger<UpdateIncomeHandler> _logger;

    public UpdateIncomeHandler(
        IHouseholdServiceAbstraction householdService,
        ILogger<UpdateIncomeHandler> logger)
    {
        _logger = logger;
        _householdService = householdService;
    }
}
