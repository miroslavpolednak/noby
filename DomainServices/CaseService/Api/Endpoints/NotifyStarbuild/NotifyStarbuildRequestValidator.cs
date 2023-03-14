using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.NotifyStarbuild;

internal sealed class NotifyStarbuildRequestValidator : AbstractValidator<LinkOwnerToCaseRequest>
{
    public NotifyStarbuildRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13016");
    }
}
