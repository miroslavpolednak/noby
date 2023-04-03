using DomainServices.SalesArrangementService.Api.Database;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetSalesArrangementByOfferId;

internal sealed class GetSalesArrangementByOfferIdHandler
    : IRequestHandler<GetSalesArrangementByOfferIdRequest, GetSalesArrangementByOfferIdResponse>
{
    public async Task<GetSalesArrangementByOfferIdResponse> Handle(GetSalesArrangementByOfferIdRequest request, CancellationToken cancellation)
    {
        var instance = await _dbContext.SalesArrangements
            .Where(t => t.OfferId == request.OfferId)
            .AsNoTracking()
            .Select(DatabaseExpressions.SalesArrangementDetail())
            .FirstOrDefaultAsync(cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.OfferId);

        return new GetSalesArrangementByOfferIdResponse
        {
            IsExisting = instance is not null,
            Instance = instance
        };
    }

    private readonly SalesArrangementServiceDbContext _dbContext;
    
    public GetSalesArrangementByOfferIdHandler(SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}