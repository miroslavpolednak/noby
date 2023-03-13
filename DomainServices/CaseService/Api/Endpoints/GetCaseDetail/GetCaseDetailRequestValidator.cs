using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.GetCaseDetail;

internal sealed class GetCaseDetailRequestValidator : AbstractValidator<GetCaseDetailRequest>
{
    public GetCaseDetailRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);
    }
}
