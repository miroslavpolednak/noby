using CIS.Core.Security;
using CIS.Foms.Enums;
using DomainServices.UserService.Clients.Authorization;
using NOBY.Infrastructure.ErrorHandling;

namespace NOBY.Infrastructure.Security;

public static class SecurityHelpers
{
    /// <summary>
    /// Autorizace uzivatele na Case podle OwnerUserId a na stav Case
    /// </summary>
    public static void CheckCaseOwnerAndState(ICurrentUserAccessor currentUser, in int ownerUserId, in int caseState)
    {
        // vidi vlastni Case nebo ma pravo videt vse
        if (currentUser.User!.Id != ownerUserId && !currentUser.HasPermission(UserPermissions.DASHBOARD_AccessAllCases))
        {
            throw new CisAuthorizationException("CaseOwnerValidation: user is not owner of the Case or does not have DASHBOARD_AccessAllCases permission");
        }

        // zakazane stavy Case
        if (caseState is (int)CaseStates.Finished or (int)CaseStates.Cancelled)
        {
            throw new NobyValidationException(90032);
        }
        else if (caseState is 5 && !currentUser.HasPermission(UserPermissions.CASE_ViewAfterDrawing))
        {
            throw new CisAuthorizationException($"CaseOwnerValidation: CASE_ViewAfterDrawing missing");
        }
    }
}
