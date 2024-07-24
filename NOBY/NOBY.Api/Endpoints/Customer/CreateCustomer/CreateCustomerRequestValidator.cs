using FluentValidation;
using NOBY.Api.Endpoints.Customer.Shared;

namespace NOBY.Api.Endpoints.Customer.CreateCustomer;

internal sealed class CreateCustomerRequestValidator : AbstractValidator<CustomerCreateCustomerRequest>
{
    private const int CustomerValidationErrorCode = 90032;

    public CreateCustomerRequestValidator()
    {
        RuleFor(r => r.CustomerOnSAId).NotEmpty().WithErrorCode(CustomerValidationErrorCode);
        RuleFor(r => r.FirstName).NotEmpty().WithErrorCode(CustomerValidationErrorCode);
        RuleFor(r => r.LastName).NotEmpty().WithErrorCode(CustomerValidationErrorCode);

        CustomerValidationExtensions.BirthDateValidation(RuleFor(r => r.BirthDate), CustomerValidationErrorCode);

        When(r => !string.IsNullOrWhiteSpace(r.BirthNumber),
             () =>
             {
                 CustomerValidationExtensions.BirthNumberValidation(RuleFor(r => r.BirthNumber)
                                                                        .Cascade(CascadeMode.Stop), r => r.BirthDate, CustomerValidationErrorCode);
             });

        When(r => !string.IsNullOrWhiteSpace(r.BirthPlace),
             () =>
             {
                 RuleFor(r => r.BirthPlace).MinimumLength(2).WithErrorCode(CustomerValidationErrorCode);
             });

        RuleFor(r => r.IdentificationDocument!)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithErrorCode(CustomerValidationErrorCode)
            .SetValidator(new IdentificationDocumentValidator());

        When(r => r.PrimaryAddress is not null && !string.IsNullOrWhiteSpace(r.PrimaryAddress.PragueDistrict),
             () =>
             {
                 RuleFor(r => r.PrimaryAddress!.PragueDistrict!)
                     .Must(dis => dis.StartsWith("Praha", StringComparison.OrdinalIgnoreCase))
                     .WithErrorCode(CustomerValidationErrorCode);
             });
    }
}