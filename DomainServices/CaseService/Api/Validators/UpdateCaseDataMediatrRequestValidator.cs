using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class UpdateCaseDataMediatrRequestValidator : AbstractValidator<Dto.UpdateCaseDataMediatrRequest>
{
    public UpdateCaseDataMediatrRequestValidator()
    {
        RuleFor(t => t.Request.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13016");

        RuleFor(t => t.Request.Data.ProductTypeId)
            .GreaterThan(0)
            .WithMessage(t => "ProductTypeId must be > 0").WithErrorCode("13002");

        RuleFor(t => t.Request.Data.ContractNumber)
            .Length(10).When(t => !string.IsNullOrEmpty(t.Request.Data.ContractNumber))
            .WithMessage("ContractNumber length must be 10").WithErrorCode("13010");

        RuleFor(t => t.Request.Data.TargetAmount)
            .InclusiveBetween(20_000, 99_999_999).When(t => t.Request.Data.TargetAmount.HasValue)
            .WithMessage("Target amount must be between 20_000 and 99_999_999").WithErrorCode("13018");
    }
}
