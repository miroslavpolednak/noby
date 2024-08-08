using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.ValidateRealEstateValuationId;

internal sealed class ValidateRealEstateValuationIdHandler(RealEstateValuationServiceDbContext _dbContext)
        : IRequestHandler<ValidateRealEstateValuationIdRequest, ValidateRealEstateValuationIdResponse>
{
    public async Task<ValidateRealEstateValuationIdResponse> Handle(ValidateRealEstateValuationIdRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .RealEstateValuations
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .Select(t => new
            {
                t.CaseId,
                t.ValuationStateId,
                t.OrderId,
                t.PreorderId,
                t.PossibleValuationTypeId,
                t.ValuationTypeId
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (request.ThrowExceptionIfNotFound && entity is null)
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);
        }

        var response = new ValidateRealEstateValuationIdResponse
        {
            Exists = entity != null,
            CaseId = entity?.CaseId,
            ValuationStateId = entity?.ValuationStateId,
            OrderId = entity?.OrderId,
            PreorderId = entity?.PreorderId,
            ValuationTypeId = entity is null ? ValuationTypes.Unknown : (ValuationTypes)entity.ValuationTypeId
        };

        if (entity?.PossibleValuationTypeId is not null)
        {
            response.PossibleValuationTypeId.AddRange(entity.PossibleValuationTypeId);
        }

        return response;
    }
}
