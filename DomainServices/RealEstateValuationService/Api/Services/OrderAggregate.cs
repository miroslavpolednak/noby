using DomainServices.CaseService.Clients.v1;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients.v1;
using DomainServices.OfferService.Clients.v1;
using DomainServices.ProductService.Clients;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Api.Database.DocumentDataEntities;
using DomainServices.RealEstateValuationService.Api.Dto;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.RealEstateValuationService.Api.Services;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed class OrderAggregate(
    Database.DocumentDataEntities.Mappers.RealEstateValuationDataMapper _mapper,
    ICustomerServiceClient _customerService,
    IDocumentDataStorage _documentDataStorage,
    IProductServiceClient _productService,
    ICodebookServiceClient _codebookService,
    ISalesArrangementServiceClient _salesArrangementService,
    IOfferServiceClient _offerService,
    RealEstateValuationServiceDbContext _dbContext,
    ICaseServiceClient _caseService,
    TimeProvider _timeProvider)
{
    public async Task<CustomerService.Contracts.Customer> GetCustomer(SharedTypes.GrpcTypes.Identity identity, CancellationToken cancellationToken)
    {
        SharedTypes.GrpcTypes.Identity finalIdentity = identity;
        if (identity.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp)
        {
            var c = await _customerService.GetCustomerDetail(identity, cancellationToken);
            finalIdentity = c.Identities.FirstOrDefault(t => t.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb)
                ?? throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.KbIdentityNotFound, identity.IdentityId);
        }

        return await _customerService.GetCustomerDetail(finalIdentity, cancellationToken);
    }

    public async Task<
        (
        Database.Entities.RealEstateValuation REVEntity, 
        RealEstateValudationData? REVData,
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
            .FirstOrDefaultByEntityId<RealEstateValudationData, int>(realEstateValuationId, cancellationToken))
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
        WorkflowTaskStates newValuationState,
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
        if (caseState == (int)EnumCaseStates.InProgress)
        {
            var offerId = (await _salesArrangementService.GetProductSalesArrangements(caseId, cancellationToken)).First().OfferId;
            var offer = await _offerService.GetOffer(offerId!.Value, cancellationToken);

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
            var loanAmount = ((decimal?)mortgage.Mortgage.AvailableForDrawing).GetValueOrDefault() > 0 ? mortgage.Mortgage.LoanAmount : mortgage.Mortgage.CurrentAmount;
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
}
