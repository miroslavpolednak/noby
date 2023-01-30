using DomainServices.RiskIntegrationService.ExternalServices.RiskCharacteristics.V1.Contracts;
using _C4M = DomainServices.RiskIntegrationService.ExternalServices.RiskCharacteristics.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate.Mappers;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.SelfService]
internal sealed class DtiRequestMapper
{
    public async Task<_C4M.DTICalculationArguments> MapToC4m(_V2.CreditWorthinessCalculateRequest request, CodebookService.Contracts.Endpoints.RiskApplicationTypes.RiskApplicationTypeItem riskApplicationType, CancellationToken cancellation)
    {
        // inicializovat ciselniky
        _obligationTypes = await _codebookService.ObligationTypes(cancellation);

        return new _C4M.DTICalculationArguments
        {
            ResourceProcessId = new()
            {
                Instance = "MPSS",
                Domain = "OM",
                Resource = "OfferInstance",
                Id = request.ResourceProcessId
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
            LoanApplicationHousehold = mapHouseholds(request.Households!, riskApplicationType.MandantId)
        };
    }

    private List<_C4M.DTILoanApplicationHousehold> mapHouseholds(List<_V2.CreditWorthinessHousehold> households, int? mandantId)
        => households.Select(household =>
        {
            var liabilitiesFlatten = household.Customers!.Where(x => x.Obligations is not null).SelectMany(x => x.Obligations!).ToList();

            return new _C4M.DTILoanApplicationHousehold
            {
                LoanApplicationCounterparty = mapCustomers(household.Customers!, mandantId),
                CreditLiabilitiesSummaryHomeCompany = new List<_C4M.CreditLiabilitiesSummary>
                {
                    createObligation(_C4M.CreditLiabilitiesSummaryProductClusterCode.ML, liabilitiesFlatten, false),
                    createObligation(_C4M.CreditLiabilitiesSummaryProductClusterCode.AD, liabilitiesFlatten, false),
                    createObligation(_C4M.CreditLiabilitiesSummaryProductClusterCode.CC, liabilitiesFlatten, false),
                    createObligation(_C4M.CreditLiabilitiesSummaryProductClusterCode.CL, liabilitiesFlatten, false)
                },
                CreditLiabilitiesSummaryOutHomeCompany = new List<_C4M.CreditLiabilitiesSummary>
                {
                    createObligation(_C4M.CreditLiabilitiesSummaryProductClusterCode.ML, liabilitiesFlatten, true),
                    createObligation(_C4M.CreditLiabilitiesSummaryProductClusterCode.AD, liabilitiesFlatten, true),
                    createObligation(_C4M.CreditLiabilitiesSummaryProductClusterCode.CC, liabilitiesFlatten, true),
                    createObligation(_C4M.CreditLiabilitiesSummaryProductClusterCode.CL, liabilitiesFlatten, true)
                }
            };
        })
        .ToList();

    private _C4M.CreditLiabilitiesSummary createObligation(_C4M.CreditLiabilitiesSummaryProductClusterCode clusterCode, List<_V2.CreditWorthinessObligation>? obligations, bool isObligationCreditorExternal)
    {
        var arr = _obligationTypes!.Where(t => t.Code == clusterCode.FastToString()).Select(t => t.Id).ToArray();
        return new ()
        {
            ProductClusterCode = clusterCode,
            Amount = (obligations?.Where(t => t.IsObligationCreditorExternal == isObligationCreditorExternal && arr.Contains(t.ObligationTypeId)).Sum(t => t.Amount) ?? 0).ToAmount(),
            AmountConsolidated = (obligations?.Where(t => t.IsObligationCreditorExternal == isObligationCreditorExternal && arr.Contains(t.ObligationTypeId)).Sum(t => t.AmountConsolidated) ?? 0).ToAmount()
        };
    }
    
    private static List<_C4M.LoanApplicationCounterparty> mapCustomers(List<_V2.CreditWorthinessCustomer> customers, int? mandantId)
        => customers.Select(customer => new _C4M.LoanApplicationCounterparty
        {
            CustomerId = string.IsNullOrEmpty(customer.PrimaryCustomerId) ? null : new()
            {
                Id = customer.PrimaryCustomerId,
                Instance = !mandantId.HasValue || (CIS.Foms.Enums.Mandants)mandantId == CIS.Foms.Enums.Mandants.Mp ? "KBCZ" : "MPSS",
                Domain = "CM",
                Resource = "Customer"
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
    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;

    public DtiRequestMapper(
        CodebookService.Clients.ICodebookServiceClients codebookService,
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor)
    {
        _codebookService = codebookService;
        _configuration = configuration;
        _serviceUserAccessor = serviceUserAccessor;
    }
}
