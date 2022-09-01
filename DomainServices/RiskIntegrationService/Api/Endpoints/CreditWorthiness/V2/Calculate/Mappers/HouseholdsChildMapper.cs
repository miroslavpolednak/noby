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
        var obligationTypes = await _codebookService.ObligationTypes(cancellation);
        var liabilitiesFlatten = households.SelectMany(
            h => h.Customers!.Where(x => x.Obligations is not null).SelectMany(x => x.Obligations!)
            ).ToList();

        return (await households.SelectAsync(async h => new _C4M.LoanApplicationHousehold
        {
            ChildrenOver10 = h.ChildrenOverTenYearsCount,
            ChildrenUnderAnd10 = h.ChildrenUpToTenYearsCount,
            ExpensesSummary = toC4m(h.ExpensesSummary ?? new Contracts.Shared.V1.ExpensesSummary()),
            Clients = await _customersMapper.MapCustomers(h.Customers!, mandantId, cancellation),
            CreditLiabilitiesSummary = toC4mCreditLiabilitiesSummary(liabilitiesFlatten, obligationTypes.Where(o => o.Id == 3 || o.Id == 4)),
            CreditLiabilitiesSummaryOut = toC4mCreditLiabilitiesSummaryOut(liabilitiesFlatten, obligationTypes.Where(o => o.Id == 3 || o.Id == 4)),
            InstallmentsSummary = toC4mInstallmentsSummary(liabilitiesFlatten, obligationTypes.Where(o => o.Id != 3 && o.Id != 4)),
            InstallmentsSummaryOut = toC4mInstallmentsSummaryOut(liabilitiesFlatten, obligationTypes.Where(o => o.Id != 3 && o.Id != 4))
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
    private static List<_C4M.CreditLiabilitiesSummaryHomeCompany> toC4mCreditLiabilitiesSummary(List<_V2.CreditWorthinessObligation>? liabilites, IEnumerable<CodebookService.Contracts.Endpoints.ObligationTypes.ObligationTypesItem> obligationTypes)
        => obligationTypes
            .Select(t =>
            {
                var coll = liabilites?.Where(x => x.ObligationTypeId == t.Id && !x.IsObligationCreditorExternal).ToList();
                return new _C4M.CreditLiabilitiesSummaryHomeCompany
                {
                    Amount = coll?.Sum(x => x.Amount) ?? 0,
                    AmountConsolidated = coll?.Sum(x => x.AmountConsolidated) ?? 0,
                    ProductGroup = FastEnum.Parse<_C4M.CreditLiabilitiesSummaryHomeCompanyProductGroup>(t.Code)
                };
            })
            .ToList();

    private static List<_C4M.CreditLiabilitiesSummary> toC4mCreditLiabilitiesSummaryOut(List<_V2.CreditWorthinessObligation>? liabilites, IEnumerable<CodebookService.Contracts.Endpoints.ObligationTypes.ObligationTypesItem> obligationTypes)
        => obligationTypes
            .Select(t =>
            {
                var coll = liabilites?.Where(x => x.ObligationTypeId == t.Id && x.IsObligationCreditorExternal).ToList();
                return new _C4M.CreditLiabilitiesSummary
                {
                    Amount = coll?.Sum(x => x.Amount) ?? 0,
                    AmountConsolidated = coll?.Sum(x => x.AmountConsolidated) ?? 0,
                    ProductGroup = FastEnum.Parse<_C4M.CreditLiabilitiesSummaryProductGroup>(t.Code)
                };
            })
            .ToList();

    private static List<_C4M.InstallmentsSummaryHomeCompany> toC4mInstallmentsSummary(List<_V2.CreditWorthinessObligation>? liabilites, IEnumerable<CodebookService.Contracts.Endpoints.ObligationTypes.ObligationTypesItem> obligationTypes)
        => obligationTypes
            .Select(t =>
            {
                var coll = liabilites?.Where(x => x.ObligationTypeId == t.Id && !x.IsObligationCreditorExternal).ToList();
                return new _C4M.InstallmentsSummaryHomeCompany
                {
                    Amount = coll?.Sum(x => x.Amount) ?? 0,
                    AmountConsolidated = coll?.Sum(x => x.AmountConsolidated) ?? 0,
                    ProductGroup = FastEnum.Parse<_C4M.InstallmentsSummaryHomeCompanyProductGroup>(t.Code)
                };
            })
            .ToList();

    private static List<_C4M.InstallmentsSummaryOutHomeCompany> toC4mInstallmentsSummaryOut(List<_V2.CreditWorthinessObligation>? liabilites, IEnumerable<CodebookService.Contracts.Endpoints.ObligationTypes.ObligationTypesItem> obligationTypes)
        => obligationTypes
            .Select(t =>
            {
                var coll = liabilites?.Where(x => x.ObligationTypeId == t.Id && x.IsObligationCreditorExternal).ToList();
                return new _C4M.InstallmentsSummaryOutHomeCompany
                {
                    Amount = coll?.Sum(x => x.Amount) ?? 0,
                    AmountConsolidated = coll?.Sum(x => x.AmountConsolidated) ?? 0,
                    ProductGroup = FastEnum.Parse<_C4M.InstallmentsSummaryOutHomeCompanyProductGroup>(t.Code)
                };
            })
            .ToList();
    #endregion liabilities

    private readonly CustomersChildMapper _customersMapper;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public HouseholdsChildMapper(
        CustomersChildMapper customersMapper,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        _customersMapper = customersMapper;
        _codebookService = codebookService;
    }
}
