using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.v1.UpdateOfferContacts;

internal sealed class UpdateOfferContactsHandler(CaseServiceDbContext _dbContext)
    : IRequestHandler<UpdateOfferContactsRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateOfferContactsRequest request, CancellationToken cancellation)
    {
        // zjistit zda existuje case
        var entity = await _dbContext.Cases.FindAsync([request.CaseId], cancellation)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);

        // ulozit do DB
        entity.EmailForOffer = request.OfferContacts.EmailForOffer;
        entity.PhoneIDCForOffer = request.OfferContacts.PhoneNumberForOffer?.PhoneIDC;
        entity.PhoneNumberForOffer = request.OfferContacts.PhoneNumberForOffer?.PhoneNumber;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}
