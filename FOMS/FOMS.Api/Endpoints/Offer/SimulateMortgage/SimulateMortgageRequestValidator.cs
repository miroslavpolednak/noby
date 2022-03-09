﻿using FluentValidation;

namespace FOMS.Api.Endpoints.Offer.SimulateMortgage;

internal class SimulateMortgageRequestValidator
     : AbstractValidator<SimulateMortgageRequest>
{
    public SimulateMortgageRequestValidator()
    {
        RuleFor(t => t.ResourceProcessId)
            .NotEmpty().WithMessage("ResourceProcessId is empty")
            .Must(t => Guid.TryParse(t, out Guid _)).WithMessage("ResourceProcessId can not be casted to .NET Guid");
        
        RuleFor(t => t.ProductTypeId)
            .GreaterThan(0).WithMessage("ProductTypeId must be > 0");
        
        RuleFor(t => t.LoanAmount)
            .GreaterThan(0).WithMessage("Částka úvěru = 0");

        RuleFor(t => t.PaymentDayOfTheMonth)
            .GreaterThan(0).WithMessage("PaymentDayOfTheMonth = 0");
    }
}
