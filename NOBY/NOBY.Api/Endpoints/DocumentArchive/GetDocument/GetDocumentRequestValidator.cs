using FluentValidation;

namespace NOBY.Api.Endpoints.DocumentArchive.GetDocument;

public class GetDocumentRequestValidator: AbstractValidator<GetDocumentRequest>
{
    public GetDocumentRequestValidator()
    {
        When(t => t.Source == EnumDocumentSource.EArchive, () =>
        {
            RuleFor(t => t.DocumentId)
                .NotEmpty();
        });
        
        When(t => t.Source == EnumDocumentSource.SbAttachment, () =>
        {
            RuleFor(t => t.ExternalId)
                .NotEmpty();
        });

        When(t => t.Source == EnumDocumentSource.SbDocument, () =>
        {
            RuleFor(t => t.ExternalId)
                .NotEmpty();
        });
    }
}