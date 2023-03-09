using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.GetOffer;

internal sealed class GetOfferHandler
    : IRequestHandler<GetOfferRequest, GetOfferResponse>
{
    public async Task<GetOfferResponse> Handle(GetOfferRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .Offers
            .AsNoTracking()
            .Where(t => t.OfferId == request.OfferId)
            .Select(t => new
            {
                t.ResourceProcessId,
                t.CreatedUserId,
                t.CreatedUserName,
                t.CreatedTime
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.OfferNotFound, request.OfferId);

        return new GetOfferResponse
        {
            OfferId = request.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(entity.CreatedUserId, entity.CreatedUserName, entity.CreatedTime)
        };
    }

    private readonly Database.OfferServiceDbContext _dbContext;

    public GetOfferHandler(Database.OfferServiceDbContext dbContext)
        => _dbContext = dbContext;
}