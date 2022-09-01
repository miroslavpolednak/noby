using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate.Mappers;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class CalculateRequestMapper
{
    public async Task<_C4M.CreditWorthinessCalculationArguments> MapToC4m(_V2.CreditWorthinessCalculateRequest request, CancellationToken cancellation)
    {
        // appl type pro aktualni produkt
        var riskApplicationType = await getRiskApplicationType(request.Product!.ProductTypeId, cancellation);
        
        var requestModel = new _C4M.CreditWorthinessCalculationArguments
        {
            ResourceProcessId = _C4M.ResourceIdentifier.CreateResourceProcessId(request.ResourceProcessId),
            ItChannel = FastEnum.Parse<_C4M.CreditWorthinessCalculationArgumentsItChannel>(_configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name)),
            //RiskBusinessCaseId = request.RiskBusinessCaseId!,//TODO ResourceIdentifier
            LoanApplicationProduct = new()
            {
                ProductClusterCode = riskApplicationType.C4mAplCode,
                AmountRequired = request.Product.LoanAmount,
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

    private async Task<CodebookService.Contracts.Endpoints.RiskApplicationTypes.RiskApplicationTypeItem> getRiskApplicationType(int productTypeId, CancellationToken cancellationToken)
        => (await _codebookService.RiskApplicationTypes(cancellationToken))
            .FirstOrDefault(t => t.ProductTypeId is not null && t.ProductTypeId.Contains(productTypeId))
        ?? throw new CisValidationException(0, $"ProductTypeId={productTypeId} is missing in RiskApplicationTypes codebook");

    private readonly HouseholdsChildMapper _householdMapper;
    private readonly CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> _xxvConnectionProvider;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;

    public CalculateRequestMapper(
        HouseholdsChildMapper householdMapper,
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> xxvConnectionProvider,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        _householdMapper = householdMapper;
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _codebookService = codebookService;
        _xxvConnectionProvider = xxvConnectionProvider;
    }
}
