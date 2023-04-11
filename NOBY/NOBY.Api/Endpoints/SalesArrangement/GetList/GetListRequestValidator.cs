using FluentValidation;

namespace NOBY.Api.Endpoints.SalesArrangement.GetList;

internal sealed class GetListRequestValidator
    : AbstractValidator<GetListRequest>
{
    public GetListRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0).WithMessage("CaseId must be > 0");
    }
}