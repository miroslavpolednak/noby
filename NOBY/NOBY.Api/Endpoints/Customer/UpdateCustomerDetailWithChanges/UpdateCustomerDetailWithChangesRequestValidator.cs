using DomainServices.CodebookService.Clients;
using FluentValidation;

namespace NOBY.Api.Endpoints.Customer.UpdateCustomerDetailWithChanges;

internal sealed class UpdateCustomerDetailWithChangesRequestValidator
    : AbstractValidator<UpdateCustomerDetailWithChangesRequest>
{
    public UpdateCustomerDetailWithChangesRequestValidator(ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.NaturalPerson!.ProfessionCategoryId)
            .NotNull()
            .MustAsync(async (categoryId, cancellationToken) => (await codebookService.ProfessionCategories(cancellationToken)).Any(t => t.IsValid && t.IsValidNoby && t.Id == categoryId))
            .When(t => t.NaturalPerson is not null);
    }
}
