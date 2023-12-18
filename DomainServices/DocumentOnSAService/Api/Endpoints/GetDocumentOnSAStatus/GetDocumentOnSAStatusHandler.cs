using DomainServices.DocumentOnSAService.Api.Database;
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
        var docOnsa = await _dbContext.DocumentOnSa
            .Where(d => d.SalesArrangementId == request.SalesArrangementId
                        && d.DocumentOnSAId == request.DocumentOnSAId)
            .Select(s => new
            {
                s.DocumentOnSAId,
                s.IsSigned,
                s.IsValid,
                s.Source,
                EArchivIdsLinked = s.EArchivIdsLinkeds.Select(s => s.EArchivId)
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSaDoesntExistForSalesArrangement, request.SalesArrangementId);

        return new()
        {
            DocumentOnSAId = docOnsa.DocumentOnSAId,
            IsSigned = docOnsa.IsSigned,
            IsValid = docOnsa.IsValid,
            Source = (Source)docOnsa.Source,
            EArchivIdsLinked = { docOnsa.EArchivIdsLinked }
        };
    }
}
