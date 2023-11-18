using FluentValidation;
using NOBY.Dto;

namespace NOBY.Api.Endpoints.Customer.Shared;

internal class IdentificationDocumentValidator : AbstractValidator<IdentificationDocumentFull>
{
    public IdentificationDocumentValidator()
    {
        RuleFor(document => document.ValidTo).LessThan(DateTime.Today);
        RuleFor(document => document.IssuedOn).GreaterThan(DateTime.Today);

        When(document => document.IssuingCountryId == 16,
             () =>
             {
                 RuleFor(document => document.IdentificationDocumentTypeId).Must(typeId => typeId is 1 or 5);

                 When(document => document.IdentificationDocumentTypeId == 5,
                      () =>
                      {
                          RuleFor(document => document.Number).Matches(@"^(\d{9}|[a-zA-Z]{2}\s?\d{6})$");
                      });
             });
    }
}