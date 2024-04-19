using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using NOBY.Infrastructure.ErrorHandling;
using NOBY.Infrastructure.Security.Attributes;

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method

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
        var skipValidateCaseStateAndProductSA = endpoint?.Metadata.OfType<NobySkipCaseStateAndProductSAValidationAttribute>().Any() ?? true;

        if (!skipCheck && (routeValues?.Any() ?? false))
        {
            var cancellationToken = context.RequestAborted;
            var preload = endpoint?.Metadata.OfType<NobyAuthorizePreloadAttribute>().FirstOrDefault()?.Preload ?? NobyAuthorizePreloadAttribute.LoadableEntities.None;
            long? caseId = null;
            int? salesArrangementTypeId = null;

            if (isInRoute(_customerOnSAIdKey))
            {
                var customerOnSAService = context.RequestServices.GetRequiredService<ICustomerOnSAServiceClient>()!;
                var customerResult = preload.HasFlag(NobyAuthorizePreloadAttribute.LoadableEntities.CustomerOnSA) switch
                {
                    true => await getCustomerFromDetail(),
                    false => await getCustomerFromValidate()
                };
                caseId = customerResult.CaseId;
                salesArrangementTypeId = (await getSalesArrangement(context, customerResult.SalesArrangementId, false, cancellationToken)).SalesArrangementTypeId;

                async Task<(long CaseId, int SalesArrangementId)> getCustomerFromDetail()
                {
                    var result = await customerOnSAService.GetCustomer(getId(_customerOnSAIdKey), cancellationToken);
                    return (result.CaseId, result.SalesArrangementId);
                }

                async Task<(long CaseId, int SalesArrangementId)> getCustomerFromValidate()
                {
                    var sa = await customerOnSAService.ValidateCustomerOnSAId(getId(_customerOnSAIdKey), true, cancellationToken);
                    return (sa.CaseId!.Value, sa.SalesArrangementId!.Value);
                }
            }
            else if (isInRoute(_householdIdKey))
            {
                var householdService = context.RequestServices.GetRequiredService<IHouseholdServiceClient>()!;
                var householdResult = preload.HasFlag(NobyAuthorizePreloadAttribute.LoadableEntities.Household) switch
                {
                    true => await getHouseholdFromDetail(),
                    false => await getHouseholdFromValidate()
                };
                caseId = householdResult.CaseId;
                salesArrangementTypeId = (await getSalesArrangement(context, householdResult.SalesArrangementId, false, cancellationToken)).SalesArrangementTypeId;

                async Task<(long CaseId, int SalesArrangementId)> getHouseholdFromDetail()
                {
                    var result = await householdService.GetHousehold(getId(_householdIdKey), cancellationToken);
                    return (result.CaseId, result.SalesArrangementId);
                }

                async Task<(long CaseId, int SalesArrangementId)> getHouseholdFromValidate()
                {
                    var sa = await householdService.ValidateHouseholdId(getId(_householdIdKey), true, cancellationToken);
                    return (sa.CaseId!.Value, sa.SalesArrangementId!.Value);
                }
            }
            // u SA potrebujeme take typ SA
            else if (isInRoute(_salesArrangementIdKey))
            {
                var saResponse = await getSalesArrangement(context, getId(_salesArrangementIdKey), preload.HasFlag(NobyAuthorizePreloadAttribute.LoadableEntities.SalesArrangement), cancellationToken);
                caseId = saResponse.CaseId;
                salesArrangementTypeId = saResponse.SalesArrangementTypeId;
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

                SecurityHelpers.CheckCaseOwnerAndState(currentUser, caseInstance.OwnerUserId, caseInstance.CaseState, !skipValidateCaseStateAndProductSA, salesArrangementTypeId);

                // pokud endpoint vyzaduje specificky stav Case
                var requiredCaseStates = endpoint?.Metadata.OfType<NobyRequiredCaseStatesAttribute>().FirstOrDefault();
                if ((requiredCaseStates?.CaseStates.Length ?? 0) > 0 && !requiredCaseStates!.CaseStates.Contains((CaseStates)caseInstance.CaseState))
                {
                    throw new NobyValidationException(90032, $"Case is in forbidden State: {caseInstance.CaseState}; required states: {string.Join(",", requiredCaseStates.CaseStates)}");
                }
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

    private static async Task<(long CaseId, int SalesArrangementTypeId)> getSalesArrangement(HttpContext context, int salesArrangementId, bool fromDetail, CancellationToken cancellationToken)
    {
        var salesArrangementService = context.RequestServices.GetRequiredService<ISalesArrangementServiceClient>();
        return fromDetail switch
        {
            true => await getSAFromDetail(),
            false => await getSAFromValidate(),
        };
        async Task<(long CaseId, int SalesArrangementTypeId)> getSAFromDetail()
        {
            var sa = await salesArrangementService!.GetSalesArrangement(salesArrangementId, cancellationToken);
            return (sa.CaseId, sa.SalesArrangementTypeId);
        }

        async Task<(long CaseId, int SalesArrangementTypeId)> getSAFromValidate()
        {
            var sa = await salesArrangementService.ValidateSalesArrangementId(salesArrangementId, true, cancellationToken);
            return (sa.CaseId!.Value, sa.SalesArrangementTypeId!.Value);
        }
    }
}
