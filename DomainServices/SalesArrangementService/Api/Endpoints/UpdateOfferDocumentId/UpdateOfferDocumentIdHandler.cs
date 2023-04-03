using CIS.Foms.Enums;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateOfferDocumentId;

internal sealed class UpdateOfferDocumentIdHandler
    : IRequestHandler<Contracts.UpdateOfferDocumentIdRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.UpdateOfferDocumentIdRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext
            .SalesArrangements
            .FirstOrDefaultAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);

        entity.OfferDocumentId = request.OfferDocumentId;

        // pokud je zadost NEW, zmenit na InProgress
        if (entity.State == (int)SalesArrangementStates.NewArrangement)
        {
            entity.State = (int)SalesArrangementStates.InProgress;
        }

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Database.SalesArrangementServiceDbContext _dbContext;

    public UpdateOfferDocumentIdHandler(Database.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
