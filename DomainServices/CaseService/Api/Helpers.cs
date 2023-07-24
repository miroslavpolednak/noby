using CIS.Foms.Enums;

namespace DomainServices.CaseService.Api;

internal static class Helpers
{
    public static void ThrowIfCaseIsCancelled(int? caseState)
    {
        if (caseState.HasValue && DisallowedStates.Contains(caseState.Value))
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.CaseCancelled);
        }
    }

    public static readonly int[] DisallowedStates = new[]
    {
        (int)CaseStates.ToBeCancelledConfirmed,
        (int)CaseStates.Cancelled
    };
}
