using Microsoft.EntityFrameworkCore;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.DeleteSalesArrangement;

internal sealed class DeleteSalesArrangementHandler
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
        await _dbContext
            .SalesArrangementsParameters
            .Where(t => t.SalesArrangementId == request.SalesArrangementId)
            .ExecuteDeleteAsync(cancellation);

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

    private readonly HouseholdService.Clients.IHouseholdServiceClient _householdService;
    private readonly Database.SalesArrangementServiceDbContext _dbContext;

    public DeleteSalesArrangementHandler(
        HouseholdService.Clients.IHouseholdServiceClient householdService,
        Database.SalesArrangementServiceDbContext dbContext)
    {
        _householdService = householdService;
        _dbContext = dbContext;
    }
}
