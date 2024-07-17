using FluentValidation;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateComment;

internal sealed class UpdateCommentRequestValidator 
    : AbstractValidator<SalesArrangementUpdateCommentRequest>
{
    public UpdateCommentRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0");

        RuleFor(t => t.Text)
            .MaximumLength(500);
    }
}