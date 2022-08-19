using DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using _RAT = DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;
using CIS.Core;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save.Mappers;

internal sealed class HouseholdChildMapper
{
    public async Task<List<_C4M.LoanApplicationHousehold>> MapHouseholds(List<_V2.LoanApplicationHousehold> households, bool verification)
    {
        var householdTypes = await _codebookService.HouseholdTypes(_cancellationToken);
        var propSettlements = await _codebookService.PropertySettlements(_cancellationToken);

        return (await households.SelectAsync(async household =>
        {
            var obligations = household?.Customers?.Where(x => x.Obligations is not null).SelectMany(x => x.Obligations!);

            return new _C4M.LoanApplicationHousehold
            {
                Id = household.HouseholdId,
                RoleCode = householdTypes.FirstOrDefault(t => t.Id == household.HouseholdTypeId)?.RdmCode,
                ChildrenUnderAnd10 = household.ChildrenUpToTenYearsCount,
                ChildrenOver10 = household.ChildrenOverTenYearsCount,
                HouseholdExpensesSummary = mapExpenses(household.Expenses),
                SettlementTypeCode = propSettlements.FirstOrDefault(t => t.Id == household.PropertySettlementId)?.Code,
                CounterParty = household.Customers is null ? null : await _customerMapper.MapCustomers(household.Customers!, verification),
                HouseholdCreditLiabilitiesSummaryOutHomeCompany = await mapObligations(obligations),
                HouseholdInstallmentsSummaryOutHomeCompany = await mapInstallments(obligations)
            };
        }))
        .ToList();
    }

    private async Task<List<_C4M.CreditLiabilitiesSummary>> mapObligations(IEnumerable<_V2.LoanApplicationObligation>? obligations)
    {
        // vytvorit prazdnou kolekci
        var list = FastEnum
            .GetValues<CreditLiabilitiesSummaryProductClusterCode>()
            .Select(t => new CreditLiabilitiesSummary
            {
                ProductClusterCode = t
            })
            .ToList();

        if (obligations is not null)
        {
            var types = await _codebookService.ObligationTypes(_cancellationToken);
            obligations.GroupBy(t =>
                {
                    var typeCode = types.FirstOrDefault(x => x.Id == t.ObligationTypeId)?.Code ?? throw new CisValidationException($"ObligationTypeId={t.ObligationTypeId} does not exist");
                    return FastEnum.Parse<CreditLiabilitiesSummaryProductClusterCode>(typeCode);
                })
                .ToList()
                .ForEach(g =>
                {
                    var o = list.First(t => t.ProductClusterCode == g.Key);
                    o.Amount = new Amount() { Value = g.Sum(t => t.Amount.GetValueOrDefault()), CurrencyCode = "CZK" };
                    o.AmountConsolidated = new Amount() { Value = g.Sum(t => t.AmountConsolidated.GetValueOrDefault()), CurrencyCode = "CZK" };
                });
        }

        return list;
    }

    private async Task<List<_C4M.LoanInstallmentsSummary>> mapInstallments(IEnumerable<_V2.LoanApplicationObligation>? obligations)
    {
        // vytvorit prazdnou kolekci
        var list = FastEnum
            .GetValues<LoanInstallmentsSummaryProductClusterCode>()
            .Select(t => new LoanInstallmentsSummary
            {
                ProductClusterCode = t
            })
            .ToList();

        if (obligations is not null)
        {
            var types = await _codebookService.ObligationTypes(_cancellationToken);
            obligations.GroupBy(t =>
            {
                var typeCode = types.FirstOrDefault(x => x.Id == t.ObligationTypeId)?.Code ?? throw new CisValidationException($"ObligationTypeId={t.ObligationTypeId} does not exist");
                return FastEnum.Parse<LoanInstallmentsSummaryProductClusterCode>(typeCode);
            })
                .ToList()
                .ForEach(g =>
                {
                    var o = list.First(t => t.ProductClusterCode == g.Key);
                    o.Amount = new Amount() { Value = g.Sum(t => t.Installment.GetValueOrDefault()), CurrencyCode = "CZK" };
                    o.AmountConsolidated = new Amount() { Value = g.Sum(t => t.InstallmentConsolidated.GetValueOrDefault()), CurrencyCode = "CZK" };
                });
        }

        return list;
    }

    /// <summary>
    /// Namapovat vydaje
    /// </summary>
    static List<_C4M.ExpensesSummary> mapExpenses(Contracts.Shared.V1.ExpensesSummary? expenses)
        => new()
        {
            new() { Amount = (expenses?.Rent ?? 0M).ToAmount(), Category = ExpensesSummaryCategory.RENT },
            new() { Amount = (expenses?.Saving ?? 0).ToAmount(), Category = ExpensesSummaryCategory.SAVINGS },
            new() { Amount = (expenses?.Insurance ?? 0).ToAmount(), Category = ExpensesSummaryCategory.INSURANCE },
            new() { Amount = (expenses?.Other ?? 0).ToAmount(), Category = ExpensesSummaryCategory.OTHER }
        };

    private readonly HouseholdCustomerChildMapper _customerMapper;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly CancellationToken _cancellationToken;
    private readonly _RAT.RiskApplicationTypeItem _riskApplicationType;

    public HouseholdChildMapper(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        _RAT.RiskApplicationTypeItem riskApplicationType,
        CancellationToken cancellationToken)
    {
        _riskApplicationType = riskApplicationType;
        _cancellationToken = cancellationToken;
        _codebookService = codebookService;
        _customerMapper = new HouseholdCustomerChildMapper(codebookService, riskApplicationType, cancellationToken);
    }
}
