using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _C4M = DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3.Contracts;
using CIS.Core;
using DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate.Mappers;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.SelfService]
internal sealed class HouseholdsChildMapper
{
    public async Task<List<_C4M.LoanApplicationHousehold>> MapHouseholds(
        List<_V2.CreditWorthinessHousehold> households, 
        int? mandantId,
        CancellationToken cancellation)
    {
        // inicializovat ciselniky
        _obligationTypes = await _codebookService.ObligationTypes(cancellation);
        
        return (await households.SelectAsync(async h =>
        {
            var liabilitiesFlatten = h.Customers!.Where(x => x.Obligations is not null).SelectMany(x => x.Obligations!).ToList();

            return new _C4M.LoanApplicationHousehold
            {
                ChildrenOver10 = h.ChildrenOverTenYearsCount,
                ChildrenUnderAnd10 = h.ChildrenUpToTenYearsCount,
                ExpensesSummary = toC4m(h.ExpensesSummary ?? new Contracts.Shared.V1.ExpensesSummary()),
                Clients = await _customersMapper.MapCustomers(h.Customers!, mandantId, cancellation),
                CreditLiabilitiesSummaryHomeCompany = createCreditLiabilitiesSummary(liabilitiesFlatten),
                CreditLiabilitiesSummaryOutHomeCompany = createCreditLiabilitiesSummaryOut(liabilitiesFlatten),
                InstallmentsSummaryHomeCompany = createInstallmentsSummary(liabilitiesFlatten),
                InstallmentsSummaryOutHomeCompany = createInstallmentsSummaryOut(liabilitiesFlatten)
            };
        })).ToList();   
    }

    private static List<_C4M.ExpensesSummary> toC4m(Contracts.Shared.V1.ExpensesSummary expenses)
        => new List<_C4M.ExpensesSummary>()
        {
            new() { Amount = expenses.Rent.GetValueOrDefault().ToAmount(), Category = _C4M.HouseholdExpenseType.RENT },
            new() { Amount = expenses.Saving.GetValueOrDefault().ToAmount(), Category = _C4M.HouseholdExpenseType.SAVINGS },
            new() { Amount = expenses.Insurance.GetValueOrDefault().ToAmount(), Category = _C4M.HouseholdExpenseType.INSURANCE },
            new() { Amount = expenses.Other.GetValueOrDefault().ToAmount(), Category = _C4M.HouseholdExpenseType.OTHER },
            new() { Amount = 0.ToAmount(), Category = _C4M.HouseholdExpenseType.ALIMONY },
        };

    #region liabilities
    private List<_C4M.CreditLiabilitiesSummary> createCreditLiabilitiesSummary(List<_V2.CreditWorthinessObligation>? liabilitiesFlatten)
       => new List<_C4M.CreditLiabilitiesSummary>
       {
            new()
            {
                ProductGroup = _C4M.CreditLiabilitiesSummaryType.AD,
                Amount = sumObligations(liabilitiesFlatten, "AD", false, _fcSumObligationsAmount),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "AD", false, _fcSumObligationsAmountConsolidated)
            },
            new()
            {
                ProductGroup = _C4M.CreditLiabilitiesSummaryType.CC,
                Amount = sumObligations(liabilitiesFlatten, "CC", false, _fcSumObligationsAmount),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "CC", false, _fcSumObligationsAmountConsolidated)
            }
       };

    private List<_C4M.CreditLiabilitiesSummary> createCreditLiabilitiesSummaryOut(List<_V2.CreditWorthinessObligation>? liabilitiesFlatten)
       => new List<_C4M.CreditLiabilitiesSummary>
       {
            new()
            {
                ProductGroup = _C4M.CreditLiabilitiesSummaryType.AD,
                Amount = sumObligations(liabilitiesFlatten, "AD", true, _fcSumObligationsAmount),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "AD", true, _fcSumObligationsAmountConsolidated)
            },
            new()
            {
                ProductGroup = _C4M.CreditLiabilitiesSummaryType.CC,
                Amount = sumObligations(liabilitiesFlatten, "CC", true, _fcSumObligationsAmount),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "CC", true, _fcSumObligationsAmountConsolidated)
            }
       };

    private List<_C4M.LoanInstallmentsSummary> createInstallmentsSummary(List<_V2.CreditWorthinessObligation>? liabilitiesFlatten)
       => new List<_C4M.LoanInstallmentsSummary>
       {
            new()
            {
                ProductGroup = _C4M.InstallmentsSummaryType.CL,
                Amount = sumObligations(liabilitiesFlatten, "CL", false, _fcSumObligationsInstallment),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "CL", false, _fcSumObligationsInstallmentConsolidated)
            },
            new()
            {
                ProductGroup = _C4M.InstallmentsSummaryType.ML,
                Amount = sumObligations(liabilitiesFlatten, "ML", false, _fcSumObligationsInstallment),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "ML", false, _fcSumObligationsInstallmentConsolidated)
            }
       };

    private List<_C4M.LoanInstallmentsSummary> createInstallmentsSummaryOut(List<_V2.CreditWorthinessObligation>? liabilitiesFlatten)
       => new List<_C4M.LoanInstallmentsSummary>
       {
            new()
            {
                ProductGroup = _C4M.InstallmentsSummaryType.CL,
                Amount = sumObligations(liabilitiesFlatten, "CL", true, _fcSumObligationsInstallment),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "CL", true, _fcSumObligationsInstallmentConsolidated)
            },
            new()
            {
                ProductGroup = _C4M.InstallmentsSummaryType.ML,
                Amount = sumObligations(liabilitiesFlatten, "ML", true, _fcSumObligationsInstallment),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "ML", true, _fcSumObligationsInstallmentConsolidated)
            }
       };
    #endregion liabilities

    private _C4M.Amount sumObligations(List<_V2.CreditWorthinessObligation>? liabilitiesFlatten, string productGroup, bool isObligationCreditorExternal, Func<_V2.CreditWorthinessObligation, decimal> fcSum)
    {
        var arr = _obligationTypes!.Where(t => t.Code == productGroup).Select(t => t.Id).ToArray();
        return (liabilitiesFlatten?
            .Where(t => t.IsObligationCreditorExternal == isObligationCreditorExternal && arr.Contains(t.ObligationTypeId))
            .Sum(fcSum) ?? 0).ToAmount();
    }

    Func<_V2.CreditWorthinessObligation, decimal> _fcSumObligationsAmount = t => t.Amount.GetValueOrDefault();
    Func<_V2.CreditWorthinessObligation, decimal> _fcSumObligationsAmountConsolidated = t => t.AmountConsolidated.GetValueOrDefault();
    Func<_V2.CreditWorthinessObligation, decimal> _fcSumObligationsInstallment = t => t.Installment.GetValueOrDefault();
    Func<_V2.CreditWorthinessObligation, decimal> _fcSumObligationsInstallmentConsolidated = t => t.InstallmentConsolidated.GetValueOrDefault();

    private List<ObligationTypesResponse.Types.ObligationTypeItem>? _obligationTypes;
    
    private readonly CustomersChildMapper _customersMapper;
    private readonly CodebookService.Clients.ICodebookServiceClient _codebookService;

    public HouseholdsChildMapper(
        CustomersChildMapper customersMapper,
        CodebookService.Clients.ICodebookServiceClient codebookService)
    {
        _customersMapper = customersMapper;
        _codebookService = codebookService;
    }
}
