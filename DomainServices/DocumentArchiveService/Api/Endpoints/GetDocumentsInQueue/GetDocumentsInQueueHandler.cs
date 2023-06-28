using DomainServices.DocumentArchiveService.Api.Database;
using DomainServices.DocumentArchiveService.Contracts;
using Google.Protobuf;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GetDocumentsInQueue;

public class GetDocumentsInQueueHandler : IRequestHandler<GetDocumentsInQueueRequest, GetDocumentsInQueueResponse>
{
    private const int MaxBatchSize = 500;

    private readonly DocumentArchiveDbContext _context;

    public GetDocumentsInQueueHandler(DocumentArchiveDbContext context)
    {
        _context = context;
    }

    public async Task<GetDocumentsInQueueResponse> Handle(GetDocumentsInQueueRequest request, CancellationToken cancellationToken)
    {
        var query = CreateQuery(request);

        var result = await query
            .Select(s => new
            {
                s.DocumentId,
                s.FileName,
                s.Status,
                s.FormId,
                s.EaCodeMainId,
                s.CreatedOn,
                s.Description,
                DocumentData = request.WithContent ? s.DocumentData : null,
            }).Take(MaxBatchSize)
              .ToListAsync(cancellationToken);

        var response = new GetDocumentsInQueueResponse();
        response.QueuedDocuments.AddRange(result.Select(d => new QueuedDocument
        {
            EArchivId = d.DocumentId,
            Filename = d.FileName,
            FormId = d.FormId,
            StatusInQueue = d.Status,
            EaCodeMainId = d.EaCodeMainId,
            CreatedOn = d.CreatedOn,
            Description = d.Description,
            DocumentData = d.DocumentData is not null ? ByteString.CopyFrom(d.DocumentData) : ByteString.CopyFromUtf8(string.Empty)
        }));

        return response;
    }

    private IQueryable<Database.Entities.DocumentInterface> CreateQuery(GetDocumentsInQueueRequest request)
    {
        var query = _context.DocumentInterface.Select(d => d);

        if (request.EArchivIds.Any())
            query = query.Where(d => request.EArchivIds.Contains(d.DocumentId));

        if (request.CaseId is not null)
            query = query.Where(d => d.CaseId == request.CaseId);

        if (request.StatusesInQueue.Any())
            query = query.Where(d => request.StatusesInQueue.Contains(d.Status));

        return query;
    }
}
