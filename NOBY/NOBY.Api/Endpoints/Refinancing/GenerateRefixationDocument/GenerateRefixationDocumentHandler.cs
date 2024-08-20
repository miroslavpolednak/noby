using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.ProductService.Clients;
using DomainServices.ProductService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using ExternalServices.SbWebApi.V1;
using NOBY.Services.MortgageRefinancing;
using NOBY.Services.WorkflowTask;
using _SA = DomainServices.SalesArrangementService.Contracts.SalesArrangement;

namespace NOBY.Api.Endpoints.Refinancing.GenerateRefixationDocument;

internal sealed class GenerateRefixationDocumentHandler(
    ICodebookServiceClient _codebookService,
    ICaseServiceClient _caseService,
    ISalesArrangementServiceClient _salesArrangementService,
    IOfferServiceClient _offerService,
    IProductServiceClient _productService,
    ISbWebApiClient _sbWebApi,
    MortgageRefinancingDocumentService _refinancingDocumentService)
        : IRequestHandler<RefinancingGenerateRefixationDocumentRequest>
{
    public async Task Handle(RefinancingGenerateRefixationDocumentRequest request, CancellationToken cancellationToken)
    {
        var product = await _productService.GetMortgage(request.CaseId, cancellationToken);

        await ValidateSignatureTypeDetailId(request, product, cancellationToken);

        var salesArrangement = await _refinancingDocumentService.LoadAndValidateSA(request.SalesArrangementId, SalesArrangementTypes.MortgageRefixation, cancellationToken);
        var offer = await LoadAndValidateOffer(salesArrangement.CaseId, cancellationToken);

        var offerIndividualPrice = new MortgageRefinancingIndividualPrice(offer.MortgageRefixation.SimulationInputs.InterestRateDiscount, default);

        if (offerIndividualPrice.HasIndividualPrice && !await _refinancingDocumentService.IsIndividualPriceValid(salesArrangement, offerIndividualPrice, cancellationToken))
            throw new NobyValidationException(90048);

        await UpdateSAParameters(request, salesArrangement, cancellationToken);

        await UpdateRefixationProcess(salesArrangement, offer, cancellationToken);

        if (salesArrangement.OfferId != offer.Data.OfferId)
            await _salesArrangementService.LinkModelationToSalesArrangement(salesArrangement.SalesArrangementId, offer.Data.OfferId, cancellationToken);

        if (request.RefixationDocumentTypeId == (int)RefixationDocumentTypes.HedgeAppendix)
            await GenerateHedgeAppendixDocument(salesArrangement, offer, offerIndividualPrice.HasIndividualPrice, product, cancellationToken);
        else
            await GenerateInterestRateNotificationDocument(salesArrangement, offer, cancellationToken);

        await _salesArrangementService.UpdateSalesArrangementState(salesArrangement.SalesArrangementId, EnumSalesArrangementStates.InSigning, cancellationToken);
    }

    private async Task ValidateSignatureTypeDetailId(RefinancingGenerateRefixationDocumentRequest request, GetMortgageResponse product, CancellationToken cancellationToken)
    {
        var signatureTypeDetails = await _codebookService.SignatureTypeDetails(cancellationToken);
        var fixedRateValidToDate = ((DateTime?)product.Mortgage.FixedRateValidTo ?? DateTime.Now).Date;

        if (request.RefixationDocumentTypeId == (int)RefixationDocumentTypes.HedgeAppendix)
        {
            if (!signatureTypeDetails.Any(s => s.Id == request.SignatureTypeDetailId && s.IsHedgeAvailable))
                throw new NobyValidationException(90032);

            if (DateTime.Now.Date.AddDays(14) >= fixedRateValidToDate) 
                throw new NobyValidationException(90062);
        }
        else
        {
            if (!signatureTypeDetails.Any(s => s.Id == request.SignatureTypeDetailId && s.IsIndividualAvailable))
                throw new NobyValidationException(90032);

            if (DateTime.Now.Date > fixedRateValidToDate) 
                throw new NobyValidationException(90062);
        }
    }

    private async Task<GetOfferListResponse.Types.GetOfferListItem> LoadAndValidateOffer(long caseId, CancellationToken cancellationToken)
    {
        var offers = await _offerService.GetOfferList(caseId, OfferTypes.MortgageRefixation, cancellationToken: cancellationToken);

        var selectedOffer = offers.FirstOrDefault(o => ((EnumOfferFlagTypes)o.Data.Flags).HasFlag(EnumOfferFlagTypes.Selected))
                            ?? throw new NobyValidationException(90032, "No offer was selected");

        if ((DateTime)selectedOffer.MortgageRefixation.SimulationInputs.InterestRateValidFrom < DateTime.UtcNow.ToLocalTime().Date || (DateTime?)selectedOffer.Data.ValidTo < DateTime.UtcNow.ToLocalTime().Date)
            throw new NobyValidationException(90051);

        return selectedOffer;
    }

    private async Task UpdateRefixationProcess(_SA salesArrangement, GetOfferListResponse.Types.GetOfferListItem offer, CancellationToken cancellationToken)
    {
        var workflowResult = await _caseService.GetProcessByProcessId(salesArrangement.CaseId, salesArrangement.ProcessId!.Value, cancellationToken);

        var updateRequest = new UpdateTaskRequest
        {
            CaseId = salesArrangement.CaseId,
            TaskIdSb = workflowResult.ProcessIdSb,
            MortgageRefixation = new()
            {
                LoanInterestRate = (decimal?)offer.MortgageRefixation.SimulationInputs.InterestRate,
                LoanInterestRateProvided = (decimal)offer.MortgageRefixation.SimulationInputs.InterestRate - ((decimal?)offer.MortgageRefixation.SimulationInputs.InterestRateDiscount ?? 0),
                LoanPaymentAmount = offer.MortgageRefixation.SimulationResults.LoanPaymentAmount,
                LoanPaymentAmountFinal = (decimal?)offer.MortgageRefixation.SimulationResults.LoanPaymentAmountDiscounted ?? offer.MortgageRefixation.SimulationResults.LoanPaymentAmount,
                FixedRatePeriod = offer.MortgageRefixation.SimulationInputs.FixedRatePeriod
            }
        };

        await _caseService.UpdateTask(updateRequest, cancellationToken);
    }

    private async Task UpdateSAParameters(RefinancingGenerateRefixationDocumentRequest request, _SA salesArrangement, CancellationToken cancellationToken)
    {
        salesArrangement.Refixation.SignatureTypeDetailId = request.SignatureTypeDetailId;
        salesArrangement.Refixation.SignatureDeadline = request.SignatureDeadline;

        var updateRequest = new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            Refixation = salesArrangement.Refixation
        };

        await _salesArrangementService.UpdateSalesArrangementParameters(updateRequest, cancellationToken);
    }

    private async Task GenerateHedgeAppendixDocument(_SA salesArrangement, GetOfferListResponse.Types.GetOfferListItem offer, bool hasIndividualPrice, GetMortgageResponse product, CancellationToken cancellationToken)
    {
        var user = await _refinancingDocumentService.LoadUserInfo(cancellationToken);

        var simulationInputs = offer.MortgageRefixation.SimulationInputs;
        var simulationResults = offer.MortgageRefixation.SimulationResults;

        var request = new ExternalServices.SbWebApi.Dto.Refinancing.GenerateRefixationDocumentRequest
        {
            CaseId = salesArrangement.CaseId,
            InterestRateProvided = simulationInputs.InterestRate - ((decimal?)simulationInputs.InterestRateDiscount).GetValueOrDefault(),
            FixedRateValidTo = product.Mortgage.FixedRateValidTo,
            PaymentAmount = (decimal?)simulationResults.LoanPaymentAmountDiscounted ?? simulationResults.LoanPaymentAmount,
            SignatureTypeDetailId = salesArrangement.Refixation.SignatureTypeDetailId!.Value,
            Cpm = user.Cpm,
            Icp = user.Icp,
            SignatureDeadline = salesArrangement.Refixation.SignatureDeadline,
            IndividualPricing = hasIndividualPrice,
            FixedRatePeriod = simulationInputs.FixedRatePeriod,
            LoanPaymentsCount = simulationResults.LoanPaymentsCount,
            MaturityDate = simulationResults.MaturityDate
        };

        await _sbWebApi.GenerateHedgeAppendixDocument(request, cancellationToken);
    }

    private async Task GenerateInterestRateNotificationDocument(_SA salesArrangement, GetOfferListResponse.Types.GetOfferListItem offer, CancellationToken cancellationToken)
    {
        var user = await _refinancingDocumentService.LoadUserInfo(cancellationToken);
        var product = await _productService.GetMortgage(salesArrangement.CaseId, cancellationToken);

        var simulationInputs = offer.MortgageRefixation.SimulationInputs;
        var simulationResults = offer.MortgageRefixation.SimulationResults;

        var request = new ExternalServices.SbWebApi.Dto.Refinancing.GenerateInterestRateNotificationDocumentRequest
        {
            CaseId = salesArrangement.CaseId,
            InterestRateProvided = simulationInputs.InterestRate - ((decimal?)simulationInputs.InterestRateDiscount).GetValueOrDefault(),
            FixedRateValidTo = product.Mortgage.FixedRateValidTo,
            PaymentAmount = (decimal?)simulationResults.LoanPaymentAmountDiscounted ?? simulationResults.LoanPaymentAmount,
            SignatureTypeDetailId = salesArrangement.Refixation.SignatureTypeDetailId!.Value,
            Cpm = user.Cpm,
            Icp = user.Icp,
            FixedRatePeriod = simulationInputs.FixedRatePeriod
        };

        await _sbWebApi.GenerateInterestRateNotificationDocument(request, cancellationToken);
    }
}