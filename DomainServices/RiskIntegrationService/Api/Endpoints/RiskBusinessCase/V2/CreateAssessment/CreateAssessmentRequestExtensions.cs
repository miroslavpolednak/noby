using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _C4M = DomainServices.RiskIntegrationService.ExternalServices.RiskBusinessCase.V3.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CreateAssessment;

internal static class CreateAssessmentRequestExtensions
{
    public static _C4M.LoanApplicationAssessmentCreate ToC4M(this _V2.RiskBusinessCaseCreateAssessmentRequest request, string chanel, string environmentName)
        => new ()
        {
            LoanApplicationId = _C4M.ResourceIdentifier.CreateLoanApplication(request.SalesArrangementId.ToEnvironmentId(environmentName), chanel).ToC4M(),
            LoanApplicationSnapshotId = request.LoanApplicationDataVersion,
            ItChannel = FastEnum.Parse<_C4M.ItChannelType>(chanel, true),
            ItChannelPrevious = request.ItChannelPrevious != Contracts.Shared.ItChannels.Unknown
                ? FastEnum.Parse<_C4M.ItChannelType>(request.ItChannelPrevious.ToString(), true)
                : null,
            AssessmentMode = FastEnum.Parse<_C4M.AssessmentMode>(request.AssessmentMode.ToString(), true),
            GrantingProcedureCode = FastEnum.Parse<_C4M.GrantingProcedure>(request.GrantingProcedureCode.ToString(), true),
            SelfApprovalRequired = request.SelfApprovalRequired,
            SystemApprovalRequired = request.SystemApprovalRequired,
            LoanApplicationException = request.LoanApplicationExceptions?.Select(t => new _C4M.LoanApplicationException
            {
                Arm = t.Arm,
                ReasonCode = t.ReasonCode
            }).ToList(),
            ExceptionHighestApprovalLevel = request.ExceptionHighestApprovalLevel,
            Expand = request.RequestedDetails?.Select(t => FastEnum.Parse<_C4M.Expandables>(t.ToString(), true)).ToList()
        };
}
