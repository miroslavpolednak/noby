using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using _CisEnum = CIS.Foms.Enums;

namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSADetail;

public class GetDocumentOnSADetailHandler : IRequestHandler<GetDocumentOnSADetailRequest, GetDocumentOnSADetailResponse>
{
    private readonly IDocumentOnSAServiceClient _documentOnSAServiceClient;
    private readonly ICodebookServiceClient _codebookServiceClient;

    public GetDocumentOnSADetailHandler(
        IDocumentOnSAServiceClient documentOnSAServiceClient,
        ICodebookServiceClient codebookServiceClient)
    {
        _documentOnSAServiceClient = documentOnSAServiceClient;
        _codebookServiceClient = codebookServiceClient;
    }

    public async Task<GetDocumentOnSADetailResponse> Handle(GetDocumentOnSADetailRequest request, CancellationToken cancellationToken)
    {
        var documentOnSas = await _documentOnSAServiceClient.GetDocumentsOnSAList(request.SalesArrangementId, cancellationToken);

        var documentOnSa = documentOnSas.DocumentsOnSA.SingleOrDefault(d => d.DocumentOnSAId == request.DocumentOnSAId);

        if (documentOnSa is null)
        {
            throw new CisNotFoundException(90001, $"DocumetnOnSa {request.DocumentOnSAId} not exist for SalesArrangement {request.SalesArrangementId}");
        }

        return await MapToResponse(documentOnSa, cancellationToken);
    }

    private async Task<GetDocumentOnSADetailResponse> MapToResponse(DocumentOnSAToSign documentOnSa, CancellationToken cancellationToken)
    {
        var documentTypes = await _codebookServiceClient.DocumentTypes(cancellationToken);
        var eACodeMains = await _codebookServiceClient.EaCodesMain(cancellationToken);
        var signatureStates = await _codebookServiceClient.SignatureStatesNoby(cancellationToken);

        return new GetDocumentOnSADetailResponse
        {
            DocumentOnSAId = documentOnSa.DocumentOnSAId,
            DocumentTypeId = documentOnSa.DocumentTypeId,
            FormId = documentOnSa.FormId,
            SignatureTypeId = documentOnSa.SignatureTypeId,
            SignatureDateTime = documentOnSa.SignatureDateTime is not null ? documentOnSa.SignatureDateTime.ToDateTime() : null,
            SignatureState = DocumentOnSaMetadataManager.GetSignatureState(new() { DocumentOnSAId = documentOnSa.DocumentOnSAId, EArchivId = documentOnSa.EArchivId, IsSigned = documentOnSa.IsSigned }, signatureStates),
            EACodeMainItem = DocumentOnSaMetadataManager.GetEaCodeMainItem(documentOnSa.DocumentTypeId.GetValueOrDefault(), documentTypes, eACodeMains),
            CustomerOnSAId = documentOnSa.CustomerOnSAId,
            IsPreviewSentToCustomer = documentOnSa.IsPreviewSentToCustomer,
            ExternalId = documentOnSa.ExternalId,
            Source = documentOnSa.Source switch
            {
                Source.Noby => _CisEnum.Source.Noby,
                Source.Workflow => _CisEnum.Source.Workflow,
                _ => _CisEnum.Source.Unknown
            }
        };
    }
}
