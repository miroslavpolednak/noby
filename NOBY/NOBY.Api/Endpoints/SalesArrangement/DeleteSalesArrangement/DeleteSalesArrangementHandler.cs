using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.SalesArrangement.DeleteSalesArrangement;

internal sealed class DeleteSalesArrangementHandler
    : IRequestHandler<DeleteSalesArrangementRequest>
{
    public async Task Handle(DeleteSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        var saCategory = (await _codebookService.SalesArrangementTypes(cancellationToken)).First(t => t.Id == saInstance.SalesArrangementTypeId);

        if (saCategory.SalesArrangementCategory == (int)SalesArrangementCategories.ProductRequest)
        {
            throw new NobyValidationException("Product SA can not be deleted");
        }

        // validace na stav SA
        if (!_allowedSAStates.Contains(saInstance.State))
        {
            throw new NobyValidationException(90032);
        }

        // pro urcite typy domacnosti smazat household
        if (_householdDeleteSATypes.Contains(saInstance.SalesArrangementTypeId))
        {
            var households = await _householdService.GetHouseholdList(saInstance.SalesArrangementId, cancellationToken);
            foreach (var household in households)
            {
                await _householdService.DeleteHousehold(household.HouseholdId, cancellationToken: cancellationToken);
            }
        }

        // smazat vlastni SA
        await _salesArrangementService.DeleteSalesArrangement(request.SalesArrangementId, cancellationToken: cancellationToken);
    }

    private static int[] _allowedSAStates = new[] 
    { 
        (int)SalesArrangementStates.InProgress,
        (int)SalesArrangementStates.NewArrangement,
        (int)SalesArrangementStates.InSigning,
        (int)SalesArrangementStates.ToSend
    };
    private static int[] _householdDeleteSATypes = new[] 
    {
        (int)SalesArrangementTypes.CustomerChange3602A,
        (int)SalesArrangementTypes.CustomerChange3602B,
        (int)SalesArrangementTypes.CustomerChange3602C
    };

    private readonly IHouseholdServiceClient _householdService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICodebookServiceClient _codebookService;

    public DeleteSalesArrangementHandler(ISalesArrangementServiceClient salesArrangementService, IHouseholdServiceClient householdService, ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
        _householdService = householdService;
        _salesArrangementService = salesArrangementService;
    }
}
