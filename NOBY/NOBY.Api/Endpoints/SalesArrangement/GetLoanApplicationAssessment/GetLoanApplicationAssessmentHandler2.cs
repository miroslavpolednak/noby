using CIS.Core.Security;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal sealed class GetLoanApplicationAssessmentHandler2
    : IRequestHandler<GetLoanApplicationAssessmentRequest, GetLoanApplicationAssessmentResponse>
{
    public async Task<GetLoanApplicationAssessmentResponse> Handle(GetLoanApplicationAssessmentRequest request, CancellationToken cancellationToken)
    {
        // instance SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        // neexistuje RBC -> vytvorit novy a ulozit
        if (string.IsNullOrEmpty(saInstance.RiskBusinessCaseId))
        {
            var createResult = await _riskCaseProcessor.CreateOrUpdateRiskCase(saInstance.SalesArrangementId, cancellationToken);

            // update
            await _salesArrangementService.UpdateLoanAssessmentParameters(new DomainServices.SalesArrangementService.Contracts.UpdateLoanAssessmentParametersRequest
            {
                SalesArrangementId = saInstance.SalesArrangementId,
                RiskSegment = createResult.RiskSegment,
                LoanApplicationDataVersion = createResult.LoanApplicationDataVersion
            }, cancellationToken);
        }
        else if (string.IsNullOrEmpty(saInstance.LoanApplicationDataVersion) || request.NewAssessmentRequired)
        {
            var laResult = await _riskCaseProcessor.SaveLoanApplication(saInstance.SalesArrangementId, saInstance.CaseId, saInstance.OfferId!.Value, cancellationToken);
            
            await _salesArrangementService.UpdateLoanAssessmentParameters(new DomainServices.SalesArrangementService.Contracts.UpdateLoanAssessmentParametersRequest
            {
                SalesArrangementId = saInstance.SalesArrangementId,
                RiskSegment = laResult.RiskSegment,
                LoanApplicationDataVersion = laResult.LoanApplicationDataVersion
            }, cancellationToken);
        }

        if (request.NewAssessmentRequired)
        {
            if (!_currentUser.HasPermission(UserPermissions.SCORING_Perform))
            {
                throw new CisAuthorizationException("SCORING_Perform permission missing");
            }

        }
    }

    private readonly ICurrentUserAccessor _currentUser;
    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly NOBY.Services.RiskCaseProcessor.RiskCaseProcessorService _riskCaseProcessor;
    private readonly DomainServices.RiskIntegrationService.Clients.CustomerExposure.V2.ICustomerExposureServiceClient _customerExposureService;

    public GetLoanApplicationAssessmentHandler2(
        ICurrentUserAccessor currentUser,
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        Services.RiskCaseProcessor.RiskCaseProcessorService riskCaseProcessor, 
        DomainServices.RiskIntegrationService.Clients.CustomerExposure.V2.ICustomerExposureServiceClient customerExposureService)
    {
        _currentUser = currentUser;
        _salesArrangementService = salesArrangementService;
        _riskCaseProcessor = riskCaseProcessor;
        _customerExposureService = customerExposureService;
    }
}
