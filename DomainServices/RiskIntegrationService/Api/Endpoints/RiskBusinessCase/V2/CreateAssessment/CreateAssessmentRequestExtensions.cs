using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CreateAssessment;

internal static class CreateAssessmentRequestExtensions
{
    public static _C4M.LoanApplicationAssessmentCreate ToC4M(this _V2.RiskBusinessCaseCreateAssessmentRequest request, string chanel)
        => new ()
        {
            LoanApplicationId = _C4M.ResourceIdentifier.CreateLoanApplication(request.SalesArrangementId, chanel),
            LoanApplicationDataVersion = request.LoanApplicationDataVersion,
            ItChannel = FastEnum.Parse<_C4M.LoanApplicationAssessmentCreateItChannel>(chanel, true),
            ItChannelPrevious = FastEnum.Parse<_C4M.LoanApplicationAssessmentCreateItChannelPrevious>(request.ItChannelPrevious.ToString(), true),
            AssessmentMode = FastEnum.Parse<_C4M.LoanApplicationAssessmentCreateAssessmentMode>(request.AssessmentMode.ToString(), true),
            GrantingProcedureCode = FastEnum.Parse<_C4M.LoanApplicationAssessmentCreateGrantingProcedureCode>(request.GrantingProcedureCode.ToString(), true),
            SelfApprovalRequired = request.SelfApprovalRequired,
            LoanApplicationException = request.LoanApplicationExceptions?.Select(t => new _C4M.LoanApplicationException
            {
                Arm = t.Arm,
                ReasonCode = t.ReasonCode
            }).ToList(),
            ExceptionHighestApprovalLevel = request.ExceptionHighestApprovalLevel,
            Expand = request.RequestedDetails?.Select(t => FastEnum.Parse<_C4M.Expand>(t.ToString(), true)).ToList()
        };
}
