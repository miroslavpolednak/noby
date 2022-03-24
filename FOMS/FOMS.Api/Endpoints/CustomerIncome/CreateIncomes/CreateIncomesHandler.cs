using DomainServices.CodebookService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.CustomerIncome.CreateIncomes;

internal class CreateIncomesHandler
    : IRequestHandler<CreateIncomesRequest, int[]>
{
    public async Task<int[]> Handle(CreateIncomesRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(CreateIncomesHandler), request.CustomerOnSAId);

        // at se mi s tim lepe pracuje
        var requestIncomes = request.Incomes ?? new List<CreateIncomeItem>(0);
        var responseModel = requestIncomes.Select(t => t.IncomeId.GetValueOrDefault()).ToArray();

        // vytahnout jiz ulozene prijmy
        var existingIncomes = ServiceCallResult.Resolve<List<_SA.IncomeInList>>(await _householdService.GetIncomeList(request.CustomerOnSAId, cancellationToken));

        // smazat smazane prijmy
        foreach (var incomeToDelete in existingIncomes.Where(t => !requestIncomes.Any(x => x.IncomeId == t.IncomeId)))
        {
            await _householdService.DeleteIncome(incomeToDelete.IncomeId, cancellationToken);
        }

        for (int i = 0; i < requestIncomes.Count; i++)
        {
            var income = requestIncomes[i];

            // zalozeni novych
            if (!income.IncomeId.HasValue)
            {
                responseModel[i] = ServiceCallResult.Resolve<int>(await _householdService.CreateIncome(new _SA.CreateIncomeRequest
                {
                    IncomeTypeId = (int)income.IncomeTypeId,
                    CustomerOnSAId = request.CustomerOnSAId,
                    BaseData = new _SA.IncomeBaseData
                    {
                        Sum = income.Sum,
                        CurrencyCode = income.CurrencyCode
                    }
                }, cancellationToken));

                _logger.EntityCreated(nameof(_SA.Income), responseModel[i]);
            }
            else // update existujicich
                await _householdService.UpdateIncomeBaseData(new _SA.UpdateIncomeBaseDataRequest
                {
                    IncomeId = income.IncomeId.Value,
                    BaseData = new _SA.IncomeBaseData
                    {
                        Sum = income.Sum,
                        CurrencyCode = income.CurrencyCode
                    }
                }, cancellationToken);
        }

        return responseModel;
    }

    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ILogger<CreateIncomesHandler> _logger;

    public CreateIncomesHandler(
        IHouseholdServiceAbstraction householdService,
        ILogger<CreateIncomesHandler> logger)
    {
        _logger = logger;
        _householdService = householdService;
    }
}
