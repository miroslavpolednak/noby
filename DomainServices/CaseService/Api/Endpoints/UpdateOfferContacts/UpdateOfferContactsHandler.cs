using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.UpdateOfferContacts;

internal sealed class UpdateOfferContactsHandler
    : IRequestHandler<UpdateOfferContactsRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateOfferContactsRequest request, CancellationToken cancellation)
    {
        // zjistit zda existuje case
        var entity = await _dbContext.Cases.FindAsync(new object[] { request.CaseId }, cancellation)
            ?? throw new CisNotFoundException(13000, "Case", request.CaseId);

        // ulozit do DB
        entity.EmailForOffer = request.OfferContacts.EmailForOffer;
        entity.PhoneIDCForOffer = request.OfferContacts.PhoneNumberForOffer?.PhoneIDC;
        entity.PhoneNumberForOffer = request.OfferContacts.PhoneNumberForOffer?.PhoneNumber;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly CaseServiceDbContext _dbContext;

    public UpdateOfferContactsHandler(CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
