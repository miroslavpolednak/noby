using FluentValidation;

namespace NOBY.Api.Endpoints.Customer.Shared;

internal sealed class IdentificationDocumentValidator : AbstractValidator<SharedTypesIdentificationDocumentFull>
{
    private const int CustomerValidationErrorCode = 90032;
    private const int IdentityCardId = 1;
    private const int PassportId = 2;
    private const int PermitToStayId = 3;
    private const int ForeignIdentityCardId = 4;

    public IdentificationDocumentValidator()
    {
        RuleFor(document => document.ValidTo)
            .GreaterThan(DateOnly.FromDateTime(DateTime.Now)).WithErrorCode(CustomerValidationErrorCode);

        RuleFor(document => document.IssuedOn)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithErrorCode(CustomerValidationErrorCode);

        RuleFor(document => document.Number)
            .NotEmpty().WithErrorCode(CustomerValidationErrorCode);

        When(document => document.IssuingCountryId == 16,
             () =>
             {
                 RuleFor(document => document.IdentificationDocumentTypeId)
                     .Must(typeId => typeId is IdentityCardId or PassportId or PermitToStayId)
                     .WithErrorCode(CustomerValidationErrorCode);

                 When(document => document.IdentificationDocumentTypeId == IdentityCardId,
                      () =>
                      {
                          RuleFor(document => document.Number)
                              .Matches(@"^(\d{9}|[a-zA-Z]{2}\s?\d{6})$")
                              .WithErrorCode(CustomerValidationErrorCode);
                      });
             })
            .Otherwise(() =>
            {
                RuleFor(document => document.IdentificationDocumentTypeId)
                    .Must(typeId => typeId is PassportId or ForeignIdentityCardId)
                    .WithErrorCode(CustomerValidationErrorCode);
            });
    }
}