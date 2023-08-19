using FluentValidation;

namespace NOBY.Api.Endpoints.DocumentArchive.GetDocument;

public class GetDocumentRequestValidator: AbstractValidator<GetDocumentRequest>
{
    public GetDocumentRequestValidator()
    {
        When(t => t.Source == Source.EArchive, () =>
        {
            RuleFor(t => t.DocumentId)
                .NotEmpty();
        });
        
        When(t => t.Source == Source.ESignature, () =>
        {
            RuleFor(t => t.ExternalId)
                .NotEmpty();
        });
    }
}