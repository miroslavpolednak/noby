using FluentValidation;

namespace DomainServices.CaseService.Api.Validators.CaseService;

internal class GetCaseDetailMediatrRequestValidator : AbstractValidator<Dto.CaseService.GetCaseDetailMediatrRequest>
{
    public GetCaseDetailMediatrRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13000");
    }
}
