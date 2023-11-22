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

        RuleFor(r => r.NaturalPerson!.DateOfBirth)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .BirthDateValidation()
            .WithErrorCode(CustomerValidationErrorCode);

        RuleFor(r => r.NaturalPerson!.BirthNumber)
            .Cascade(CascadeMode.Stop)
            .BirthNumberValidation(r => r.NaturalPerson!.DateOfBirth!.Value)
            .WithErrorCode(CustomerValidationErrorCode);
    }
}
