using CIS.Infrastructure.CisMediatR.GrpcValidation;
using DomainServices.CustomerService.Api.Validators;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.v1.SearchCustomers;

internal sealed class SearchCustomersRequestValidator
    : AbstractValidator<SearchCustomersRequest>
{
    public SearchCustomersRequestValidator()
    {
        RuleFor(t => t.Identity)
            .SetValidator(new IdentityValidator())
            .When(t => t.Identity is not null);

        RuleFor(t => t.Mandant)
            .IsInEnum()
            .NotEqual(SharedTypes.GrpcTypes.Mandants.Unknown)
            .WithErrorCode(ErrorCodeMapper.MandantIsEmpty);

        RuleFor(t => t)
            .Must(t => t.Identity != null || t.NaturalPerson != null || t.IdentificationDocument != null || !string.IsNullOrEmpty(t.Email?.EmailAddress) || !string.IsNullOrEmpty(t.MobilePhone?.PhoneNumber))
            .WithErrorCode(ErrorCodeMapper.SearchFieldsEmpty);

        RuleFor(t => t.NaturalPerson.LastName)
            .NotEmpty()
            .When(t => t.NaturalPerson != null && !string.IsNullOrEmpty(t.NaturalPerson.FirstName))
            .WithErrorCode(ErrorCodeMapper.LastNameIsEmpty);

        When(t => t.Email is not null, () =>
        {
            RuleFor(t => t.Email.EmailAddress)
                .EmailAddress()
                .When(t => !string.IsNullOrEmpty(t.Email.EmailAddress))
                .WithErrorCode(ErrorCodeMapper.InvalidEmail);
        });

        When(t => t.NaturalPerson?.DateOfBirth != null,
             () =>
             {
                 RuleFor(t => t)
                     .Must(t => t.Identity != null || !string.IsNullOrWhiteSpace(t.NaturalPerson.LastName) || !string.IsNullOrWhiteSpace(t.NaturalPerson.BirthNumber) ||
                                t.IdentificationDocument != null || !string.IsNullOrEmpty(t.Email?.EmailAddress) || !string.IsNullOrEmpty(t.MobilePhone?.PhoneNumber))
                     .WithErrorCode(ErrorCodeMapper.DateOfBirthSearchInvalid);
             });

        When(t => t.IdentificationDocument != null, () =>
        {
            RuleFor(t => t.IdentificationDocument.Number)
                .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.IdentDocNumberIsEmpty);

            RuleFor(t => t.IdentificationDocument.IdentificationDocumentTypeId)
                .GreaterThan(0)
                .WithErrorCode(ErrorCodeMapper.IdentDocTypeIsEmpty);

            RuleFor(t => t.IdentificationDocument.IssuingCountryId)
                .GreaterThan(0)
                .WithErrorCode(ErrorCodeMapper.IdentDocCountryIsEmpty);
        });
    }
}
