using DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save;

internal static class SaveRequestExtensions
{
    public static async Task<LoanApplicationProduct> ToC4m(this _V2.LoanApplicationProduct product, CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService, CancellationToken cancellation)
    {
        // repayment type
        var repaymentType = (await _codebookService.RepaymentScheduleTypes(cancellation)).FirstOrDefault(t => t.Id == product.RepaymentScheduleTypeId)?.Code ?? "A";
        if (!FastEnum.TryParse(repaymentType, out LoanApplicationProductRepaymentScheduleType repaymentTypeEnum))
            throw new CisValidationException(0, $"Can't cast RepaymentScheduleTypeId '{product.RepaymentScheduleTypeId}' to C4M enum");

        // product 
        var products = (await _codebookService.RiskApplicationTypes(cancellation))
            .Where(t =>
                // Dle produktu - vždy vyplněn
                t.ProductTypeId is not null && t.ProductTypeId.Contains(product.ProductTypeId)
                // LTV
                && (product.Ltv <= (t.LtvTo ?? int.MaxValue) && product.Ltv >= (t.LtvFrom ?? 0))
            )
            .ToList();
        // Druh uveru
        products = (products.Any(t => t.LoanKindId == product.LoanKindId) 
            ? products.Where(t => t.LoanKindId == product.LoanKindId) 
            : products.Where(t => !t.LoanKindId.HasValue))
            .ToList();
        // MA
        bool requestContainsMa = product.MarketingActions is not null && product.MarketingActions.Any();
        products.Where(t =>
            // v req neni MA, hledam jen v items bez MA
            !(requestContainsMa && (t.MarketingActions is null || !t.MarketingActions.Any()))
            || (requestContainsMa && (t.MarketingActions is not null || t.MarketingActions.Any(x => product.MarketingActions.Contains(x))))
        );
        

        return new LoanApplicationProduct
        {
            /*ProductClusterCode =
            AplType = product.AplType,
            Maturity = product.LoanDuration,
            Annuity = Convert.ToInt64(product.LoanPaymentAmount),
            FixationPeriod = product.FixedRatePeriod,
            InterestRate = product.LoanInterestRate,
            AmountForeignResources = product.ForeignResourcesAmount.ToAmount(),
            AmountInvestment = product.InvestmentAmount.ToAmount(),
            AmountOwnResources = product.OwnResourcesAmount.ToAmount(),
            AmountRequired = product.RequiredAmount.ToAmount(),
            RepaymentScheduleType = repaymentTypeEnum,
            InstallmentCount = product.InstallmentCount,
            DrawingPeriodStart = product.DrawingPeriodStart,
            DrawingPeriodEnd = product.DrawingPeriodEnd,
            RepaymentPeriodStart = product.RepaymentPeriodStart,
            RepaymentPeriodEnd = product.RepaymentPeriodEnd*/
        };
    }

    public static async Task<List<LoanApplicationHousehold>> ToC4m(this List<_V2.LoanApplicationHousehold> households, CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService, CancellationToken cancellation)
    {
        var convertedHouseholds = new List<LoanApplicationHousehold>();
        foreach (var household in households)
        {
            var roleCode = (await _codebookService.HouseholdTypes(cancellation)).FirstOrDefault(t => t.Id == household.HouseholdTypeId)?.RdmCode;

            convertedHouseholds.Add(new LoanApplicationHousehold
            {
                Id = household.HouseholdId,
                RoleCode = roleCode,
                ChildrenUnderAnd10 = household.ChildrenUpToTenYearsCount,
                ChildrenOver10 = household.ChildrenOverTenYearsCount,
                SettlementTypeCode = (await _codebookService.PropertySettlements(cancellation)).FirstOrDefault(t => t.Id == household.PropertySettlementId)?.Code,
                HouseholdExpensesSummary = null,
                CounterParty = null
            });
        }
        return convertedHouseholds;
    }

    /*public static List<ExpensesSummary> ToC4m(this Contracts.Shared.ExpensesSummary.V1.ExpensesSummary expenses)
        => new List<ExpensesSummary>()
        {
            new() { Amount = expenses.Rent.GetValueOrDefault(), Category = ExpensesSummaryCategory.RENT },
            new() { Amount = expenses.Saving.GetValueOrDefault(), Category = ExpensesSummaryCategory.SAVINGS },
            new() { Amount = expenses.Insurance.GetValueOrDefault(), Category = ExpensesSummaryCategory.INSURANCE },
            new() { Amount = expenses.Other.GetValueOrDefault(), Category = ExpensesSummaryCategory.OTHER },
            new() { Amount = 0, Category = _C4M.ExpensesSummaryCategory.ALIMONY },
        };*/
}
