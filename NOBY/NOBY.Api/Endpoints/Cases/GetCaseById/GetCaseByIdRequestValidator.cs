using FluentValidation;

namespace NOBY.Api.Endpoints.Cases.GetCaseById;

internal sealed class GetCaseByIdRequestValidator
    : AbstractValidator<GetCaseByIdRequest>
{
    public GetCaseByIdRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0).WithMessage("CaseId must be > 0");
    }
}
