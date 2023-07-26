using CIS.Core.Security;
using DomainServices.CaseService.Clients;
using DomainServices.UserService.Clients.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NOBY.Infrastructure.Security.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public sealed class AuthorizeCaseOwnerAttribute
    : TypeFilterAttribute
{
    public AuthorizeCaseOwnerAttribute()
        : base(typeof(CaseOwnerAuthorizeFilter))
    {
    }

    private sealed class CaseOwnerAuthorizeFilter
        : IAsyncAuthorizationFilter
    {
        const string _caseIdKey = "caseId";

        private readonly ICurrentUserAccessor _currentUser;
        private readonly ICaseServiceClient _caseService;

        public CaseOwnerAuthorizeFilter(ICurrentUserAccessor currentUser, ICaseServiceClient caseService)
        {
            _caseService = caseService;
            _currentUser = currentUser;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.RouteValues.ContainsKey(_caseIdKey))
            {
                throw new ArgumentNullException(nameof(context.HttpContext.Request.RouteValues), $"{_caseIdKey} is missing in route values");
            }

            long caseId = long.Parse(context.HttpContext.Request.RouteValues[_caseIdKey]!.ToString()!, CultureInfo.InvariantCulture);

            var ownerUserId = (await _caseService.ValidateCaseId(caseId, true)).OwnerUserId;
            
            if (_currentUser.User!.Id != ownerUserId && !_currentUser.HasPermission(UserPermissions.DASHBOARD_AccessAllCases))
            {
                throw new CisAuthorizationException();
            }
        }
    }
}



