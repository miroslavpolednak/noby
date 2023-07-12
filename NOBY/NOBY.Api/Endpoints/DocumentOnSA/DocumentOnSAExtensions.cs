using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.DocumentOnSAService.Contracts;
using Newtonsoft.Json;
using NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSAData;
using __Contracts = CIS.InternalServices.DocumentGeneratorService.Contracts;
using GetDocumentOnSADataResponse = DomainServices.DocumentOnSAService.Contracts.GetDocumentOnSADataResponse;

namespace NOBY.Api.Endpoints.DocumentOnSA;

public static class DocumentOnSAExtensions
{
    public static __Contracts.GenerateDocumentRequest CreateGenerateDocumentRequest(DocumentOnSAToSign documentOnSA, GetDocumentOnSADataResponse documentOnSAData) => new ()
    {
        DocumentTypeId = documentOnSA.DocumentTypeId!.Value,
        DocumentTemplateVersionId = documentOnSA.DocumentTemplateVersionId!.Value,
        DocumentTemplateVariantId = documentOnSA.DocumentTemplateVariantId,
        ForPreview = true,
        OutputType = __Contracts.OutputFileType.Pdfa,
        Parts = { documentOnSAData.CreateDocPart() },
        DocumentFooter = documentOnSA.CreateFooter()
    };

    private static __Contracts.GenerateDocumentPart CreateDocPart(this GetDocumentOnSADataResponse documentOnSaData)
    {
        var documentDataDtos = JsonConvert.DeserializeObject<List<DocumentDataDto>>(documentOnSaData.Data);
        
        return new __Contracts.GenerateDocumentPart
        {
            DocumentTypeId = documentOnSaData.DocumentTypeId!.Value,
            DocumentTemplateVersionId = documentOnSaData.DocumentTemplateVersionId!.Value,
            DocumentTemplateVariantId = documentOnSaData.DocumentTemplateVariantId,
            Data = { documentDataDtos.Map() }
        };
    }

    private static __Contracts.DocumentFooter CreateFooter(this DocumentOnSAToSign documentOnSa) => new()
    {
        SalesArrangementId = documentOnSa.SalesArrangementId,
        DocumentId = documentOnSa.EArchivId,
        BarcodeText = documentOnSa.FormId
    };

    private static IEnumerable<__Contracts.GenerateDocumentPartData> Map(this IEnumerable<DocumentDataDto>? documentDataDtos) =>
        documentDataDtos == null
            ? Enumerable.Empty<__Contracts.GenerateDocumentPartData>()
            : documentDataDtos.Select(Map);

    private static __Contracts.GenerateDocumentPartData Map(this DocumentDataDto documentDataDto)
    {
        var documentPartData = new __Contracts.GenerateDocumentPartData
        {
            Key = documentDataDto.FieldName,
            StringFormat = documentDataDto.StringFormat,
            TextAlign = (__Contracts.TextAlign)(documentDataDto.TextAlign ?? 0)
        };
                            
        switch (documentDataDto.ValueCase)
        {
            case 0:
                break;
            case 3:
                documentPartData.Text = documentDataDto.Text;
                break;
            case 4:
                documentPartData.Date = new DateTime(documentDataDto.Date!.Year, documentDataDto.Date!.Month, documentDataDto.Date!.Day);
                break;
            case 5:
                documentPartData.Number = documentDataDto.Number;
                break;
            case 6:
                documentPartData.DecimalNumber = new GrpcDecimal(documentDataDto.DecimalNumber!.Units, documentDataDto.DecimalNumber!.Nanos);
                break;
            case 7:
                documentPartData.LogicalValue = documentDataDto.LogicalValue;
                break;
            case 8:
                throw new NotSupportedException("GenericTable is not supported");
            default:
                throw new NotSupportedException("Notsupported oneof object");
        }
                            
        return documentPartData;
    }
}