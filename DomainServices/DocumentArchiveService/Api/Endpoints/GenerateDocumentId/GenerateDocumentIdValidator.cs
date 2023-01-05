using DomainServices.DocumentArchiveService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GenerateDocumentId;

internal sealed class GenerateDocumentIdValidator
    : AbstractValidator<GenerateDocumentIdRequest>
{
    public GenerateDocumentIdValidator()
    {
        RuleFor(t => t)
            .NotEmpty();

        When(t => t is not null, () =>
        {
            RuleFor(t => t.EnvironmentName)
                .IsInEnum()
                .WithMessage("Unknown EnvironmentName.").WithErrorCode("14009");
        });
    }
}
