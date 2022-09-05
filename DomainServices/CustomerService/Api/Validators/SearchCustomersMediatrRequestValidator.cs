using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Api.Dto;
using FluentValidation;
namespace DomainServices.CustomerService.Api.Validators;

internal class SearchCustomersMediatrRequestValidator : AbstractValidator<SearchCustomersMediatrRequest>
{
    public SearchCustomersMediatrRequestValidator(ICodebookServiceAbstraction codebooks)
    {
        RuleFor(t => t.Request.Identity)
            .SetValidator(new IdentityValidator())
            .When(t => t.Request.Identity is not null);

        RuleFor(t => t.Request.Mandant)
            .IsInEnum()
            .NotEqual(Mandants.Unknown)
            .WithMessage("Mandant must be not empty").WithErrorCode("11008");

        RuleFor(t => t.Request)
            .Must(t => t.Identity != null || t.NaturalPerson != null || t.IdentificationDocument != null || !string.IsNullOrEmpty(t.Email) || !string.IsNullOrEmpty(t.PhoneNumber))
            .WithMessage("At least one of search field is required").WithErrorCode("11009");

        RuleFor(t => t.Request.NaturalPerson.LastName)
            .NotEmpty()
            .When(t => t.Request.NaturalPerson != null && !string.IsNullOrEmpty(t.Request.NaturalPerson.FirstName))
            .WithMessage("LastName must be not empty").WithErrorCode("11010");

        RuleFor(t => t.Request.Email)
            .EmailAddress()
            .When(t => !string.IsNullOrEmpty(t.Request.Email))
            .WithMessage("Email is not valid").WithErrorCode("11011");

        When(t => t.Request.IdentificationDocument != null, () =>
        {
            RuleFor(t => t.Request.IdentificationDocument.Number)
                .NotEmpty()
                .WithMessage("IdentificationDocument.Number must be not empty").WithErrorCode("11012");

            RuleFor(t => t.Request.IdentificationDocument.IdentificationDocumentTypeId)
                .GreaterThan(0)
                .MustAsync(async (t, cancellationToken) => (await codebooks.IdentificationDocumentTypes(cancellationToken)).Any(c => c.Id == t))
                .WithMessage("IdentificationDocumentTypeId is not valid").WithErrorCode("11013");

            RuleFor(t => t.Request.IdentificationDocument.IssuingCountryId)
                .GreaterThan(0)
                .MustAsync(async (t, cancellationToken) => (await codebooks.Countries(cancellationToken)).Any(c => c.Id == t))
                .WithMessage("IssuingCountryId is not valid").WithErrorCode("11014");
        });

    }
}
