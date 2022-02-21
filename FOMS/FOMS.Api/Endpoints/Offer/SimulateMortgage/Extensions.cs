namespace FOMS.Api.Endpoints.Offer.SimulateMortgage;

internal static class Extensions
{
    public static DomainServices.OfferService.Contracts.SimulateMortgageRequest ToDomainServiceRequest(this SimulateMortgageRequest request)
        => new()
        {
            ResourceProcessId = request.ResourceProcessId,
            Inputs = new()
            {
                ProductTypeId = request.ProductTypeId,
                LoanKindId = request.LoanKindId,
                LoanAmount = request.LoanAmount,
                LoanDuration = request.LoanDuration,
                LoanPaymentAmount = request.LoanPaymentAmount,
                FixedLengthPeriod = request.FixationPeriod,
                EmployeeBonusLoanCode = request.EmployeeBonusLoanCode,
                CollateralAmount = request.CollateralAmount,
                LoanToValue = request.LoanToValue,
                PaymentDayOfTheMonth = request.PaymentDayOfTheMonth,
                EmployeeBonusRequested = request.EmployeeBonusRequested
            }
        };
}