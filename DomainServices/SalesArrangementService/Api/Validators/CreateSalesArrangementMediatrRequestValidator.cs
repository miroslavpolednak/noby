using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class CreateSalesArrangementMediatrRequestValidator : AbstractValidator<Dto.CreateSalesArrangementMediatrRequest>
{
    public CreateSalesArrangementMediatrRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13000");

        RuleFor(t => t.SalesArrangementType)
            .GreaterThan(0)
            .WithMessage("SalesArrangementType must be > 0").WithErrorCode("13000");

        RuleFor(t => t.ProductInstanceId)
            .GreaterThan(0)
            .WithMessage("ProductInstanceId must be > 0").WithErrorCode("13000");
    }
}

