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
        var endpoint = context.GetEndpoint();
        var skipCheck = endpoint?.Metadata.OfType<NobySkipCaseOwnerValidationAttribute>().Any() ?? true;
        
        if (!skipCheck)
        {
            var cancellationToken = context.RequestAborted;
            var routeValues = context.GetRouteData().Values;
            var preload = endpoint?.Metadata.OfType<NobyCaseOwnerValidationPreloadAttribute>().FirstOrDefault()?.Preload ?? NobyCaseOwnerValidationPreloadAttribute.LoadableEntities.None;
            long? caseId = null;

            if (routeValues.ContainsKey(_customerOnSAIdKey))
            {
                int customerOnSAId = int.Parse(routeValues[_customerOnSAIdKey]!.ToString()!, CultureInfo.InvariantCulture);
                caseId = (await customerOnSAService.ValidateCustomerOnSAId(customerOnSAId, true, cancellationToken)).CaseId;
            }
            else if (routeValues.ContainsKey(_householdIdKey))
            {
                int householdId = int.Parse(routeValues[_householdIdKey]!.ToString()!, CultureInfo.InvariantCulture);
                caseId = (await householdService.ValidateHouseholdId(householdId, true, cancellationToken)).CaseId;
            }
            else if (routeValues.ContainsKey(_salesArrangementIdKey))
            {
                int salesArrangementId = int.Parse(routeValues[_salesArrangementIdKey]!.ToString()!, CultureInfo.InvariantCulture);
                caseId = (await salesArrangementService.ValidateSalesArrangementId(salesArrangementId, true, cancellationToken)).CaseId;
            }
            else if (routeValues.ContainsKey(_caseIdKey))
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
    }
}
