using DomainServices.OfferService.Api.Database;
using DomainServices.OfferService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.v1.UpdateOffer;

internal sealed class UpdateOfferHandler(OfferServiceDbContext _dbContext)
        : IRequestHandler<UpdateOfferRequest, Empty>
{
    public async Task<Empty> Handle(UpdateOfferRequest request, CancellationToken cancellationToken)
    {
        var offer = await _dbContext.Offers.FirstOrDefaultAsync(t => t.OfferId == request.OfferId, cancellationToken)
                    ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.OfferNotFound, request.OfferId);

        if (!string.IsNullOrEmpty(request.DocumentId))
        {
            offer.DocumentId = request.DocumentId;
            offer.FirstGeneratedDocumentDate ??= DateTime.Now;
        }

        if (request.CaseId.HasValue)
        {
            offer.CaseId = request.CaseId.Value == 0 ? null : request.CaseId.Value;
        }

        if (request.SalesArrangementId.HasValue)
        {
            offer.SalesArrangementId = request.SalesArrangementId.Value == 0 ? null : request.SalesArrangementId.Value;
        }

        if (request.Flags.HasValue)
        {
            offer.Flags = request.Flags.Value;
        }

        if (request.ValidTo is not null)
        {
            offer.ValidTo = request.ValidTo;
        }

        if (request.RemoveIsCommunicatedFlag && ((offer.Flags & (int)EnumOfferFlagTypes.Communicated) == (int)EnumOfferFlagTypes.Communicated))
        {
            // Remove communicated flag
            offer.Flags -= (int)EnumOfferFlagTypes.Communicated;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }
}