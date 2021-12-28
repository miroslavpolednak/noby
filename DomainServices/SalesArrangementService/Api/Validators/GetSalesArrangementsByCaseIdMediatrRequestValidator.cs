using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class GetSalesArrangementsByCaseIdMediatrRequestValidator
    : AbstractValidator<Dto.GetSalesArrangementsByCaseIdMediatrRequest>
{
    public GetSalesArrangementsByCaseIdMediatrRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13000");
    }
}
