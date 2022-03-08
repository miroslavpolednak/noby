using FluentValidation;

namespace FOMS.Api.Endpoints.Case.GetById;

internal class GetByIdRequestValidator
    : AbstractValidator<GetByIdRequest>
{
    public GetByIdRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0).WithMessage("CaseId must be > 0");
    }
}
