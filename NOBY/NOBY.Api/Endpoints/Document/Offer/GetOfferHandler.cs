using NOBY.Api.Endpoints.Document.Shared;
using NOBY.Api.Endpoints.Document.Shared.DocumentIdManager;

namespace NOBY.Api.Endpoints.Document.Offer;

internal class GetOfferHandler : IRequestHandler<GetOfferRequest, ReadOnlyMemory<byte>>
{
    private readonly DocumentGenerator _documentGenerator;
    private readonly DocumentManager _documentManager;
    private readonly DocumentArchiveManager<OfferDocumentIdManager, int> _documentArchiveManager;

    public GetOfferHandler(DocumentGenerator documentGenerator,
                           DocumentManager documentManager,
                           DocumentArchiveManager<OfferDocumentIdManager, int> documentArchiveManager)
    {
        _documentGenerator = documentGenerator;
        _documentManager = documentManager;
        _documentArchiveManager = documentArchiveManager;
    }

    public async Task<ReadOnlyMemory<byte>> Handle(GetOfferRequest request, CancellationToken cancellationToken)
    {
        var salesArrangementId = request.InputParameters.SalesArrangementId!.Value;
        var documentInfo = await _documentArchiveManager.GetDocumentInfo(salesArrangementId, cancellationToken);

        if (string.IsNullOrWhiteSpace(documentInfo.DocumentId))
            return await GenerateAndSaveOffer(request, salesArrangementId, documentInfo.ContractNumber, cancellationToken);

        return await _documentArchiveManager.GetDocument(documentInfo.DocumentId, request, cancellationToken);
    }

    private async Task<ReadOnlyMemory<byte>> GenerateAndSaveOffer(GetDocumentBaseRequest request, int salesArrangementId, string? contractNumber, CancellationToken cancellationToken)
    {
        var documentId = await _documentArchiveManager.GenerateDocumentId(cancellationToken);

        var documentData = await GenerateOfferDocument(request, documentId, cancellationToken);

        await SaveDocument();

        return documentData;

        async Task SaveDocument()
        {
            var archiveData = new DocumentArchiveData
            {
                DocumentId = documentId,
                CaseId = request.InputParameters.CaseId!.Value,
                UserId = _documentManager.UserId,
                DocumentData = documentData,
                FileName = await _documentManager.GetFileName(request, cancellationToken),
                DocumentTypeId = (int)request.DocumentType,
                ContractNumber = contractNumber
            };

            await _documentArchiveManager.SaveDocumentToArchive(salesArrangementId, archiveData, cancellationToken);
        }
    }

    private async Task<ReadOnlyMemory<byte>> GenerateOfferDocument(GetDocumentBaseRequest request, string documentId, CancellationToken cancellationToken)
    {
        var generateDocumentRequest = await _documentGenerator.CreateRequest(request, cancellationToken);

        generateDocumentRequest.DocumentFooter.DocumentId = documentId;

        return await _documentGenerator.GenerateDocument(generateDocumentRequest, cancellationToken);
    }
}