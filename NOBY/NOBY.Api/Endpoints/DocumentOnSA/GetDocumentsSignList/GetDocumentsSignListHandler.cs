using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Extensions;

namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentsSignList;

public class GetDocumentsSignListHandler : IRequestHandler<GetDocumentsSignListRequest, GetDocumentsSignListResponse>
{
    private readonly IDocumentOnSAServiceClient _client;
    private readonly ICodebookServiceClient _codebookServiceClient;
    private readonly ISalesArrangementServiceClient _salesArrangementServiceClient;

    public GetDocumentsSignListHandler(
        IDocumentOnSAServiceClient client,
        ICodebookServiceClient codebookServiceClient,
        ISalesArrangementServiceClient salesArrangementServiceClient)
    {
        _client = client;
        _codebookServiceClient = codebookServiceClient;
        _salesArrangementServiceClient = salesArrangementServiceClient;
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
        // All docsOnSa have same salesArrangementId
        var salesArrangementId = result.DocumentsOnSAToSign.FirstOrDefault()?.SalesArrangementId;
        var salesArrangement = salesArrangementId is not null 
              ? await _salesArrangementServiceClient.GetSalesArrangement(salesArrangementId.Value, cancellationToken) 
              : null;

        var response = new GetDocumentsSignListResponse
        {
            Data = result.DocumentsOnSAToSign
            .Select(s => new GetDocumentsSignListData
            {
                DocumentOnSAId = s.DocumentOnSAId,
                DocumentTypeId = s.DocumentTypeId,
                FormId = s.FormId,
                IsSigned = s.IsSigned,
                SignatureTypeId = s.SignatureTypeId,
                SignatureDateTime = s.SignatureDateTime?.ToDateTime(),
                SignatureState = DocumentOnSaMetadataManager.GetSignatureState(new()
                {
                    IsValid = s.IsValid,
                    DocumentOnSAId = s.DocumentOnSAId,
                    IsSigned = s.IsSigned,
                    Source = s.Source.MapToCisEnum(),
                    SalesArrangementTypeId = salesArrangement?.SalesArrangementTypeId,
                    EArchivIdsLinked = s.EArchivIdsLinked
                },
              signatureStates),
                EACodeMainItem = DocumentOnSaMetadataManager.GetEaCodeMainItem(s.DocumentTypeId.GetValueOrDefault(), documentTypes, eACodeMains),
                CustomerOnSAId = s.CustomerOnSAId,
                IsPreviewSentToCustomer = s.IsPreviewSentToCustomer,
                ExternalId = s.ExternalId,
                Source = s.Source.MapToCisEnum()
            }).ToList()
        };

        return response;
    }

}
