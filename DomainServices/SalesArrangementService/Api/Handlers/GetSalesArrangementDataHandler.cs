using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class GetSalesArrangementDataHandler
    : IRequestHandler<Dto.GetSalesArrangementDataMediatrRequest, GetSalesArrangementDataResponse>
{
    public async Task<GetSalesArrangementDataResponse> Handle(Dto.GetSalesArrangementDataMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetSalesArrangementDataHandler), request.SalesArrangementId);

        _ = await _repository.GetSalesArrangement(request.SalesArrangementId, cancellation);
        //TODO zkontrolovat jestli ma pravo?

        var data = await _repository.GetSalesArrangementData(request.SalesArrangementId);
        
        var model = new GetSalesArrangementDataResponse
        {
            SalesArrangementId = request.SalesArrangementId,
            SalesArrangementDataId = data?.SalesArrangementDataId ?? default(int?),
            Data = data?.Data ?? ""
        };

        return model;
    }

    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<GetSalesArrangementDataHandler> _logger;

    public GetSalesArrangementDataHandler(
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<GetSalesArrangementDataHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
