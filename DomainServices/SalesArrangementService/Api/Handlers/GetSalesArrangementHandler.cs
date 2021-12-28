using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class GetSalesArrangementHandler
    : IRequestHandler<Dto.GetSalesArrangementMediatrRequest, GetSalesArrangementResponse>
{
    public async Task<GetSalesArrangementResponse> Handle(Dto.GetSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogDebug("Get detail for #{id}", request.SalesArrangementId);

        var sa = await _repository.GetSalesArrangement(request.SalesArrangementId);
        
        var model = new GetSalesArrangementResponse
        {
            SalesArrangementId = sa.SalesArrangementId,
            SalesArrangementType = sa.SalesArrangementType,
            State = sa.State,
            CaseId = sa.CaseId,
            OfferInstanceId = sa.OfferInstanceId
        };

        _logger.LogDebug("Found {sa}", sa);

        return model;
    }

    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<GetSalesArrangementHandler> _logger;

    public GetSalesArrangementHandler(
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<GetSalesArrangementHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
