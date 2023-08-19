using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.DeleteRealEstateValuationAttachment;

internal sealed class DeleteRealEstateValuationAttachmentRequestValidator
    : AbstractValidator<DeleteRealEstateValuationAttachmentRequest>
{
    public DeleteRealEstateValuationAttachmentRequestValidator()
    {
        RuleFor(t => t.RealEstateValuationAttachmentId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationAttachmentIdIsEmpty);
    }
}
