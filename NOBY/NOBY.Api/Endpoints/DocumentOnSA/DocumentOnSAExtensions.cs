using CIS.InternalServices.DocumentGeneratorService.Contracts;
using DomainServices.DocumentOnSAService.Contracts;
using Newtonsoft.Json;
using GetDocumentOnSADataResponse = DomainServices.DocumentOnSAService.Contracts.GetDocumentOnSADataResponse;

namespace NOBY.Api.Endpoints.DocumentOnSA;

public static class DocumentOnSAExtensions
{
    public static GenerateDocumentRequest CreateGenerateDocumentRequest(DocumentOnSAToSign documentOnSA, GetDocumentOnSADataResponse documentOnSAData, bool forPreview = true) => new ()
    {
        DocumentTypeId = documentOnSA.DocumentTypeId!.Value,
        DocumentTemplateVersionId = documentOnSA.DocumentTemplateVersionId!.Value,
        DocumentTemplateVariantId = documentOnSA.DocumentTemplateVariantId,
        ForPreview = forPreview,
        OutputType = OutputFileType.Pdfa,
        Parts = { documentOnSAData.CreateDocPart() },
        DocumentFooter = documentOnSA.CreateFooter()
    };

    private static GenerateDocumentPart CreateDocPart(this GetDocumentOnSADataResponse documentOnSaData)
    {
        var documentDataDtos = JsonConvert.DeserializeObject<List<DocumentDataDto>>(documentOnSaData.Data)!;
        
        return new GenerateDocumentPart
        {
            DocumentTypeId = documentOnSaData.DocumentTypeId!.Value,
            DocumentTemplateVersionId = documentOnSaData.DocumentTemplateVersionId!.Value,
            DocumentTemplateVariantId = documentOnSaData.DocumentTemplateVariantId,
            Data = { documentDataDtos.CreateDocumentData() }
        };
    }

    private static DocumentFooter CreateFooter(this DocumentOnSAToSign documentOnSa) => new()
    {
        CaseId = documentOnSa.CaseId,
        SalesArrangementId = documentOnSa.SalesArrangementId,
        DocumentOnSaId = documentOnSa.DocumentOnSAId,
        DocumentId = documentOnSa.EArchivId,
        BarcodeText = documentOnSa.FormId
    };
}