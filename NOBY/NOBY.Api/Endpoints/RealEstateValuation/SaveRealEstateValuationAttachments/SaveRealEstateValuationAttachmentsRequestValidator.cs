using FluentValidation;

namespace NOBY.Api.Endpoints.RealEstateValuation.SaveRealEstateValuationAttachments;

internal sealed class SaveRealEstateValuationAttachmentsRequestValidator
    : AbstractValidator<SaveRealEstateValuationAttachmentsRequest>
{
    public SaveRealEstateValuationAttachmentsRequestValidator(DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.Attachments)
            .NotEmpty();

        RuleForEach(t => t.Attachments)
            .MustAsync(async (t, cancallationToken) => (await codebookService.AcvAttachmentCategories(cancallationToken)).Any(x => x.Id == t.AcvAttachmentCategoryId));
    }
}
