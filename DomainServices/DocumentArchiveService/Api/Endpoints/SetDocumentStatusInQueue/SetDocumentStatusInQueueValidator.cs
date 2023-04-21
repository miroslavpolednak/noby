using DomainServices.DocumentArchiveService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.SetDocumentStatusInQueue;

public class SetDocumentStatusInQueueValidator : AbstractValidator<SetDocumentStatusInQueueRequest>
{
    public SetDocumentStatusInQueueValidator()
    {
        RuleFor(t => t.EArchivId).NotEmpty();
        RuleFor(t => t.StatusInQueue).NotNull();

        RuleFor(t => t)
        .Must(ValidateState)
        .WithErrorCode(ErrorCodeMapper.StateInQueueNotAllowed);
    }

    private readonly Func<SetDocumentStatusInQueueRequest, bool> ValidateState = (request) =>
    {
        if (request.StatusInQueue == 302)
            return true;

        return false;
    };
}
