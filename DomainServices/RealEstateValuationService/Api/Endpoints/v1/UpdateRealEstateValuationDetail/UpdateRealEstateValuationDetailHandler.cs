using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;
using Google.Protobuf.WellKnownTypes;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.UpdateRealEstateValuationDetail;

internal sealed class UpdateRealEstateValuationDetailHandler(
    Database.DocumentDataEntities.Mappers.RealEstateValuationDataMapper _mapper,
    IDocumentDataStorage _documentDataStorage,
    RealEstateValuationServiceDbContext _dbContext)
        : IRequestHandler<UpdateRealEstateValuationDetailRequest, Empty>
{
    public async Task<Empty> Handle(UpdateRealEstateValuationDetailRequest request, CancellationToken cancellationToken)
    {
        var realEstate = await _dbContext.RealEstateValuations
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);

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
                throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.MaxValuationsForCase);
            }
        }

        // general detail
        realEstate.Address = request.Address;
        realEstate.IsLoanRealEstate = request.IsLoanRealEstate;
        realEstate.RealEstateStateId = request.RealEstateStateId;
        realEstate.RealEstateSubtypeId = request.RealEstateSubtypeId;

        await _dbContext.SaveChangesAsync(cancellationToken);

        // ulozit json data
        var revDetailData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.RealEstateValudationData, int>(request.RealEstateValuationId, cancellationToken);
        _mapper.MapToData(request, revDetailData!.Data);

        await _documentDataStorage.Update(revDetailData.DocumentDataStorageId, revDetailData.Data!);

        return new Empty();
    }

    private static readonly int[] _stateIdsForValidation = [4, 5];
}
