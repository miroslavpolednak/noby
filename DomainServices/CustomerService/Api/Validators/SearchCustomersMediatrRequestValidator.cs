using DomainServices.CodebookService.Abstraction;
using FluentValidation;
namespace DomainServices.CustomerService.Api.Validators;

internal class SearchCustomersMediatrRequestValidator : AbstractValidator<Dto.SearchCustomersMediatrRequest>
{
    public SearchCustomersMediatrRequestValidator(ICodebookServiceAbstraction codebooks)
    {
        RuleFor(t => t.Request)
            .Must(t => t.Identity != null || t.NaturalPerson != null || t.IdentificationDocument != null || !string.IsNullOrEmpty(t.Email) || !string.IsNullOrEmpty(t.PhoneNumber))
            .WithMessage("At least one of field is required").WithErrorCode("17000");

        RuleFor(t => t.Request.Identity)
            .SetValidator(new IdentityValidator());

        RuleFor(t => t.Request.Mandant)
            .IsInEnum()
            .NotEqual(CIS.Infrastructure.gRPC.CisTypes.Mandants.Unknown)
            .WithMessage("Mandant must be not empty").WithErrorCode("17000");

        RuleFor(t => t.Request.NaturalPerson.LastName)
            .NotEmpty()
            .When(t => t.Request.NaturalPerson != null && !string.IsNullOrEmpty(t.Request.NaturalPerson.FirstName))
            .WithMessage("LastName must be not empty").WithErrorCode("17000");

        RuleFor(t => t.Request.Email)
            .EmailAddress()
            .When(t => !string.IsNullOrEmpty(t.Request.Email))
            .WithMessage("Email is not valid").WithErrorCode("17000");

        When(t => t.Request.IdentificationDocument != null, () =>
        {
            RuleFor(t => t.Request.IdentificationDocument.Number)
                .NotEmpty()
                .WithMessage("IdentificationDocument.Number must be not empty").WithErrorCode("17000");

            RuleFor(t => t.Request.IdentificationDocument.IdentificationDocumentTypeId)
                .GreaterThan(0)
                .MustAsync(async (t, cancellationToken) => (await codebooks.IdentificationDocumentTypes(cancellationToken)).Any(c => c.Id == t))
                .WithMessage("IdentificationDocumentTypeId is not valid").WithErrorCode("17000");

            RuleFor(t => t.Request.IdentificationDocument.IssuingCountryId)
                .GreaterThan(0)
                .MustAsync(async (t, cancellationToken) => (await codebooks.Countries(cancellationToken)).Any(c => c.Id == t))
                .WithMessage("IssuingCountryId is not valid").WithErrorCode("17000");
        });

    }
}
