namespace DomainServices.CaseService.Api.Handlers;

internal sealed class UpdateOfferContactsHandler
    : IRequestHandler<Dto.UpdateOfferContactsMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateOfferContactsMediatrRequest request, CancellationToken cancellation)
    {
        // zjistit zda existuje case
        var entity = await _dbContext.Cases.FindAsync(new object[] { request.Request.CaseId }, cancellation) 
            ?? throw new CisNotFoundException(13000, $"Case #{request.Request.CaseId} not found");

        // ulozit do DB
        entity.EmailForOffer = request.Request.OfferContacts.EmailForOffer;
        entity.PhoneNumberForOffer = request.Request.OfferContacts.PhoneNumberForOffer;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.CaseServiceDbContext _dbContext;

    public UpdateOfferContactsHandler(Repositories.CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
