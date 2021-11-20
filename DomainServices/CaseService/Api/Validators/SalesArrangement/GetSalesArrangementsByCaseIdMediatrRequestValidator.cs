using FluentValidation;

namespace DomainServices.CaseService.Api.Validators.SalesArrangement;

internal class GetSalesArrangementsByCaseIdMediatrRequestValidator
    : AbstractValidator<Dto.SalesArrangement.GetSalesArrangementsByCaseIdMediatrRequest>
{
    public GetSalesArrangementsByCaseIdMediatrRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13000");
    }
}
