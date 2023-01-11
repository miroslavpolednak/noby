using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetSalesArrangementByOfferId;

internal sealed class GetSalesArrangementByOfferIdHandler
    : IRequestHandler<GetSalesArrangementByOfferIdRequest, GetSalesArrangementByOfferIdResponse>
{
    public async Task<GetSalesArrangementByOfferIdResponse> Handle(GetSalesArrangementByOfferIdRequest request, CancellationToken cancellation)
    {
        var instance = await _repository.GetSalesArrangementByOfferId(request.OfferId, cancellation);

        return new GetSalesArrangementByOfferIdResponse
        {
            IsExisting = instance is not null,
            Instance = instance
        };
    }

    private readonly Database.SalesArrangementServiceRepository _repository;
    
    public GetSalesArrangementByOfferIdHandler(Database.SalesArrangementServiceRepository repository)
    {
        _repository = repository;
    }
}