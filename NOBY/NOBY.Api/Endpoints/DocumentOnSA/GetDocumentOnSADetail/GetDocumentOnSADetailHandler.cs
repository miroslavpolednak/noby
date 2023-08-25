﻿using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Extensions;

namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSADetail;

public class GetDocumentOnSADetailHandler : IRequestHandler<GetDocumentOnSADetailRequest, GetDocumentOnSADetailResponse>
{
    private readonly IDocumentOnSAServiceClient _documentOnSAServiceClient;
    private readonly ICodebookServiceClient _codebookServiceClient;
    private readonly ISalesArrangementServiceClient _salesArrangementServiceClient;

    public GetDocumentOnSADetailHandler(
        IDocumentOnSAServiceClient documentOnSAServiceClient,
        ICodebookServiceClient codebookServiceClient,
        ISalesArrangementServiceClient salesArrangementServiceClient)
    {
        _documentOnSAServiceClient = documentOnSAServiceClient;
        _codebookServiceClient = codebookServiceClient;
        _salesArrangementServiceClient = salesArrangementServiceClient;
    }

    public async Task<GetDocumentOnSADetailResponse> Handle(GetDocumentOnSADetailRequest request, CancellationToken cancellationToken)
    {
        var documentOnSas = await _documentOnSAServiceClient.GetDocumentsOnSAList(request.SalesArrangementId, cancellationToken);

        var documentOnSa = documentOnSas.DocumentsOnSA.SingleOrDefault(d => d.DocumentOnSAId == request.DocumentOnSAId);

        if (documentOnSa is null)
        {
            throw new CisNotFoundException(NobyValidationException.DefaultExceptionCode, $"DocumetnOnSa {request.DocumentOnSAId} not exist for SalesArrangement {request.SalesArrangementId}");
        }

        return await MapToResponse(documentOnSa, cancellationToken);
    }

    private async Task<GetDocumentOnSADetailResponse> MapToResponse(DocumentOnSAToSign documentOnSa, CancellationToken cancellationToken)
    {
        var documentTypes = await _codebookServiceClient.DocumentTypes(cancellationToken);
        var eACodeMains = await _codebookServiceClient.EaCodesMain(cancellationToken);
        var signatureStates = await _codebookServiceClient.SignatureStatesNoby(cancellationToken);
        var salesArrangement = await _salesArrangementServiceClient.GetSalesArrangement(documentOnSa.SalesArrangementId, cancellationToken);

        return new GetDocumentOnSADetailResponse
        {
            DocumentOnSAId = documentOnSa.DocumentOnSAId,
            DocumentTypeId = documentOnSa.DocumentTypeId,
            FormId = documentOnSa.FormId,
            SignatureTypeId = documentOnSa.SignatureTypeId,
            SignatureDateTime = documentOnSa.SignatureDateTime?.ToDateTime(),
            SignatureState = DocumentOnSaMetadataManager.GetSignatureState(new()
            {
                IsValid = documentOnSa.IsValid,
                DocumentOnSAId = documentOnSa.DocumentOnSAId,
                IsSigned = documentOnSa.IsSigned,
                Source = documentOnSa.Source.MapToCisEnum(),
                SalesArrangementTypeId = salesArrangement.SalesArrangementTypeId,
                EArchivIdsLinked = documentOnSa.EArchivIdsLinked,
            },
              signatureStates),
            EACodeMainItem = DocumentOnSaMetadataManager.GetEaCodeMainItem(documentOnSa.DocumentTypeId.GetValueOrDefault(), documentTypes, eACodeMains),
            CustomerOnSa = new() {
                CustomerOnSAId = documentOnSa.CustomerOnSA?.CustomerOnSAId,
                FirstName = documentOnSa.CustomerOnSA?.FirstName,
                LastName = documentOnSa.CustomerOnSA?.LastName
            },
            IsPreviewSentToCustomer = documentOnSa.IsPreviewSentToCustomer,
            ExternalId = documentOnSa.ExternalId,
            Source = documentOnSa.Source.MapToCisEnum()
        };
    }
}
