using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;

namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentsSignList;

public class GetDocumentsSignListHandler : IRequestHandler<GetDocumentsSignListRequest, GetDocumentsSignListResponse>
{
    private readonly IDocumentOnSAServiceClient _client;
    private readonly ICodebookServiceClient _codebookServiceClient;

    public GetDocumentsSignListHandler(
        IDocumentOnSAServiceClient client,
        ICodebookServiceClient codebookServiceClient)
    {
        _client = client;
        _codebookServiceClient = codebookServiceClient;
    }

    public async Task<GetDocumentsSignListResponse> Handle(GetDocumentsSignListRequest request, CancellationToken cancellationToken)
    {
        var result = await _client.GetDocumentsToSignList(request.SalesArrangementId, cancellationToken);

        return await MapToResponseAndFilter(result, cancellationToken);
    }

    private async Task<GetDocumentsSignListResponse> MapToResponseAndFilter(GetDocumentsToSignListResponse result, CancellationToken cancellationToken)
    {
        var documentTypes = await _codebookServiceClient.DocumentTypes(cancellationToken);
        var eACodeMains = await _codebookServiceClient.EaCodesMain(cancellationToken);
        var signatureStates = await _codebookServiceClient.SignatureStatesNoby(cancellationToken);

        var response = new GetDocumentsSignListResponse();
        response.Data = result.DocumentsOnSAToSign
            .Select(s => new GetDocumentsSignListData
            {
                DocumentOnSAId = s.DocumentOnSAId,
                DocumentTypeId = s.DocumentTypeId,
                FormId = s.FormId,
                IsSigned = s.IsSigned,
                SignatureMethodCode = s.SignatureMethodCode,
                SignatureTypeId = s.SignatureTypeId,
                SignatureDateTime = s.SignatureDateTime is not null ? s.SignatureDateTime.ToDateTime() : null,
                SignatureState = DocumentOnSaMetadataManager.GetSignatureState(new() { DocumentOnSAId = s.DocumentOnSAId, EArchivId = s.EArchivId, IsSigned = s.IsSigned }, signatureStates),
                EACodeMainItem = DocumentOnSaMetadataManager.GetEaCodeMainItem(s.DocumentTypeId.GetValueOrDefault(), documentTypes, eACodeMains)
            }).ToList();

        return response;
    }

}
