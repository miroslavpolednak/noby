using FluentValidation;
using NOBY.Infrastructure.Configuration;

namespace NOBY.Api.Endpoints.DocumentArchive.UploadDocument;

public class UploadDocumentValidator : AbstractValidator<UploadDocumentRequest>
{
    public UploadDocumentValidator(AppConfiguration configuration)
    {
        RuleFor(r => r).Must(ValidateFile).WithMessage("No file has been uploaded");
        RuleFor(r => r.File).Must(file => file.Length <= configuration.MaxFileSize * 1024 * 1024).WithMessage($"The maximum file size {configuration.MaxFileSize}MB has been exceeded");
    }


    private readonly Func<UploadDocumentRequest, bool> ValidateFile = (request) =>
    {
        if (request.File is null || request.File.Length <= 0)
            return false;

        return true;
    };
}
