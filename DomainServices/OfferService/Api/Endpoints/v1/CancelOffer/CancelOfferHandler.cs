using DomainServices.OfferService.Api.Database;
using DomainServices.OfferService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.v1.CancelOffer;

internal sealed class CancelOfferHandler(OfferServiceDbContext _dbContext, TimeProvider _timeProvider)
    : IRequestHandler<CancelOfferRequest, Empty>
{
    public async Task<Empty> Handle(CancelOfferRequest request, CancellationToken cancellationToken)
    {
        if (request.OfferId == 0)
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.OfferIdIsEmpty);
        }
        
        var offer = await _dbContext.Offers.FirstOrDefaultAsync(t => t.OfferId == request.OfferId, cancellationToken)
                   ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.OfferNotFound, request.OfferId);

        offer.ValidTo = _timeProvider.GetLocalNow().Date.AddDays(-1);

        await _dbContext.SaveChangesAsync(default);
        
        return new Empty();
    }
}
