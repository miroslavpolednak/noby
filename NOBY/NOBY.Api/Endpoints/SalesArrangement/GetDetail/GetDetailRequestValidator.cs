using FluentValidation;

namespace NOBY.Api.Endpoints.SalesArrangement.GetDetail;

internal class GetDetailRequestValidator
    : AbstractValidator<GetDetailRequest>
{
    public GetDetailRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0");
    }
}