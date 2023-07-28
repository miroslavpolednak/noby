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

        var revType = await _codebookService.GetAcvRealEstateType(
            revInstance.RealEstateStateId, 
            revInstance.RealEstateSubtypeId.GetValueOrDefault(), 
            revInstance.RealEstateTypeId, 
            cancellationToken);

        // ulozit revType
        await _mediator.Send(new SetACVRealEstateTypeByRealEstateValuationRequest
        {
            RealEstateValuationId = request.RealEstateValuationId,
            ACVRealEstateType = revType,
        }, cancellationToken);

        // get revids
        var deedsRealEstateIds = await _dbContext.DeedOfOwnershipDocuments
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId && !string.IsNullOrEmpty(t.RealEstateIds))
            .Select(t => t.RealEstateIds)
            .ToListAsync(cancellationToken);
        var realEstateIds = deedsRealEstateIds.SelectMany(t =>
        {
            return System.Text.Json.JsonSerializer.Deserialize<long[]>(t!)!;
        }).ToArray();
        
        var purposes = await _codebookService.LoanPurposes(cancellationToken);
        var acvRequest = new ExternalServices.PreorderService.V1.Contracts.AvailableValuationTypesRequestDTO
        {
            RealEstateType = revType,
            IsLeased = revInstance.HouseAndFlatDetails?.FinishedHouseAndFlatDetails?.Leased,
            IsCellarFlat = revInstance.HouseAndFlatDetails?.FlatOnlyDetails?.Basement,
            IsNonApartmentBuildingFlat = revInstance.HouseAndFlatDetails?.FlatOnlyDetails.SpecialPlacement,
            IsNotUsableTechnicalState = revInstance.HouseAndFlatDetails?.PoorCondition,
            PurposesLoan = request.LoanPurposes?.Select(t => purposes.First(x => t == x.Id).AcvId).ToList(),
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
