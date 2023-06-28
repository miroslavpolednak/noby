using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.CreateRealEstateValuation;

internal sealed class CreateRealEstateValuationHandler
    : IRequestHandler<CreateRealEstateValuationRequest, CreateRealEstateValuationResponse>
{
    public async Task<CreateRealEstateValuationResponse> Handle(CreateRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var entity = new Database.Entities.RealEstateValuation
        {
            CaseId = request.CaseId,
            DeveloperAllowed = request.DeveloperAllowed,
            DeveloperApplied = request.DeveloperApplied,
            IsLoanRealEstate = request.IsLoanRealEstate,
            IsRevaluationRequired = request.IsRevaluationRequired,
            RealEstateTypeId = request.RealEstateTypeId,
            ValuationStateId = request.ValuationStateId,
            ValuationTypeId = (int)request.ValuationTypeId,
            RealEstateStateId = request.RealEstateStateId
        };
        _dbContext.RealEstateValuations.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateRealEstateValuationResponse
        {
            RealEstateValuationId = entity.RealEstateValuationId
        };
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public CreateRealEstateValuationHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
