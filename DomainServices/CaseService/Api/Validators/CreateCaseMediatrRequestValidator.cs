using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class CreateCaseMediatrRequestValidator : AbstractValidator<Dto.CreateCaseMediatrRequest>
{
    public CreateCaseMediatrRequestValidator()
    {
        RuleFor(t => t.Request.Data.ProductTypeId)
            .GreaterThan(0)
            .WithMessage(t => "ProductTypeId must be > 0").WithErrorCode("13002");

        RuleFor(t => (decimal)t.Request.Data.TargetAmount)
            .GreaterThan(0)
            .WithMessage("Target amount must be > 0").WithErrorCode("13018");

        RuleFor(t => t.Request.CaseOwnerUserId)
            .GreaterThan(0)
            .WithMessage("Case Owner Id not must be > 0").WithErrorCode("13003");

        RuleFor(t => t.Request.Customer.Name)
            .NotEmpty()
            .WithMessage("Customer Name must not be empty").WithErrorCode("13012");
    }
}
