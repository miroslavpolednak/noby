using CIS.Core.Configuration;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using ExternalServices.SbWebApi.V1;
using NOBY.Services.MortgageRefinancing;
using NOBY.Services.WorkflowTask;
using _SA = DomainServices.SalesArrangementService.Contracts.SalesArrangement;

namespace NOBY.Api.Endpoints.Refinancing.GenerateRefixationDocument;

internal sealed class GenerateRefixationDocumentHandler : IRequestHandler<GenerateRefixationDocumentRequest>
{
    private readonly ICodebookServiceClient _codebookService;
    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly ISbWebApiClient _sbWebApi;
    private readonly MortgageRefinancingDocumentService _refinancingDocumentService;
    private readonly ICisEnvironmentConfiguration _configuration;

    public GenerateRefixationDocumentHandler(
        ICodebookServiceClient codebookService,
        ICaseServiceClient caseService,
        ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService,
        ISbWebApiClient sbWebApi,
        MortgageRefinancingDocumentService refinancingDocumentService,
        ICisEnvironmentConfiguration configuration)
    {
        _codebookService = codebookService;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _sbWebApi = sbWebApi;
        _refinancingDocumentService = refinancingDocumentService;
        _configuration = configuration;
    }

    public async Task Handle(GenerateRefixationDocumentRequest request, CancellationToken cancellationToken)
    {
        //TODO: we have just one CaseId, we cannot lose it
        if (_configuration.EnvironmentName != "PROD" && request.CaseId == 3075599)
            throw new NotSupportedException("Case ID for testing only");

        await ValidateSignatureTypeDetailId(request, cancellationToken);

        var salesArrangement = await _refinancingDocumentService.LoadAndValidateSA(request.SalesArrangementId, SalesArrangementTypes.MortgageRefixation, cancellationToken);
        var offer = await LoadAndValidateOffer(salesArrangement.CaseId, cancellationToken);

        var offerIndividualPrice = new MortgageRefinancingIndividualPrice(offer.MortgageRefixation.SimulationInputs.InterestRateDiscount, default);

        if (offerIndividualPrice.HasIndividualPrice && !await _refinancingDocumentService.IsIndividualPriceValid(salesArrangement, offerIndividualPrice, cancellationToken))
            throw new NobyValidationException(90048);

        await UpdateSAParameters(request, salesArrangement, cancellationToken);

        await UpdateRefixationProcess(salesArrangement, offer, cancellationToken);
        await _salesArrangementService.LinkModelationToSalesArrangement(salesArrangement.SalesArrangementId, offer.Data.OfferId, cancellationToken);

        if (request.RefinancingDocumentTypeId == 3)
            await GenerateRefixationDocument(salesArrangement, offer, offerIndividualPrice.HasIndividualPrice, cancellationToken);
        else
            await GenerateInterestRateNotificationDocument(salesArrangement, offer, cancellationToken);

        await _salesArrangementService.UpdateSalesArrangementState(salesArrangement.SalesArrangementId, (int)SalesArrangementStates.InSigning, cancellationToken);
    }

    private async Task ValidateSignatureTypeDetailId(GenerateRefixationDocumentRequest request, CancellationToken cancellationToken)
    {
        var signatureTypeDetails = await _codebookService.SignatureTypeDetails(cancellationToken);

        if (signatureTypeDetails.Any(s => s.Id == request.SignatureTypeDetailId &&
                                          s.IsHedgeAvailable == (request.RefinancingDocumentTypeId == 3) &&
                                          s.IsIndividualAvailable == (request.RefinancingDocumentTypeId == 2)))
        {
            return;
        }

        throw new NobyValidationException(90032);
    }

    private async Task<GetOfferListResponse.Types.GetOfferListItem> LoadAndValidateOffer(long caseId, CancellationToken cancellationToken)
    {
        var offers = await _offerService.GetOfferList(caseId, OfferTypes.MortgageRefixation, cancellationToken: cancellationToken);

        var selectedOffer = offers.FirstOrDefault(o => ((OfferFlagTypes)o.Data.Flags).HasFlag(OfferFlagTypes.Selected))
                            ?? throw new NobyValidationException(90032, "No offer was selected");

        if ((DateTime)selectedOffer.MortgageRefixation.SimulationInputs.InterestRateValidFrom < DateTime.UtcNow.ToLocalTime().Date || (DateTime?)selectedOffer.Data.ValidTo < DateTime.UtcNow.ToLocalTime().Date)
            throw new NobyValidationException(90051);

        return selectedOffer;
    }

