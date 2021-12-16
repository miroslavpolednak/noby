using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class GetSalesArrangementDetailHandler
    : IRequestHandler<Dto.GetSalesArrangementDetailMediatrRequest, GetSalesArrangementDetailResponse>
{
    public async Task<GetSalesArrangementDetailResponse> Handle(Dto.GetSalesArrangementDetailMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get detail for #{id}", request.SalesArrangementId);

        var sa = await _repository.GetSalesArrangement(request.SalesArrangementId);
        var data = await _repository.GetSalesArrangementData(request.SalesArrangementId);

        var model = new GetSalesArrangementDetailResponse
        {
            SalesArrangementId = sa.SalesArrangementId,
            SalesArrangementType = sa.SalesArrangementType,
            State = sa.State,
            CaseId = sa.CaseId,
            OfferInstanceId = sa.OfferInstanceId,
            SalesArrangement = data.Data
        };

        return model;
    }

    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<GetSalesArrangementDetailHandler> _logger;

    public GetSalesArrangementDetailHandler(
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<GetSalesArrangementDetailHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
