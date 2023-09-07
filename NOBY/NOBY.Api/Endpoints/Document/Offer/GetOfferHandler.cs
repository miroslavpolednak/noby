using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.Document.Shared;

namespace NOBY.Api.Endpoints.Document.Offer;

internal sealed class GetOfferHandler : IRequestHandler<GetOfferRequest, ReadOnlyMemory<byte>>
{
    private readonly DocumentGenerator _documentGenerator;
    private readonly DocumentManager _documentManager;
    private readonly DocumentArchiveManager _documentArchiveManager;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public GetOfferHandler(DocumentGenerator documentGenerator,
                           DocumentManager documentManager,
                           DocumentArchiveManager documentArchiveManager,
                           ISalesArrangementServiceClient salesArrangementService)
    {
        _documentGenerator = documentGenerator;
        _documentManager = documentManager;
        _documentArchiveManager = documentArchiveManager;
        _salesArrangementService = salesArrangementService;
    }

    public async Task<ReadOnlyMemory<byte>> Handle(GetOfferRequest request, CancellationToken cancellationToken)
    {
        var salesArrangementId = request.InputParameters.SalesArrangementId!.Value;

        var salesArrangement = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);

        if (string.IsNullOrWhiteSpace(salesArrangement.OfferDocumentId))
            return await GenerateAndSaveOffer(request, salesArrangementId, salesArrangement.ContractNumber, cancellationToken);

        return await _documentArchiveManager.GetDocument(salesArrangement.OfferDocumentId, request, cancellationToken);
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

            await _documentArchiveManager.SaveDocumentToArchive(archiveData, cancellationToken);
            await _salesArrangementService.UpdateOfferDocumentId(salesArrangementId, documentId, cancellationToken);
        }
    }

    private async Task<ReadOnlyMemory<byte>> GenerateOfferDocument(GetDocumentBaseRequest request, string documentId, CancellationToken cancellationToken)
    {
        var generateDocumentRequest = await _documentGenerator.CreateRequest(request, cancellationToken);

        generateDocumentRequest.DocumentFooter.DocumentId = documentId;
        generateDocumentRequest.DocumentFooter.SalesArrangementId = request.InputParameters.SalesArrangementId;

        return await _documentGenerator.GenerateDocument(generateDocumentRequest, cancellationToken);
    }
}