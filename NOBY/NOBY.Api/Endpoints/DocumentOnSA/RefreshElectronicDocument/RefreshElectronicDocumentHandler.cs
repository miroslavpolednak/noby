using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Extensions;

namespace NOBY.Api.Endpoints.DocumentOnSA.RefreshElectronicDocument;

public class RefreshElectronicDocumentHandler : IRequestHandler<RefreshElectronicDocumentRequest, RefreshElectronicDocumentResponse>
{
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public RefreshElectronicDocumentHandler(
        IDocumentOnSAServiceClient documentOnSAService,
        ICodebookServiceClient codebookService,
        ISalesArrangementServiceClient salesArrangementService)
    {
        _documentOnSAService = documentOnSAService;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
    }

    public async Task<RefreshElectronicDocumentResponse> Handle(RefreshElectronicDocumentRequest request, CancellationToken cancellationToken)
    {
        await _documentOnSAService.RefreshElectronicDocument(request.DocumentOnSAId, cancellationToken);
        // after refresh
        var docOnSa = await GetDocumentOnSa(request, cancellationToken);

        return await MapToResponse(request, docOnSa, cancellationToken);
    }

    private async Task<RefreshElectronicDocumentResponse> MapToResponse(RefreshElectronicDocumentRequest request, DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign docOnSa, CancellationToken cancellationToken)
    {
        var documentTypes = await _codebookService.DocumentTypes(cancellationToken);
        var eACodeMains = await _codebookService.EaCodesMain(cancellationToken);
        var signatureStates = await _codebookService.SignatureStatesNoby(cancellationToken);

        // All docsOnSa have same salesArrangementId
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        return new RefreshElectronicDocumentResponse
        {
            Data = new Dto.Signing.DocumentData
            {
                DocumentOnSAId = docOnSa.DocumentOnSAId,
                DocumentTypeId = docOnSa.DocumentTypeId,
                FormId = docOnSa.FormId,
                SignatureTypeId = docOnSa.SignatureTypeId,
                SignatureDateTime = docOnSa.SignatureDateTime?.ToDateTime(),
                SignatureState = DocumentOnSaMetadataManager.GetSignatureState(new()
                {
                    IsValid = docOnSa.IsValid,
                    DocumentOnSAId = docOnSa.DocumentOnSAId,
                    IsSigned = docOnSa.IsSigned,
                    Source = docOnSa.Source.MapToCisEnum(),
                    SalesArrangementTypeId = salesArrangement?.SalesArrangementTypeId,
                    EArchivIdsLinked = docOnSa.EArchivIdsLinked
                },
              signatureStates),
                EACodeMainItem = DocumentOnSaMetadataManager.GetEaCodeMainItem(docOnSa.DocumentTypeId.GetValueOrDefault(), documentTypes, eACodeMains),
                CustomerOnSa = new()
                {
                    CustomerOnSAId = docOnSa.CustomerOnSA?.CustomerOnSAId,
                    FirstName = docOnSa.CustomerOnSA?.FirstName,
                    LastName = docOnSa.CustomerOnSA?.LastName
                },
                IsPreviewSentToCustomer = docOnSa.IsPreviewSentToCustomer,
                ExternalId = docOnSa.ExternalId,
                Source = docOnSa.Source.MapToCisEnum()
            }
        };
    }

    private async Task<DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign> GetDocumentOnSa(RefreshElectronicDocumentRequest request, CancellationToken cancellationToken)
    {
        var documentOnSas = await _documentOnSAService.GetDocumentsOnSAList(request.SalesArrangementId, cancellationToken);

        var documentOnSa = documentOnSas.DocumentsOnSA.SingleOrDefault(d => d.DocumentOnSAId == request.DocumentOnSAId)
            ?? throw new NobyValidationException($"DocumetnOnSa {request.DocumentOnSAId} not exist for SalesArrangement {request.SalesArrangementId}");

        return documentOnSa;
    }

}
