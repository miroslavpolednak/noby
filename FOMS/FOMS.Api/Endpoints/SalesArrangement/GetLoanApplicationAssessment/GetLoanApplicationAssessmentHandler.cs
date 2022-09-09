using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.RiskIntegrationService.Abstraction.LoanApplication.V2;
using DomainServices.RiskIntegrationService.Abstraction.RiskBusinessCase.V2;
using DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal class GetLoanApplicationAssessmentHandler
    : IRequestHandler<GetLoanApplicationAssessmentRequest, GetLoanApplicationAssessmentResponse>
{

    #region Construction

    private readonly LoanApplicationDataService _loanApplicationDataService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ILoanApplicationServiceAbstraction _loanApplicationService;
    private readonly IRiskBusinessCaseServiceAbstraction _riskBusinessCaseService;

    public GetLoanApplicationAssessmentHandler(
        LoanApplicationDataService loanApplicationDataService,
        ISalesArrangementServiceAbstraction salesArrangementService,
        ILoanApplicationServiceAbstraction loanApplicationService,
        IRiskBusinessCaseServiceAbstraction riskBusinessCaseService

        )
    {
        _loanApplicationDataService = loanApplicationDataService;
        _salesArrangementService = salesArrangementService;
        _loanApplicationService = loanApplicationService;
        _riskBusinessCaseService = riskBusinessCaseService;
    }

    #endregion

    public async Task<GetLoanApplicationAssessmentResponse> Handle(GetLoanApplicationAssessmentRequest request, CancellationToken cancellationToken)
    {
        // instance SA
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.SalesArrangementService.Contracts.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));

        // if LoanApplication wasn't created so far, request without NewAssessmentRequired = true is senceless
        if (!request.NewAssessmentRequired && string.IsNullOrEmpty(saInstance.LoanApplicationAssessmentId))
            throw new CisValidationException($"LoanApplicationAssessmentId is missing for SA #{saInstance.SalesArrangementId}");

        var loanApplicationAssessmentId = saInstance.LoanApplicationAssessmentId;

        // create new assesment, if required
        if (request.NewAssessmentRequired)
        {
            // load data
            var loanApplicationData = await _loanApplicationDataService.LoadData(request.SalesArrangementId, cancellationToken);

            // loan application save
            var loanApplicationSaveRequest = loanApplicationData.ToLoanApplicationSaveRequest();
            var loanApplicationSaveResponse = ServiceCallResult.ResolveAndThrowIfError<LoanApplicationSaveResponse>(await _loanApplicationService.Save(loanApplicationSaveRequest, cancellationToken));
            var riskSegment = loanApplicationSaveResponse.RiskSegment.ToString();

            // create assesment
            var createAssesmentRequest = loanApplicationData.ToRiskBusinessCaseCreateAssesmentRequest();
            var createAssesmentResponse = ServiceCallResult.ResolveAndThrowIfError<DomainServices.RiskIntegrationService.Contracts.Shared.V1.LoanApplicationAssessmentResponse>(await _riskBusinessCaseService.CreateAssesment(createAssesmentRequest, cancellationToken));
            loanApplicationAssessmentId = createAssesmentResponse.LoanApplicationAssessmentId;

            // update sales arrangement (loanApplicationAssessmentId, riskSegment)
            ServiceCallResult.Resolve(await _salesArrangementService.UpdateLoanAssessmentParameters(request.SalesArrangementId, loanApplicationAssessmentId, riskSegment, saInstance.CommandId, cancellationToken));
        }

        // load assesment by ID
        var getAssesmentRequest = new RiskBusinessCaseGetAssesmentRequest
        {
            LoanApplicationAssessmentId = loanApplicationAssessmentId!,
            RequestedDetails = new List<RiskBusinessCaseRequestedDetails> { RiskBusinessCaseRequestedDetails.assessmentDetail, RiskBusinessCaseRequestedDetails.householdAssessmentDetail, RiskBusinessCaseRequestedDetails.counterpartyAssessmentDetail, RiskBusinessCaseRequestedDetails.collateralRiskCharacteristics }
        };
        var getAssesmentResponse = ServiceCallResult.ResolveAndThrowIfError<DomainServices.RiskIntegrationService.Contracts.Shared.V1.LoanApplicationAssessmentResponse>(await _riskBusinessCaseService.GetAssesment(getAssesmentRequest, cancellationToken));

        // convert to ApiResponse
        return getAssesmentResponse.ToApiResponse();
    }
}
