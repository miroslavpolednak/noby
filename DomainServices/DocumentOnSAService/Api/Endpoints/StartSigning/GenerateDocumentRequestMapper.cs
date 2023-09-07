using CIS.InternalServices.DocumentGeneratorService.Contracts;
using Newtonsoft.Json;
using __Entity = DomainServices.DocumentOnSAService.Api.Database.Entities;
namespace DomainServices.DocumentOnSAService.Api.Endpoints.StartSigning;

public static class GenerateDocumentRequestMapper
{
    public static GenerateDocumentRequest CreateGenerateDocumentRequest(__Entity.DocumentOnSa documentOnSa) => new()
    {
        DocumentTypeId = documentOnSa.DocumentTypeId!.Value,
        DocumentTemplateVersionId = documentOnSa.DocumentTemplateVersionId!.Value,
        DocumentTemplateVariantId = documentOnSa.DocumentTemplateVariantId,
        ForPreview = true,
        OutputType = OutputFileType.Pdfa,
        Parts = { CreateDocPart(documentOnSa) },
        DocumentFooter = CreateFooter(documentOnSa)
    };

    private static GenerateDocumentPart CreateDocPart(__Entity.DocumentOnSa documentOnSA)
    {
        var documentDataDtos = JsonConvert.DeserializeObject<List<DocumentDataDto>>(documentOnSA.Data!)!;

        return new GenerateDocumentPart
        {
            DocumentTypeId = documentOnSA.DocumentTypeId!.Value,
            DocumentTemplateVersionId = documentOnSA.DocumentTemplateVersionId!.Value,
            DocumentTemplateVariantId = documentOnSA.DocumentTemplateVariantId,
            Data = { documentDataDtos.CreateDocumentData() }
        };
    }

    private static DocumentFooter CreateFooter(__Entity.DocumentOnSa documentOnSa) => new()
    {
        CaseId = documentOnSa.CaseId,
        SalesArrangementId = documentOnSa.SalesArrangementId,
        DocumentOnSaId = documentOnSa.DocumentOnSAId,
        DocumentId = documentOnSa.EArchivId,
        BarcodeText = documentOnSa.FormId
    };
}
