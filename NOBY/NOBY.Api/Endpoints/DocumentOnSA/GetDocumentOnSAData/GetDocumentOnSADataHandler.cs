using CIS.Core;
using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Mime;
using _DocOnSaSource = DomainServices.DocumentOnSAService.Contracts;

namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSAData;

public class GetDocumentOnSADataHandler : IRequestHandler<GetDocumentOnSADataRequest, GetDocumentOnSADataResponse>
{
    private readonly IDocumentOnSAServiceClient _documentOnSaClient;
    private readonly IDocumentGeneratorServiceClient _documentGeneratorServiceClient;
    private readonly ICodebookServiceClient _codebookServiceClients;
    private readonly IDateTime _dateTime;

    public GetDocumentOnSADataHandler(
        IDocumentOnSAServiceClient documentOnSaClient,
        IDocumentGeneratorServiceClient documentGeneratorServiceClient,
        ICodebookServiceClient codebookServiceClients,
        IDateTime dateTime)
    {
        _documentOnSaClient = documentOnSaClient;
        _documentGeneratorServiceClient = documentGeneratorServiceClient;
        _codebookServiceClients = codebookServiceClients;
        _dateTime = dateTime;
    }

    public async Task<GetDocumentOnSADataResponse> Handle(GetDocumentOnSADataRequest request, CancellationToken cancellationToken)
    {
        var documentOnSas = await _documentOnSaClient.GetDocumentsToSignList(request.SalesArrangementId, cancellationToken);

        if (!documentOnSas.DocumentsOnSAToSign.Any(d => d.DocumentOnSAId == request.DocumentOnSAId))
            throw new NobyValidationException($"DocumetnOnSa {request.DocumentOnSAId} not exist for SalesArrangement {request.SalesArrangementId}");

        var documentOnSa = documentOnSas.DocumentsOnSAToSign.Single(r => r.DocumentOnSAId == request.DocumentOnSAId);
        var documentOnSaData = await _documentOnSaClient.GetDocumentOnSAData(documentOnSa.DocumentOnSAId!.Value, cancellationToken);

        if (documentOnSaData.SignatureTypeId is not null && documentOnSaData.SignatureTypeId != (int)SignatureTypes.Paper)
            throw new NobyValidationException("Only paper signed documents can be generated");

        if (!documentOnSaData.IsValid)
            throw new NobyValidationException("Unable to generate document for invalid document");

        return documentOnSaData.Source switch
        {
            _DocOnSaSource.Source.Noby => await GetDocumentFromDocumentGenerator(documentOnSa, documentOnSaData, cancellationToken),
            _DocOnSaSource.Source.Workflow => await GetDocumentFromEQueue(documentOnSa, cancellationToken),
            _ => throw new NobyValidationException("Unsupported kind of document source")
        };
    }

    private async Task<GetDocumentOnSADataResponse> GetDocumentFromEQueue(_DocOnSaSource.DocumentOnSAToSign documentOnSa, CancellationToken cancellationToken)
    {
        var docData = await _documentOnSaClient.GetElectronicDocumentFromQueue(new()
        {
            DocumentOnSAId = documentOnSa.DocumentOnSAId!.Value,
            //ToDo fill in correct (other) params
            Attachment = new _DocOnSaSource.Attachment
            {
                AttachmentId = 1
            }
        }, cancellationToken);

        return new GetDocumentOnSADataResponse
        {
            ContentType = MediaTypeNames.Application.Pdf,
            Filename = await GetFileName(documentOnSa, cancellationToken),
            FileData = docData.BinaryData.ToArrayUnsafe()
        };
    }

    private async Task<GetDocumentOnSADataResponse> GetDocumentFromDocumentGenerator(_DocOnSaSource.DocumentOnSAToSign documentOnSa, _DocOnSaSource.GetDocumentOnSADataResponse documentOnSaData, CancellationToken cancellationToken)
    {
        var generateDocumentRequest = CreateDocumentRequest(documentOnSa, documentOnSaData);

        var resutl = await _documentGeneratorServiceClient.GenerateDocument(generateDocumentRequest, cancellationToken);

        return new GetDocumentOnSADataResponse
        {
            ContentType = MediaTypeNames.Application.Pdf,
            Filename = await GetFileName(documentOnSa, cancellationToken),
            FileData = resutl.Data.ToArrayUnsafe()
        };
    }

    private async Task<string> GetFileName(DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign documentOnSa, CancellationToken cancellationToken)
    {
        var templates = await _codebookServiceClients.DocumentTypes(cancellationToken);
        var fileName = templates.First(t => t.Id == (int)documentOnSa.DocumentTypeId!).FileName;
        return $"{fileName}_{documentOnSa.DocumentOnSAId}_{_dateTime.Now.ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture)}.pdf";
    }

    private static GenerateDocumentRequest CreateDocumentRequest(_DocOnSaSource.DocumentOnSAToSign documentOnSa, _DocOnSaSource.GetDocumentOnSADataResponse documentOnSaData)
    {
        var generateDocumentRequest = new GenerateDocumentRequest
        {
            DocumentTypeId = documentOnSaData.DocumentTypeId!.Value,
            DocumentTemplateVersionId = documentOnSaData.DocumentTemplateVersionId!.Value,
            DocumentTemplateVariantId = documentOnSaData.DocumentTemplateVariantId,
            ForPreview = false,
            OutputType = OutputFileType.Pdfa,
            Parts = { CreateDocPart(documentOnSaData) },
            DocumentFooter = CreateFooter(documentOnSa)
        };

        return generateDocumentRequest;
    }

    private static DocumentFooter CreateFooter(_DocOnSaSource.DocumentOnSAToSign documentOnSa)
    {
        return new DocumentFooter
        {
            SalesArrangementId = documentOnSa.SalesArrangementId,
            DocumentId = documentOnSa.EArchivId,
            BarcodeText = documentOnSa.FormId
        };
    }

    private static GenerateDocumentPart CreateDocPart(_DocOnSaSource.GetDocumentOnSADataResponse documentOnSaData)
    {
        var documentDataDtos = JsonConvert.DeserializeObject<List<DocumentDataDto>>(documentOnSaData.Data);

        return new GenerateDocumentPart
        {
            DocumentTypeId = documentOnSaData.DocumentTypeId!.Value,
            DocumentTemplateVersionId = documentOnSaData.DocumentTemplateVersionId!.Value,
            DocumentTemplateVariantId = documentOnSaData.DocumentTemplateVariantId,
            Data = { CreateData(documentDataDtos) }
        };
    }

    private static IEnumerable<GenerateDocumentPartData> CreateData(List<DocumentDataDto>? documentDataDtos)
    {
        ArgumentNullException.ThrowIfNull(documentDataDtos);

        foreach (var documentDataDto in documentDataDtos)
        {
            var documentPartData = new GenerateDocumentPartData
            {
                Key = documentDataDto.FieldName,
                StringFormat = documentDataDto.StringFormat,
                TextAlign = (TextAlign)(documentDataDto.TextAlign ?? 0)
            };

            switch (documentDataDto.ValueCase)
            {
                case 0: break; //Should be just a StringFormat, the DataAggregator sends only the necessary data 

                case 3:
                    documentPartData.Text = documentDataDto.Text ?? string.Empty;
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

            yield return documentPartData;
        }
    }
}
