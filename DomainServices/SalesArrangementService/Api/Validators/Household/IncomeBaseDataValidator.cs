﻿using DomainServices.SalesArrangementService.Contracts;
using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

public class IncomeBaseDataValidator
    : AbstractValidator<IncomeBaseData>
{
    public IncomeBaseDataValidator(DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        // check codebooks
        RuleFor(t => t.CurrencyCode)
            .MustAsync(async (currencyCode, cancellation) =>
            {
                return (await codebookService.Currencies(cancellation)).Any(t => t.Code == currencyCode);
            })
            .When(t => !string.IsNullOrEmpty(t.CurrencyCode))
            .WithMessage("CurrencyId is not valid").WithErrorCode("16030");
    }
}
