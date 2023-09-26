using CIS.Core.Security;
using SharedTypes.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.UserService.Clients.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using NOBY.Infrastructure.Security.Attributes;

namespace NOBY.Infrastructure.Security.Middleware;

/// <summary>
/// Middleware pro kontrolu vlastníka Case.
/// </summary>
public sealed class CaseOwnerValidationMiddleware
{
    // klíče pod kterými jsou v route daná ID entit
    const string _caseIdKey = "caseId";
    const string _salesArrangementIdKey = "salesArrangementId";
    const string _customerOnSAIdKey = "customerOnSAId";
    const string _householdIdKey = "householdId";

    private readonly RequestDelegate _next;

    public CaseOwnerValidationMiddleware(RequestDelegate next) =>
        _next = next;

    public async Task Invoke(
        HttpContext context,
        ICurrentUserAccessor currentUser,
        ICaseServiceClient caseService)
    {
        var routeValues = context.GetRouteData().Values;
        var endpoint = context.GetEndpoint();
        var skipCheck = endpoint?.Metadata.OfType<NobySkipCaseOwnerValidationAttribute>().Any() ?? true;
        
        if (!skipCheck && (routeValues?.Any() ?? false))
        {
            var cancellationToken = context.RequestAborted;
            var preload = endpoint?.Metadata.OfType<NobyAuthorizePreloadAttribute>().FirstOrDefault()?.Preload ?? NobyAuthorizePreloadAttribute.LoadableEntities.None;
            long? caseId = null;

            if (isInRoute(_customerOnSAIdKey))
            {
                var customerOnSAService = context.RequestServices.GetRequiredService<ICustomerOnSAServiceClient>();
                caseId = preload.HasFlag(NobyAuthorizePreloadAttribute.LoadableEntities.CustomerOnSA) switch
                {
                    true => (await customerOnSAService.GetCustomer(getId(_customerOnSAIdKey), cancellationToken)).CaseId,
                    false => (await customerOnSAService.ValidateCustomerOnSAId(getId(_customerOnSAIdKey), true, cancellationToken)).CaseId
                };  
            }
            else if (isInRoute(_householdIdKey))
            {
                var householdService = context.RequestServices.GetRequiredService<IHouseholdServiceClient>();
                caseId = preload.HasFlag(NobyAuthorizePreloadAttribute.LoadableEntities.Household) switch
                {
                    true => (await householdService.GetHousehold(getId(_householdIdKey), cancellationToken)).CaseId,
                    false => (await householdService.ValidateHouseholdId(getId(_householdIdKey), true, cancellationToken)).CaseId
                };
            }
            else if (isInRoute(_salesArrangementIdKey))
            {
                var salesArrangementService = context.RequestServices.GetRequiredService<ISalesArrangementServiceClient>();
                caseId = preload.HasFlag(NobyAuthorizePreloadAttribute.LoadableEntities.SalesArrangement) switch
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
                var caseInstance = preload.HasFlag(NobyAuthorizePreloadAttribute.LoadableEntities.Case) switch
                {
                    true => await getCaseDataFromDetail(),
                    false => await getCaseDataFromValidate()
                };

                SecurityHelpers.CheckCaseOwnerAndState(currentUser, caseInstance.OwnerUserId, caseInstance.CaseState);
            }

            async Task<(int OwnerUserId, int CaseState)> getCaseDataFromDetail()
            {
                var instance = await caseService.GetCaseDetail(caseId!.Value, cancellationToken);
                return (instance.CaseOwner.UserId, instance.State);
            }

            async Task<(int OwnerUserId, int CaseState)> getCaseDataFromValidate()
            {
                var instance = await caseService.ValidateCaseId(caseId!.Value, true, cancellationToken);
                return (instance.OwnerUserId!.Value, instance.State!.Value);
            }
        }

        await _next.Invoke(context!);

        bool isInRoute(in string key)
            => routeValues?.ContainsKey(key) ?? false;

        int getId(in string key)
            => int.Parse(routeValues![key]!.ToString()!, CultureInfo.InvariantCulture);
    }
}
