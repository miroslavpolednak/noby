using DomainServices.RealEstateValuationService.Clients;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;

namespace NOBY.Api.Endpoints.RealEstateValuation;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
internal sealed class RealEstateValuationStateValidationAttribute
    : TypeFilterAttribute
{
    public RealEstateValuationStateValidationAttribute()
        : base(typeof(RealEstateValuationStateValidationFilter))
    {
    }

    private sealed class RealEstateValuationStateValidationFilter
        : IAsyncAuthorizationFilter
    {
        const string _caseIdKey = "caseId";
        const string _realEstateValuationIdKey = "realEstateValuationId";

        private readonly IRealEstateValuationServiceClient _realEstateValuationService;

        public RealEstateValuationStateValidationFilter(IRealEstateValuationServiceClient realEstateValuationService)
        {
            _realEstateValuationService = realEstateValuationService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.RouteValues.ContainsKey(_caseIdKey))
            {
                throw new ArgumentNullException(nameof(context.HttpContext.Request.RouteValues), $"{_caseIdKey} is missing in route values");
            }

            if (!context.HttpContext.Request.RouteValues.ContainsKey(_realEstateValuationIdKey))
            {
                throw new ArgumentNullException(nameof(context.HttpContext.Request.RouteValues), $"{_realEstateValuationIdKey} is missing in route values");
            }

            long caseId = long.Parse(context.HttpContext.Request.RouteValues[_caseIdKey]!.ToString()!, CultureInfo.InvariantCulture);
            int realEstateValuationId = int.Parse(context.HttpContext.Request.RouteValues[_realEstateValuationIdKey]!.ToString()!, CultureInfo.InvariantCulture);

            //TODO udedlat endpoint, ktery vraci jen zakladni udaje
            var instance = await _realEstateValuationService.ValidateRealEstateValuationId(realEstateValuationId, true);

            // podvrhnute caseId
            if (instance.CaseId != caseId)
            {
                throw new CisAuthorizationException();
            }

            // spatny stav REV
            if (instance.ValuationStateId != 7)
            {
                throw new CisAuthorizationException();
            }
        }
    }
}
