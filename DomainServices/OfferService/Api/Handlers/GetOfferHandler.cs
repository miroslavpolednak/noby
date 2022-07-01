using _OS = DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Handlers;

internal class GetOfferHandler
    : IRequestHandler<Dto.GetOfferMediatrRequest, _OS.GetOfferResponse>
{
    #region Construction

    private readonly Repositories.OfferServiceDbContext _dbContext;

    public GetOfferHandler(Repositories.OfferServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #endregion

    public async Task<_OS.GetOfferResponse> Handle(Dto.GetOfferMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.Offers
           .AsNoTracking()
           .Where(t => t.OfferId == request.OfferId)
           .Select(t => new { 
               ResourceProcessId = t.ResourceProcessId, 
               CreatedUserId = t.CreatedUserId, 
               CreatedUserName = t.CreatedUserName, 
               CreatedTime = t.CreatedTime 
           })
           .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(13000, $"Offer #{request.OfferId} not found");

        return new _OS.GetOfferResponse
        {
            OfferId = request.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(entity.CreatedUserId, entity.CreatedUserName, entity.CreatedTime)
        };
    }
  
}