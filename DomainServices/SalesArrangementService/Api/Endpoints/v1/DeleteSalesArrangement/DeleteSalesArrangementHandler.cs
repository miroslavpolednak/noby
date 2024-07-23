using Microsoft.EntityFrameworkCore;
using DomainServices.SalesArrangementService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.SalesArrangementService.Api.Endpoints.DeleteSalesArrangement;

internal sealed class DeleteSalesArrangementHandler(
	HouseholdService.Clients.IHouseholdServiceClient _householdService,
	Database.SalesArrangementServiceDbContext _dbContext,
	IDocumentDataStorage _documentDataStorage)
		: IRequestHandler<DeleteSalesArrangementRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteSalesArrangementRequest request, CancellationToken cancellation)
    {
        var saInstance = await _dbContext.SalesArrangements.FirstOrDefaultAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);

        if (!request.HardDelete)
        {
            // kontrola na stav
            if (!_allowedStates.Contains(saInstance.State))
            {
                throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.SalesArrangementCantDelete, saInstance.State);
            }
        }

        // smazat navazne entity
        var households = await _householdService.GetHouseholdList(request.SalesArrangementId, cancellation);
        foreach (var household in households)
        {
            await _householdService.DeleteHousehold(household.HouseholdId, true, cancellation);
        }

        // smazat SA
        _dbContext.Remove(saInstance);

        // smazat parametry
        await _documentDataStorage.DeleteByEntityId(request.SalesArrangementId, Database.DocumentDataEntities.SalesArrangementParametersConst.TableName);

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private static readonly int[] _allowedStates =
    [
        (int)SalesArrangementStates.NewArrangement,
        (int)SalesArrangementStates.InProgress,
        (int)SalesArrangementStates.ToSend,
        (int)SalesArrangementStates.InSigning
    ];
}
