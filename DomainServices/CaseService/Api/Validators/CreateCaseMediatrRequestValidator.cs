using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class CreateCaseMediatrRequestValidator : AbstractValidator<Dto.CreateCaseMediatrRequest>
{
    public CreateCaseMediatrRequestValidator()
    {
        RuleFor(t => t.Request.Data.ProductInstanceTypeId)
            .GreaterThan(0)
            .WithMessage(t => "ProductInstanceTypeId must be > 0").WithErrorCode("13002");

        RuleFor(t => t.Request.Data.ContractNumber)
            .Length(10).When(t => !string.IsNullOrEmpty(t.Request.Data.ContractNumber))
            .WithMessage("ContractNumber length must be 10").WithErrorCode("13010");

        RuleFor(t => t.Request.Data.TargetAmount)
            .InclusiveBetween(20_000, 99_999_999).When(t => t.Request.Data.TargetAmount.HasValue)
            .WithMessage("Target amount must be between 20_000 and 99_999_999").WithErrorCode("13018");

        RuleFor(t => t.Request.CaseOwnerUserId)
            .GreaterThan(0)
            .WithMessage("Case Owner Id not must be > 0").WithErrorCode("13003");

        RuleFor(t => t.Request.Customer.Name)
            .NotEmpty()
            .WithMessage("Customer Name must not be empty").WithErrorCode("13012");
    }
}
