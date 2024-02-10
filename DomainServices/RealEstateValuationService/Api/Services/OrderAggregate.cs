using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.ProductService.Clients;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Api.Database.DocumentDataEntities;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.RealEstateValuationService.Api.Services;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed class OrderAggregate
{
    public async Task<
        (
        Database.Entities.RealEstateValuation REVEntity, 
        Database.DocumentDataEntities.RealEstateValudationData? REVData,
        long[]? RealEstateIds, 
        long[]? Attachments, 
        CaseService.Contracts.Case Case, 
        long? AddressPointId
        )> GetAggregatedData(int realEstateValuationId, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .RealEstateValuations
            .FirstOrDefaultAsync(t => t.RealEstateValuationId == realEstateValuationId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, realEstateValuationId);

        // validace
        if (string.IsNullOrEmpty(entity.ACVRealEstateTypeId))
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.OrderDataValidation, nameof(entity.ACVRealEstateTypeId));
        }

        var deedOfOwnerships = await _dbContext
            .DeedOfOwnershipDocuments
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == realEstateValuationId)
            .Select(t => new { t.AddressPointId, t.RealEstateIds })
            .ToListAsync(cancellationToken);

        var addressPointId = deedOfOwnerships
            .FirstOrDefault(t => t.AddressPointId.HasValue)
            ?.AddressPointId;

        var attachments = await _dbContext
            .Attachments
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == realEstateValuationId)
            .Select(t => t.ExternalId)
            .ToArrayAsync(cancellationToken);

        // realestateids
        var realEstateIds = deedOfOwnerships
            .Where(t => t.RealEstateIds != null)
            .SelectMany(t => t.RealEstateIds!)
            .Distinct()
            .ToArray();

        // case detail
        var caseInstance = await _caseService.GetCaseDetail(entity.CaseId, cancellationToken);

        // REV data
        var revDetailData = (await _documentDataStorage
            .FirstOrDefaultByEntityId<Database.DocumentDataEntities.RealEstateValudationData>(realEstateValuationId, cancellationToken))
            ?.Data;

        return (entity, revDetailData, realEstateIds, attachments, caseInstance, addressPointId);
    }

    public async Task UpdateOnlinePreorderDetailsOnly(
        int realEstateValuationId,
        OnlinePreorderData? onlinePreorderDetails,
        RealEstateValudationData? revDetailData,
        CancellationToken cancellationToken)
    {
        bool dataExists = revDetailData != null;
        revDetailData ??= new RealEstateValudationData();

        // vlozit data z requestu
        revDetailData.OnlinePreorderDetails = _mapper.MapPreorderDetails(onlinePreorderDetails);

        if (dataExists)
        {
            await _documentDataStorage.UpdateByEntityId(realEstateValuationId, revDetailData);
        }
        else
        {
            await _documentDataStorage.Add(realEstateValuationId, revDetailData, cancellationToken);
        }
    }

    public async Task UpdateLocalSurveyDetailsOnly(
        int realEstateValuationId,
        LocalSurveyData? localSurveyDetails,
        RealEstateValudationData? revDetailData,
        CancellationToken cancellationToken)
    {
        bool dataExists = revDetailData != null;
        revDetailData ??= new RealEstateValudationData();

        // vlozit data z requestu
        revDetailData.LocalSurveyDetails = _mapper.MapLocalSurveyDetails(localSurveyDetails);

        if (dataExists)
        {
            await _documentDataStorage.UpdateByEntityId(realEstateValuationId, revDetailData);
        }
        else
        {
            await _documentDataStorage.Add(realEstateValuationId, revDetailData, cancellationToken);
        }
    }

    public async Task SaveResultsAndUpdateEntity(
        Database.Entities.RealEstateValuation entity, 
        long orderId,
        RealEstateValuationStates newValuationState,
        CancellationToken cancellationToken)
    {
        // ulozeni vysledku
        entity.OrderId = orderId;
        entity.ValuationSentDate = _timeProvider.GetLocalNow().DateTime;
        entity.ValuationStateId = (int)newValuationState;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetProductPropertiesResult> GetProductProperties(int caseState, long caseId, CancellationToken cancellationToken)
    {
        if (caseState == (int)CaseStates.InProgress)
        {
            var offerId = (await _salesArrangementService.GetProductSalesArrangements(caseId, cancellationToken)).First().OfferId;
            var offer = await _offerService.GetOfferDetail(offerId!.Value, cancellationToken);

            var collateralAmount = offer.MortgageOffer.SimulationInputs.CollateralAmount;
            var loanDuration = offer.MortgageOffer.SimulationInputs.LoanDuration;
            var purpose = await getLoanPurpose(offer.MortgageOffer.SimulationInputs.LoanPurposes?.FirstOrDefault()?.LoanPurposeId);
            var loanAmount = offer.MortgageOffer.SimulationInputs.LoanAmount;
            return new GetProductPropertiesResult(collateralAmount, loanAmount, loanDuration, purpose);
        }
        else
        {
            var mortgage = await _productService.GetMortgage(caseId, cancellationToken);

            var purpose = await getLoanPurpose(mortgage.Mortgage.LoanPurposes?.FirstOrDefault()?.LoanPurposeId);
            var loanAmount = mortgage.Mortgage.LoanPaymentAmount;
            return new GetProductPropertiesResult(null, loanAmount, null, purpose);
        }
    }

    private async Task<string?> getLoanPurpose(int? loanPurposeId)
    {
        return (await _codebookService.LoanPurposes())
            .OrderByDescending(t => t.AcvIdPriority)
            .FirstOrDefault(t => t.Id == loanPurposeId)
            ?.AcvId;
    }

    public sealed record GetProductPropertiesResult(decimal? CollateralAmount, decimal? LoanAmount, int? LoanDuration, string? LoanPurpose)
    {
    }

    private readonly Database.DocumentDataEntities.Mappers.RealEstateValuationDataMapper _mapper;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly IProductServiceClient _productService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly RealEstateValuationServiceDbContext _dbContext;
    private readonly ICaseServiceClient _caseService;
    private readonly TimeProvider _timeProvider;

    public OrderAggregate(
        Database.DocumentDataEntities.Mappers.RealEstateValuationDataMapper mapper,
        IDocumentDataStorage documentDataStorage,
        IProductServiceClient productService,
        ICodebookServiceClient codebookService,
        ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService,
        RealEstateValuationServiceDbContext dbContext,
        ICaseServiceClient caseService,
        TimeProvider timeProvider)
    {
        _mapper = mapper;
        _documentDataStorage = documentDataStorage;
        _productService = productService;
        _codebookService = codebookService;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
        _dbContext = dbContext;
        _timeProvider = timeProvider;
    }
}
