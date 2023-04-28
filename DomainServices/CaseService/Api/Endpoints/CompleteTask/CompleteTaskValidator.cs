using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.CompleteTask;

internal class CompleteTaskValidator : AbstractValidator<CompleteTaskRequest>
{
    public CompleteTaskValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);
    }
}