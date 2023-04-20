using DomainServices.DocumentArchiveService.Api.Database;
using DomainServices.DocumentArchiveService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.SetDocumentStatusInQueue;

public class SetDocumentStatusInQueueHandler : IRequestHandler<SetDocumentStatusInQueueRequest, Empty>
{
    private readonly DocumentArchiveDbContext _dbContext;

    public SetDocumentStatusInQueueHandler(DocumentArchiveDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Empty> Handle(SetDocumentStatusInQueueRequest request, CancellationToken cancellationToken)
    {
        var document = await _dbContext.DocumentInterface.FirstOrDefaultAsync(d => d.DocumentId == request.EArchivId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentWithEArchiveIdNotExist);

        document.Status = request.StatusInQueue!.Value;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new Empty();
    }
}
