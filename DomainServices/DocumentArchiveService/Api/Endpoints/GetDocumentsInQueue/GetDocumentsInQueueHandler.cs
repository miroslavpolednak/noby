using DomainServices.DocumentArchiveService.Api.Database;
using DomainServices.DocumentArchiveService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GetDocumentsInQueue;

public class GetDocumentsInQueueHandler : IRequestHandler<GetDocumentsInQueueRequest, GetDocumentsInQueueResponse>
{
    private readonly DocumentArchiveDbContext _context;

    public GetDocumentsInQueueHandler(DocumentArchiveDbContext context)
    {
        _context = context;
    }

    public async Task<GetDocumentsInQueueResponse> Handle(GetDocumentsInQueueRequest request, CancellationToken cancellationToken)
    {
        var result = await _context.DocumentInterface.Where(d => request.EArchivIds.Contains(d.DocumentId))
            .Select(s => new
            {
                s.DocumentId,
                s.FileName,
                s.Status,
                s.FormId
            }).ToListAsync(cancellationToken);

        var response = new GetDocumentsInQueueResponse();
        response.QueuedDocuments.AddRange(result.Select(d => new QueuedDocument
        {
            EArchivId = d.DocumentId,
            Filename = d.FileName,
            FormId = d.FormId,
            Status = d.Status
        }));

        return response;
    }
}
