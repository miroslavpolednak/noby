using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class GetSalesArrangementDataHandler
    : IRequestHandler<Dto.GetSalesArrangementDataMediatrRequest, GetSalesArrangementDataResponse>
{
    public async Task<GetSalesArrangementDataResponse> Handle(Dto.GetSalesArrangementDataMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get data for #{id}", request.SalesArrangementId);

        var saInstance = await _repository.GetSalesArrangement(request.SalesArrangementId);
        // zkontrolovat jestli ma pravo?

        var data = await _repository.GetSalesArrangementData(request.SalesArrangementId);
        
        var model = new GetSalesArrangementDataResponse
        {
            SalesArrangementId = request.SalesArrangementId,
            SalesArrangementDataId = data is null ? default(int?) : data.SalesArrangementDataId,
            Data = data is null ? "" : data.Data
        };

        return model;
    }

    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<GetSalesArrangementHandler> _logger;

    public GetSalesArrangementDataHandler(
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<GetSalesArrangementHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
