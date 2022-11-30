using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;


namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.SimpleCalculate;

internal static class SimpleCalculateRequestExtensions
{
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
