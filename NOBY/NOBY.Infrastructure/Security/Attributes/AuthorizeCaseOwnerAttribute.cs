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
    public bool FromCaseDetail { get; init; }

    /// <param name="fromCaseDetail">Pokud bude nastaveno na true, vola se misto ValidateCaseId -> GetCaseDetail. Pouzije se v pripade, ze handler stejne vola GetCaseDetail a v tu chvili se pouzije jiz nakesovana instance Case.</param>
    public AuthorizeCaseOwnerAttribute(bool fromCaseDetail = false)
        : base(typeof(CaseOwnerAuthorizeFilter))
    {
        FromCaseDetail = fromCaseDetail;
        Arguments = new object[] { FromCaseDetail };
    }

    private sealed class CaseOwnerAuthorizeFilter
        : IAsyncAuthorizationFilter
    {
        const string _caseIdKey = "caseId";

        private readonly bool _fromCaseDetail;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly ICaseServiceClient _caseService;

        public CaseOwnerAuthorizeFilter(bool fromCaseDetail, ICurrentUserAccessor currentUser, ICaseServiceClient caseService)
        {
            _fromCaseDetail = fromCaseDetail;
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

            int? ownerUserId = null;
            if (_fromCaseDetail)
            {
                ownerUserId = (await _caseService.GetCaseDetail(caseId)).CaseOwner.UserId;
            }
            else
            {
                ownerUserId = (await _caseService.ValidateCaseId(caseId, true)).OwnerUserId;
            }
            
            if (_currentUser.User!.Id != ownerUserId && !_currentUser.HasPermission(UserPermissions.DASHBOARD_AccessAllCases))
            {
                throw new CisAuthorizationException();
            }
        }
    }
}



