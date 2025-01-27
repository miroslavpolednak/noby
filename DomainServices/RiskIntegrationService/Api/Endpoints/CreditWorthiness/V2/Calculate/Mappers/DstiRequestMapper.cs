using _C4M = DomainServices.RiskIntegrationService.ExternalServices.RiskCharacteristics.V2.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate.Mappers;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.SelfService]
internal sealed class DstiRequestMapper
{
    public async Task<_C4M.DSTICalculationArguments> MapToC4m(_V2.CreditWorthinessCalculateRequest request, RiskApplicationTypesResponse.Types.RiskApplicationTypeItem riskApplicationType, CancellationToken cancellation)
    {
        // inicializovat ciselniky
        _obligationTypes = await _codebookService.ObligationTypes(cancellation);

        return new _C4M.DSTICalculationArguments
        {
            ResourceProcessId = _C4M.ResourceIdentifier.CreateResourceProcessId(request.ResourceProcessId ?? "").ToC4M(),
            ItChannel = FastEnum.Parse<_C4M.ItChannel>(_configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name)),
            LoanApplicationProduct = new()
            {
                ProductClusterCode = riskApplicationType.C4MAplCode,
                Annuity = request.Product!.LoanPaymentAmount.ToRiskCharacteristicsAmount(),
                AmountRequired = request.Product!.LoanAmount.ToRiskCharacteristicsAmount()
            },
            LoanApplicationHousehold = mapHouseholds(request.Households!, riskApplicationType.MandantId)
        };
    }

    private List<_C4M.DSTILoanApplicationHousehold> mapHouseholds(List<_V2.CreditWorthinessHousehold> households, int? mandantId)
        => households.Select(household =>
        {
            var liabilitiesFlatten = household.Customers!.Where(x => x.Obligations is not null).SelectMany(x => x.Obligations!).ToList();

            return new _C4M.DSTILoanApplicationHousehold
            {
                LoanApplicationCounterparty = mapCustomers(household.Customers!, mandantId),
                CreditLiabilitiesSummaryHomeCompany = createCreditLiabilitiesSummary(liabilitiesFlatten, false),
                CreditLiabilitiesSummaryOutHomeCompany = createCreditLiabilitiesSummary(liabilitiesFlatten, true),
                LoanInstallmentsSummaryHomeCompany = createInstallmentsSummary(liabilitiesFlatten, false),
                LoanInstallmentsSummaryOutHomeCompany = createInstallmentsSummary(liabilitiesFlatten, true)
            };
        })
        .ToList();

    private static List<_C4M.LoanApplicationCounterparty> mapCustomers(List<_V2.CreditWorthinessCustomer> customers, int? mandantId)
        => customers.Select(customer => new _C4M.LoanApplicationCounterparty
        {
            CustomerId = string.IsNullOrEmpty(customer.PrimaryCustomerId) ? null : _C4M.ResourceIdentifier.CreateCustomerId(customer.PrimaryCustomerId, !mandantId.HasValue || (SharedTypes.Enums.Mandants)mandantId == SharedTypes.Enums.Mandants.Kb ? "KBCZ" : "MPSS").ToC4M(),
            MonthlyEmploymentIncomeSumAmount = (customer.Incomes?.Where(t => t.IncomeTypeId == 1).Sum(t => t.Amount) ?? 0).ToRiskCharacteristicsAmount(),
            MonthlyRentIncomeSumAmount = (customer.Incomes?.Where(t => t.IncomeTypeId == 3).Sum(t => t.Amount) ?? 0).ToRiskCharacteristicsAmount(),
            EntrepreneurAnnualIncomeAmount = (customer.Incomes?.Where(t => t.IncomeTypeId == 2).Sum(t => t.Amount) ?? 0).ToRiskCharacteristicsAmount(),
            MonthlyOtherIncomeAmount = (customer.Incomes?.Where(t => t.IncomeTypeId == 4).Sum(t => t.Amount) ?? 0).ToRiskCharacteristicsAmount()
        })
        .ToList();

    #region liabilities
    private List<_C4M.DSTICreditLiabilitiesSummary> createCreditLiabilitiesSummary(List<_V2.CreditWorthinessObligation>? liabilitiesFlatten, bool isExternal)
       => new List<_C4M.DSTICreditLiabilitiesSummary>
       {
            new()
            {
                ProductClusterCode = _C4M.DSTICreditLiabilitiesSummaryType.AD,
                Amount = sumObligations(liabilitiesFlatten, "AD", isExternal, _fcSumObligationsAmount).ToRiskCharacteristicsAmount(),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "AD", isExternal, _fcSumObligationsAmountConsolidated).ToRiskCharacteristicsAmount()
            },
            new()
            {
                ProductClusterCode = _C4M.DSTICreditLiabilitiesSummaryType.CC,
                Amount = sumObligations(liabilitiesFlatten, "CC", isExternal, _fcSumObligationsAmount).ToRiskCharacteristicsAmount(),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "CC", isExternal, _fcSumObligationsAmountConsolidated).ToRiskCharacteristicsAmount()
            }
       };

    private List<_C4M.LoanInstallmentsSummary> createInstallmentsSummary(List<_V2.CreditWorthinessObligation>? liabilitiesFlatten, bool isExternal)
       => new List<_C4M.LoanInstallmentsSummary>
       {
            new()
            {
                ProductClusterCode = _C4M.LoanInstallmentsSummaryType.CL,
                Amount = sumObligations(liabilitiesFlatten, "CL", isExternal, _fcSumObligationsInstallment).ToRiskCharacteristicsAmount(),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "CL", isExternal, _fcSumObligationsInstallmentConsolidated).ToRiskCharacteristicsAmount()
            },
            new()
            {
                ProductClusterCode = _C4M.LoanInstallmentsSummaryType.ML,
                Amount = sumObligations(liabilitiesFlatten, "ML", isExternal, _fcSumObligationsInstallment).ToRiskCharacteristicsAmount(),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "ML", isExternal, _fcSumObligationsInstallmentConsolidated).ToRiskCharacteristicsAmount()
            }
       };
    #endregion liabilities

    private decimal sumObligations(List<_V2.CreditWorthinessObligation>? liabilitiesFlatten, string productGroup, bool isObligationCreditorExternal, Func<_V2.CreditWorthinessObligation, decimal> fcSum)
    {
        var arr = _obligationTypes!.Where(t => t.Code == productGroup).Select(t => t.Id).ToArray();
        return liabilitiesFlatten?
            .Where(t => t.IsObligationCreditorExternal == isObligationCreditorExternal && arr.Contains(t.ObligationTypeId))
            .Sum(fcSum) ?? 0;
    }

    Func<_V2.CreditWorthinessObligation, decimal> _fcSumObligationsAmount = t => t.Amount.GetValueOrDefault();
    Func<_V2.CreditWorthinessObligation, decimal> _fcSumObligationsAmountConsolidated = t => t.AmountConsolidated.GetValueOrDefault();
    Func<_V2.CreditWorthinessObligation, decimal> _fcSumObligationsInstallment = t => t.Installment.GetValueOrDefault();
    Func<_V2.CreditWorthinessObligation, decimal> _fcSumObligationsInstallmentConsolidated = t => t.InstallmentConsolidated.GetValueOrDefault();

    private List<ObligationTypesResponse.Types.ObligationTypeItem>? _obligationTypes;

    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;
    private readonly CodebookService.Clients.ICodebookServiceClient _codebookService;

    public DstiRequestMapper(
        CodebookService.Clients.ICodebookServiceClient codebookService,
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor)
    {
        _codebookService = codebookService;
        _configuration = configuration;
        _serviceUserAccessor = serviceUserAccessor;
    }
}
