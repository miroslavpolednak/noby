using System.Globalization;
using System.Net.Mime;
using CIS.Core;
using SharedTypes.Enums;
using SharedAudit;
using CIS.Infrastructure.gRPC;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using _Domain = DomainServices.DocumentOnSAService.Contracts;

namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSAPreview;

public class GetDocumentOnSAPreviewHandler : IRequestHandler<GetDocumentOnSAPreviewRequest, GetDocumentOnSAPreviewResponse>
{
    public async Task<GetDocumentOnSAPreviewResponse> Handle(GetDocumentOnSAPreviewRequest request, CancellationToken cancellationToken)
    {
        var documentsResponse = await _documentOnSaService.GetDocumentsOnSAList(request.SalesArrangementId, cancellationToken);
        var documentOnSA = documentsResponse.DocumentsOnSA.FirstOrDefault(d => d.DocumentOnSAId == request.DocumentOnSAId);

        if (documentOnSA is null)
        {
            throw new CisNotFoundException(NobyValidationException.DefaultExceptionCode, "DocumentOnSA does not exist on provided sales arrangement.");
        }

        var response = documentOnSA.Source switch
        {
            _Domain.Source.Noby => await HandleSourceNoby(documentOnSA, cancellationToken),
            _Domain.Source.Workflow => await HandleSourceWorkflow(documentOnSA, cancellationToken),
            _ => throw new NobyValidationException("Unsupported kind of document source")
        };

        _auditLogger.Log(
            AuditEventTypes.Noby010,
            "Dokument byl zobrazen v aplikaci",
            products: new List<AuditLoggerHeaderItem>
            {
                new(AuditConstants.ProductNamesSalesArrangement, documentOnSA.SalesArrangementId),
                new(AuditConstants.ProductNamesForm, documentOnSA.FormId)
            },
            bodyBefore: new Dictionary<string, string>
            {
                { "sha2", AuditLoggerHelpers.GenerateSHA2(response.FileData) },
                { "sha3", AuditLoggerHelpers.GenerateSHA3(response.FileData) }
            }
        );

        return response;
    }

    private async Task<GetDocumentOnSAPreviewResponse> HandleSourceNoby(
        DocumentOnSAToSign documentOnSA,
        CancellationToken cancellationToken)
    {
        if (documentOnSA.SignatureTypeId != (int)SignatureTypes.Electronic &&
            documentOnSA.SignatureTypeId != (int)SignatureTypes.Paper)
        {
            throw new NobyValidationException("Signature type not allowed.");
        }

        if (documentOnSA is { SignatureTypeId: (int)SignatureTypes.Electronic, IsValid: false })
        {
            throw new NobyValidationException("Invalid electronic document is not allowed.");
        }

        if (documentOnSA.SignatureTypeId == (int)SignatureTypes.Paper && ((documentOnSA.IsSigned && documentOnSA.EArchivIdsLinked.Any()) || !documentOnSA.IsValid))
        {
            throw new NobyValidationException("Signed or invalid paper document is not allowed.");
        }

        //Should be in the cache, so load it because we need CaseId
        var saValidationResult = await _salesArrangementService.ValidateSalesArrangementId(documentOnSA.SalesArrangementId, false, cancellationToken);

        var documentOnSAData = await _documentOnSaService.GetDocumentOnSAData(documentOnSA.DocumentOnSAId ?? 0, cancellationToken);
        var generateDocumentRequest = DocumentOnSAExtensions.CreateGenerateDocumentRequest(documentOnSA, documentOnSAData, saValidationResult.CaseId);
        var document = await _documentGeneratorService.GenerateDocument(generateDocumentRequest, cancellationToken);

        var templates = await _codebookService.DocumentTypes(cancellationToken);
        var fileName = templates.First(t => t.Id == (int)documentOnSA.DocumentTypeId!).FileName;

        return new GetDocumentOnSAPreviewResponse
        {
            FileData = document.Data.ToByteArray(),
            Filename = $"{fileName}_{documentOnSA.DocumentOnSAId}_{_dateTime.Now.ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture)}.pdf",
            ContentType = MediaTypeNames.Application.Pdf
        };
    }

    private async Task<GetDocumentOnSAPreviewResponse> HandleSourceWorkflow(
        DocumentOnSAToSign documentOnSA,
        CancellationToken cancellationToken)
    {
        if (documentOnSA.SignatureTypeId != (int)SignatureTypes.Electronic)
        {
            throw new NobyValidationException("Signature type not allowed.");
        }

        if (documentOnSA.IsSigned || !documentOnSA.IsValid)
        {
            throw new NobyValidationException("Signed or invalid electronic document is not allowed.");
        }

        var previewResponse = await _documentOnSaService.GetElectronicDocumentPreview(documentOnSA.DocumentOnSAId ?? 0, cancellationToken);

        return new GetDocumentOnSAPreviewResponse
        {
            FileData = previewResponse.BinaryData.ToArrayUnsafe(),
            Filename = previewResponse.Filename,
            ContentType = previewResponse.MimeType
        };
    }

    private readonly ICodebookServiceClient _codebookService;
    private readonly IDateTime _dateTime;
    private readonly IDocumentOnSAServiceClient _documentOnSaService;
    private readonly IDocumentGeneratorServiceClient _documentGeneratorService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IAuditLogger _auditLogger;


    public GetDocumentOnSAPreviewHandler(
        ICodebookServiceClient codebookService,
        IDateTime dateTime,
        IDocumentOnSAServiceClient documentOnSaService,
        IDocumentGeneratorServiceClient documentGeneratorService,
        ISalesArrangementServiceClient salesArrangementService,
        IAuditLogger auditLogger)
    {
        _codebookService = codebookService;
        _dateTime = dateTime;
        _documentOnSaService = documentOnSaService;
        _documentGeneratorService = documentGeneratorService;
        _salesArrangementService = salesArrangementService;
        _auditLogger = auditLogger;
    }
}