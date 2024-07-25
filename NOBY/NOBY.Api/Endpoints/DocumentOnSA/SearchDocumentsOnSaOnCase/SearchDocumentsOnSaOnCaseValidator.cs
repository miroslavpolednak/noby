using FluentValidation;

namespace NOBY.Api.Endpoints.DocumentOnSA.SearchDocumentsOnSaOnCase;

public class SearchDocumentsOnSaOnCaseValidator : AbstractValidator<DocumentOnSaSearchDocumentsOnSaOnCaseRequest>
{
    public SearchDocumentsOnSaOnCaseValidator()
    {
        RuleFor(e => e.EaCodeMainId).NotNull().WithMessage($"{nameof(DocumentOnSaSearchDocumentsOnSaOnCaseRequest.EaCodeMainId)} is required");
    }
}
