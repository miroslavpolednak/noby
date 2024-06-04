using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Extensions;
using NOBY.Dto.Signing;

namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentsSignList;

public class GetDocumentsSignListHandler : IRequestHandler<GetDocumentsSignListRequest, GetDocumentsSignListResponse>
{
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public GetDocumentsSignListHandler(
        IDocumentOnSAServiceClient documentOnSAService,
        ICodebookServiceClient codebookService,
        ISalesArrangementServiceClient salesArrangementServiceClient)
    {
        _documentOnSAService = documentOnSAService;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementServiceClient;
    }

    public async Task<GetDocumentsSignListResponse> Handle(GetDocumentsSignListRequest request, CancellationToken cancellationToken)
    {
        var result = await _documentOnSAService.GetDocumentsToSignList(request.SalesArrangementId, cancellationToken);
        return await MapToResponseAndOrder(result, cancellationToken);
    }

    private async Task<GetDocumentsSignListResponse> MapToResponseAndOrder(GetDocumentsToSignListResponse result, CancellationToken cancellationToken)
    {
        var documentTypes = await _codebookService.DocumentTypes(cancellationToken);
        var eACodeMains = await _codebookService.EaCodesMain(cancellationToken);
        var signatureStates = await _codebookService.SignatureStatesNoby(cancellationToken);
        // All docsOnSa have same salesArrangementId
        var salesArrangementId = result.DocumentsOnSAToSign.FirstOrDefault()?.SalesArrangementId;
        var salesArrangement = salesArrangementId is not null
              ? await _salesArrangementService.GetSalesArrangement(salesArrangementId.Value, cancellationToken)
              : null;

        return new GetDocumentsSignListResponse
        {
            Data = result.DocumentsOnSAToSign
            .Select(s => new DocumentData
            {
                DocumentOnSAId = s.DocumentOnSAId,
                DocumentTypeId = s.DocumentTypeId,
                FormId = s.FormId,
                SignatureTypeId = s.SignatureTypeId,
                SignatureDateTime = s.SignatureDateTime?.ToDateTime(),
                SignatureState = DocumentOnSaMetadataManager.GetSignatureState(new()
                {
                    IsValid = s.IsValid,
                    DocumentOnSAId = s.DocumentOnSAId,
                    IsSigned = s.IsSigned,
                    Source = s.Source.MapToCisEnum(),
                    SalesArrangementTypeId = salesArrangement?.SalesArrangementTypeId,
                    EArchivIdsLinked = s.EArchivIdsLinked,
                    SignatureTypeId = s.SignatureTypeId ?? 0,
                    EaCodeMainId = s.EACodeMainId
                },
              signatureStates),
                EACodeMainItem = DocumentOnSaMetadataManager.GetEaCodeMainItem(
                    new() { DocumentTypeId = s.DocumentTypeId, EACodeMainId = s.EACodeMainId }, documentTypes, eACodeMains),
                CustomerOnSa = new()
                {
                    CustomerOnSAId = s.CustomerOnSA?.CustomerOnSAId,
                    FirstName = s.CustomerOnSA?.FirstName,
                    LastName = s.CustomerOnSA?.LastName
                },
                IsPreviewSentToCustomer = s.IsPreviewSentToCustomer,
                ExternalId = s.ExternalId,
                Source = s.Source.MapToCisEnum(),
                IsCustomerPreviewSendingAllowed = s.IsCustomerPreviewSendingAllowed
            }).OrderBy(o => o.DocumentTypeId).ThenBy(c => c.CustomerOnSa.CustomerOnSAId).ToList()
        };
    }
}
