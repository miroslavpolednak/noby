using DomainServices.RealEstateValuationService.Api.Database;
using __Contracts = DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.GetRealEstateValuationDetailByOrderId;

internal sealed class GetRealEstateValuationDetailByOrderIdHandler
    : IRequestHandler<__Contracts.GetRealEstateValuationDetailByOrderIdRequest, __Contracts.RealEstateValuationDetail>
{
    public async Task<__Contracts.RealEstateValuationDetail> Handle(__Contracts.GetRealEstateValuationDetailByOrderIdRequest request, CancellationToken cancellationToken)
    {
        var id = await _dbContext.RealEstateValuations
            .AsNoTracking()
            .Where(t => t.OrderId == request.OrderId)
            .Select(t => new { t.RealEstateValuationId })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.OrderId);

        return await _mediator.Send(new __Contracts.GetRealEstateValuationDetailRequest
        { 
            RealEstateValuationId = id.RealEstateValuationId 
        }, cancellationToken);
    }

    private readonly IMediator _mediator;
    private readonly RealEstateValuationServiceDbContext _dbContext;

    public GetRealEstateValuationDetailByOrderIdHandler(RealEstateValuationServiceDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }
}
