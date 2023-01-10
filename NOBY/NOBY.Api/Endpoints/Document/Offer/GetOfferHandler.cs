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
        //var documentId = await _documentArchiveManager.GetDocumentId(salesArrangementId, cancellationToken);

        return await GenerateAndSaveOffer(request, salesArrangementId, cancellationToken);

        //TODO: DocumentArchiveService - GetDocument funguje z důvodu chybějícího propisu dokumentů do eArchivu.
        //if (string.IsNullOrWhiteSpace(documentId))
        //    return await GenerateAndSaveOffer(request, salesArrangementId, cancellationToken);

        //return await _documentArchiveManager.GetDocument(documentId, request, cancellationToken);
    }

    private async Task<ReadOnlyMemory<byte>> GenerateAndSaveOffer(GetDocumentBaseRequest request, int salesArrangementId, CancellationToken cancellationToken)
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
                FileName = await _documentManager.GetFileName(request, cancellationToken)
            };

            await _documentArchiveManager.SaveDocumentToArchive(salesArrangementId, archiveData, cancellationToken);
        }
    }

    private async Task<ReadOnlyMemory<byte>> GenerateOfferDocument(GetDocumentBaseRequest request, string documentId, CancellationToken cancellationToken)
    {
        var generateDocumentRequest = await _documentGenerator.CreateRequest(request);

        generateDocumentRequest.DocumentFooter.DocumentId = documentId;

        return await _documentGenerator.GenerateDocument(generateDocumentRequest, cancellationToken);
    }
}