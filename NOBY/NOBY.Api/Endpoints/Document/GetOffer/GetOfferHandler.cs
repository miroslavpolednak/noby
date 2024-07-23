using DomainServices.OfferService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.Document.SharedDto;

namespace NOBY.Api.Endpoints.Document.GetOffer;

internal sealed class GetOfferHandler : IRequestHandler<GetOfferRequest, ReadOnlyMemory<byte>>
{
    private readonly DocumentGenerator _documentGenerator;
    private readonly DocumentManager _documentManager;
    private readonly DocumentArchiveManager _documentArchiveManager;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;

    public GetOfferHandler(DocumentGenerator documentGenerator,
                           DocumentManager documentManager,
                           DocumentArchiveManager documentArchiveManager,
                           ISalesArrangementServiceClient salesArrangementService,
                           IOfferServiceClient offerService)
    {
        _documentGenerator = documentGenerator;
        _documentManager = documentManager;
        _documentArchiveManager = documentArchiveManager;
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
    }

    public async Task<ReadOnlyMemory<byte>> Handle(GetOfferRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.InputParameters.SalesArrangementId!.Value, cancellationToken);

        if (salesArrangement.SalesArrangementTypeId != (int)SalesArrangementTypes.Mortgage)
        {
            throw new NobyValidationException(90032, "SalesArrangementTypeId != 1");
        }

        var offer = await _offerService.GetOffer(salesArrangement.OfferId!.Value, cancellationToken);

        if (string.IsNullOrWhiteSpace(offer.Data.DocumentId))
        {
            request.InputParameters.CaseId = salesArrangement.CaseId;

            return await GenerateAndSaveOffer(request, salesArrangement.OfferId!.Value, salesArrangement.ContractNumber, cancellationToken);
        }

        return await _documentArchiveManager.GetDocument(offer.Data.DocumentId, request, cancellationToken);
    }

    private async Task<ReadOnlyMemory<byte>> GenerateAndSaveOffer(GetDocumentBaseRequest request, int offerId, string? contractNumber, CancellationToken cancellationToken)
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
            await _offerService.UpdateOffer(new DomainServices.OfferService.Contracts.UpdateOfferRequest
            {
                OfferId = offerId,
                DocumentId = documentId
            }, cancellationToken);
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