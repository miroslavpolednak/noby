namespace FOMS.Api.Endpoints.SalesArrangement.Handlers;

internal class GetPartHandler
    : IRequestHandler<Dto.GetPartRequest, object>
{
    public async Task<object> Handle(Dto.GetPartRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Create Document Factory for SA #{salesArrangementId}", request.SalesArrangementId);
        var processor = await _documentFactory.CreateDocumentProcessor(request.SalesArrangementId);

        _logger.LogDebug("Get part {partId} of document", request.PartId);
        return await processor.GetPart(request.PartId);
    }

    private readonly ILogger<GetPartHandler> _logger;
    private readonly DocumentProcessing.IDocumentProcessorFactory _documentFactory;

    public GetPartHandler(
        DocumentProcessing.IDocumentProcessorFactory documentFactory,
        ILogger<GetPartHandler> logger)
    {
        _logger = logger;
        _documentFactory = documentFactory;
    }
}
