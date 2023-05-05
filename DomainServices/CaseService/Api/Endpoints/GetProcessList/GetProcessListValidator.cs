using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.GetProcessList;

internal class GetProcessListValidator : AbstractValidator<GetProcessListRequest>
{
    public GetProcessListValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty)
            .WithMessage(ErrorCodeMapper.GetMessage(ErrorCodeMapper.CaseIdIsEmpty));
    }
}