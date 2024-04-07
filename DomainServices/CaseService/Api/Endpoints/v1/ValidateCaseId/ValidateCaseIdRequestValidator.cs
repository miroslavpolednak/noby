using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.v1.ValidateCaseId;

internal sealed class ValidateCaseIdRequestValidator
    : AbstractValidator<GetProcessListRequest>
{
    public ValidateCaseIdRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty)
            .WithMessage(CIS.Core.ErrorCodes.ErrorCodeMapperBase.GetMessage(ErrorCodeMapper.CaseIdIsEmpty));
    }
}
