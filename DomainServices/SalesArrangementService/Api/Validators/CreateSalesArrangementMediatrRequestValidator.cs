using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class CreateSalesArrangementMediatrRequestValidator : AbstractValidator<Dto.CreateSalesArrangementMediatrRequest>
{
    public CreateSalesArrangementMediatrRequestValidator()
    {
        RuleFor(t => t.Request.CaseId)
            .GreaterThan(0)
            .WithMessage("Case Id must be > 0").WithErrorCode("16008");

        RuleFor(t => t.Request.SalesArrangementTypeId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementTypeId must be > 0").WithErrorCode("16009");
    }
}

