using DomainServices.RealEstateValuationService.Clients;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;

namespace NOBY.Api.Endpoints.RealEstateValuation;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
internal sealed class RealEstateValuationStateValidationAttribute
    : TypeFilterAttribute
{
    public int[] ValuationStateId { get; init; }

    public RealEstateValuationStateValidationAttribute(params int[] valuationStateId)
        : base(typeof(RealEstateValuationStateValidationFilter))
    {
        ValuationStateId = valuationStateId.Length == 0 ? new[] { 7 } : valuationStateId;
        Arguments = new object[] { ValuationStateId };
    }

    private sealed class RealEstateValuationStateValidationFilter
        : IAsyncAuthorizationFilter
    {
        const string _caseIdKey = "caseId";
        const string _realEstateValuationIdKey = "realEstateValuationId";
        private readonly int[] _valuationStateId;

        private readonly IRealEstateValuationServiceClient _realEstateValuationService;

        public RealEstateValuationStateValidationFilter(IRealEstateValuationServiceClient realEstateValuationService, int[] valuationStateId)
        {
            _valuationStateId = valuationStateId;
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

            //TODO udedlat endpoint, ktery vraci jen zakladni udaje?
            var instance = await _realEstateValuationService.ValidateRealEstateValuationId(realEstateValuationId, true);

            // podvrhnute caseId
            if (instance.CaseId != caseId)
            {
                throw new CisAuthorizationException();
            }

            // spatny stav REV
            if (!_valuationStateId.Contains(instance.ValuationStateId.GetValueOrDefault()))
            {
                throw new CisAuthorizationException();
            }
        }
    }
}
