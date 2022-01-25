using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class UpdateCaseCustomerMediatrRequestValidator : AbstractValidator<Dto.UpdateCaseCustomerMediatrRequest>
{
    public UpdateCaseCustomerMediatrRequestValidator()
    {
        RuleFor(t => t.Request.Customer.Name)
            .NotEmpty()
            .WithMessage("Customer Name must not be empty").WithErrorCode("13012");
    }
}
