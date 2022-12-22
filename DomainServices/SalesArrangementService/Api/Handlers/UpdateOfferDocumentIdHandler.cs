namespace DomainServices.SalesArrangementService.Api.Handlers;

internal sealed class UpdateOfferDocumentIdHandler
    : IRequestHandler<Dto.UpdateOfferDocumentIdMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateOfferDocumentIdMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.SalesArrangements.FindAsync(new object[] { request.SalesArrangementId }, cancellation) 
            ?? throw new CisNotFoundException(18000, $"Sales arrangement ID {request.SalesArrangementId} does not exist.");

        entity.OfferDocumentId = request.OfferDocumentId;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;

    public UpdateOfferDocumentIdHandler(Repositories.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
