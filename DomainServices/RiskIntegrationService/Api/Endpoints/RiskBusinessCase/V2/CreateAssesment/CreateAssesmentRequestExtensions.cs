using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V0_2.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CreateAssesment;

internal static class CreateAssesmentRequestExtensions
{
    public static _C4M.AssessmentRequest ToC4M(this _V2.CreateAssesmentRequest request, string chanel)
        => new _C4M.AssessmentRequest
        {
            LoanApplicationId = _C4M.ResourceIdentifier.CreateLoanApplication(request.SalesArrangementId, chanel),
            LoanApplicationDataVersion = request.LoanApplicationDataVersion,
            ItChannel = FastEnum.Parse<_C4M.AssessmentRequestItChannel>(chanel, true),
            ItChannelPrevious = FastEnum.Parse<_C4M.AssessmentRequestItChannelPrevious>(request.ItChannelPrevious.ToString()),
            AssessmentMode = FastEnum.Parse<_C4M.AssessmentRequestAssessmentMode>(request.AssessmentMode.ToString()),
            GrantingProcedureCode = FastEnum.Parse<_C4M.AssessmentRequestGrantingProcedureCode>(request.GrantingProcedureCode.ToString()),
            SelfApprovalRequired = request.SelfApprovalRequired,
            LoanApplicationException = request.LoanApplicationExceptions?.Select(t => new _C4M.LoanApplicationException
            {
                Arm = t.Arm,
                ReasonCode = t.ReasonCode
            }).ToList(),
            ExceptionHighestApprovalLevel = request.ExceptionHighestApprovalLevel,
            Expand = request.RequestedDetails?.Select(t => FastEnum.Parse<_C4M.Expand>(t.ToString())).ToList()
        };
}
