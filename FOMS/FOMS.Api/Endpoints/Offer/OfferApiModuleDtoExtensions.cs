namespace FOMS.Api.Endpoints.Offer;

internal static class OfferApiModuleDtoExtensions
{
    public static Dto.MortgageInputs ToApiResponse(this DomainServices.OfferService.Contracts.MortgageInput result)
        => new()
        {
            ProductTypeId = result.ProductTypeId,
            LoanKindId = result.LoanKindId,
            LoanAmount = result.LoanAmount,
            LoanDuration = result.LoanDuration,
            LoanPaymentAmount = result.LoanPaymentAmount,
            EmployeeBonusLoanCode = result.EmployeeBonusLoanCode,
            CollateralAmount = result.CollateralAmount,
            LoanToValue = result.LoanToValue,
            PaymentDayOfTheMonth = result.PaymentDayOfTheMonth,
            EmployeeBonusRequested = result.EmployeeBonusRequested,
            LoanPurpose = result.LoanPurpose?.Select(t => new Dto.LoanPurposeItem() { Id = t.LoanPurposeId, Sum = t.Sum }).ToList()
        };
    
    public static Dto.MortgageOutputs ToApiResponse(this DomainServices.OfferService.Contracts.MortgageOutput result)
        => new()
        {
            StatementTypeId = result.StatementTypeId,
            Aprc = result.Aprc,
            EmployeeBonusLoanCode = result.EmployeeBonusLoanCode,
            InterestRate = result.LoanInterestRate,
            InterestRateAnnounced = result.InterestRateAnnounced,
            LoanDuration = result.LoanDuration,
            LoanToCost = result.LoanToCost,
            LoanTotalAmount = result.LoanTotalAmount,
            LoanToValue = result.LoanToValue,
            LoanAmount = result.LoanAmount,
            LoanPaymentAmount = result.LoanPaymentAmount,
            LoanPurpose = result.LoanPurpose?.Select(t => new Dto.LoanPurposeItem() { Id = t.LoanPurposeId, Sum = t.Sum }).ToList(),
            PaymentDayOfTheMonth = result.PaymentDayOfTheMonth
        };
}