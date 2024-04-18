using DomainServices.OfferService.Api.Database;
using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.v1.DeleteOfferList;

internal sealed class DeleteOfferListHandler : IRequestHandler<DeleteOfferListRequest>
{
    private readonly OfferServiceDbContext _dbContext;

    public DeleteOfferListHandler(OfferServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteOfferListRequest request, CancellationToken cancellationToken)
    {
        await _dbContext.Offers
                        .Where(offer => request.OfferIds.Contains(offer.OfferId))
                        .ExecuteDeleteAsync(cancellationToken);
    }
}