using DomainServices.OfferService.Api.Database;
using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.v1.DeleteOffers;

internal sealed class DeleteOffersHandler : IRequestHandler<DeleteOffersRequest>
{
    private readonly OfferServiceDbContext _dbContext;

    public DeleteOffersHandler(OfferServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteOffersRequest request, CancellationToken cancellationToken)
    {
        await _dbContext.Offers
                        .Where(offer => request.OfferIds.Contains(offer.OfferId))
                        .ExecuteDeleteAsync(cancellationToken);
    }
}