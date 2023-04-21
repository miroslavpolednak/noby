using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Mappers;
using DomainServices.DocumentOnSAService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.GetDocumentsOnSAList;

public class GetDocumentsOnSAListHandler : IRequestHandler<GetDocumentsOnSAListRequest, GetDocumentsOnSAListResponse>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IDocumentOnSaMapper _documentOnSaMapper;

    public GetDocumentsOnSAListHandler(
        DocumentOnSAServiceDbContext dbContext,
        IDocumentOnSaMapper documentOnSaMapper)
    {
        _dbContext = dbContext;
        _documentOnSaMapper = documentOnSaMapper;
    }

    public async Task<GetDocumentsOnSAListResponse> Handle(GetDocumentsOnSAListRequest request, CancellationToken cancellationToken)
    {
        var documentsOnSaRealEntity = await _dbContext.DocumentOnSa
                                                   .AsNoTracking()
                                                   .Where(e => e.SalesArrangementId == request.SalesArrangementId)
                                                   .ToListAsync(cancellationToken);

        var response = new GetDocumentsOnSAListResponse();
        response.DocumentsOnSA.AddRange(_documentOnSaMapper.MapDocumentOnSaToSign(documentsOnSaRealEntity));
        return response;
    }
}
