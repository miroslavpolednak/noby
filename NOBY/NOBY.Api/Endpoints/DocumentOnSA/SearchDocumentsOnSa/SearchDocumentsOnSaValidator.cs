using FluentValidation;

namespace NOBY.Api.Endpoints.DocumentOnSA.Search;

public class SearchDocumentsOnSaValidator : AbstractValidator<DocumentOnSaSearchDocumentsOnSaRequest>
{
	public SearchDocumentsOnSaValidator()
	{
        RuleFor(e => e.EaCodeMainId).NotNull().WithMessage($"{nameof(DocumentOnSaSearchDocumentsOnSaRequest.EaCodeMainId)} is required");
    }
}
