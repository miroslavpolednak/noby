using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateSalesArrangementState;

internal sealed class UpdateSalesArrangementStateHandler(
    Database.SalesArrangementServiceDbContext _dbContext, 
    TimeProvider _timeProvider)
		: IRequestHandler<Contracts.UpdateSalesArrangementStateRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.UpdateSalesArrangementStateRequest request, CancellationToken cancellation)
    {
        // kontrola existence SA
        var entity = await _dbContext
            .SalesArrangements
            .FirstOrDefaultAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);

        // update stavu SA
        entity.State = request.State;
        entity.StateUpdateTime = _timeProvider.GetLocalNow().DateTime;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}
