using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.ValidateCaseId;

internal sealed class ValidateCaseIdRequestValidator
    : AbstractValidator<GetProcessListRequest>
{
    public ValidateCaseIdRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty)
            .WithMessage(ErrorCodeMapper.GetMessage(ErrorCodeMapper.CaseIdIsEmpty));
    }
}
