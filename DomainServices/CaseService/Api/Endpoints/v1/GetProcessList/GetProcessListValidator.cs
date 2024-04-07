using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.v1.GetProcessList;

internal sealed class GetProcessListValidator
    : AbstractValidator<GetProcessListRequest>
{
    public GetProcessListValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty)
            .WithMessage(CIS.Core.ErrorCodes.ErrorCodeMapperBase.GetMessage(ErrorCodeMapper.CaseIdIsEmpty));
    }
}