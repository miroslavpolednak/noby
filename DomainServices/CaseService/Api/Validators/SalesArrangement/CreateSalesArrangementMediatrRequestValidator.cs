using FluentValidation;

namespace DomainServices.CaseService.Api.Validators.SalesArrangement;

internal class CreateSalesArrangementMediatrRequestValidator : AbstractValidator<Dto.SalesArrangement.CreateSalesArrangementMediatrRequest>
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

