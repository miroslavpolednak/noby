using FluentValidation;

namespace NOBY.Api.Endpoints.DocumentArchive.SaveDocumentsToArchive;

public class SaveDocumentToArchiveRequestValidator : AbstractValidator<SaveDocumentsToArchiveRequest>
{
    public SaveDocumentToArchiveRequestValidator()
    {
        RuleForEach(t => t.DocumentsInformation).NotEmpty().ChildRules(ch =>
        {
            ch.RuleFor(t => t.DocumentInformation.Guid).NotNull().WithMessage($"{nameof(DocumentsInformation.DocumentInformation.Guid)} is required");
            ch.RuleFor(t => t.DocumentInformation.FileName).NotNull().WithMessage($"{nameof(DocumentsInformation.DocumentInformation.FileName)} is required");
            ch.RuleFor(t => t.DocumentInformation.EaCodeMainId).NotNull().WithMessage($"{nameof(DocumentsInformation.DocumentInformation.EaCodeMainId)} is required");
        });
    }
}
