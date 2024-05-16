using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.RefreshElectronicDocExternalId;

public class RefreshElectronicDocExternalIdHandler(DocumentOnSAServiceDbContext context, IMediator mediator) : IRequestHandler<RefreshElectronicDocExternalIdRequest, Empty>
{
    private readonly DocumentOnSAServiceDbContext _context = context;
    private readonly IMediator _mediator = mediator;

    public async Task<Empty> Handle(RefreshElectronicDocExternalIdRequest request, CancellationToken cancellationToken)
    {
        var docOnSa = await _context.DocumentOnSa
            .Select(s => new
            {
                s.DocumentOnSAId,
                s.ExternalIdESignatures,
                s.SignatureTypeId,
                s.IsValid,
                s.IsSigned
            }).FirstOrDefaultAsync(d => d.ExternalIdESignatures == request.ExternalIdESignatures, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist, request.ExternalIdESignatures);

        if (request.Operation == Operation.SignDocument && docOnSa is { IsValid: true, IsSigned: false })
        {
            await _mediator.Send(new SignDocumentRequest { DocumentOnSAId = docOnSa.DocumentOnSAId, SignatureTypeId = docOnSa.SignatureTypeId }, cancellationToken);
        }
        else if (request.Operation == Operation.StopSigning)
        {
            await _mediator.Send(new StopSigningRequest { DocumentOnSAId = docOnSa.DocumentOnSAId, NotifyESignatures = false }, cancellationToken);
        }

        return new();
    }
}
