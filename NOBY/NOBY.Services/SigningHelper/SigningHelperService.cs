using DomainServices.DocumentOnSAService.Clients;
using FastEnumUtility;
using NOBY.Infrastructure.ErrorHandling;

namespace NOBY.Services.SigningHelper;

public class DocumentOnSADto
{
    public int DocumentOnSAId { get; set; }

    public int? SignatureTypeId { get; set; }

    public int SalesArrangementId { get; set; }
}

public interface ISigningHelperService
{
    Task StopSinningAccordingState(DocumentOnSADto documentOnSA, CancellationToken cancellationToken);

    Task<DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign> GetDocumentOnSa(DocumentOnSADto documentOnSA, CancellationToken cancellationToken);
}

[ScopedService, AsImplementedInterfacesService]
public class SigningHelperService : ISigningHelperService
{
    private readonly IDocumentOnSAServiceClient _documentOnSAService;

    public SigningHelperService(IDocumentOnSAServiceClient documentOnSAService)
    {
        _documentOnSAService = documentOnSAService;
    }

    public async Task StopSinningAccordingState(DocumentOnSADto documentOnSA, CancellationToken cancellationToken)
    {
        if (documentOnSA.SignatureTypeId == SignatureTypes.Electronic.ToByte())
        {
            await _documentOnSAService.RefreshElectronicDocument(documentOnSA.DocumentOnSAId, cancellationToken);
            var docOnSaAfterRefresh = await GetDocumentOnSa(documentOnSA, cancellationToken);

            if (docOnSaAfterRefresh.IsValid == false)
                return;
            else if (docOnSaAfterRefresh.IsSigned)
                await _documentOnSAService.StopSigning(new() { DocumentOnSAId = documentOnSA.DocumentOnSAId }, cancellationToken);
            else
                await _documentOnSAService.StopSigning(new() { DocumentOnSAId = documentOnSA.DocumentOnSAId, NotifyESignatures = true }, cancellationToken);
        }
        else
        {
            await _documentOnSAService.StopSigning(new() { DocumentOnSAId = documentOnSA.DocumentOnSAId }, cancellationToken);
        }
    }

    public async Task<DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign> GetDocumentOnSa(DocumentOnSADto documentOnSA, CancellationToken cancellationToken)
    {
        var documentOnSas = await _documentOnSAService.GetDocumentsOnSAList(documentOnSA.SalesArrangementId, cancellationToken);

        return documentOnSas.DocumentsOnSA.SingleOrDefault(d => d.DocumentOnSAId == documentOnSA.DocumentOnSAId)
            ?? throw new NobyValidationException($"DocumetnOnSa {documentOnSA.DocumentOnSAId} not exist for SalesArrangement {documentOnSA.SalesArrangementId}");
    }

}
