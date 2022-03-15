namespace FOMS.Api.Endpoints.Offer.SimulateMortgage;

internal static class Extensions
{
    public static DomainServices.OfferService.Contracts.SimulateMortgageRequest ToDomainServiceRequest(this SimulateMortgageRequest request)
    {
        var model = new DomainServices.OfferService.Contracts.SimulateMortgageRequest()
        {
            ResourceProcessId = request.ResourceProcessId,
            Inputs = new()
            {
                ProductTypeId = request.ProductTypeId,
                LoanKindId = request.LoanKindId,
                LoanAmount = request.LoanAmount,
                LoanDuration = request.LoanDuration,
                LoanPaymentAmount = request.LoanPaymentAmount,
                FixedRatePeriod = request.FixedRatePeriod,
                EmployeeBonusLoanCode = request.EmployeeBonusLoanCode,
                CollateralAmount = request.CollateralAmount,
                LoanToValue = request.LoanToValue,
                PaymentDayOfTheMonth = request.PaymentDayOfTheMonth,
                EmployeeBonusRequested = request.EmployeeBonusRequested
            }
        };
        
        if (request.LoanPurpose is not null && request.LoanPurpose.Any())
            model.Inputs.LoanPurpose.AddRange(
                request.LoanPurpose.Select(t => new DomainServices.OfferService.Contracts.LoanPurpose() { LoanPurposeId = t.Id, Sum = t.Sum })
                );

        return model;
    }
}