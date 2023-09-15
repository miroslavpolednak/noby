using CIS.Foms.Enums;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.GetRealEstateValuationTypes;

internal sealed class GetRealEstateValuationTypesHandler
    : IRequestHandler<GetRealEstateValuationTypesRequest, GetRealEstateValuationTypesReponse>
{
    public async Task<GetRealEstateValuationTypesReponse> Handle(GetRealEstateValuationTypesRequest request, CancellationToken cancellationToken)
    {
        var revInstance = await _mediator.Send(new Contracts.GetRealEstateValuationDetailRequest
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
        var realEstateIds = deedsRealEstateIds.SelectMany(t =>
        {
            if (string.IsNullOrEmpty(t))
            {
                throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.MissingRealEstateId); 
            }

            var arr = System.Text.Json.JsonSerializer.Deserialize<long[]>(t);
            if (arr is null || arr.Length == 0)
            {
                throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.MissingRealEstateId);
            }

            return arr;
        }).ToArray();
        
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
            RealEstateIds = realEstateIds,
            DealType = request.DealType ?? "",
            LoanAmount = request.LoanAmount
        };
        var acvResponse = await _preorderService.GetValuationTypes(acvRequest, cancellationToken);

        var response = new GetRealEstateValuationTypesReponse();
        response.ValuationTypeId.AddRange(acvResponse.Select(t => (int)t));
        return response;
    }

    private readonly IMediator _mediator;
    private readonly ExternalServices.PreorderService.V1.IPreorderServiceClient _preorderService;
    private readonly RealEstateValuationServiceDbContext _dbContext;
    private readonly CodebookService.Clients.ICodebookServiceClient _codebookService;

    public GetRealEstateValuationTypesHandler(
        RealEstateValuationServiceDbContext dbContext, 
        CodebookService.Clients.ICodebookServiceClient codebookService, 
        ExternalServices.PreorderService.V1.IPreorderServiceClient preorderService, 
        IMediator mediator)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _codebookService = codebookService;
        _preorderService = preorderService;
    }
}
