using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdatePcpId;

internal sealed class UpdatePcpIdHandler(Database.SalesArrangementServiceDbContext _dbContext)
		: IRequestHandler<Contracts.UpdatePcpIdRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.UpdatePcpIdRequest request, CancellationToken cancellation)
    {
        // kontrola existence SA
        var entity = await _dbContext
            .SalesArrangements
            .FirstOrDefaultAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);

        // update stavu SA
        entity.PcpId = request.PcpId;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}
