using FluentValidation;

namespace NOBY.Api.Endpoints.RealEstateValuation.CreateRealEstateValuationAttachment;

internal sealed class CreateRealEstateValuationAttachmentRequestValidator
    : AbstractValidator<CreateRealEstateValuationAttachmentRequest>
{
    public CreateRealEstateValuationAttachmentRequestValidator()
    {
        RuleFor(t => t.File)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .Must(t => t!.Length > 0);
    }
}
