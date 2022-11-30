using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;
using DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using System.Runtime.CompilerServices;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.SimpleCalculate;

internal static class SimpleCalculateRequestExtensions
{
    public static CreditWorthinessCalculateRequest ToFullRequest(this CreditWorthinessSimpleCalculateRequest request)
    {
        var customer = new CreditWorthinessCustomer()
        {
            PrimaryCustomerId = request.PrimaryCustomerId,
            Incomes = new List<CreditWorthinessIncome>
            {
                new() { IncomeTypeId = 4, Amount = request.TotalMonthlyIncome.GetValueOrDefault() }
            },
            Obligations = new List<CreditWorthinessObligation>
            {
                new()
                {
                    Amount = request.ObligationsSummary?.AuthorizedOverdraftsAmount ?? 0,
                    ObligationTypeId = 4,
                    IsObligationCreditorExternal = true
                },
                new()
                {
                    Amount = request.ObligationsSummary?.CreditCardsAmount ?? 0,
                    ObligationTypeId = 3,
                    IsObligationCreditorExternal = true
                },
                new()
                {
                    Installment = request.ObligationsSummary?.LoansInstallmentsAmount ?? 0,
                    ObligationTypeId = 2,
                    IsObligationCreditorExternal = true
                },
            }
        };

        return new CreditWorthinessCalculateRequest()
        {
            ResourceProcessId = request.ResourceProcessId,
            UserIdentity = request.UserIdentity,
            Product = request.Product,
            Households = new List<CreditWorthinessHousehold>() {
                new()
                {
                    ChildrenOverTenYearsCount = request.ChildrenCount,
                    Customers = new List<CreditWorthinessCustomer>(){ customer },
                    ExpensesSummary = new Contracts.Shared.V1.ExpensesSummary
                    {
                        Rent = request.ExpensesSummary?.Rent,
                        Other = request.ExpensesSummary?.Other
                    }
                }
            }
        };
    }

    public static List<_C4M.ExpensesSummary> ToC4M(this _V2.CreditWorthinessSimpleExpensesSummary expenses)
        => new()
        {
            new() { Amount = expenses.Rent.GetValueOrDefault(), Category = _C4M.ExpensesSummaryCategory.RENT },
            new() { Amount = 0, Category = _C4M.ExpensesSummaryCategory.SAVING },
            new() { Amount = 0, Category = _C4M.ExpensesSummaryCategory.INSURANCE },
            new() { Amount = expenses.Other.GetValueOrDefault(), Category = _C4M.ExpensesSummaryCategory.OTHER },
            new() { Amount = 0, Category = _C4M.ExpensesSummaryCategory.ALIMONY },
        };

    public static _V2.CreditWorthinessSimpleCalculateResponse ToServiceResponse(this _C4M.CreditWorthinessCalculation response, int loanPaymentAmount)
        => new()
        {
            InstallmentLimit = response.InstallmentLimit,
            MaxAmount = response.MaxAmount,
            RemainsLivingAnnuity = response.RemainsLivingAnnuity,
            RemainsLivingInst = response.RemainsLivingInst,
            ResultReason = response.ResultReason is null ? null : new Contracts.Shared.ResultReasonDetail
            {
                Code = response.ResultReason.Code,
                Description = response.ResultReason.Description
            },
            WorthinessResult = response.InstallmentLimit > loanPaymentAmount
                ? _V2.CreditWorthinessResults.Success
                : _V2.CreditWorthinessResults.Failed
        };
}
