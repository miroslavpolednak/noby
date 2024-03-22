using DomainServices.OfferService.Api.Database;
using DomainServices.OfferService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.UpdateOffer;

internal sealed class UpdateOfferHandler 
    : IRequestHandler<UpdateOfferRequest, Empty>
{
    private readonly OfferServiceDbContext _dbContext;

    public UpdateOfferHandler(OfferServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Empty> Handle(UpdateOfferRequest request, CancellationToken cancellationToken)
    {
        var offer = await _dbContext.Offers.FirstOrDefaultAsync(t => t.OfferId == request.OfferId, cancellationToken)
                    ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.OfferNotFound, request.OfferId);

        if (!string.IsNullOrEmpty(request.DocumentId))
        {
            offer.DocumentId = request.DocumentId;
            offer.FirstGeneratedDocumentDate ??= DateTime.Now;
        }

        if (request.CaseId.HasValue)
        {
            offer.CaseId = request.CaseId.Value;
        }

        if (request.SalesArrangementId.HasValue)
        {
            offer.SalesArrangementId = request.SalesArrangementId.Value;
        }

        if (request.Flags.HasValue)
        {
            offer.Flags = request.Flags.Value;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }
}