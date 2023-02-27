using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;

namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentsSignList;

public class GetDocumentsSignListHandler : IRequestHandler<GetDocumentsSignListRequest, GetDocumentsSignListResponse>
{
    private readonly IDocumentOnSAServiceClient _client;

    public GetDocumentsSignListHandler(IDocumentOnSAServiceClient client)
    {
        _client = client;
    }

    public async Task<GetDocumentsSignListResponse> Handle(GetDocumentsSignListRequest request, CancellationToken cancellationToken)
    {
        var result = await _client.GetDocumentsToSignList(request.SalesArrangementId, cancellationToken);

        return MapToResponseAndFilter(result);
    }

    private static GetDocumentsSignListResponse MapToResponseAndFilter(GetDocumentsToSignListResponse result)
    {
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
                SignatureState = GetSignatureState(s)
            }).ToList();

        return response;
    }

    private static SignatureState GetSignatureState(DocumentOnSAToSign docSa) => docSa switch
    {
        DocumentOnSAToSign doc when doc.DocumentOnSAId is null => SignatureState.Ready,
        DocumentOnSAToSign doc when doc.DocumentOnSAId is not null && doc.IsSigned == false => SignatureState.InTheProcess,
        DocumentOnSAToSign doc when doc.IsSigned && doc.EArchivId is null => SignatureState.WaitingForScan,
        DocumentOnSAToSign doc when doc.IsSigned && doc.EArchivId is not null => SignatureState.Signed,
        _ => SignatureState.Unknown,
    };
}
