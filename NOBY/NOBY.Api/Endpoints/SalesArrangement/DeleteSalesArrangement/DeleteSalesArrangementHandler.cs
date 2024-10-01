using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using NOBY.Services.SalesArrangementAuthorization;

namespace NOBY.Api.Endpoints.SalesArrangement.DeleteSalesArrangement;

internal sealed class DeleteSalesArrangementHandler(
    ISalesArrangementServiceClient _salesArrangementService, 
    IHouseholdServiceClient _householdService, 
    ICodebookServiceClient _codebookService)
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
        if (!saInstance.IsInState(_allowedSAStates)
            // validace na typ SA
            || ISalesArrangementAuthorizationService.RefinancingSATypes.Contains(saInstance.SalesArrangementTypeId))
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

    private static readonly EnumSalesArrangementStates[] _allowedSAStates =
    [
        EnumSalesArrangementStates.InProgress,
        EnumSalesArrangementStates.NewArrangement,
        EnumSalesArrangementStates.InSigning,
        EnumSalesArrangementStates.ToSend
    ];

    private static readonly int[] _householdDeleteSATypes =
    [
        (int)SalesArrangementTypes.CustomerChange3602A,
        (int)SalesArrangementTypes.CustomerChange3602B,
        (int)SalesArrangementTypes.CustomerChange3602C
    ];
}
