using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.GetCaseDetail;

internal class GetCaseDetailRequestValidator : AbstractValidator<GetCaseDetailRequest>
{
    public GetCaseDetailRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("Case Id must be > 0").WithErrorCode("13016");
    }
}
