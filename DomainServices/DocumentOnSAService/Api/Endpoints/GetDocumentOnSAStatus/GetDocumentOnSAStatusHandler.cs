using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.GetDocumentOnSAStatus;

public class GetDocumentOnSAStatusHandler : IRequestHandler<GetDocumentOnSAStatusRequest, GetDocumentOnSAStatusResponse>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;

    public GetDocumentOnSAStatusHandler(DocumentOnSAServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetDocumentOnSAStatusResponse> Handle(GetDocumentOnSAStatusRequest request, CancellationToken cancellationToken)
    {
        var docOnSa = await _dbContext.DocumentOnSa
            .AsNoTracking()
            .Include(i => i.EArchivIdsLinkeds)
            .FirstOrDefaultAsync(d => d.SalesArrangementId == request.SalesArrangementId
                                                            && d.DocumentOnSAId == request.DocumentOnSAId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSaDoesntExistForSalesArrangement, request.SalesArrangementId);

        return new()
        {
            DocumentOnSAId = docOnSa.DocumentOnSAId,
            IsSigned = docOnSa.IsSigned,
            IsValid = docOnSa.IsValid,
            Source = (Source)docOnSa.Source,
            EArchivIdsLinked = { docOnSa.EArchivIdsLinkeds.Select(s => s.EArchivId) }
        };
    }
}
