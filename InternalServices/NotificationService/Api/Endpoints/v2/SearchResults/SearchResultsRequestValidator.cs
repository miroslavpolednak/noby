using CIS.InternalServices.NotificationService.Api.Validators;
using CIS.InternalServices.NotificationService.Contracts.v2;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SearchResults;

internal sealed class SearchResultsRequestValidator
    : AbstractValidator<SearchResultsRequest>
{
    public SearchResultsRequestValidator()
    {
        RuleFor(request => request)
            .Must(request =>
                request.CaseId.HasValue ||
                !string.IsNullOrEmpty(request.CustomId) ||
                !string.IsNullOrEmpty(request.DocumentId) ||
                request.Identifier is not null)
                .WithErrorCode(ErrorCodeMapper.AtLeastOneParameterRequired);

        RuleFor(request => request.Identifier)
            .SetValidator(new IdentifierValidator())
            .When(t => t.Identifier is not null)
            .WithErrorCode(ErrorCodeMapper.IdentifierInvalid);

        When(request => request.CaseId.HasValue, () =>
        {
            RuleFor(request => request.CaseId!.Value)
                .GreaterThan(0)
                    .WithErrorCode(ErrorCodeMapper.CaseIdInvalid);
        });

        When(request => request.CustomId is not null, () =>
        {
            RuleFor(request => request.CustomId!)
                .SetValidator(new CustomIdValidator())
                    .WithErrorCode(ErrorCodeMapper.CustomIdInvalid);
        });

        When(request => request.DocumentId is not null, () =>
        {
            RuleFor(request => request.DocumentId!)
                .SetValidator(new DocumentIdValidator())
                    .WithErrorCode(ErrorCodeMapper.DocumentIdInvalid);
        });
    }
}
