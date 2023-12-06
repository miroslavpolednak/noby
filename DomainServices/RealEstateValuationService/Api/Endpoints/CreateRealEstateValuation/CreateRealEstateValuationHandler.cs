using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.CreateRealEstateValuation;

internal sealed class CreateRealEstateValuationHandler
    : IRequestHandler<CreateRealEstateValuationRequest, CreateRealEstateValuationResponse>
{
    public async Task<CreateRealEstateValuationResponse> Handle(CreateRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        // kontrola CaseId
        await _caseService.ValidateCaseId(request.CaseId, true, cancellationToken);

        // Kontrola, zda na daném CaseId nedojde k porušení limitu na maximálně 3 Ocenění, která jsou zároveň objektem úvěru
        if (request.IsLoanRealEstate)
        {
            var existingRev = await _dbContext.RealEstateValuations
                .AsNoTracking()
                .Where(t => t.CaseId == request.CaseId && t.IsLoanRealEstate && !_stateIdsForValidation.Contains(t.ValuationStateId))
                .CountAsync(cancellationToken);
            if (existingRev > 2)
            {
                throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.MaxValuationsForCase);
            }
        }

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
            RealEstateStateId = request.RealEstateStateId,
            IsOnlineDisqualified = request.IsOnlineDisqualified
        };
        _dbContext.RealEstateValuations.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // zalozit prazdny detail
        await _documentDataStorage.Add(entity.RealEstateValuationId, new Database.DocumentDataEntities.RealEstateValudationData(), cancellationToken);

        return new CreateRealEstateValuationResponse
        {
            RealEstateValuationId = entity.RealEstateValuationId
        };
    }

    private static int[] _stateIdsForValidation = new[] { 4, 5 };

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;
    private readonly RealEstateValuationServiceDbContext _dbContext;

    public CreateRealEstateValuationHandler(RealEstateValuationServiceDbContext dbContext, CaseService.Clients.ICaseServiceClient caseService, IDocumentDataStorage documentDataStorage)
    {
        _documentDataStorage = documentDataStorage;
        _dbContext = dbContext;
        _caseService = caseService;
    }
}
