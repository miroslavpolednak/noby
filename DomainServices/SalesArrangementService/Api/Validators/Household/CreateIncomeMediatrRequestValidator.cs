using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators.Household;

internal class CreateIncomeMediatrRequestValidator
    : AbstractValidator<Dto.CreateIncomeMediatrRequest>
{
    public CreateIncomeMediatrRequestValidator(DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        RuleFor(t => t.Request.CustomerOnSAId)
            .GreaterThan(0)
            .WithMessage("CustomerOnSAId must be > 0").WithErrorCode("16024");

        RuleFor(t => t.Request.IncomeTypeId)
            .GreaterThan(0)
            .WithMessage("IncomeTypeId must be > 0").WithErrorCode("16028");

        RuleFor(t => t.Request.IncomeTypeId)
            .Must(t => (CIS.Foms.Enums.HouseholdTypes)t != CIS.Foms.Enums.HouseholdTypes.Unknown)
            .WithMessage("IncomeTypeId must be > 0").WithErrorCode("16028");

        // check codebooks
        /*RuleFor(t => t.Request.CurrencyId)
            .MustAsync(async (currencyId, cancellation) =>
            {
                return (await codebookService.Currencies(cancellation)).Any(t => t.Code == currencyId);
            })
            .When(t => t.Request.CurrencyId.HasValue)
            .WithMessage("CurrencyId is not valid").WithErrorCode("16030");*/

    }
}
