using DomainServices.CustomerService.Api.Validators;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.SearchCustomers;

internal sealed class SearchCustomersRequestValidator : AbstractValidator<SearchCustomersRequest>
{
    public SearchCustomersRequestValidator()
    {
        RuleFor(t => t.Identity)
            .SetValidator(new IdentityValidator())
            .When(t => t.Identity is not null);

        RuleFor(t => t.Mandant)
            .IsInEnum()
            .NotEqual(Mandants.Unknown)
            .WithMessage("Mandant must be not empty").WithErrorCode("11008");

        RuleFor(t => t)
            .Must(t => t.Identity != null || t.NaturalPerson != null || t.IdentificationDocument != null || !string.IsNullOrEmpty(t.Email?.EmailAddress) || !string.IsNullOrEmpty(t.MobilePhone?.PhoneNumber))
            .WithMessage("At least one of search field is required").WithErrorCode("11009");

        RuleFor(t => t.NaturalPerson.LastName)
            .NotEmpty()
            .When(t => t.NaturalPerson != null && !string.IsNullOrEmpty(t.NaturalPerson.FirstName))
            .WithMessage("LastName must be not empty").WithErrorCode("11010");

        When(t => t.Email is not null, () =>
        {
            RuleFor(t => t.Email.EmailAddress)
                .EmailAddress()
                .When(t => !string.IsNullOrEmpty(t.Email.EmailAddress))
                .WithMessage("Email is not valid").WithErrorCode("11011");
        });

        When(t => t.NaturalPerson?.DateOfBirth != null,
             () =>
             {
                 RuleFor(t => t)
                     .Must(t => t.Identity != null || !string.IsNullOrWhiteSpace(t.NaturalPerson.LastName) || !string.IsNullOrWhiteSpace(t.NaturalPerson.BirthNumber) ||
                                t.IdentificationDocument != null || !string.IsNullOrEmpty(t.Email?.EmailAddress) || !string.IsNullOrEmpty(t.MobilePhone?.PhoneNumber))
                     .WithMessage("One more parameter, which can be sent to CM, is needed to search by date of birth.")
                     .WithErrorCode("11029");
             });

        When(t => t.IdentificationDocument != null, () =>
        {
            RuleFor(t => t.IdentificationDocument.Number)
                .NotEmpty()
                .WithMessage("IdentificationDocument.Number must be not empty").WithErrorCode("11012");

            RuleFor(t => t.IdentificationDocument.IdentificationDocumentTypeId)
                .GreaterThan(0)
                .WithMessage("IdentificationDocumentTypeId is not valid").WithErrorCode("11013");

            RuleFor(t => t.IdentificationDocument.IssuingCountryId)
                .GreaterThan(0)
                .WithMessage("IssuingCountryId is not valid").WithErrorCode("11014");
        });

    }
}
