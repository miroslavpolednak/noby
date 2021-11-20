using FluentValidation;

namespace FOMS.Api.Endpoints.Test.Validators;

internal class BadRequestValidator : AbstractValidator<Dto.BadRequestRequest>
{
    public BadRequestValidator()
    {
        RuleFor(t => t.Id)
            .NotNull().WithMessage("Id je null!")
            .GreaterThan(0).WithMessage("Id <= 0");
    }
}
