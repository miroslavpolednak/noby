using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;
using CIS.Core;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate.Mappers;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class HouseholdsChildMapper
{
    public async Task<List<_C4M.LoanApplicationHousehold>> MapHouseholds(
        List<_V2.CreditWorthinessHousehold> households, 
        int mandantId,
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
                CreditLiabilitiesSummary = createCreditLiabilitiesSummary(liabilitiesFlatten),
                CreditLiabilitiesSummaryOut = createCreditLiabilitiesSummaryOut(liabilitiesFlatten),
                InstallmentsSummary = createInstallmentsSummary(liabilitiesFlatten),
                InstallmentsSummaryOut = createInstallmentsSummaryOut(liabilitiesFlatten)
            };
        })).ToList();   
    }

    private static List<_C4M.ExpensesSummary> toC4m(Contracts.Shared.V1.ExpensesSummary expenses)
        => new List<_C4M.ExpensesSummary>()
        {
            new() { Amount = expenses.Rent.GetValueOrDefault(), Category = _C4M.ExpensesSummaryCategory.RENT },
            new() { Amount = expenses.Saving.GetValueOrDefault(), Category = _C4M.ExpensesSummaryCategory.SAVING },
            new() { Amount = expenses.Insurance.GetValueOrDefault(), Category = _C4M.ExpensesSummaryCategory.INSURANCE },
            new() { Amount = expenses.Other.GetValueOrDefault(), Category = _C4M.ExpensesSummaryCategory.OTHER },
            new() { Amount = 0, Category = _C4M.ExpensesSummaryCategory.ALIMONY },
        };

    #region liabilities
    private List<_C4M.CreditLiabilitiesSummaryHomeCompany> createCreditLiabilitiesSummary(List<_V2.CreditWorthinessObligation>? liabilitiesFlatten)
       => new List<_C4M.CreditLiabilitiesSummaryHomeCompany>
       {
            new()
            {
                ProductGroup = _C4M.CreditLiabilitiesSummaryHomeCompanyProductGroup.AD,
                Amount = sumObligations(liabilitiesFlatten, "AD", false, _fcSumObligationsAmount),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "AD", false, _fcSumObligationsAmountConsolidated)
            },
            new()
            {
                ProductGroup = _C4M.CreditLiabilitiesSummaryHomeCompanyProductGroup.CC,
                Amount = sumObligations(liabilitiesFlatten, "CC", false, _fcSumObligationsAmount),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "CC", false, _fcSumObligationsAmountConsolidated)
            }
       };

    private List<_C4M.CreditLiabilitiesSummary> createCreditLiabilitiesSummaryOut(List<_V2.CreditWorthinessObligation>? liabilitiesFlatten)
       => new List<_C4M.CreditLiabilitiesSummary>
       {
            new()
            {
                ProductGroup = _C4M.CreditLiabilitiesSummaryProductGroup.AD,
                Amount = sumObligations(liabilitiesFlatten, "AD", true, _fcSumObligationsAmount),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "AD", true, _fcSumObligationsAmountConsolidated)
            },
            new()
            {
                ProductGroup = _C4M.CreditLiabilitiesSummaryProductGroup.CC,
                Amount = sumObligations(liabilitiesFlatten, "CC", true, _fcSumObligationsAmount),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "CC", true, _fcSumObligationsAmountConsolidated)
            }
       };

    private List<_C4M.InstallmentsSummaryHomeCompany> createInstallmentsSummary(List<_V2.CreditWorthinessObligation>? liabilitiesFlatten)
       => new List<_C4M.InstallmentsSummaryHomeCompany>
       {
            new()
            {
                ProductGroup = _C4M.InstallmentsSummaryHomeCompanyProductGroup.CL,
                Amount = sumObligations(liabilitiesFlatten, "CL", false, _fcSumObligationsInstallment),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "CL", false, _fcSumObligationsInstallmentConsolidated)
            },
            new()
            {
                ProductGroup = _C4M.InstallmentsSummaryHomeCompanyProductGroup.ML,
                Amount = sumObligations(liabilitiesFlatten, "ML", false, _fcSumObligationsInstallment),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "ML", false, _fcSumObligationsInstallmentConsolidated)
            }
       };

    private List<_C4M.InstallmentsSummaryOutHomeCompany> createInstallmentsSummaryOut(List<_V2.CreditWorthinessObligation>? liabilitiesFlatten)
       => new List<_C4M.InstallmentsSummaryOutHomeCompany>
       {
            new()
            {
                ProductGroup = _C4M.InstallmentsSummaryOutHomeCompanyProductGroup.CL,
                Amount = sumObligations(liabilitiesFlatten, "CL", true, _fcSumObligationsInstallment),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "CL", true, _fcSumObligationsInstallmentConsolidated)
            },
            new()
            {
                ProductGroup = _C4M.InstallmentsSummaryOutHomeCompanyProductGroup.ML,
                Amount = sumObligations(liabilitiesFlatten, "ML", true, _fcSumObligationsInstallment),
                AmountConsolidated = sumObligations(liabilitiesFlatten, "ML", true, _fcSumObligationsInstallmentConsolidated)
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

    private List<CodebookService.Contracts.Endpoints.ObligationTypes.ObligationTypesItem>? _obligationTypes;
    
    private readonly CustomersChildMapper _customersMapper;
    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;

    public HouseholdsChildMapper(
        CustomersChildMapper customersMapper,
        CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        _customersMapper = customersMapper;
        _codebookService = codebookService;
    }
}
