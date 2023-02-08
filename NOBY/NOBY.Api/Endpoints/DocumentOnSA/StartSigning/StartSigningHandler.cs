using DomainServices.DocumentOnSAService.Clients;

namespace NOBY.Api.Endpoints.DocumentOnSA.StartSigning;

public class StartSigningHandler : IRequestHandler<StartSigningRequest, StartSigningResponse>
{
    private readonly IDocumentOnSAServiceClient _client;

    public StartSigningHandler(IDocumentOnSAServiceClient client)
    {
        _client = client;
    }

    public async Task<StartSigningResponse> Handle(StartSigningRequest request, CancellationToken cancellationToken)
    {
        var result = await _client.StartSigning(new()
        {
            DocumentTypeId = request.DocumentTypeId,
            SalesArrangementId = request.SalesArrangementId,
            SignatureMethodCode = request.SignatureMethodCode,
        }, cancellationToken);

        return MapToResponse(result);
    }

    private static StartSigningResponse MapToResponse(DomainServices.DocumentOnSAService.Contracts.StartSigningResponse result)
    {
        ArgumentNullException.ThrowIfNull(result.DocumentOnSa, nameof(result.DocumentOnSa));

        return new StartSigningResponse
        {
            DocumentOnSAId = result.DocumentOnSa.DocumentOnSAId,
            DocumentTypeId = result.DocumentOnSa.DocumentTypeId,
            FormId = result.DocumentOnSa.FormId,
            IsSigned = result.DocumentOnSa.IsSigned,
            SignatureMethodCode = result.DocumentOnSa.SignatureMethodCode,
        };
    }
}
