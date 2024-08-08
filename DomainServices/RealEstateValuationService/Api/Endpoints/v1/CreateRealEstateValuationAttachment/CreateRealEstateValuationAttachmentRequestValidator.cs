using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.CreateRealEstateValuationAttachment;

internal sealed class CreateRealEstateValuationAttachmentRequestValidator
    : AbstractValidator<CreateRealEstateValuationAttachmentRequest>
{
    public CreateRealEstateValuationAttachmentRequestValidator()
    {
        RuleFor(t => t.RealEstateValuationId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationIdEmpty);

        RuleFor(t => t.FileData)
            .NotNull()
            .Must(t => t.Length > 0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationAttachmentIsEmpty);
    }
}
