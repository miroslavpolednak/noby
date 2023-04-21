using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateSalesArrangementState;

internal sealed class UpdateSalesArrangementStateHandler
    : IRequestHandler<Contracts.UpdateSalesArrangementStateRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.UpdateSalesArrangementStateRequest request, CancellationToken cancellation)
    {
        // kontrola existence SA
        var entity = await _dbContext
            .SalesArrangements
            .FirstOrDefaultAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);

        // kontrola aktualniho stavu vuci novemu stavu
        if (entity.State == request.State)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.AlreadyInSalesArrangementState, request.State);

        // update stavu SA
        entity.State = request.State;
        entity.StateUpdateTime = _dbContext.CisDateTime.Now;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Database.SalesArrangementServiceDbContext _dbContext;

    public UpdateSalesArrangementStateHandler(Database.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
