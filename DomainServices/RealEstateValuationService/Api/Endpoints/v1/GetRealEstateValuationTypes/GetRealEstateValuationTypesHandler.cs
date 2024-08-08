using SharedTypes.Enums;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;
using System.Threading;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.GetRealEstateValuationTypes;

internal sealed class GetRealEstateValuationTypesHandler(
    RealEstateValuationServiceDbContext _dbContext,
    CodebookService.Clients.ICodebookServiceClient _codebookService,
    ExternalServices.PreorderService.V1.IPreorderServiceClient _preorderService,
    IMediator _mediator)
        : IRequestHandler<GetRealEstateValuationTypesRequest, GetRealEstateValuationTypesReponse>
{
    public async Task<GetRealEstateValuationTypesReponse> Handle(GetRealEstateValuationTypesRequest request, CancellationToken cancellationToken)
    {
        var revInstance = await _mediator.Send(new GetRealEstateValuationDetailRequest
        {
            RealEstateValuationId = request.RealEstateValuationId
        }, cancellationToken);

        var (acvRealEstateTypeId, bagmanRealEstateTypeId) = await _codebookService.GetACVAndBagmanRealEstateType(
            revInstance.RealEstateStateId,
            revInstance.RealEstateSubtypeId.GetValueOrDefault(),
            revInstance.RealEstateTypeId,
            cancellationToken);

        // ulozit revType
        await _mediator.Send(new SetForeignRealEstateTypesByRealEstateValuationRequest
        {
            RealEstateValuationId = request.RealEstateValuationId,
            ACVRealEstateTypeId = acvRealEstateTypeId,
            BagmanRealEstateTypeId = bagmanRealEstateTypeId
        }, cancellationToken);

        // get revids
        var deedsRealEstateIds = await _dbContext.DeedOfOwnershipDocuments
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .Select(t => t.RealEstateIds)
            .ToListAsync(cancellationToken);
        if (deedsRealEstateIds.Any(t => t is null || t.Count == 0))
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.MissingRealEstateId);
        }

        var purposes = await _codebookService.LoanPurposes(cancellationToken);
        var acvRequest = new ExternalServices.PreorderService.V1.Contracts.AvailableValuationTypesRequestDTO
        {
            RealEstateType = acvRealEstateTypeId,
            IsLeased = revInstance.HouseAndFlatDetails?.FinishedHouseAndFlatDetails?.Leased,
            IsCellarFlat = revInstance.HouseAndFlatDetails?.FlatOnlyDetails?.Basement,
            IsNonApartmentBuildingFlat = revInstance.HouseAndFlatDetails?.FlatOnlyDetails?.SpecialPlacement,
            IsNotUsableTechnicalState = revInstance.HouseAndFlatDetails?.PoorCondition,
            HasOwnershipLimitations = revInstance.HouseAndFlatDetails?.OwnershipRestricted,
            PurposesLoan = request.LoanPurposes
                                  .Select(purposeId => purposes.FirstOrDefault(x => x.MandantId == (int)Mandants.Kb && purposeId == x.Id)?.AcvId)
                                  .Where(t => !string.IsNullOrEmpty(t))
                                  .Cast<string>()
                                  .ToList(),
            RealEstateIds = deedsRealEstateIds.SelectMany(t => t!).Distinct().ToArray(),
            DealType = request.DealType ?? "",
            LoanAmount = request.LoanAmount
        };
        var acvResponse = await _preorderService.GetValuationTypes(acvRequest, cancellationToken);

        var response = new GetRealEstateValuationTypesReponse();
        response.ValuationTypeId.AddRange(acvResponse.Select(t => (int)t));

        // Uložení výsledku ACV trychtýře a zvoleného typu ocenění do Noby DB
        await saveValuationType(request.RealEstateValuationId, response.ValuationTypeId.ToList(), cancellationToken);

        if (revInstance.IsOnlineDisqualified && response.ValuationTypeId.Contains(1))
        {
            response.ValuationTypeId.Remove(1);
        }

        return response;
    }

    public async Task saveValuationType(int realEstateValuationId, List<int> valuationTypeId, CancellationToken cancellationToken)
    {
        var revEntity = await _dbContext
            .RealEstateValuations
            .FirstAsync(t => t.RealEstateValuationId == realEstateValuationId, cancellationToken);

        revEntity.PossibleValuationTypeId = valuationTypeId;

        if (valuationTypeId.Count == 1)
        {
            revEntity.ValuationTypeId = valuationTypeId[0];
        }
        else if (valuationTypeId.Any(t => t == 1))
        {
            revEntity.ValuationTypeId = 1;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
