using DomainServices.DocumentOnSAService.Api.Database;
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
        var response = await _dbContext.DocumentOnSa
            .Where(r => r.DocumentOnSAId == request.DocumentOnSAId!.Value)
            .Select(d => new GetDocumentOnSADataResponse
            {
                DocumentTypeId = d.DocumentTypeId,
                DocumentTemplateVersionId = d.DocumentTemplateVersionId,
                DocumentTemplateVariantId = d.DocumentTemplateVariantId,
                Data = d.Data
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            throw new CisNotFoundException(19003, $"DocumentOnSA {request.DocumentOnSAId!.Value} does not exist.");
        }

        return response;
    }
}
