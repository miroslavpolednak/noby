using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.ValidateRealEstateValuationId;

internal sealed class ValidateRealEstateValuationIdHandler
    : IRequestHandler<ValidateRealEstateValuationIdRequest, ValidateRealEstateValuationIdResponse>
{
    public async Task<ValidateRealEstateValuationIdResponse> Handle(ValidateRealEstateValuationIdRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .RealEstateValuations
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .Select(t => new { t.CaseId, t.ValuationStateId, t.OrderId, t.PreorderId })
            .FirstOrDefaultAsync(cancellationToken);

        if (request.ThrowExceptionIfNotFound && entity is null)
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);
        }

        return new ValidateRealEstateValuationIdResponse
        {
            Exists = entity != null,
            CaseId = entity?.CaseId,
            ValuationStateId = entity?.ValuationStateId,
            OrderId = entity?.OrderId,
            PreorderId = entity?.PreorderId
        };
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public ValidateRealEstateValuationIdHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
