﻿namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateOfferDocumentId;

internal sealed class UpdateOfferDocumentIdHandler
    : IRequestHandler<Contracts.UpdateOfferDocumentIdRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.UpdateOfferDocumentIdRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.SalesArrangements.FindAsync(new object[] { request.SalesArrangementId }, cancellation)
            ?? throw new CisNotFoundException(18000, $"Sales arrangement ID {request.SalesArrangementId} does not exist.");

        entity.OfferDocumentId = request.OfferDocumentId;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Database.SalesArrangementServiceDbContext _dbContext;

    public UpdateOfferDocumentIdHandler(Database.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}