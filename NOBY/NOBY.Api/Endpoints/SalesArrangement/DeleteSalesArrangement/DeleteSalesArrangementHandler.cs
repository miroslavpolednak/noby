using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.SalesArrangement.DeleteSalesArrangement;

internal sealed class DeleteSalesArrangementHandler
    : IRequestHandler<DeleteSalesArrangementRequest>
{
    public async Task Handle(DeleteSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

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

    private static int[] _householdDeleteSATypes = new[] { 10, 11, 12 };

    private readonly IHouseholdServiceClient _householdService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public DeleteSalesArrangementHandler(ISalesArrangementServiceClient salesArrangementService, IHouseholdServiceClient householdService)
    {
        _householdService = householdService;
        _salesArrangementService = salesArrangementService;
    }
}