    private async Task UpdateRefixationProcess(_SA salesArrangement, GetOfferListResponse.Types.GetOfferListItem offer, CancellationToken cancellationToken)
    {
        var workflowResult = await _caseService.GetProcessByProcessId(salesArrangement.CaseId, salesArrangement.TaskProcessId!.Value, cancellationToken);

        var updateRequest = new UpdateTaskRequest
        {
            CaseId = salesArrangement.CaseId,
            TaskIdSb = workflowResult.ProcessIdSb,
            Retention = new Retention
            {
                LoanInterestRate = (decimal?)offer.MortgageRefixation.SimulationInputs.InterestRate,
                LoanInterestRateProvided = (decimal)offer.MortgageRefixation.SimulationInputs.InterestRate - ((decimal?)offer.MortgageRefixation.SimulationInputs.InterestRateDiscount ?? 0),
                LoanPaymentAmount = offer.MortgageRefixation.SimulationResults.LoanPaymentAmount,
                LoanPaymentAmountFinal = offer.MortgageRefixation.SimulationResults.LoanPaymentAmountDiscounted,
                FixedRatePeriod = offer.MortgageRefixation.SimulationInputs.FixedRatePeriod
            }
        };

        await _caseService.UpdateTask(updateRequest, cancellationToken);
    }

    private async Task UpdateSAParameters(GenerateRefixationDocumentRequest request, _SA salesArrangement, CancellationToken cancellationToken)
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

    private async Task GenerateRefixationDocument(_SA salesArrangement, GetOfferListResponse.Types.GetOfferListItem offer, bool hasIndividualPrice, CancellationToken cancellationToken)
    {
        var user = await _refinancingDocumentService.LoadUserInfo(cancellationToken);

        var simulationInputs = offer.MortgageRefixation.SimulationInputs;
        var simulationResults = offer.MortgageRefixation.SimulationResults;

        var request = new ExternalServices.SbWebApi.Dto.Refinancing.GenerateRefixationDocumentRequest
        {
            CaseId = salesArrangement.CaseId,
            InterestRateProvided = simulationInputs.InterestRate - ((decimal?)simulationInputs.InterestRateDiscount).GetValueOrDefault(),
            FixedRateValidTo = simulationInputs.InterestRateValidFrom,
            PaymentAmount = (decimal?)simulationResults.LoanPaymentAmountDiscounted ?? simulationResults.LoanPaymentAmount,
            SignatureTypeDetailId = salesArrangement.Refixation.SignatureTypeDetailId!.Value,
            Cpm = user.Cpm,
            Icp = user.Icp,
            SignatureDeadline = salesArrangement.Refixation.SignatureDeadline,
            IndividualPricing = hasIndividualPrice,
            FixedRatePeriod = simulationInputs.FixedRatePeriod
        };

        await _sbWebApi.GenerateRefixationDocument(request, cancellationToken);
    }

    private async Task GenerateInterestRateNotificationDocument(_SA salesArrangement, GetOfferListResponse.Types.GetOfferListItem offer, CancellationToken cancellationToken)
    {
        var user = await _refinancingDocumentService.LoadUserInfo(cancellationToken);

        var simulationInputs = offer.MortgageRefixation.SimulationInputs;
        var simulationResults = offer.MortgageRefixation.SimulationResults;

        var request = new ExternalServices.SbWebApi.Dto.Refinancing.GenerateInterestRateNotificationDocumentRequest
        {
            CaseId = salesArrangement.CaseId,
            InterestRateProvided = simulationInputs.InterestRate - ((decimal?)simulationInputs.InterestRateDiscount).GetValueOrDefault(),
            FixedRateValidTo = simulationInputs.InterestRateValidFrom,
            PaymentAmount = (decimal?)simulationResults.LoanPaymentAmountDiscounted ?? simulationResults.LoanPaymentAmount,
            SignatureTypeDetailId = salesArrangement.Refixation.SignatureTypeDetailId!.Value,
            Cpm = user.Cpm,
            Icp = user.Icp,
            FixedRatePeriod = simulationInputs.FixedRatePeriod
        };

        await _sbWebApi.GenerateInterestRateNotificationDocument(request, cancellationToken);
    }
}