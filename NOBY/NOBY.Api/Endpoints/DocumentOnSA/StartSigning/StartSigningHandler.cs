using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;

namespace NOBY.Api.Endpoints.DocumentOnSA.StartSigning;

public class StartSigningHandler : IRequestHandler<StartSigningRequest, StartSigningResponse>
{
    private readonly IDocumentOnSAServiceClient _client;
    private readonly ICodebookServiceClient _codebookServiceClient;

    public StartSigningHandler(
        IDocumentOnSAServiceClient client,
        ICodebookServiceClient codebookServiceClient)
    {
        _client = client;
        _codebookServiceClient = codebookServiceClient;
    }

    public async Task<StartSigningResponse> Handle(StartSigningRequest request, CancellationToken cancellationToken)
    {
        var result = await _client.StartSigning(new()
        {
            DocumentTypeId = request.DocumentTypeId,
            SalesArrangementId = request.SalesArrangementId,
            SignatureMethodCode = request.SignatureMethodCode,
        }, cancellationToken);

        return await MapToResponse(result, cancellationToken);
    }

    private async Task<StartSigningResponse> MapToResponse(DomainServices.DocumentOnSAService.Contracts.StartSigningResponse result, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(result.DocumentOnSa, nameof(result.DocumentOnSa));

        var documentTypes = await _codebookServiceClient.DocumentTypes(cancellationToken);
        var eACodeMains = await _codebookServiceClient.EaCodesMain(cancellationToken);
        var signatureStates = await _codebookServiceClient.SignatureStatesNoby(cancellationToken);

        return new StartSigningResponse
        {
            DocumentOnSAId = result.DocumentOnSa.DocumentOnSAId,
            DocumentTypeId = result.DocumentOnSa.DocumentTypeId,
            FormId = result.DocumentOnSa.FormId,
            IsSigned = result.DocumentOnSa.IsSigned,
            SignatureMethodCode = result.DocumentOnSa.SignatureMethodCode,
            SignatureState = DocumentOnSaMetadataManager.GetSignatureState(new()
            {
                DocumentOnSAId = result.DocumentOnSa.DocumentOnSAId,
                EArchivId = result.DocumentOnSa.EArchivId,
                IsSigned = result.DocumentOnSa.IsSigned
            },
            signatureStates
            ),
            EACodeMainItem = DocumentOnSaMetadataManager.GetEaCodeMainItem(result.DocumentOnSa.DocumentTypeId.GetValueOrDefault(), documentTypes, eACodeMains)
        };
    }
}
