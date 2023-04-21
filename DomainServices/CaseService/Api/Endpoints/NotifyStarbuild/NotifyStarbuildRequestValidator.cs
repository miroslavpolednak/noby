using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.NotifyStarbuild;

internal sealed class NotifyStarbuildRequestValidator : AbstractValidator<LinkOwnerToCaseRequest>
{
    public NotifyStarbuildRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);
    }
}
