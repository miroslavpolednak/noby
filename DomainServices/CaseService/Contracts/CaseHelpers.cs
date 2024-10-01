using SharedTypes.Enums;
using System.Diagnostics.CodeAnalysis;

namespace DomainServices.CaseService.Contracts;

public static class CaseHelpers
{
    public static bool IsCaseInState(IEnumerable<EnumCaseStates> allowedStates, in EnumCaseStates caseState)
    {
        return allowedStates.Contains(caseState);
    }

    public static bool IsInState([NotNull] this Contracts.Case caseInstance, IEnumerable<EnumCaseStates> allowedStates)
        => IsCaseInState(allowedStates, (EnumCaseStates)caseInstance.State);

    public static bool IsInState([NotNull] this Contracts.ValidateCaseIdResponse caseInstance, IEnumerable<EnumCaseStates> allowedStates)
        => IsCaseInState(allowedStates, (EnumCaseStates)caseInstance.State!);

    public static readonly EnumCaseStates[] AllExceptInProgressStates =
    [
        EnumCaseStates.InApproval,
        EnumCaseStates.InSigning,
        EnumCaseStates.InDisbursement,
        EnumCaseStates.InAdministration,
        EnumCaseStates.Finished,
        EnumCaseStates.Cancelled,
        EnumCaseStates.InApprovalConfirmed,
        EnumCaseStates.ToBeCancelled,
        EnumCaseStates.ToBeCancelledConfirmed
    ];
}
