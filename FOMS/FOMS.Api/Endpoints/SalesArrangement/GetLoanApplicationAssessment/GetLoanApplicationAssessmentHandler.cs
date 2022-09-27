using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.RiskIntegrationService.Clients.LoanApplication.V2;
using DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2;
using DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal class GetLoanApplicationAssessmentHandler
    : IRequestHandler<GetLoanApplicationAssessmentRequest, GetLoanApplicationAssessmentResponse>
{

    #region Construction

    private readonly LoanApplicationDataService _loanApplicationDataService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ILoanApplicationServiceClient _loanApplicationService;
    private readonly IRiskBusinessCaseServiceClient _riskBusinessCaseService;

    public GetLoanApplicationAssessmentHandler(
        LoanApplicationDataService loanApplicationDataService,
        ISalesArrangementServiceAbstraction salesArrangementService,
        ILoanApplicationServiceClient loanApplicationService,
        IRiskBusinessCaseServiceClient riskBusinessCaseService

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
            if (String.IsNullOrEmpty(saInstance.RiskBusinessCaseId))
            {
                throw new CisValidationException(99999, $"SalesArrangement.RiskBusinessCaseId not defined."); //TODO: error code
            }

            // load data
            var loanApplicationData = await _loanApplicationDataService.LoadData(request.SalesArrangementId, cancellationToken);

            // loan application save
            var loanApplicationSaveRequest = loanApplicationData.ToLoanApplicationSaveRequest();
            var loanApplicationSaveResponse = ServiceCallResult.ResolveAndThrowIfError<LoanApplicationSaveResponse>(await _loanApplicationService.Save(loanApplicationSaveRequest, cancellationToken));
            var riskSegment = loanApplicationSaveResponse.RiskSegment.ToString();

            // create assesment
            var createAssesmentRequest = loanApplicationData.ToRiskBusinessCaseCreateAssesmentRequest();
            var createAssesmentResponse = ServiceCallResult.ResolveAndThrowIfError<DomainServices.RiskIntegrationService.Contracts.Shared.V1.LoanApplicationAssessmentResponse>(await _riskBusinessCaseService.CreateAssessment(createAssesmentRequest, cancellationToken));
            loanApplicationAssessmentId = createAssesmentResponse.LoanApplicationAssessmentId;

            // update sales arrangement (loanApplicationAssessmentId, riskSegment)
            ServiceCallResult.Resolve(await _salesArrangementService.UpdateLoanAssessmentParameters(request.SalesArrangementId, loanApplicationAssessmentId, riskSegment, saInstance.CommandId, createAssesmentResponse?.RiskBusinessCaseExpirationDate, cancellationToken));
        }

        // load assesment by ID
        var getAssesmentRequest = new  RiskBusinessCaseGetAssessmentRequest
        {
            LoanApplicationAssessmentId = loanApplicationAssessmentId!,
            RequestedDetails = new List<RiskBusinessCaseRequestedDetails> { RiskBusinessCaseRequestedDetails.assessmentDetail, RiskBusinessCaseRequestedDetails.householdAssessmentDetail, RiskBusinessCaseRequestedDetails.counterpartyAssessmentDetail, RiskBusinessCaseRequestedDetails.collateralRiskCharacteristics }
        };
        var getAssesmentResponse = ServiceCallResult.ResolveAndThrowIfError<DomainServices.RiskIntegrationService.Contracts.Shared.V1.LoanApplicationAssessmentResponse>(await _riskBusinessCaseService.GetAssessment(getAssesmentRequest, cancellationToken));

        // convert to ApiResponse
        return getAssesmentResponse.ToApiResponse();
    }
}
