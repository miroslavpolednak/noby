using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate.Mappers;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class CalculateRequestMapper
{
    public async Task<_C4M.CreditWorthinessCalculationArguments> MapToC4m(_V2.CreditWorthinessCalculateRequest request, CodebookService.Contracts.Endpoints.RiskApplicationTypes.RiskApplicationTypeItem riskApplicationType, CancellationToken cancellation)
    {
        var requestModel = new _C4M.CreditWorthinessCalculationArguments
        {
            ResourceProcessId = new()
            {
                Instance = "MPSS",
                Domain = "OM",
                Resource = "OfferInstance",
                Id = request.ResourceProcessId
            },
            ItChannel = FastEnum.Parse<_C4M.CreditWorthinessCalculationArgumentsItChannel>(_configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name)),
            //RiskBusinessCaseId = request.RiskBusinessCaseId!,//TODO ResourceIdentifier
            LoanApplicationProduct = new()
            {
                ProductClusterCode = riskApplicationType.C4mAplCode,
                AmountRequired = request.Product!.LoanAmount,
                Annuity = request.Product.LoanPaymentAmount,
                FixationPeriod = request.Product.FixedRatePeriod,
                InterestRate = request.Product.LoanInterestRate,
                Maturity = request.Product.LoanDuration
            },
            Households = await _householdMapper.MapHouseholds(request.Households!, riskApplicationType.MandantId, cancellation)
        };

        // human user instance
        if (request.UserIdentity is not null)
        {
            var userInstance = await _xxvConnectionProvider.GetC4mUserInfo(request.UserIdentity, cancellation);
            if (Helpers.IsDealerSchema(request.UserIdentity!.IdentityScheme))
                requestModel.LoanApplicationDealer = _C4M.C4mUserInfoDataExtensions.ToC4mDealer(userInstance, request.UserIdentity);
            else
                requestModel.KbGroupPerson = _C4M.C4mUserInfoDataExtensions.ToC4mPerson(userInstance, request.UserIdentity);
        }

        return requestModel;
    }

    private readonly HouseholdsChildMapper _householdMapper;
    private readonly CIS.Core.Data.IConnectionProvider<Data.IXxvDapperConnectionProvider> _xxvConnectionProvider;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;

    public CalculateRequestMapper(
        HouseholdsChildMapper householdMapper,
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        CIS.Core.Data.IConnectionProvider<Data.IXxvDapperConnectionProvider> xxvConnectionProvider)
    {
        _householdMapper = householdMapper;
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _xxvConnectionProvider = xxvConnectionProvider;
    }
}
