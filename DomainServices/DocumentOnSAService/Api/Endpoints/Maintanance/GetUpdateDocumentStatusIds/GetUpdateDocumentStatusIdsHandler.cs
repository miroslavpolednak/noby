using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using FastEnumUtility;
using Microsoft.EntityFrameworkCore;
using SharedTypes.Enums;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.Maintanance.GetUpdateDocumentStatusIds;

internal sealed class GetUpdateDocumentStatusIdsHandler
    : IRequestHandler<GetUpdateDocumentStatusIdsRequest, GetUpdateDocumentStatusIdsResponse>
{
    public async Task<GetUpdateDocumentStatusIdsResponse> Handle(GetUpdateDocumentStatusIdsRequest request, CancellationToken cancellationToken)
    {
        var documentOnSas = await _dbContext.DocumentOnSa
            .Where(s => s.SignatureTypeId == SignatureTypes.Electronic.ToByte() && s.IsValid && !s.IsSigned && !s.IsFinal)
            .Select(s => new GetUpdateDocumentStatusIdsResponse.Types.GetUpdateDocumentStatusIdsResponseItem
            {
                DocumentOnSAId = s.DocumentOnSAId,
                ExternalIdESignatures = s.ExternalIdESignatures
            })
            .ToListAsync(cancellationToken);

        GetUpdateDocumentStatusIdsResponse response = new();
        response.Items.AddRange(documentOnSas);
        return response;
    }

    private readonly DocumentOnSAServiceDbContext _dbContext;

    public GetUpdateDocumentStatusIdsHandler(DocumentOnSAServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
