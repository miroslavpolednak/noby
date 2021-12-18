namespace FOMS.Api.Endpoints.SalesArrangement.Handlers;

internal class SavePartHandler
    : AsyncRequestHandler<Dto.SavePartRequest>
{
    protected override async Task Handle(Dto.SavePartRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Create Document Factory for SA #{salesArrangementId}", request.SalesArrangementId);
        var processor = await _documentFactory.CreateDocumentProcessor(request.SalesArrangementId);

        _logger.LogDebug("Save part {partId} of document", request.PartId);
        await processor.SavePart(request.PartId, request.PartData);
    }

    private readonly ILogger<SavePartHandler> _logger;
    private readonly DocumentProcessing.IDocumentProcessorFactory _documentFactory;

    public SavePartHandler(
        DocumentProcessing.IDocumentProcessorFactory documentFactory,
        ILogger<SavePartHandler> logger)
    {
        _logger = logger;
        _documentFactory = documentFactory;
    }
}
