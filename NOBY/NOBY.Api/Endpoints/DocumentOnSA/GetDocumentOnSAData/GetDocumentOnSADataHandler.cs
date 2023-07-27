using CIS.Core;
using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
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
            MainDocument = new _DocOnSaSource.MainDocument
            {
                DocumentOnSAId = documentOnSa.DocumentOnSAId!.Value,
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
        var generateDocumentRequest = DocumentOnSAExtensions.CreateGenerateDocumentRequest(documentOnSa, documentOnSaData, forPreview: false);

        var result = await _documentGeneratorServiceClient.GenerateDocument(generateDocumentRequest, cancellationToken);

        return new GetDocumentOnSADataResponse
        {
            ContentType = MediaTypeNames.Application.Pdf,
            Filename = await GetFileName(documentOnSa, cancellationToken),
            FileData = result.Data.ToArrayUnsafe()
        };
    }

    private async Task<string> GetFileName(DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign documentOnSa, CancellationToken cancellationToken)
    {
        var templates = await _codebookServiceClients.DocumentTypes(cancellationToken);
        var fileName = templates.First(t => t.Id == (int)documentOnSa.DocumentTypeId!).FileName;
        return $"{fileName}_{documentOnSa.DocumentOnSAId}_{_dateTime.Now.ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture)}.pdf";
    }
}
