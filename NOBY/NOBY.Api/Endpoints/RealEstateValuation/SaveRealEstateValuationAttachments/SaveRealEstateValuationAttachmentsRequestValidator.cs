using FluentValidation;

namespace NOBY.Api.Endpoints.RealEstateValuation.SaveRealEstateValuationAttachments;

internal sealed class SaveRealEstateValuationAttachmentsRequestValidator
    : AbstractValidator<SaveRealEstateValuationAttachmentsRequest>
{
    public SaveRealEstateValuationAttachmentsRequestValidator()
    {
        RuleFor(t => t.Attachments)
            .NotEmpty();
    }
}
