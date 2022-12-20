using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.GetTaskList;

internal class GetTaskListRequestValidator : AbstractValidator<GetTaskListRequest>
{
    public GetTaskListRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("Case Id must be > 0").WithErrorCode("13016");
    }
}