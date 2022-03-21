using DomainServices.SalesArrangementService.Abstraction;

namespace FOMS.Api.Endpoints.CustomerIncome.GetIncomes;

internal class GetIncomesHandler
    : IRequestHandler<GetIncomesRequest, List<IncomeInList>>
{
    public async Task<List<IncomeInList>> Handle(GetIncomesRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetIncomesHandler), request.CustomerOnSAId);

        var list = ServiceCallResult.Resolve<List<DomainServices.SalesArrangementService.Contracts.IncomeInList>>(await _householdService.GetIncomeList(request.CustomerOnSAId, cancellationToken));

        var model = list.Select(t => new IncomeInList
            {
                IncomeId = t.IncomeId,
                CurrencyCode = t.CurrencyCode,
                IncomeTypeId = (CIS.Foms.Enums.CustomerIncomeTypes)t.IncomeTypeId,
                Sum = t.Sum,
            })
            .ToList();

        _logger.FoundItems(model.Count, nameof(IncomeInList));

        return model;
    }

    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ILogger<GetIncomesHandler> _logger;

    public GetIncomesHandler(
        IHouseholdServiceAbstraction householdService,
        ILogger<GetIncomesHandler> logger)
    {
        _logger = logger;
        _householdService = householdService;
    }
}
