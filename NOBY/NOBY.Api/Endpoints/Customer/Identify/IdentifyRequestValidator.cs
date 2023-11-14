using FluentValidation;

namespace NOBY.Api.Endpoints.Customer.Identify;

internal sealed class IdentifyRequestValidator
    : AbstractValidator<IdentifyRequest>
{
    public IdentifyRequestValidator()
    {
        RuleFor(t => t.FirstName)
            .NotEmpty();

        RuleFor(t => t.LastName)
            .NotEmpty();

        RuleFor(t => t.DateOfBirth)
            .NotEmpty();

        RuleFor(t => t.IssuingCountryId)
            .GreaterThan(0);

        RuleFor(t => t.IdentificationDocumentNumber)
            .NotEmpty();

        RuleFor(t => t.IdentificationDocumentTypeId)
            .GreaterThan(0);
    }
}
