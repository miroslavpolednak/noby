using FluentValidation;
using NOBY.Infrastructure.Configuration;

namespace NOBY.Api.Endpoints.RealEstateValuation.CreateRealEstateValuationAttachment;

internal sealed class UploadRealEstateValuationAttachmentRequestValidator
    : AbstractValidator<UploadRealEstateValuationAttachmentRequest>
{
    public UploadRealEstateValuationAttachmentRequestValidator(AppConfiguration configuration)
    {
        RuleFor(t => t.File)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .Must(t => t!.Length > 0)
            .Must(file => file!.Length <= configuration.MaxFileSize * 1024 * 1024)
            .WithMessage($"The maximum file size {configuration.MaxFileSize}MB has been exceeded"); ;
    }
}
