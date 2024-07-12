using FluentValidation;

namespace NOBY.Api.Endpoints.DocumentArchive.SaveDocumentsToArchive;

public class SaveDocumentToArchiveRequestValidator : AbstractValidator<DocumentArchiveSaveDocumentsToArchiveRequest>
{
    public SaveDocumentToArchiveRequestValidator()
    {
        RuleForEach(t => t.DocumentsInformation).NotEmpty().ChildRules(ch =>
        {
            ch.RuleFor(t => t.DocumentInformation.Guid).NotNull().WithMessage($"{nameof(DocumentArchiveDocumentsInformation.DocumentInformation.Guid)} is required");
            ch.RuleFor(t => t.DocumentInformation.EaCodeMainId).NotNull().WithMessage($"{nameof(DocumentArchiveDocumentsInformation.DocumentInformation.EaCodeMainId)} is required");
        });
    }
}
