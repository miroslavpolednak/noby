using DomainServices.CodebookService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.CustomerIncome.UpdateIncomes;

internal class UpdateIncomesHandler
    : IRequestHandler<UpdateIncomesRequest, int[]>
{
    public async Task<int[]> Handle(UpdateIncomesRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateIncomesHandler), request.CustomerOnSAId);

        // at se mi s tim lepe pracuje
        var requestIncomes = request.Incomes ?? new List<Dto.IncomeBaseData>(0);
        var responseModel = requestIncomes.Select(t => t.IncomeId.GetValueOrDefault()).ToArray();

        // vytahnout jiz ulozene prijmy
        var existingIncomes = ServiceCallResult.Resolve<List<_SA.IncomeInList>>(await _customerService.GetIncomeList(request.CustomerOnSAId, cancellationToken));

        // smazat smazane prijmy
        foreach (var incomeToDelete in existingIncomes.Where(t => !requestIncomes.Any(x => x.IncomeId == t.IncomeId)))
        {
            await _customerService.DeleteIncome(incomeToDelete.IncomeId, cancellationToken);
        }

        for (int i = 0; i < requestIncomes.Count; i++)
        {
            var income = requestIncomes[i];

            // zalozeni novych
            if (!income.IncomeId.HasValue)
            {
                responseModel[i] = ServiceCallResult.Resolve<int>(await _customerService.CreateIncome(new()
                {
                    IncomeTypeId = (int)income.IncomeTypeId,
                    CustomerOnSAId = request.CustomerOnSAId,
                    BaseData = new()
                    {
                        Sum = income.Sum,
                        CurrencyCode = income.CurrencyCode
                    }
                }, cancellationToken));

                _logger.EntityCreated(nameof(_SA.Income), responseModel[i]);
            }
            else // update existujicich
                await _customerService.UpdateIncomeBaseData(new()
                {
                    IncomeId = income.IncomeId.Value,
                    BaseData = new()
                    {
                        Sum = income.Sum,
                        CurrencyCode = income.CurrencyCode
                    }
                }, cancellationToken);
        }

        return responseModel;
    }

    private readonly ICustomerOnSAServiceAbstraction _customerService;
    private readonly ILogger<UpdateIncomesHandler> _logger;

    public UpdateIncomesHandler(
        ICustomerOnSAServiceAbstraction customerService,
        ILogger<UpdateIncomesHandler> logger)
    {
        _logger = logger;
        _customerService = customerService;
    }
}
