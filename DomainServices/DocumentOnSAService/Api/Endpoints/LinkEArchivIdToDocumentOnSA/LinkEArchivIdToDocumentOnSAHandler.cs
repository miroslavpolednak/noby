using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.LinkEArchivIdToDocumentOnSA;

public class LinkEArchivIdToDocumentOnSAHandler : IRequestHandler<LinkEArchivIdToDocumentOnSARequest, Empty>
{
    private readonly DocumentOnSAServiceDbContext _context;

    public LinkEArchivIdToDocumentOnSAHandler(DocumentOnSAServiceDbContext context)
    {
        _context = context;
    }

    public async Task<Empty> Handle(LinkEArchivIdToDocumentOnSARequest request, CancellationToken cancellationToken)
    {
        var documentOnSaEntity = _context.DocumentOnSa.FirstOrDefault(r => r.DocumentOnSAId == request.DocumentOnSAId)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist);

        documentOnSaEntity.EArchivIdsLinkeds.Add(new()
        {
            EArchivId = request.EArchivId,
        });

        await _context.SaveChangesAsync(cancellationToken);
        return new Empty();
    }
}
