using DomainServices.DocumentArchiveService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GenerateDocumentId;

internal sealed class GenerateDocumentIdMediatrRequestValidator
    : AbstractValidator<GenerateDocumentIdMediatrRequest>
{
    public GenerateDocumentIdMediatrRequestValidator()
    {
        RuleFor(t => t.Request)
            .NotEmpty();

        When(t => t.Request is not null, () =>
        {
            RuleFor(t => t.Request.EnvironmentName)
                .IsInEnum()
                .NotEqual(EnvironmentNames.Unknown).WithErrorCode("14009");

            RuleFor(t => t.Request.EnvironmentIndex)
                .LessThanOrEqualTo(9)
                .WithMessage("Environment Index must be less than 10").WithErrorCode("14010");
        });
    }
}
