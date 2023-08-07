using FluentValidation;

namespace NOBY.Api.Endpoints.DocumentOnSA.Search;

public class SearchDocumentsOnSaValidator : AbstractValidator<SearchDocumentsOnSaRequest>
{
	public SearchDocumentsOnSaValidator()
	{
        RuleFor(e => e.EACodeMainId).NotNull().WithMessage($"{nameof(SearchDocumentsOnSaRequest.EACodeMainId)} is required");
    }
}
