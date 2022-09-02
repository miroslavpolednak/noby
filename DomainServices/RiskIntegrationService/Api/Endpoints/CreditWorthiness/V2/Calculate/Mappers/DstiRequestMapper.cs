using DomainServices.RiskIntegrationService.Api.Clients.RiskCharakteristics.V1.Contracts;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.RiskCharakteristics.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate.Mappers;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class DstiRequestMapper
{
    public async Task<_C4M.DSTICalculationArguments> MapToC4m(_V2.CreditWorthinessCalculateRequest request, CodebookService.Contracts.Endpoints.RiskApplicationTypes.RiskApplicationTypeItem riskApplicationType, CancellationToken cancellation)
    {
        // inicializovat ciselniky
        _obligationTypes = await _codebookService.ObligationTypes(cancellation);

        return new _C4M.DSTICalculationArguments
        {
            ResourceProcessId = new()
            {
                Instance = "MPSS",
                Domain = "OM",
                Resource = "OfferInstance",
                Id = request.ResourceProcessId
            },
            ItChannel = FastEnum.Parse<_C4M.DSTICalculationArgumentsItChannel>(_configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name)),
            LoanApplicationProduct = new()
            {
                ProductClusterCode = riskApplicationType.C4mAplCode,
                Annuity = request.Product!.LoanPaymentAmount.ToAmount(),
                AmountRequired = new()
                {
                    CurrencyCode = GlobalConstants.CurrencyCode,
                    Value = request.Product!.LoanAmount
                }
            },
            LoanApplicationHousehold = mapHouseholds(request.Households!, riskApplicationType.MandantId)
        };
    }

    private List<_C4M.DSTILoanApplicationHousehold> mapHouseholds(List<_V2.CreditWorthinessHousehold> households, int mandantId)
        => households.Select(household =>
        {
            var liabilitiesFlatten = household.Customers!.Where(x => x.Obligations is not null).SelectMany(x => x.Obligations!).ToList();

            return new _C4M.DSTILoanApplicationHousehold
            {
                LoanApplicationCounterparty = mapCustomers(household.Customers!, mandantId)
            };
        })
        .ToList();

    private static List<_C4M.LoanApplicationCounterparty> mapCustomers(List<_V2.CreditWorthinessCustomer> customers, int mandantId)
        => customers.Select(customer => new _C4M.LoanApplicationCounterparty
        {
            CustomerId = new()
            {
                Id = customer.InternalCustomerId,
                Instance = (CIS.Foms.Enums.Mandants)mandantId == CIS.Foms.Enums.Mandants.Mp ? "MPSS" : "KBCZ",
            },
            MonthlyEmploymentIncomeSumAmount = (customer.Incomes?.Where(t => t.IncomeTypeId == 1).Sum(t => t.Amount) ?? 0).ToAmount(),
            MonthlyRentIncomeSumAmount = (customer.Incomes?.Where(t => t.IncomeTypeId == 3).Sum(t => t.Amount) ?? 0).ToAmount(),
            EntrepreneurAnnualIncomeAmount = (customer.Incomes?.Where(t => t.IncomeTypeId == 2).Sum(t => t.Amount) ?? 0).ToAmount(),
            MonthlyOtherIncomeAmount = (customer.Incomes?.Where(t => t.IncomeTypeId == 4).Sum(t => t.Amount) ?? 0).ToAmount()
        })
        .ToList();

    private List<CodebookService.Contracts.Endpoints.ObligationTypes.ObligationTypesItem>? _obligationTypes;

    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public DstiRequestMapper(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor)
    {
        _codebookService = codebookService;
        _configuration = configuration;
        _serviceUserAccessor = serviceUserAccessor;
    }
}
