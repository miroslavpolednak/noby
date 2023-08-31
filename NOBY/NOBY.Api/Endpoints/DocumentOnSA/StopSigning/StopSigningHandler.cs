using CIS.Foms.Enums;
using DomainServices.DocumentOnSAService.Clients;
using FastEnumUtility;

namespace NOBY.Api.Endpoints.DocumentOnSA.StopSigning;

public class StopSigningHandler : IRequestHandler<StopSigningRequest>
{
    private readonly IDocumentOnSAServiceClient _documentOnSAService;

    public StopSigningHandler(IDocumentOnSAServiceClient documentOnSAService)
    {
        _documentOnSAService = documentOnSAService;
    }

    public async Task Handle(StopSigningRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await GetDocumentOnSa(request, cancellationToken);

        if (documentOnSa.SignatureTypeId == SignatureTypes.Electronic.ToByte())
        {
            await _documentOnSAService.RefreshElectronicDocument(documentOnSa.DocumentOnSAId!.Value, cancellationToken);
            var docOnSaAfterRefresh = await GetDocumentOnSa(request, cancellationToken);

            if (docOnSaAfterRefresh.IsValid == false)
                return;
            else if (docOnSaAfterRefresh.IsSigned)
                await _documentOnSAService.StopSigning(new() { DocumentOnSAId = request.DocumentOnSAId }, cancellationToken);
            else
                await _documentOnSAService.StopSigning(new() { DocumentOnSAId = request.DocumentOnSAId, NotifyESignatures = true }, cancellationToken);
        }
        else
        {
            await _documentOnSAService.StopSigning(new() { DocumentOnSAId = request.DocumentOnSAId }, cancellationToken);
        }
    }

    private async Task<DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign> GetDocumentOnSa(StopSigningRequest request, CancellationToken cancellationToken)
    {
        var documentOnSas = await _documentOnSAService.GetDocumentsOnSAList(request.SalesArrangementId, cancellationToken);

        var documentOnSa = documentOnSas.DocumentsOnSA.SingleOrDefault(d => d.DocumentOnSAId == request.DocumentOnSAId)
            ?? throw new NobyValidationException($"DocumetnOnSa {request.DocumentOnSAId} not exist for SalesArrangement {request.SalesArrangementId}");

        return documentOnSa;
    }
}
