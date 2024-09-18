using CIS.Core.Security;
using DomainServices.UserService.Clients.Authorization;
using NOBY.Infrastructure.ErrorHandling;

namespace NOBY.Infrastructure.Security;

public static class SecurityHelpers
{
    /// <summary>
    /// Autorizace uzivatele na Case podle OwnerUserId a na stav Case
    /// </summary>
    public static void CheckCaseOwnerAndState(
        ICurrentUserAccessor currentUser, 
        in int ownerUserId, 
        in EnumCaseStates caseState, 
        in DateTime? stateUpdatedOn,
        in bool validateCaseStateAndProductSA = true, 
        in int? salesArrangementTypeId = null)
    {
        // vidi vlastni Case nebo ma pravo videt vse
        if (currentUser.User!.Id != ownerUserId && !currentUser.HasPermission(UserPermissions.DASHBOARD_AccessAllCases))
        {
            throw new CisAuthorizationException("CaseOwnerValidation: user is not owner of the Case or does not have DASHBOARD_AccessAllCases permission");
        }

        // zakazane stavy Case
        if (caseState is EnumCaseStates.Finished or EnumCaseStates.Cancelled or EnumCaseStates.ToBeCancelledConfirmed)
        {
            throw new NobyValidationException(90074);
        }
        else if (caseState is EnumCaseStates.InAdministration && !currentUser.HasPermission(UserPermissions.CASE_ViewAfterDrawing) && stateUpdatedOn.HasValue && stateUpdatedOn.Value < DateTime.Now.AddDays(-90))
        {
            throw new CisAuthorizationException($"CaseOwnerValidation: CASE_ViewAfterDrawing missing");
        }
        else if (validateCaseStateAndProductSA && caseState != EnumCaseStates.InProgress && salesArrangementTypeId == (int)SalesArrangementTypes.Mortgage)
        {
            throw new CisAuthorizationException($"CaseOwnerValidation: is product SA and CaseState > 1");
        }
    }

    /// <summary>
    /// Ziskej redirectUri z query stringu nebo vrat default.
    /// </summary>
    /// <returns>
    /// Kontroluje, zda uri v query stringu je validni a z teto domeny.
    /// </returns>
    public static string GetSafeRedirectUri(Microsoft.AspNetCore.Http.HttpRequest request, Configuration.AppConfigurationSecurity configuration)
    {
        var redirectUri = request.Query[AuthenticationConstants.RedirectUriQueryParameter];
        if (!string.IsNullOrEmpty(redirectUri))
        {
            try
            {
                var safeUri = new Uri(redirectUri!);
                if (string.IsNullOrEmpty(safeUri.Host))
                {
                    return $"https://{request.Host}{redirectUri}";
                }
                else if (configuration.AllowAnyUrlInSigninRedirect) // pouze pro testovani
                {
                    return safeUri.ToString();
                }
                else if (safeUri.Authority == request.Host.Value && safeUri.Scheme == "https")
                {
                    return safeUri.ToString();
                }
            }
            catch
            {
                // spatne zadane URI v query stringu. Zalogovat?
            }
        }
        return $"https://{request.Host}{configuration.DefaultRedirectPathAfterSignIn}";
    }
}
