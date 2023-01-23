using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.GetDocumentOnSAData;

public class GetDocumentOnSADataHandler : IRequestHandler<GetDocumentOnSADataRequest, GetDocumentOnSADataResponse>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;

    public GetDocumentOnSADataHandler(DocumentOnSAServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetDocumentOnSADataResponse> Handle(GetDocumentOnSADataRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await _dbContext.DocumentOnSa.FirstOrDefaultAsync(r => r.DocumentOnSAId == request.DocumentOnSAId!.Value, cancellationToken);
        
        if (documentOnSa is null)
        {
            throw new CisNotFoundException(19003, $"DocumentOnSA {request.DocumentOnSAId!.Value} does not exist.");
        }
        return MapResponse(documentOnSa);
    }

    private GetDocumentOnSADataResponse MapResponse(DocumentOnSa documentOnSa)
    {
        return new()
        {
            DocumentTypeId = documentOnSa.DocumentTypeId,
            DocumentTemplateVersionId = documentOnSa.DocumentTemplateVersionId,
            Data = documentOnSa.Data
        };
    }
}
