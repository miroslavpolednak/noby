using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.LinkOwnerToCase;

internal sealed class LinkOwnerToCaseRequestValidator 
    : AbstractValidator<LinkOwnerToCaseRequest>
{
    public LinkOwnerToCaseRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);

        RuleFor(t => t.CaseOwnerUserId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseOwnerIsEmpty);
    }
}
