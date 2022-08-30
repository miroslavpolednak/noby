using DomainServices.OfferService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using ExternalServices.Rip.V1.RipWrapper;

namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal class GetLoanApplicationAssessmentHandler
    : IRequestHandler<GetLoanApplicationAssessmentRequest, GetLoanApplicationAssessmentResponse>
{

    #region Construction

    private readonly LoanApplicationDataService _loanApplicationDataService;
    private readonly ExternalServices.Rip.V1.IRipClient _ripClient;
    private readonly IOfferServiceAbstraction _offerService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;

    public GetLoanApplicationAssessmentHandler(
        LoanApplicationDataService loanApplicationDataService,
        IOfferServiceAbstraction offerService,
        ISalesArrangementServiceAbstraction salesArrangementService,
        ExternalServices.Rip.V1.IRipClient ripClient
        )
    {
        _loanApplicationDataService = loanApplicationDataService;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _ripClient = ripClient;
    }

    #endregion

    public async Task<GetLoanApplicationAssessmentResponse> Handle(GetLoanApplicationAssessmentRequest request, CancellationToken cancellationToken)
    {
        // instance SA
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.SalesArrangementService.Contracts.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));

        // if LoanApplication wasn't created so far, request without NewAssessmentRequired = true is senceless
        if (!request.NewAssessmentRequired && string.IsNullOrEmpty(saInstance.LoanApplicationAssessmentId))
            throw new CisValidationException($"LoanApplicationAssessmentId is missing for SA #{saInstance.SalesArrangementId}");

        // instance Offer
        var offerInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.OfferService.Contracts.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(saInstance.OfferId!.Value, cancellationToken));

        var loanApplicationAssessmentId = saInstance.LoanApplicationAssessmentId;

        if (request.NewAssessmentRequired) // vytvorit novy assessment
        {
            var loanApplicationData = await _loanApplicationDataService.LoadData(request.SalesArrangementId, cancellationToken);
            var loanApplicationRequest = loanApplicationData.ToLoanApplicationRequest();
            loanApplicationAssessmentId = ServiceCallResult.ResolveAndThrowIfError<string>(await _ripClient.CreateLoanApplication(loanApplicationRequest));
            ServiceCallResult.Resolve(await _salesArrangementService.UpdateLoanAssessmentParameters(request.SalesArrangementId, loanApplicationAssessmentId, saInstance.RiskSegment, saInstance.CommandId, cancellationToken));
        }

        var result = ServiceCallResult.ResolveAndThrowIfError<LoanApplicationAssessmentResponse>(await _ripClient.GetLoanApplication(loanApplicationAssessmentId, new List<string>
            {
                "assessmentDetail",
                "householdAssessmentDetail",
                "counterpartyAssessmentDetail",
                "collateralRiskCharacteristics"
            }));

        var model = result.ToApiResponse();
        model.Application!.LoanAmount = offerInstance.SimulationResults.LoanAmount ?? 0;
        model.Application!.LoanPaymentAmount = offerInstance.SimulationResults.LoanPaymentAmount ?? 0;

        return model;
    }
}
