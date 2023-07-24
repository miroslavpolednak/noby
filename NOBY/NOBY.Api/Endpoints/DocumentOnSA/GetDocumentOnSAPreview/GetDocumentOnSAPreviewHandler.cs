using System.Globalization;
using System.Net.Mime;
using CIS.Core;
using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
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
            throw new CisNotFoundException(90001, "DocumentOnSA does not exist on provided sales arrangement.");
        }

        return documentOnSA.Source switch
        {
            _Domain.Source.Noby => await HandleSourceNoby(documentOnSA, cancellationToken),
            _Domain.Source.Workflow => await HandleSourceWorkflow(documentOnSA, cancellationToken),
            _ => throw new NobyValidationException("Unsupported kind of document source")
        };
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

        if (documentOnSA.SignatureTypeId == (int)SignatureTypes.Paper && (documentOnSA.IsSigned || !documentOnSA.IsValid))
        {
            throw new NobyValidationException("Signed or invalid paper document is not allowed.");
        }

        var documentOnSAData = await _documentOnSaService.GetDocumentOnSAData(documentOnSA.DocumentOnSAId ?? 0, cancellationToken);
        var generateDocumentRequest = DocumentOnSAExtensions.CreateGenerateDocumentRequest(documentOnSA, documentOnSAData);
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
    

    public GetDocumentOnSAPreviewHandler(
        ICodebookServiceClient codebookService,
        IDateTime dateTime,
        IDocumentOnSAServiceClient documentOnSaService,
        IDocumentGeneratorServiceClient documentGeneratorService)
    {
        _codebookService = codebookService;
        _dateTime = dateTime;
        _documentOnSaService = documentOnSaService;
        _documentGeneratorService = documentGeneratorService;
    }
}