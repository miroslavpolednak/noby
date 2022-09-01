using _C4M = DomainServices.RiskIntegrationService.Api.Clients.RiskCharakteristics.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate.Mappers;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class DtiRequestMapper
{
    public async Task<_C4M.DTICalculationArguments> MapToC4m(_V2.CreditWorthinessCalculateRequest request, CancellationToken cancellation)
    {
        return new _C4M.DTICalculationArguments
        {
            ResourceProcessId = new()
            {
            },
            ItChannel = FastEnum.Parse<_C4M.DTICalculationArgumentsItChannel>(_configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name)),
            LoanApplicationProduct = new()
            {
                AmountRequired = new()
                {
                    CurrencyCode = GlobalConstants.CurrencyCode,
                    Value = request.Product!.LoanAmount
                }
            },
            LoanApplicationHousehold = new List<_C4M.DTILoanApplicationHousehold>
            {
                new _C4M.DTILoanApplicationHousehold
                {
                    CreditLiabilitiesSummaryHomeCompany = null,
                    CreditLiabilitiesSummaryOutHomeCompany = null
                }
            }
        };
    }

    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;

    public DtiRequestMapper(
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor)
    {
        _configuration = configuration;
        _serviceUserAccessor = serviceUserAccessor;
    }
}
