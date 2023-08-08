using CIS.Core.Security;
using DomainServices.CaseService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.UserService.Clients.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NOBY.Infrastructure.Security.Attributes;

namespace NOBY.Infrastructure.Security.Middleware;

public sealed class CaseOwnerValidationMiddleware
{
    const string _caseIdKey = "caseId";
    const string _salesArrangementIdKey = "salesArrangementId";
    const string _customerOnSAIdKey = "_customerOnSAId";
    const string _householdIdKey = "householdId";

    private readonly RequestDelegate _next;

    public CaseOwnerValidationMiddleware(RequestDelegate next) =>
        _next = next;

    public async Task Invoke(
        HttpContext context,
        ICurrentUserAccessor currentUser,
        ICaseServiceClient caseService,
        IHouseholdServiceClient householdService,
        ICustomerOnSAServiceClient customerOnSAService,
        ISalesArrangementServiceClient salesArrangementService)
    {
        var routeValues = context.GetRouteData().Values;
        var endpoint = context.GetEndpoint();
        var skipCheck = endpoint?.Metadata.OfType<NobySkipCaseOwnerValidationAttribute>().Any() ?? true;
        
        if (!skipCheck && (routeValues?.Any() ?? false))
        {
            var cancellationToken = context.RequestAborted;
            var preload = endpoint?.Metadata.OfType<NobyCaseOwnerValidationPreloadAttribute>().FirstOrDefault()?.Preload ?? NobyCaseOwnerValidationPreloadAttribute.LoadableEntities.None;
            long? caseId = null;

            if (isInRoute(_customerOnSAIdKey))
            {
                caseId = (await customerOnSAService.ValidateCustomerOnSAId(getId(_customerOnSAIdKey), true, cancellationToken)).CaseId;
            }
            else if (isInRoute(_householdIdKey))
            {
                caseId = (await householdService.ValidateHouseholdId(getId(_householdIdKey), true, cancellationToken)).CaseId;
            }
            else if (isInRoute(_salesArrangementIdKey))
            {
                caseId = preload.HasFlag(NobyCaseOwnerValidationPreloadAttribute.LoadableEntities.SalesArrangement) switch
                {
                    true => (await salesArrangementService.GetSalesArrangement(getId(_salesArrangementIdKey), cancellationToken)).CaseId,
                    false => (await salesArrangementService.ValidateSalesArrangementId(getId(_salesArrangementIdKey), true, cancellationToken)).CaseId,
                };
            }
            else if (isInRoute(_caseIdKey))
            {
                caseId = long.Parse(routeValues[_caseIdKey]!.ToString()!, CultureInfo.InvariantCulture);
            }

            if (caseId.HasValue)
            {
                int? ownerUserId = null;
                if (preload.HasFlag(NobyCaseOwnerValidationPreloadAttribute.LoadableEntities.Case))
                {
                    ownerUserId = (await caseService.GetCaseDetail(caseId.Value, cancellationToken)).CaseOwner.UserId;
                }
                else
                {
                    ownerUserId = (await caseService.ValidateCaseId(caseId.Value, true, cancellationToken)).OwnerUserId;
                }

                if (currentUser.User!.Id != ownerUserId && !currentUser.HasPermission(UserPermissions.DASHBOARD_AccessAllCases))
                {
                    throw new CisAuthorizationException();
                }
            }
        }

        await _next.Invoke(context!);

        bool isInRoute(in string key)
            => routeValues?.ContainsKey(key) ?? false;

        int getId(in string key)
            => int.Parse(routeValues![key]!.ToString()!, CultureInfo.InvariantCulture);
    }
}
