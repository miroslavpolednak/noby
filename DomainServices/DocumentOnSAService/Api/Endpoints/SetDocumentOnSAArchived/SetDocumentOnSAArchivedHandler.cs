using CIS.Foms.Enums;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.SetDocumentOnSAArchived;

public class SetDocumentOnSAArchivedHandler : IRequestHandler<SetDocumentOnSAArchivedRequest, Empty>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;

    public SetDocumentOnSAArchivedHandler(DocumentOnSAServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Empty> Handle(SetDocumentOnSAArchivedRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await _dbContext.DocumentOnSa.Where(r => r.DocumentOnSAId == request.DocumentOnSAId)
                                               .FirstOrDefaultAsync(cancellationToken)
                                               ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist, request.DocumentOnSAId);
        
        if (documentOnSa.SignatureTypeId is not null && documentOnSa.SignatureTypeId == (int)SignatureTypes.Electronic)
        {
            // ToDo call SubmitDispatchForm (This method is't ready yet)
        }

        documentOnSa.IsArchived = true;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new Empty();
    }
}
