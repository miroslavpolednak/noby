using FluentValidation;

namespace NOBY.Api.Endpoints.DocumentArchive.SaveDocumentsToArchive;

public class SaveDocumentToArchiveRequestValidator : AbstractValidator<SaveDocumentsToArchiveRequest>
{
    public SaveDocumentToArchiveRequestValidator()
    {
        RuleForEach(t => t.DocumentsInformation).NotEmpty().ChildRules(ch =>
        {
            ch.RuleFor(t => t.Guid).NotNull().WithMessage($"{nameof(DocumentsInformation.Guid)} is required");
            ch.RuleFor(t => t.FileName).NotNull().WithMessage($"{nameof(DocumentsInformation.FileName)} is required");
            ch.RuleFor(t => t.EaCodeMainId).NotNull().WithMessage($"{nameof(DocumentsInformation.EaCodeMainId)} is required");
        });
    }
}
