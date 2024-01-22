using DomainServices.OfferService.Api.Database;
using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.UpdateOfferDocumentId;

internal class UpdateOfferDocumentIdHandler : IRequestHandler<UpdateOfferDocumentIdRequest>
{
    private readonly OfferServiceDbContext _dbContext;

    public UpdateOfferDocumentIdHandler(OfferServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(UpdateOfferDocumentIdRequest request, CancellationToken cancellationToken)
    {
        var offer = await _dbContext.Offers.FirstOrDefaultAsync(t => t.OfferId == request.OfferId, cancellationToken)
                    ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.OfferNotFound, request.OfferId);

        offer.DocumentId = request.DocumentId;
        offer.FirstGeneratedDocumentDate ??= DateTime.Now;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}