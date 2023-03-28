using System.Globalization;
using CIS.Core;
using CIS.Core.Attributes;
using CIS.Core.Security;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Contracts;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Api.Endpoints.SendToCmp;
using DomainServices.SalesArrangementService.Contracts;
using Newtonsoft.Json;

namespace DomainServices.SalesArrangementService.Api.Services.Forms;

[ScopedService, SelfService]
internal class FormsDocumentService
{
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly IDocumentGeneratorServiceClient _documentGeneratorService;
    private readonly ICodebookServiceClients _codebookService;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IDateTime _dateTime;

    public FormsDocumentService(IDocumentOnSAServiceClient documentOnSAService,
                                IDocumentArchiveServiceClient documentArchiveService,
                                IDocumentGeneratorServiceClient documentGeneratorService,
                                ICodebookServiceClients codebookService,
                                ICurrentUserAccessor currentUserAccessor,
                                IDateTime dateTime)
    {
        _documentOnSAService = documentOnSAService;
        _documentArchiveService = documentArchiveService;
        _documentGeneratorService = documentGeneratorService;
        _codebookService = codebookService;
        _currentUserAccessor = currentUserAccessor;
        _dateTime = dateTime;
    }

    public async Task<CreateDocumentOnSAResponse> CreateFinalDocumentOnSa(int salesArrangementId, DynamicFormValues dynamicValue, CancellationToken cancellationToken)
    {
        var request = new CreateDocumentOnSARequest
        {
            SalesArrangementId = salesArrangementId,
            DocumentTypeId = dynamicValue.DocumentTypeId,
            FormId = await _documentOnSAService.GenerateFormId(new GenerateFormIdRequest { IsFormIdFinal = true }, cancellationToken),
            EArchivId = await _documentArchiveService.GenerateDocumentId(new GenerateDocumentIdRequest(), cancellationToken),
            IsFinal = true
        };

        return await _documentOnSAService.CreateDocumentOnSA(request, cancellationToken);
    }

    public async Task SaveFormToEArchive(GetEasFormResponse easFormResponse, SalesArrangement salesArrangement, IEnumerable<CreateDocumentOnSAResponse> createdFinalVersionOfDocOnSa, CancellationToken cancellationToken)
    {
        foreach (var documentOnSaResponse in createdFinalVersionOfDocOnSa)
        {
            var generatedDocument = await GenerateDocument(salesArrangement, documentOnSaResponse, cancellationToken);

            var request = new UploadDocumentRequest
            {
                BinaryData = generatedDocument.Data,
                SendDocumentOnly = false,
                Metadata = new DocumentMetadata
                {
                    CaseId = salesArrangement.CaseId,
                    DocumentId = documentOnSaResponse.DocumentOnSa.EArchivId,
                    FormId = documentOnSaResponse.DocumentOnSa.FormId,
                    EaCodeMainId = int.Parse(easFormResponse.Forms
                                                            .Single(s => s.DynamicFormValues.DocumentId == documentOnSaResponse.DocumentOnSa.EArchivId).DefaultValues.PasswordCode,
                                             CultureInfo.InvariantCulture),
                    Filename = await GetFileName(documentOnSaResponse.DocumentOnSa, cancellationToken),
                    CreatedOn = _dateTime.Now.Date,
                    ContractNumber = easFormResponse.ContractNumber,
                    AuthorUserLogin = _currentUserAccessor.User!.Id.ToString(CultureInfo.InvariantCulture)
                }
            };

            await _documentArchiveService.UploadDocument(request, cancellationToken);
        }
    }

    private async Task<Document> GenerateDocument(SalesArrangement salesArrangement, CreateDocumentOnSAResponse documentOnSaResponse, CancellationToken cancellationToken)
    {
        var documentOnSaData = await _documentOnSAService.GetDocumentOnSAData(documentOnSaResponse.DocumentOnSa.DocumentOnSAId!.Value, cancellationToken);

        var documentTemplateVersions = await _codebookService.DocumentTemplateVersions(cancellationToken);
        var documentTemplateVersion = documentTemplateVersions.Single(r => r.Id == documentOnSaData.DocumentTemplateVersionId).DocumentVersion;

        var generateDocumentRequest = CreateDocumentRequest(documentOnSaData, salesArrangement, documentTemplateVersion);

        return await _documentGeneratorService.GenerateDocument(generateDocumentRequest, cancellationToken);
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

    private async Task<string> GetFileName(DocumentOnSAToSign documentOnSa, CancellationToken cancellationToken)
    {
        var templates = await _codebookService.DocumentTypes(cancellationToken);
        var fileName = templates.First(t => t.Id == (int)documentOnSa.DocumentTypeId!).FileName;
        return $"{fileName}_{documentOnSa.DocumentOnSAId}_{_dateTime.Now.ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture)}.pdf";
    }

    private static GenerateDocumentRequest CreateDocumentRequest(GetDocumentOnSADataResponse documentOnSaData, SalesArrangement salesArrangement, string documentTemplateVersion)
    {
        return new GenerateDocumentRequest
        {
            DocumentTypeId = documentOnSaData.DocumentTypeId!.Value,
            DocumentTemplateVersion = documentTemplateVersion,
            ForPreview = false,
            OutputType = OutputFileType.Pdfa,
            Parts = { CreateDocPart(documentOnSaData, documentTemplateVersion) },
            DocumentFooter = CreateFooter(salesArrangement)
        };
    }

    private static GenerateDocumentPart CreateDocPart(GetDocumentOnSADataResponse documentOnSaData, string documentTemplateVersion)
    {
        var docPart = new GenerateDocumentPart
        {
            DocumentTypeId = documentOnSaData.DocumentTypeId!.Value,
            DocumentTemplateVersion = documentTemplateVersion
        };

        var documentDataDtos = JsonConvert.DeserializeObject<List<DocumentDataDto>>(documentOnSaData.Data);
        docPart.Data.AddRange(CreateData(documentDataDtos));
        return docPart;
    }

    private static DocumentFooter CreateFooter(SalesArrangement salesArrangement)
    {
        return new DocumentFooter
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            CaseId = salesArrangement.CaseId
        };
    }
}