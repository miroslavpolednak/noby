using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.ValidateOfferId;

internal sealed class ValidateOfferIdHandler
    : IRequestHandler<ValidateOfferIdRequest, ValidateOfferIdResponse>
{
    public async Task<ValidateOfferIdResponse> Handle(ValidateOfferIdRequest request, CancellationToken cancellationToken)
    {
        var instance = await _dbContext.Offers
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.OfferId == request.OfferId, cancellationToken);

        if (request.ThrowExceptionIfNotFound && instance is null)
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.OfferNotFound, request.OfferId);
        }
        
        return new ValidateOfferIdResponse
        {
            Exists = instance is not null,
            Data = instance is null ? null : new CommonOfferData
            {
                CaseId = instance.CaseId,
                SalesArrangementId = instance.SalesArrangementId,
                OfferId = instance.OfferId,
                ResourceProcessId = instance.ResourceProcessId.ToString(),
                DocumentId = instance.DocumentId,
                OfferType = (OfferTypes)instance.OfferType,
                ValidTo = instance.ValidTo,
                IsCreditWorthinessSimpleRequested = instance.IsCreditWorthinessSimpleRequested,
                Created = new SharedTypes.GrpcTypes.ModificationStamp
                {
                    DateTime = instance.CreatedTime,
                    UserId = instance.CreatedUserId,
                    UserName = instance.CreatedUserName
                }
            }
        };
    }

    private readonly Database.OfferServiceDbContext _dbContext;

    public ValidateOfferIdHandler(Database.OfferServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
