using FluentValidation;
using NOBY.Dto;

namespace NOBY.Api.Endpoints.Customer.SharedDto;

internal class IdentificationDocumentValidator : AbstractValidator<IdentificationDocumentFull>
{
    private const int CustomerValidationErrorCode = 90032;
    private const int IdentityCardId = 1;
    private const int PassportId = 2;
    private const int PermitToStayId = 3;
    private const int ForeignIdentityCardId = 4;

    public IdentificationDocumentValidator()
    {
        RuleFor(document => document.ValidTo)
            .GreaterThan(DateTime.Today).WithErrorCode(CustomerValidationErrorCode);

        RuleFor(document => document.IssuedOn)
            .LessThanOrEqualTo(DateTime.Today).WithErrorCode(CustomerValidationErrorCode);

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