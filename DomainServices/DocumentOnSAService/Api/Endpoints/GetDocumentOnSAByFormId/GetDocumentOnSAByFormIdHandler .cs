using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.GetDocumentOnSAByFormId;

public class GetDocumentOnSAByFormIdHandler : IRequestHandler<GetDocumentOnSAByFormIdRequest, GetDocumentOnSAByFormIdResponse>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;

    public GetDocumentOnSAByFormIdHandler(DocumentOnSAServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetDocumentOnSAByFormIdResponse> Handle(GetDocumentOnSAByFormIdRequest request, CancellationToken cancellationToken)
    {
        var docOnSaMapped = await _dbContext.DocumentOnSa.Select(s => new DocumentOnSA
        {
            DocumentOnSAId = s.DocumentOnSAId,
            DocumentTypeId = s.DocumentTypeId,
            FormId = s.FormId,
            HouseholdId = s.HouseholdId,
            IsValid = s.IsValid,
            EArchivId = s.EArchivId,
            IsSigned = s.IsSigned,
            IsArchived = s.IsArchived,
            SignatureTypeId = s.SignatureTypeId,
            Source = (Source)s.Source,
            SalesArrangementId = s.SalesArrangementId,
        }).FirstOrDefaultAsync(r => r.FormId == request.FormId, cancellationToken)
          ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSaDoesntExistForFormId, request.FormId);

        return new() { DocumentOnSa = docOnSaMapped };
    }
}
