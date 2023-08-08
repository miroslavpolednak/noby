using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _C4M = DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3.Contracts;
using DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3.Contracts;
using DomainServices.CodebookService.Contracts.v1;
using CIS.Core.Exceptions;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate.Mappers;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.SelfService]
internal sealed class CalculateRequestMapper
{
    public async Task<_C4M.CreditWorthinessCalculationArguments> MapToC4m(_V2.CreditWorthinessCalculateRequest request, RiskApplicationTypesResponse.Types.RiskApplicationTypeItem riskApplicationType, CancellationToken cancellation)
    {
        var requestModel = new _C4M.CreditWorthinessCalculationArguments
        {
            ResourceProcessId = _C4M.ResourceIdentifier.CreateResourceProcessId(request.ResourceProcessId!).ToC4M(),
            ItChannel = FastEnum.Parse<_C4M.ItChannelType>(_configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name)),
            //RiskBusinessCaseId = request.RiskBusinessCaseId!,//TODO ResourceIdentifier
            LoanApplicationProduct = new()
            {
                ProductClusterCode = riskApplicationType.C4MAplCode,
                AmountRequired = request.Product!.LoanAmount.ToCreditWorthinessAmount(),
                Annuity = request.Product.LoanPaymentAmount,
                FixationPeriod = request.Product.FixedRatePeriod,
                InterestRate = request.Product.LoanInterestRate,
                Maturity = request.Product.LoanDuration
            },
            LoanApplicationHousehold = await _householdMapper.MapHouseholds(request.Households!, riskApplicationType.MandantId, cancellation)
        };

        // human user instance
        if (request.UserIdentity is not null)
        {
            try
            {
                var userInstance = await _userService.GetUserRIPAttributes(request.UserIdentity.IdentityId ?? "", request.UserIdentity.IdentityScheme ?? "", cancellation);
                if (userInstance != null)
                {
                    if (Helpers.IsDealerSchema(userInstance.DealerCompanyId))
                        requestModel.LoanApplicationDealer = userInstance.ToC4mDealer(request.UserIdentity);
                    else
                        requestModel.Person = userInstance.ToC4mPerson(request.UserIdentity);
                }
            }
            catch (CisNotFoundException) { }
            catch (Exception)
            {
                throw;
            }    
        }

        return requestModel;
    }

    private readonly HouseholdsChildMapper _householdMapper;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;
    private readonly UserService.Clients.IUserServiceClient _userService;

    public CalculateRequestMapper(
        HouseholdsChildMapper householdMapper,
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        UserService.Clients.IUserServiceClient userService)
    {
        _householdMapper = householdMapper;
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _userService = userService;
    }
}
