using CIS.Infrastructure.gRPC.CisTypes;
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
        var documentDataDtos = JsonConvert.DeserializeObject<List<DocumentDataDto>>(documentOnSA.Data!);

        return new GenerateDocumentPart
        {
            DocumentTypeId = documentOnSA.DocumentTypeId!.Value,
            DocumentTemplateVersionId = documentOnSA.DocumentTemplateVersionId!.Value,
            DocumentTemplateVariantId = documentOnSA.DocumentTemplateVariantId,
            Data = { documentDataDtos.Map() }
        };
    }

    private static DocumentFooter CreateFooter(__Entity.DocumentOnSa documentOnSa) => new()
    {
        SalesArrangementId = documentOnSa.SalesArrangementId,
        DocumentId = documentOnSa.EArchivId,
        BarcodeText = documentOnSa.FormId
    };

    private static IEnumerable<GenerateDocumentPartData> Map(this IEnumerable<DocumentDataDto>? documentDataDtos) =>
        documentDataDtos == null
            ? Enumerable.Empty<GenerateDocumentPartData>()
            : documentDataDtos.Select(Map);

    private static GenerateDocumentPartData Map(this DocumentDataDto documentDataDto)
    {
        var documentPartData = new GenerateDocumentPartData
        {
            Key = documentDataDto.FieldName,
            StringFormat = documentDataDto.StringFormat,
            TextAlign = (TextAlign)(documentDataDto.TextAlign ?? 0)
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
