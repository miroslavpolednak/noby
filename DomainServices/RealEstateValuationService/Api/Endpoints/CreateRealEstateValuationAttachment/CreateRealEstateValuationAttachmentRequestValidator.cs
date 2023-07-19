using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.CreateRealEstateValuationAttachment;

internal sealed class CreateRealEstateValuationAttachmentRequestValidator
    : AbstractValidator<CreateRealEstateValuationAttachmentRequest>
{
    public CreateRealEstateValuationAttachmentRequestValidator()
    {
        RuleFor(t => t.RealEstateValuationId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationIdEmpty);
    }
}
