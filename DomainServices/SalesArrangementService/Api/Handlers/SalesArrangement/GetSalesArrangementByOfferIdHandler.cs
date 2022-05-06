using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class GetSalesArrangementByOfferIdHandler
    : IRequestHandler<Dto.GetSalesArrangementByOfferIdMediatrRequest, GetSalesArrangementByOfferIdResponse>
{
    public async Task<GetSalesArrangementByOfferIdResponse> Handle(Dto.GetSalesArrangementByOfferIdMediatrRequest request, CancellationToken cancellation)
    {
        var instance = await _repository.GetSalesArrangementByOfferId(request.OfferId, cancellation);

        return new GetSalesArrangementByOfferIdResponse
        {
            IsExisting = instance is not null,
            Instance = instance
        };
    }

    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<GetSalesArrangementByOfferIdHandler> _logger;

    public GetSalesArrangementByOfferIdHandler(
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<GetSalesArrangementByOfferIdHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}