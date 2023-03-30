using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;

namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentsSignList;

public class GetDocumentsSignListHandler : IRequestHandler<GetDocumentsSignListRequest, GetDocumentsSignListResponse>
{
    private readonly IDocumentOnSAServiceClient _client;
    private readonly ICodebookServiceClients _codebookServiceClient;

    public GetDocumentsSignListHandler(
        IDocumentOnSAServiceClient client,
        ICodebookServiceClients codebookServiceClient)
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

        var response = new GetDocumentsSignListResponse();
        response.Data = result.DocumentsOnSAToSign
            .Where(r => r.IsValid == true)
            .Select(s => new GetDocumentsSignListData
            {
                DocumentOnSAId = s.DocumentOnSAId,
                DocumentTypeId = s.DocumentTypeId,
                FormId = s.FormId,
                IsSigned = s.IsSigned,
                SignatureMethodCode = s.SignatureMethodCode,
                SignatureDateTime = s.SignatureDateTime is not null ? s.SignatureDateTime.ToDateTime() : null,
                SignatureState = DocumentOnSaMetadataManager.GetSignatureState(new() { DocumentOnSAId = s.DocumentOnSAId, EArchivId = s.EArchivId, IsSigned = s.IsSigned }),
                EACodeMainItem = DocumentOnSaMetadataManager.GetEaCodeMainItem(s.DocumentTypeId.GetValueOrDefault(), documentTypes, eACodeMains)
            }).ToList();

        return response;
    }

}
