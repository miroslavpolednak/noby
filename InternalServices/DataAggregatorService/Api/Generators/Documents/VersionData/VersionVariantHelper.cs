﻿using CIS.Core.Exceptions;
using DomainServices.CodebookService.Contracts.v1;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.VersionData;

public static class VersionVariantHelper
{
    public static DocumentTemplateVariantsResponse.Types.DocumentTemplateVariantItem GetDocumentVariant(ICollection<DocumentTemplateVariantsResponse.Types.DocumentTemplateVariantItem> documentVariants, int documentVersionId, string documentVariant)
    {
        var variant = documentVariants.FirstOrDefault(v => v.DocumentTemplateVersionId == documentVersionId && v.DocumentVariant == documentVariant)
                      ?? throw new CisValidationException($"The template variant {documentVariant} does not exist for the template version {documentVersionId}");
       
        return variant;
    }

    public static void CheckIfDocumentVariantExists(ICollection<DocumentTemplateVariantsResponse.Types.DocumentTemplateVariantItem> documentVariants, int documentVersionId, int documentVariantId)
    {
        var exists = documentVariants.Any(v => v.DocumentTemplateVersionId == documentVersionId && v.Id == documentVariantId);

        if (exists)
            return;

        throw new CisValidationException($"The template variant ID {documentVariantId} does not exist for the template version {documentVersionId}");
    }
}