using FluentValidation;

namespace NOBY.Api.Endpoints.Cases.GetById;

internal sealed class GetByIdRequestValidator
    : AbstractValidator<GetByIdRequest>
{
    public GetByIdRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0).WithMessage("CaseId must be > 0");
    }
}
