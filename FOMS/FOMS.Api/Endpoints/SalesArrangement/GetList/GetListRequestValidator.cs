using FluentValidation;

namespace FOMS.Api.Endpoints.SalesArrangement.Validators;

internal class GetListRequestValidator
    : AbstractValidator<Dto.GetListRequest>
{
    public GetListRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0");
    }
}