using FluentValidation;
using NOBY.Api.Endpoints.Customer.Shared;

namespace NOBY.Api.Endpoints.Customer.CreateCustomer;

internal class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    private const int CustomerValidationErrorCode = 90032;

    public CreateCustomerRequestValidator()
    {
        RuleFor(r => r.CustomerOnSAId).NotEmpty().WithErrorCode(CustomerValidationErrorCode);
        RuleFor(r => r.FirstName).NotNull().MinimumLength(1).WithErrorCode(CustomerValidationErrorCode);
        RuleFor(r => r.LastName).NotNull().MinimumLength(1).WithErrorCode(CustomerValidationErrorCode);

        RuleFor(r => r.BirthDate)
            .BirthDateValidation()
            .WithErrorCode(90032);

        RuleFor(r => r.BirthNumber)
            .Cascade(CascadeMode.Stop)
            .BirthNumberValidation(r => r.BirthDate)
            .WithErrorCode(90032);

        When(r => !string.IsNullOrWhiteSpace(r.BirthPlace),
             () =>
             {
                 RuleFor(r => r.BirthPlace).MinimumLength(2).WithErrorCode(CustomerValidationErrorCode);
             });

        RuleFor(r => r.IdentificationDocument!)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .SetValidator(new IdentificationDocumentValidator())
            .WithErrorCode(90032);
    }
}