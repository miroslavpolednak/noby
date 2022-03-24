using DomainServices.SalesArrangementService.Abstraction;

namespace FOMS.Api.Endpoints.CustomerIncome.DeleteIncome;

internal class DeleteIncomeHandler
        : IRequestHandler<DeleteIncomeRequest, int>
{
    public async Task<int> Handle(DeleteIncomeRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(DeleteIncomeHandler), request.IncomeId);

        ServiceCallResult.Resolve(await _householdService.DeleteIncome(request.IncomeId, cancellationToken));

        return request.IncomeId;
    }

    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ILogger<DeleteIncomeHandler> _logger;

    public DeleteIncomeHandler(
        IHouseholdServiceAbstraction householdService,
        ILogger<DeleteIncomeHandler> logger)
    {
        _logger = logger;
        _householdService = householdService;
    }
}
