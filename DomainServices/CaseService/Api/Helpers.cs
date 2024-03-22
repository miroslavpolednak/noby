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

    public static readonly int[] AllowedTaskTypeId = [1, 2, 3, 4, 6, 7, 8, 9];

    public static readonly int[] DisallowedStates = 
    [
        (int)CaseStates.ToBeCancelledConfirmed,
        (int)CaseStates.Cancelled
    ];
}
