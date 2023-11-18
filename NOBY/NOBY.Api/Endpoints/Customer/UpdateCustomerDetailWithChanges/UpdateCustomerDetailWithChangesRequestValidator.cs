using DomainServices.CodebookService.Clients;
using FluentValidation;
using NOBY.Api.Endpoints.Customer.Shared;

namespace NOBY.Api.Endpoints.Customer.UpdateCustomerDetailWithChanges;

internal sealed class UpdateCustomerDetailWithChangesRequestValidator : AbstractValidator<UpdateCustomerDetailWithChangesRequest>
{
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
            .WithErrorCode(90032);

        RuleFor(r => r.NaturalPerson!.BirthNumber)
            .Cascade(CascadeMode.Stop)
            .BirthNumberValidation(r => r.NaturalPerson!.DateOfBirth!.Value)
            .WithErrorCode(90032);

        When(r => r.EmailAddress is not null,
             () =>
             {
                 RuleFor(r => r.EmailAddress!.EmailAddress).NotEmpty().EmailAddress().WithErrorCode(90032);
             });
    }
}
