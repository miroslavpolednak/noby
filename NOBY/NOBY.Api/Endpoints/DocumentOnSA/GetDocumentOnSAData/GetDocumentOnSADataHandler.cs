using CIS.Core;
using CIS.Core.Security;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.UserService.Clients;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Mime;

namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSAData;

public class GetDocumentOnSADataHandler : IRequestHandler<GetDocumentOnSADataRequest, GetDocumentOnSADataResponse>
{
    private readonly IDocumentOnSAServiceClient _documentOnSaClient;
    private readonly IDocumentArchiveServiceClient _documentArchiveServiceClient;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IDocumentGeneratorServiceClient _documentGeneratorServiceClient;
    private readonly IUserServiceClient _userServiceClient;
    private readonly ICodebookServiceClients _codebookServiceClients;
    private readonly IDateTime _dateTime;

    public GetDocumentOnSADataHandler(
        IDocumentOnSAServiceClient documentOnSaClient,
        IDocumentArchiveServiceClient documentArchiveServiceClient,
        ICurrentUserAccessor currentUserAccessor,
        IDocumentGeneratorServiceClient documentGeneratorServiceClient,
        IUserServiceClient userServiceClient,
        ICodebookServiceClients codebookServiceClients,
        IDateTime dateTime)
    {
        _documentOnSaClient = documentOnSaClient;
        _documentArchiveServiceClient = documentArchiveServiceClient;
        _currentUserAccessor = currentUserAccessor;
        _documentGeneratorServiceClient = documentGeneratorServiceClient;
        _userServiceClient = userServiceClient;
        _codebookServiceClients = codebookServiceClients;
        _dateTime = dateTime;
    }

    public async Task<GetDocumentOnSADataResponse> Handle(GetDocumentOnSADataRequest request, CancellationToken cancellationToken)
    {
        var documentOnSas = await _documentOnSaClient.GetDocumentsToSignList(request.SalesArrangementId, cancellationToken);

        if (!documentOnSas.DocumentsOnSAToSign.Any(d => d.DocumentOnSAId == request.DocumentOnSAId))
        {
            throw new CisNotFoundException(ErrorCodes.DocumentOnSaNotExistForSalesArrangement, $"DocumetnOnSa {request.DocumentOnSAId} not exist for SalesArrangement {request.SalesArrangementId}");
        }

        var documentOnSa = documentOnSas.DocumentsOnSAToSign.Single(r => r.DocumentOnSAId == request.DocumentOnSAId);

        if (documentOnSa.IsDocumentArchived)
        {
            return await GetDocumentFromEArchive(documentOnSa, cancellationToken);
        }
        else
        {
            return await GetDocumentFromDocumentGenerator(documentOnSa, cancellationToken);
        }
    }

    private async Task<GetDocumentOnSADataResponse> GetDocumentFromEArchive(DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign documentOnSa, CancellationToken cancellationToken)
    {
        var user = _currentUserAccessor.User;

        var document = await _documentArchiveServiceClient.GetDocument(new()
        {
            DocumentId = documentOnSa.EArchivId,
            UserLogin = user is null ? "Unknow NOBY user" : user.Id.ToString(),
            WithContent = true

        }, cancellationToken);

        return new GetDocumentOnSADataResponse
        {
            ContentType = document.Content.MineType,
            Filename = document.Metadata.Filename,
            FileData = document.Content.BinaryData.ToArrayUnsafe()
        };
    }

    private async Task<GetDocumentOnSADataResponse> GetDocumentFromDocumentGenerator(DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign documentOnSa, CancellationToken cancellationToken)
    {
        var documentOnSaData = await _documentOnSaClient.GetDocumentOnSAData(documentOnSa.DocumentOnSAId!.Value, cancellationToken);

        var documentTemplateVersions = await _codebookServiceClients.DocumentTemplateVersions(cancellationToken);
        var documentTemplateVersion = documentTemplateVersions.Single(r => r.Id == documentOnSaData.DocumentTemplateVersionId).DocumentVersion;

        var generateDocumentRequest = CreateDocumentRequest(documentOnSa, documentOnSaData, documentTemplateVersion);

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
        return $"{fileName}_{documentOnSa.DocumentOnSAId}_{_dateTime.Now.ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture)}";
    }

    private static GenerateDocumentRequest CreateDocumentRequest(DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign documentOnSa, DomainServices.DocumentOnSAService.Contracts.GetDocumentOnSADataResponse documentOnSaData, string documentTemplateVersion)
    {
        var generateDocumentRequest = new GenerateDocumentRequest();
        generateDocumentRequest.DocumentTypeId = documentOnSaData.DocumentTypeId!.Value;
        generateDocumentRequest.DocumentTemplateVersion = documentTemplateVersion;
        generateDocumentRequest.OutputType = OutputFileType.Pdfa;
        generateDocumentRequest.Parts.Add(CreateDocPart(documentOnSaData, documentTemplateVersion));
        generateDocumentRequest.DocumentFooter = CreateFooter(documentOnSa);
        return generateDocumentRequest;
    }

    private static DocumentFooter CreateFooter(DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign documentOnSa)
    {
        return new DocumentFooter
        {
            SalesArrangementId = documentOnSa.SalesArrangementId
        };
    }

    private static GenerateDocumentPart CreateDocPart(DomainServices.DocumentOnSAService.Contracts.GetDocumentOnSADataResponse documentOnSaData, string documentTemplateVersion)
    {
        var docPart = new GenerateDocumentPart();
        docPart.DocumentTypeId = documentOnSaData.DocumentTypeId!.Value;
        docPart.DocumentTemplateVersion = documentTemplateVersion;
        var documentDataDtos = JsonConvert.DeserializeObject<List<DocumentDataDto>>(documentOnSaData.Data);
        docPart.Data.AddRange(CreateData(documentDataDtos));
        return docPart;
    }

    private static IEnumerable<GenerateDocumentPartData> CreateData(List<DocumentDataDto>? documentDataDtos)
    {
        ArgumentNullException.ThrowIfNull(documentDataDtos);

        foreach (var documentDataDto in documentDataDtos)
        {
            var documentPartData = new GenerateDocumentPartData();
            documentPartData.Key = documentDataDto.FieldName;

            documentPartData.StringFormat = documentDataDto.StringFormat;

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
}
