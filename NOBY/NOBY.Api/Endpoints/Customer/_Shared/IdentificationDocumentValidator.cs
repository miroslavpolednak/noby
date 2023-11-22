using FluentValidation;
using NOBY.Dto;

namespace NOBY.Api.Endpoints.Customer.Shared;

internal class IdentificationDocumentValidator : AbstractValidator<IdentificationDocumentFull>
{
    private const int IdentityCardId = 1;
    private const int PassportId = 2;
    private const int PermitToStayId = 3;
    private const int ForeignIdentityCardId = 4;

    public IdentificationDocumentValidator()
    {
        RuleFor(document => document.ValidTo).LessThan(DateTime.Today);
        RuleFor(document => document.IssuedOn).GreaterThan(DateTime.Today);

        When(document => document.IssuingCountryId == 16,
             () =>
             {
                 RuleFor(document => document.IdentificationDocumentTypeId).Must(typeId => typeId is IdentityCardId or PassportId or PermitToStayId);

                 When(document => document.IdentificationDocumentTypeId == IdentityCardId,
                      () =>
                      {
                          RuleFor(document => document.Number).Matches(@"^(\d{9}|[a-zA-Z]{2}\s?\d{6})$");
                      });
             })
            .Otherwise(() =>
            {
                RuleFor(document => document.IdentificationDocumentTypeId).Must(typeId => typeId is PassportId or ForeignIdentityCardId);
            });
    }
}