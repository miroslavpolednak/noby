using FluentValidation;

namespace NOBY.Api.Endpoints.DocumentOnSA.Search;

public class SearchValidator : AbstractValidator<SearchRequest>
{
	public SearchValidator()
	{
        RuleFor(e => e.EACodeMainId).NotNull().WithMessage($"{nameof(SearchRequest.EACodeMainId)} is required");
    }
}
