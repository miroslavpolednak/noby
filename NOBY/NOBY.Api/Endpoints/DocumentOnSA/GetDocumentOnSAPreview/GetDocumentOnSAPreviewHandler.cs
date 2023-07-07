using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using Newtonsoft.Json;
using NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSAData;

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
            Source.Noby => await HandleSourceNoby(documentOnSA, cancellationToken),
            Source.Workflow => await HandleSourceWorkflow(documentOnSA, cancellationToken),
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
        var documentDataList = JsonConvert.DeserializeObject<List<DocumentDataDto>>(documentOnSAData.Data);
        
        var generateDocumentRequest = new GenerateDocumentRequest
        {
            DocumentTypeId = documentOnSA.DocumentTypeId!.Value,
            DocumentTemplateVersionId = documentOnSA.DocumentTemplateVersionId!.Value,
            DocumentTemplateVariantId = documentOnSA.DocumentTemplateVariantId,
            ForPreview = true,
            OutputType = OutputFileType.Pdfa,
            Parts =
            {
                new GenerateDocumentPart
                {
                    DocumentTypeId = documentOnSA.DocumentTypeId!.Value,
                    DocumentTemplateVersionId = documentOnSA.DocumentTemplateVersionId!.Value,
                    DocumentTemplateVariantId = documentOnSA.DocumentTemplateVariantId,
                    Data =
                    {
                        documentDataList?.Select(d =>
                        {
                            var documentPartData = new GenerateDocumentPartData
                            {
                                Key = d.FieldName,
                                StringFormat = d.StringFormat,
                                TextAlign = (TextAlign)(d.TextAlign ?? 0)
                            };
                            
                            switch (d.ValueCase)
                            {
                                case 0:
                                    break;
                                case 3:
                                    documentPartData.Text = d.Text;
                                    break;
                                case 4:
                                    documentPartData.Date = new DateTime(d.Date!.Year, d.Date!.Month, d.Date!.Day);
                                    break;
                                case 5:
                                    documentPartData.Number = d.Number;
                                    break;
                                case 6:
                                    documentPartData.DecimalNumber = new GrpcDecimal(d.DecimalNumber!.Units, d.DecimalNumber!.Nanos);
                                    break;
                                case 7:
                                    documentPartData.LogicalValue = d.LogicalValue;
                                    break;
                                case 8:
                                    throw new NotSupportedException("GenericTable is not supported");
                                default:
                                    throw new NotSupportedException("Notsupported oneof object");
                            }
                            
                            return documentPartData;
                        })
                    }
                }
            },
            DocumentFooter = new DocumentFooter
            {
                SalesArrangementId = documentOnSA.SalesArrangementId,
                DocumentId = documentOnSA.EArchivId,
                BarcodeText = documentOnSA.FormId
            }
        };
        
        var document = await _documentGeneratorService.GenerateDocument(generateDocumentRequest, cancellationToken);

        return new GetDocumentOnSAPreviewResponse
        {
            FileData = document.Data.ToByteArray()
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
            FileData = previewResponse.BinaryData.ToArrayUnsafe()
        };
    }
    
    private readonly IDocumentOnSAServiceClient _documentOnSaService;
    private readonly IDocumentGeneratorServiceClient _documentGeneratorService;

    public GetDocumentOnSAPreviewHandler(
        IDocumentOnSAServiceClient documentOnSaService,
        IDocumentGeneratorServiceClient documentGeneratorService)
    {
        _documentOnSaService = documentOnSaService;
        _documentGeneratorService = documentGeneratorService;
    }
}