using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.v1.GetTaskList;

internal sealed class GetTaskListRequestValidator
    : AbstractValidator<GetTaskListRequest>
{
    public GetTaskListRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);
    }
}