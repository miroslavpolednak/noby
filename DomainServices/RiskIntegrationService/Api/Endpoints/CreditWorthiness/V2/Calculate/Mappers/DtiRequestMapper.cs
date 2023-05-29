using DomainServices.RiskIntegrationService.ExternalServices.RiskCharacteristics.V2.Contracts;
using _C4M = DomainServices.RiskIntegrationService.ExternalServices.RiskCharacteristics.V2.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate.Mappers;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.SelfService]
internal sealed class DtiRequestMapper
{
    public async Task<_C4M.DTICalculationArguments> MapToC4m(_V2.CreditWorthinessCalculateRequest request, RiskApplicationTypesResponse.Types.RiskApplicationTypeItem riskApplicationType, CancellationToken cancellation)
    {
        // inicializovat ciselniky
        _obligationTypes = await _codebookService.ObligationTypes(cancellation);

        return new _C4M.DTICalculationArguments
        {
            ResourceProcessId = _C4M.ResourceIdentifier.CreateResourceProcessId(request.ResourceProcessId ?? "").ToC4M(),
            ItChannel = FastEnum.Parse<_C4M.ItChannel>(_configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name)),
            LoanApplicationProduct = new()
            {
                AmountRequired = request.Product!.LoanAmount.ToAmount()
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
                    createObligation(_C4M.CreditLiabilitiesSummaryType.ML, liabilitiesFlatten, false),
                    createObligation(_C4M.CreditLiabilitiesSummaryType.AD, liabilitiesFlatten, false),
                    createObligation(_C4M.CreditLiabilitiesSummaryType.CC, liabilitiesFlatten, false),
                    createObligation(_C4M.CreditLiabilitiesSummaryType.CL, liabilitiesFlatten, false)
                },
                CreditLiabilitiesSummaryOutHomeCompany = new List<_C4M.CreditLiabilitiesSummary>
                {
                    createObligation(_C4M.CreditLiabilitiesSummaryType.ML, liabilitiesFlatten, true),
                    createObligation(_C4M.CreditLiabilitiesSummaryType.AD, liabilitiesFlatten, true),
                    createObligation(_C4M.CreditLiabilitiesSummaryType.CC, liabilitiesFlatten, true),
                    createObligation(_C4M.CreditLiabilitiesSummaryType.CL, liabilitiesFlatten, true)
                }
            };
        })
        .ToList();

    private _C4M.CreditLiabilitiesSummary createObligation(_C4M.CreditLiabilitiesSummaryType clusterCode, List<_V2.CreditWorthinessObligation>? obligations, bool isObligationCreditorExternal)
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
            CustomerId = string.IsNullOrEmpty(customer.PrimaryCustomerId) ? null : _C4M.ResourceIdentifier.CreateCustomerId(customer.PrimaryCustomerId, !mandantId.HasValue || (CIS.Foms.Enums.Mandants)mandantId == CIS.Foms.Enums.Mandants.Mp ? "KBCZ" : "MPSS").ToC4M(),
            MonthlyEmploymentIncomeSumAmount = (customer.Incomes?.Where(t => t.IncomeTypeId == 1).Sum(t => t.Amount) ?? 0).ToAmount(),
            MonthlyRentIncomeSumAmount = (customer.Incomes?.Where(t => t.IncomeTypeId == 3).Sum(t => t.Amount) ?? 0).ToAmount(),
            EntrepreneurAnnualIncomeAmount = (customer.Incomes?.Where(t => t.IncomeTypeId == 2).Sum(t => t.Amount) ?? 0).ToAmount(),
            MonthlyOtherIncomeAmount = (customer.Incomes?.Where(t => t.IncomeTypeId == 4).Sum(t => t.Amount) ?? 0).ToAmount()
        })
        .ToList();

    private List<ObligationTypesResponse.Types.ObligationTypeItem>? _obligationTypes;

    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;
    private readonly CodebookService.Clients.ICodebookServiceClient _codebookService;

    public DtiRequestMapper(
        CodebookService.Clients.ICodebookServiceClient codebookService,
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor)
    {
        _codebookService = codebookService;
        _configuration = configuration;
        _serviceUserAccessor = serviceUserAccessor;
    }
}
