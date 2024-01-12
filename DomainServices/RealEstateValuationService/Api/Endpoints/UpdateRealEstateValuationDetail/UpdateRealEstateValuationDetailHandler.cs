using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;
using Google.Protobuf.WellKnownTypes;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.UpdateRealEstateValuationDetail;

internal sealed class UpdateRealEstateValuationDetailHandler
    : IRequestHandler<UpdateRealEstateValuationDetailRequest, Empty>
{
    public async Task<Empty> Handle(UpdateRealEstateValuationDetailRequest request, CancellationToken cancellationToken)
    {
        var realEstate = await _dbContext.RealEstateValuations
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);

        // Kontrola, zda na daném CaseId nedojde k porušení limitu na maximálně 3 Ocenění, která jsou zároveň objektem úvěru
        if (request.IsLoanRealEstate)
        {
            var existingRev = await _dbContext.RealEstateValuations
                .AsNoTracking()
                .Where(t => t.CaseId == realEstate.CaseId 
                    && t.RealEstateValuationId != request.RealEstateValuationId
                    && t.IsLoanRealEstate 
                    && !_stateIdsForValidation.Contains(t.ValuationStateId))
                .CountAsync(cancellationToken);
            if (existingRev > 2)
            {
                throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.MaxValuationsForCase);
            }
        }

        // general detail
        realEstate.Address = request.Address;
        realEstate.IsLoanRealEstate = request.IsLoanRealEstate;
        realEstate.RealEstateStateId = request.RealEstateStateId;
        realEstate.RealEstateSubtypeId = request.RealEstateSubtypeId;

        await _dbContext.SaveChangesAsync(cancellationToken);

        // ulozit json data
        var revDetailData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.RealEstateValudationData>(request.RealEstateValuationId, cancellationToken);
        _mapper.MapToData(request, revDetailData!.Data);

        await _documentDataStorage.Update(revDetailData.DocumentDataStorageId, revDetailData.Data!);

        return new Empty();
    }

    private static int[] _stateIdsForValidation = new[] { 4, 5 };

    private readonly Database.DocumentDataEntities.Mappers.RealEstateValuationDataMapper _mapper;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly RealEstateValuationServiceDbContext _dbContext;

    public UpdateRealEstateValuationDetailHandler(
        Database.DocumentDataEntities.Mappers.RealEstateValuationDataMapper mapper,
        IDocumentDataStorage documentDataStorage,
        RealEstateValuationServiceDbContext dbContext)
    {
        _mapper = mapper;
        _documentDataStorage = documentDataStorage;
        _dbContext = dbContext;
    }
}
