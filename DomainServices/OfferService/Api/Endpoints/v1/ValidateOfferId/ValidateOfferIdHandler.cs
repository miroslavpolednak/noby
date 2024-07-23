using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.v1.ValidateOfferId;

internal sealed class ValidateOfferIdHandler(Database.OfferServiceDbContext _dbContext)
        : IRequestHandler<ValidateOfferIdRequest, ValidateOfferIdResponse>
{
    public async Task<ValidateOfferIdResponse> Handle(ValidateOfferIdRequest request, CancellationToken cancellationToken)
    {
        var instance = await _dbContext.Offers
            .AsNoTracking()
            .Where(t => t.OfferId == request.OfferId)
            .Select(Database.DatabaseExpressions.CreateCommonOfferData())
            .FirstOrDefaultAsync(cancellationToken);

        if (request.ThrowExceptionIfNotFound && instance is null)
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.OfferNotFound, request.OfferId);
        }

        return new ValidateOfferIdResponse
        {
            Exists = instance is not null,
            Data = instance
        };
    }
}
