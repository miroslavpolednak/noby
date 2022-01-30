using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class UpdateCaseCustomerMediatrRequestValidator : AbstractValidator<Dto.UpdateCaseCustomerMediatrRequest>
{
    public UpdateCaseCustomerMediatrRequestValidator()
    {
        RuleFor(t => t.Request.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13016");

        RuleFor(t => t.Request.Customer.Name)
            .NotEmpty()
            .WithMessage("Customer Name must not be empty").WithErrorCode("13012");
    }
}
