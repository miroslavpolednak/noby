using DomainServices.CodebookService.Clients;
using FluentValidation;
using NOBY.Api.Endpoints.Customer.Shared;

namespace NOBY.Api.Endpoints.Customer.UpdateCustomerDetailWithChanges;

internal sealed class UpdateCustomerDetailWithChangesRequestValidator : AbstractValidator<UpdateCustomerDetailWithChangesRequest>
{
    private const int CustomerValidationErrorCode = 90032;

    public UpdateCustomerDetailWithChangesRequestValidator(ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.NaturalPerson!.ProfessionCategoryId)
            .NotNull()
            .MustAsync(async (categoryId, cancellationToken) => (await codebookService.ProfessionCategories(cancellationToken)).Any(t => t.IsValid && t.IsValidNoby && t.Id == categoryId))
            .When(t => t.NaturalPerson is not null);

        CustomerValidationExtensions.BirthDateValidation(RuleFor(r => r.NaturalPerson!.DateOfBirth)
                                                         .Cascade(CascadeMode.Stop)
                                                         .NotNull().WithErrorCode(CustomerValidationErrorCode), CustomerValidationErrorCode);

        When(r => !string.IsNullOrWhiteSpace(r.NaturalPerson!.BirthNumber),
             () =>
             {
                 CustomerValidationExtensions.BirthNumberValidation(RuleFor(r => r.NaturalPerson!.BirthNumber)
                                                                        .Cascade(CascadeMode.Stop), r => r.NaturalPerson!.DateOfBirth!.Value, CustomerValidationErrorCode);
             });

        When(r => r.IdentificationDocument is not null,
             () =>
             {
                 RuleFor(r => r.IdentificationDocument!).SetValidator(new IdentificationDocumentValidator());
             });

        When(r => r.Addresses is not null && r.Addresses!.Count != 0,
             () =>
             {
                 RuleForEach(r => r.Addresses!)
                     .Where(address => !string.IsNullOrWhiteSpace(address.PragueDistrict))
                     .Must(address => address.PragueDistrict!.StartsWith("Praha", StringComparison.OrdinalIgnoreCase));
             });
    }
}
