using DomainServices.DocumentOnSAService.Clients;

namespace NOBY.Api.Endpoints.DocumentOnSA.StopSigning;

public class StopSigningHandler : IRequestHandler<StopSigningRequest>
{
    private readonly IDocumentOnSAServiceClient _client;

    public StopSigningHandler(IDocumentOnSAServiceClient client)
    {
        _client = client;
    }

    public async Task<Unit> Handle(StopSigningRequest request, CancellationToken cancellationToken)
    {
        var documentOnSas = await _client.GetDocumentsToSignList(request.SalesArrangementId, cancellationToken);

        if (!documentOnSas.DocumentsOnSAToSign.Any(d => d.DocumentOnSAId == request.DocumentOnSAId))
        {
            throw new CisNotFoundException(ErrorCodes.DocumentOnSaNotExistForSalesArrangement, $"DocumetnOnSa {request.DocumentOnSAId} not exist for SalesArrangement {request.SalesArrangementId}");
        }

        await _client.StopSigning(request.DocumentOnSAId, cancellationToken);

        return Unit.Value;
    }
}
