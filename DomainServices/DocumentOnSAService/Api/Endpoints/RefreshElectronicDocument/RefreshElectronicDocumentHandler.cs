using CIS.Foms.Enums;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using ExternalServices.ESignatures.V1;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.RefreshElectronicDocument;

public class RefreshElectronicDocumentHandler : IRequestHandler<RefreshElectronicDocumentRequest, Empty>
{
    private readonly DocumentOnSAServiceDbContext _context;
    private readonly IESignaturesClient _eSignaturesClient;
    private readonly IMediator _mediator;

    public RefreshElectronicDocumentHandler(
        DocumentOnSAServiceDbContext context,
        IESignaturesClient eSignaturesClient,
        IMediator mediator)
    {
        _context = context;
        _eSignaturesClient = eSignaturesClient;
        _mediator = mediator;
    }

    public async Task<Empty> Handle(RefreshElectronicDocumentRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await _context.DocumentOnSa
                                    .Select(s => new
                                    {
                                        s.DocumentOnSAId,
                                        s.ExternalId,
                                        s.SignatureTypeId
                                    })
                                    .FirstOrDefaultAsync(d => d.DocumentOnSAId == request.DocumentOnSAId, cancellationToken)
                                    ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist, request.DocumentOnSAId);

        if (documentOnSa.ExternalId is null)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.UnableGetExternalIdForDocumentOnSaId, request.DocumentOnSAId);

        var elDocumentStatus = await _eSignaturesClient.GetDocumentStatus(documentOnSa.ExternalId, cancellationToken);

        switch (elDocumentStatus)
        {
            case EDocumentStatuses.SIGNED or EDocumentStatuses.VERIFIED or EDocumentStatuses.SENT:
                await _mediator.Send(new SignDocumentRequest { DocumentOnSAId = documentOnSa.DocumentOnSAId, SignatureTypeId = documentOnSa.SignatureTypeId }, cancellationToken);
                break;
            case EDocumentStatuses.DELETED:
                await _mediator.Send(new StopSigningRequest { DocumentOnSAId = documentOnSa.DocumentOnSAId, NotifyESignatures = false }, cancellationToken);
                break;
            case EDocumentStatuses.NEW or EDocumentStatuses.IN_PROGRESS or EDocumentStatuses.APPROVED:
                // Ignore
                break;
            default:
                throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.UnsupportedStatusReturnedFromESignature, elDocumentStatus);
        }

        return new Empty();
    }
}
