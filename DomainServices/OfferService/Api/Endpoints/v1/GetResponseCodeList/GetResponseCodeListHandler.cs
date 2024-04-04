using DomainServices.OfferService.Api.Database;
using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.v1.GetResponseCodeList;

internal sealed class GetResponseCodeListHandler
    : IRequestHandler<GetResponseCodeListRequest, GetResponseCodeListResponse>
{
    public async Task<GetResponseCodeListResponse> Handle(GetResponseCodeListRequest request, CancellationToken cancellationToken)
    {
        var responses = await _dbContext.ResponseCodes
            .AsNoTracking()
            .Where(t => t.CaseId == request.CaseId)
            .Select(t => new GetResponseCodeListResponse.Types.GetResponseCodeItem
            {
                ResponseCodeTypeId = t.ResponseCodeTypeId,
                ResponseCodeId = t.ResponseCodeId,
                Data = t.Data,
                Created = new SharedTypes.GrpcTypes.ModificationStamp(t.CreatedUserId, t.CreatedUserName, t.CreatedTime)
            })
            .ToListAsync(cancellationToken);

        var response = new GetResponseCodeListResponse();
        response.Responses.AddRange(responses);
        return response;
    }

    private readonly Database.OfferServiceDbContext _dbContext;

    public GetResponseCodeListHandler(OfferServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
