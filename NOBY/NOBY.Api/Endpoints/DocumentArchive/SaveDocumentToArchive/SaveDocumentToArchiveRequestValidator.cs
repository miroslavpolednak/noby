using FluentValidation;

namespace NOBY.Api.Endpoints.DocumentArchive.SaveDocumentToArchive;

public class SaveDocumentToArchiveRequestValidator : AbstractValidator<SaveDocumentToArchiveRequest>
{
    public SaveDocumentToArchiveRequestValidator()
    {
        RuleFor(t => t.Guid).NotNull().WithMessage($"{nameof(SaveDocumentToArchiveRequest.Guid)} is required");
        RuleFor(t => t.FileName).NotNull().WithMessage($"{nameof(SaveDocumentToArchiveRequest.FileName)} is required");
        RuleFor(t => t.EaCodeMainId).NotNull().WithMessage($"{nameof(SaveDocumentToArchiveRequest.EaCodeMainId)} is required");
    }
}
