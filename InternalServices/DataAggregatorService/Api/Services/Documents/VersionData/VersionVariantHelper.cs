using CIS.Core.Exceptions;
using DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateVariants;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.VersionData;

public static class VersionVariantHelper
{
    public static DocumentTemplateVariantItem GetDocumentVariant(ICollection<DocumentTemplateVariantItem> documentVariants, int documentVersionId, string documentVariant)
    {
        var variant = documentVariants.FirstOrDefault(v => v.DocumentTemplateVersionId == documentVersionId && v.DocumentVariant == documentVariant)
                      ?? throw new CisValidationException($"The template variant {documentVariant} does not exist for the template version {documentVersionId}");
       
        return variant;
    }

    public static void CheckIfDocumentVariantExists(ICollection<DocumentTemplateVariantItem> documentVariants, int documentVersionId, int documentVariantId)
    {
        var exists = documentVariants.Any(v => v.DocumentTemplateVersionId == documentVersionId && v.Id == documentVariantId);

        if (exists)
            return;

        throw new CisValidationException($"The template variant ID {documentVariantId} does not exist for the template version {documentVersionId}");
    }
}