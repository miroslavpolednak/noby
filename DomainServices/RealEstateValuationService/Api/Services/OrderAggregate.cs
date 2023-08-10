using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.OfferService.Clients;
using DomainServices.ProductService.Clients;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using Google.Protobuf;

namespace DomainServices.RealEstateValuationService.Api.Services;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed class OrderAggregate
{
    public async Task<(Database.Entities.RealEstateValuation REVEntity, long[]? RealEstateIds, long[]? Attachments, CaseService.Contracts.Case Case, long? AddressPointId)> GetAggregatedData(int realEstateValuationId, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .RealEstateValuations
            .FirstOrDefaultAsync(t => t.RealEstateValuationId == realEstateValuationId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, realEstateValuationId);

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
            .Where(t => !string.IsNullOrEmpty(t.RealEstateIds))
            .SelectMany(t =>
            {
                return System.Text.Json.JsonSerializer.Deserialize<long[]>(t.RealEstateIds!)!;
            })
            .ToArray();

        // case detail
        var caseInstance = await _caseService.GetCaseDetail(entity.CaseId, cancellationToken);

        // validace
        if (string.IsNullOrEmpty(entity.ACVRealEstateTypeId))
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.OrderDataValidation, nameof(entity.ACVRealEstateTypeId));
        }

        return (entity, realEstateIds, attachments, caseInstance, addressPointId);
    }

    public async Task SaveResults(Database.Entities.RealEstateValuation entity, long orderId, OrdersStandard? data, CancellationToken cancellationToken)
    {
        // ulozeni vysledku
        entity.OrderId = orderId;
        entity.ValuationSentDate = _dbContext.CisDateTime.Now;
        entity.ValuationStateId = (int)RealEstateValuationStates.Dokonceno;

        // if revaluation
        if (entity.IsRevaluationRequired && data is not null)
        {
            var orderEntity = new Database.Entities.RealEstateValuationOrder
            {
                RealEstateValuationId = entity.RealEstateValuationId,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(data),
                DataBin = data.ToByteArray()
            };
            _dbContext.RealEstateValuationOrders.Add(orderEntity);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public static SpecificDetailHouseAndFlatObject? GetHouseAndFlat(Database.Entities.RealEstateValuation entity)
    {
        if (entity.SpecificDetailBin is not null)
        {
            switch (Helpers.GetRealEstateType(entity))
            {
                case CIS.Foms.Enums.RealEstateTypes.Hf:
                case CIS.Foms.Enums.RealEstateTypes.Hff:
                    return SpecificDetailHouseAndFlatObject.Parser.ParseFrom(entity.SpecificDetailBin);
            }
        }
        return null;
    }

    public async Task<(decimal? CollateralAmount, decimal? LoanAmount, int? LoanDuration, string? LoanPurpose)> GetProductProperties(int caseState, long caseId, CancellationToken cancellationToken)
    {
        if (caseState == (int)CaseStates.InProgress)
        {
            var (_, offerId) = await _salesArrangementService.GetProductSalesArrangement(caseId, cancellationToken);
            var offer = await _offerService.GetMortgageOfferDetail(offerId!.Value, cancellationToken);

            var collateralAmount = offer.SimulationInputs.CollateralAmount;
            var loanDuration = offer.SimulationInputs.LoanDuration;
            var purpose = await getLoanPurpose(offer.SimulationInputs.LoanPurposes?.FirstOrDefault()?.LoanPurposeId);
            var loanAmount = offer.SimulationInputs.LoanAmount;
            return (collateralAmount, loanAmount, loanDuration, purpose);
        }
        else
        {
            var mortgage = await _productService.GetMortgage(caseId, cancellationToken);

            var purpose = await getLoanPurpose(mortgage.Mortgage.LoanPurposes?.FirstOrDefault()?.LoanPurposeId);
            var loanAmount = mortgage.Mortgage.LoanPaymentAmount;
            return (null, loanAmount, null, purpose);
        }
    }

    private async Task<string?> getLoanPurpose(int? loanPurposeId)
    {
        return (await _codebookService.LoanPurposes()).FirstOrDefault(t => t.Id == loanPurposeId)?.AcvId;
    }

    private readonly IProductServiceClient _productService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly RealEstateValuationServiceDbContext _dbContext;
    private readonly ICaseServiceClient _caseService;
    
    public OrderAggregate(
        IProductServiceClient productService,
        ICodebookServiceClient codebookService,
        ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService,
        RealEstateValuationServiceDbContext dbContext,
        ICaseServiceClient caseService)
    {
        _productService = productService;
        _codebookService = codebookService;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
        _dbContext = dbContext;
    }
}
